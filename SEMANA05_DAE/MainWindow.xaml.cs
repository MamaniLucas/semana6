using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static SEMANA05_DAE.MainWindow;

namespace SEMANA05_DAE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        private List<Cliente> clientes = new List<Cliente>();

        public MainWindow()

        {

            InitializeComponent();
        }
        private string connectionString = "Data Source=LAB1504-11\\SQLEXPRESS; Initial Catalog=NeptunoDB; User Id=Luis; Password=123456";
        public class Cliente
        {
            public string idCliente { get; set; }
            public string NombreCompañia { get; set; }
            public string NombreContacto { get; set; }
            public string CargoContacto { get; set; }
            public string Direccion { get; set; }
            public string Ciudad { get; set; }
            public string Region { get; set; }
            public string CodPostal { get; set; }
            public string Pais { get; set; }
            public string Telefono { get; set; }
            public string Fax { get; set; }
            public bool Activo { get; set; }
        }

        private void Button_CLIENTE(object sender, RoutedEventArgs e)
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("ListarClientes", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string idCliente = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                                string NombreCompañia = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                                string NombreContacto = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                                string CargoContacto = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                                string Direccion = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                                string Ciudad = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                                string Region = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                                string CodPostal = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                                string Pais = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                                string Telefono = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                                string Fax = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                                bool Activo = reader.IsDBNull(11) && reader.GetBoolean(11);

                                clientes.Add(new Cliente { idCliente = idCliente, NombreCompañia = NombreCompañia, NombreContacto = NombreContacto, 
                                                            CargoContacto = CargoContacto, Direccion = Direccion, Ciudad = Ciudad, Region = Region, 
                                                            CodPostal = CodPostal, Pais = Pais, Telefono = Telefono, Fax = Fax, Activo = Activo});
                            }
                        }
                    }
                }

                dgvDemo.ItemsSource = clientes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recuperar los clientes: " + ex.Message);
            }
        }

        private void Button_Insertar(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("InsertarCliente", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        // Asignar valores de los controles de entrada a los parámetros del procedimiento almacenado
                        command.Parameters.AddWithValue("@ID", idCliente.Text);
                        command.Parameters.AddWithValue("@NombreC", NombreCompañia.Text);
                        command.Parameters.AddWithValue("@NombreCo", NombreContacto.Text);
                        command.Parameters.AddWithValue("@CargoCo", CargoContacto.Text);
                        command.Parameters.AddWithValue("@Direccion", Direccion.Text);
                        command.Parameters.AddWithValue("@Ciudad", Ciudad.Text);
                        command.Parameters.AddWithValue("@Region", Region.Text);
                        command.Parameters.AddWithValue("@CodPostal", CodPostal.Text);
                        command.Parameters.AddWithValue("@Pais", Pais.Text);
                        command.Parameters.AddWithValue("@Telefono", Telefono.Text);
                        command.Parameters.AddWithValue("@Fax", Fax.Text);
                        if (ActivoCheckBox.IsChecked == true)
                        {
                            command.Parameters.AddWithValue("@Activo", 1);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Activo", 0);
                        }
                        command.ExecuteNonQuery();
                    }
                    LimpiarCampos();
                }

                // Actualizar la vista de la tabla después de la inserción
                Button_CLIENTE(sender, e);

            

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el cliente: " + ex.Message);
            }
        }


        private void Button_Buscar(object sender, RoutedEventArgs e)
        {
            string codigoCliente = idCliente.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("ListarClientes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idCliente", codigoCliente);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Cliente encontrado, llenar los campos de texto
                                NombreCompañia.Text = reader["NombreCompañia"].ToString();
                                NombreContacto.Text = reader["NombreContacto"].ToString();
                                CargoContacto.Text = reader["CargoContacto"].ToString();
                                Direccion.Text = reader["Direccion"].ToString();
                                Ciudad.Text = reader["Ciudad"].ToString();
                                Region.Text = reader["Region"].ToString();
                                CodPostal.Text = reader["CodPostal"].ToString();
                                Pais.Text = reader["Pais"].ToString();
                                Telefono.Text = reader["Telefono"].ToString();
                                Fax.Text = reader["Fax"].ToString();
                                ActivoCheckBox.IsChecked = Convert.ToBoolean(reader["Activo"]);
                            }
                            else
                            {
                                // Cliente no encontrado, limpiar los campos de texto
                                LimpiarCampos();
                                MessageBox.Show("Cliente no encontrado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar el cliente: " + ex.Message);
            }

        }

        private void Button_Eliminar(object sender, RoutedEventArgs e)
        {
            string codigoCliente = idCliente.Text;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("EliminarCliente", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", codigoCliente);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente eliminado correctamente.");
                            LimpiarCampos();

                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún cliente con el ID especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el cliente: " + ex.Message);
            }

        }
        private void LimpiarCampos()
        {
            // Esta función limpia los campos de texto y el estado del CheckBox
            NombreCompañia.Text = string.Empty;
            NombreContacto.Text = string.Empty;
            CargoContacto.Text = string.Empty;
            Direccion.Text = string.Empty;
            Ciudad.Text = string.Empty;
            Region.Text = string.Empty;
            CodPostal.Text = string.Empty;
            Pais.Text = string.Empty;
            Telefono.Text = string.Empty;
            Fax.Text = string.Empty;
            ActivoCheckBox.IsChecked = false;
        }

        private void Button_Actualizar(object sender, RoutedEventArgs e)
        {
            string codigoCliente = idCliente.Text;
            string nombreCompania = NombreCompañia.Text;
            string nombreContacto = NombreContacto.Text;
            string cargoContacto = CargoContacto.Text;
            string direccion = Direccion.Text;
            string ciudad = Ciudad.Text;
            string region = Region.Text;
            string codPostal = CodPostal.Text;
            string pais = Pais.Text;
            string telefono = Telefono.Text;
            string fax = Fax.Text;
            bool activo = (ActivoCheckBox.IsChecked == true);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("ActualizarCliente", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", codigoCliente);
                        command.Parameters.AddWithValue("@NombreC", nombreCompania);
                        command.Parameters.AddWithValue("@NombreCo", nombreContacto);
                        command.Parameters.AddWithValue("@CargoCo", cargoContacto);
                        command.Parameters.AddWithValue("@Direccion", direccion);
                        command.Parameters.AddWithValue("@Ciudad", ciudad);
                        command.Parameters.AddWithValue("@Region", region);
                        command.Parameters.AddWithValue("@CodPostal", codPostal);
                        command.Parameters.AddWithValue("@Pais", pais);
                        command.Parameters.AddWithValue("@Telefono", telefono);
                        command.Parameters.AddWithValue("@Fax", fax);
                        command.Parameters.AddWithValue("@Activo", activo);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cliente actualizado correctamente.");
                            LimpiarCampos();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún cliente con el ID especificado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el cliente: " + ex.Message);
            }

        }
    }
}