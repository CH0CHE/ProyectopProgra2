using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectopProgra2
{
    public partial class _05_DISTRIBUIDORES : Form
    {
        public _05_DISTRIBUIDORES()
        {
            InitializeComponent();
        }

        private void _05_DISTRIBUIDORES_Load(object sender, EventArgs e)
        {
            CargarDistribuidores();
        }
        private void Distribuidores_Load(object sender, EventArgs e)
        {
            CargarDistribuidores();
        }

        private void CargarDistribuidores()
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT codigo_distribuidor, nombre_distribuidor, direccion FROM Distribuidores";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDistribuidores.DataSource = dt;
            }
        }
        private void LimpiarCampos()
        {
            txtNombreDistribuidor.Clear();
            txtDireccion.Clear();
        }

        private void dgvDistribuidores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreDistribuidor.Text = dgvDistribuidores.Rows[e.RowIndex].Cells["nombre_distribuidor"].Value.ToString();
                txtDireccion.Text = dgvDistribuidores.Rows[e.RowIndex].Cells["direccion"].Value.ToString();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Distribuidores 
                                 (nombre_distribuidor, telefono, correo, direccion, estado_distribuidor) 
                                 VALUES (@nombre, @telefono, @correo, @direccion, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreDistribuidor.Text);
                cmd.Parameters.AddWithValue("@telefono", "0000-0000"); // valor fijo demo
                cmd.Parameters.AddWithValue("@correo", "demo@ferre502.com"); // valor fijo demo
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@estado", "Activo"); // valor fijo demo
                cmd.ExecuteNonQuery();

                MessageBox.Show("Distribuidor agregado correctamente.");
                CargarDistribuidores();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDistribuidores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvDistribuidores.SelectedRows[0].Cells["codigo_distribuidor"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "UPDATE Distribuidores SET nombre_distribuidor=@nombre, direccion=@direccion WHERE codigo_distribuidor=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreDistribuidor.Text);
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Distribuidor actualizado correctamente.");
                CargarDistribuidores();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDistribuidores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvDistribuidores.SelectedRows[0].Cells["codigo_distribuidor"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Distribuidores WHERE codigo_distribuidor=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Distribuidor eliminado correctamente.");
                CargarDistribuidores();
                LimpiarCampos();
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();

            // Ocultar o cerrar el login
            this.Hide();
        }
    }
}
