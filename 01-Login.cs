using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProyectopProgra2
{
    public partial class Form1 : Form
    {
        // Constructor
        public Form1()
        {
            InitializeComponent();
        }

        // Evento que se ejecuta al cargar el formulario
        private void Form1_Load(object sender, EventArgs e)
        {
            // Opcional: limpiar campos al inicio
            txtUsuario.Text = "";
            txtContraseña.Text = "";
        }

        // Evento del botón Ingresar
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContraseña.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Por favor ingrese usuario y contraseña.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string connectionString = @"Server=localhost\SQLEXPRESS;Database=Ferreteria_Ferre502;Trusted_Connection=True;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM Usuarios WHERE nombre_usuario = @usuario AND clave_usuario = @clave AND estado_usuario = 'Activo'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@usuario", usuario);
                        cmd.Parameters.AddWithValue("@clave", contrasena);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Inicio de sesión exitoso.", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Abrir el formulario principal (CRUD)
                            CRUD ventanaPrincipal = new CRUD();
                            ventanaPrincipal.Show();

                            // Ocultar o cerrar el login
                            this.Hide();
                            // this.Close(); 
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
