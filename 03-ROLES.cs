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
    public partial class _03_ROLES : Form
    {
        public _03_ROLES()
        {
            InitializeComponent();
        }
        private void _03_ROLES_Load(object sender, EventArgs e)
        {
            CargarRoles();
        }

        private void CargarRoles()
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT codigo_rol, nombre_rol, descripcion FROM Roles";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvRoles.DataSource = dt;
            }
        }


        private void dgvRoles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "INSERT INTO Roles (nombre_rol, descripcion, pantalla, tipo_permiso, estado_rol) VALUES (@nombre, @descripcion, @pantalla, @permiso, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreRol.Text);
                cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                cmd.Parameters.AddWithValue("@pantalla", "Principal");    // valor fijo
                cmd.Parameters.AddWithValue("@permiso", "Lectura");       // valor fijo
                cmd.Parameters.AddWithValue("@estado", "Activo");         // valor fijo
                cmd.ExecuteNonQuery();

                MessageBox.Show("Rol agregado correctamente.");
                CargarRoles();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvRoles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvRoles.SelectedRows[0].Cells["codigo_rol"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "UPDATE Roles SET nombre_rol=@nombre, descripcion=@descripcion WHERE codigo_rol=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreRol.Text);
                cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Rol actualizado correctamente.");
                CargarRoles();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvRoles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvRoles.SelectedRows[0].Cells["codigo_rol"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Roles WHERE codigo_rol=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Rol eliminado correctamente.");
                CargarRoles();
                LimpiarCampos();
            }
        }

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreRol.Text = dgvRoles.Rows[e.RowIndex].Cells["nombre_rol"].Value.ToString();
                txtDescripcion.Text = dgvRoles.Rows[e.RowIndex].Cells["descripcion"].Value.ToString();
            }
        }

        private void LimpiarCampos()
        {
            txtNombreRol.Clear();
            txtDescripcion.Clear();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();

            // Ocultar o cerrar el login
            this.Hide();
            // this.Close(); 
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtNombreRol_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
