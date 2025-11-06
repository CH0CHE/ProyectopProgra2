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
    public partial class _04_SUCURSALES : Form
    {
        public _04_SUCURSALES()
        {
            InitializeComponent();
        }

        private void _04_SUCURSALES_Load(object sender, EventArgs e)
        {
            CargarSucursales();
        }

        private void Sucursales_Load(object sender, EventArgs e)
        {
            CargarSucursales();
        }

        private void CargarSucursales()
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT codigo_sucursal, nombre_sucursal, direccion FROM Sucursales";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSucursales.DataSource = dt;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "INSERT INTO Sucursales (nombre_sucursal, direccion, pais, estado) VALUES (@nombre, @direccion, @pais, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreSucursal.Text);
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@pais", "Guatemala"); // valor fijo
                cmd.Parameters.AddWithValue("@estado", "Activo");  // valor fijo
                cmd.ExecuteNonQuery();

                MessageBox.Show("Sucursal agregada correctamente.");
                CargarSucursales();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvSucursales.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvSucursales.SelectedRows[0].Cells["codigo_sucursal"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "UPDATE Sucursales SET nombre_sucursal=@nombre, direccion=@direccion WHERE codigo_sucursal=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreSucursal.Text);
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Sucursal actualizada correctamente.");
                CargarSucursales();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvSucursales.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvSucursales.SelectedRows[0].Cells["codigo_sucursal"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Sucursales WHERE codigo_sucursal=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Sucursal eliminada correctamente.");
                CargarSucursales();
                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            txtNombreSucursal.Clear();
            txtDireccion.Clear();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();

            // Ocultar o cerrar el login
            this.Hide();
            // this.Close(); 
        }

        private void dgvSucursales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreSucursal.Text = dgvSucursales.Rows[e.RowIndex].Cells["nombre_sucursal"].Value.ToString();
                txtDireccion.Text = dgvSucursales.Rows[e.RowIndex].Cells["direccion"].Value.ToString();
            }
        }
    }
}
