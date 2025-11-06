using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectopProgra2
{
    public partial class _08_USUARIOS : Form
    {
        public _08_USUARIOS()
        {
            InitializeComponent();
        }

        private void _08_USUARIOS_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }
        private void CargarUsuarios()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_usuario, nombre_usuario, contrasena_usuario FROM Usuarios";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUsuarios.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los usuarios: " + ex.Message);
            }
        }
        private void LimpiarCampos()
        {
            txtUsuario.Clear();
            txtContrasena.Clear();
        }

        private void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtUsuario.Text = dgvUsuarios.Rows[e.RowIndex].Cells["nombre_usuario"].Value.ToString();
                txtContrasena.Text = dgvUsuarios.Rows[e.RowIndex].Cells["contrasena_usuario"].Value.ToString();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");
                return;
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Usuarios 
                                 (nombre_usuario, contrasena_usuario, rol_usuario, estado_usuario)
                                 VALUES (@nombre, @contrasena, @rol, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtUsuario.Text);
                cmd.Parameters.AddWithValue("@contrasena", txtContrasena.Text);
                cmd.Parameters.AddWithValue("@rol", "Administrador"); // valor fijo demo
                cmd.Parameters.AddWithValue("@estado", "Activo"); // valor fijo demo
                cmd.ExecuteNonQuery();

                MessageBox.Show("Usuario agregado correctamente.");
                CargarUsuarios();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["codigo_usuario"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "UPDATE Usuarios SET nombre_usuario=@nombre, contrasena_usuario=@contrasena WHERE codigo_usuario=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtUsuario.Text);
                cmd.Parameters.AddWithValue("@contrasena", txtContrasena.Text);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Usuario actualizado correctamente.");
                CargarUsuarios();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["codigo_usuario"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Usuarios WHERE codigo_usuario=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Usuario eliminado correctamente.");
                CargarUsuarios();
                LimpiarCampos();
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD crudForm = new CRUD();
            crudForm.Show();
            this.Hide();
        }
    }
}
