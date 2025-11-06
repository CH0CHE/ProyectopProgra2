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
    public partial class _11_VENTAS : Form
    {
        public _11_VENTAS()
        {
            InitializeComponent();
        }

        private void _11_VENTAS_Load(object sender, EventArgs e)
        {
            CargarVentas();
        }

        private void CargarVentas()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_venta, fecha_venta, total FROM Ventas";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvVentas.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar ventas: " + ex.Message);
            }
        }

        private void dgvVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dtpFecha.Value = Convert.ToDateTime(dgvVentas.Rows[e.RowIndex].Cells["fecha_venta"].Value);
                txtTotal.Text = dgvVentas.Rows[e.RowIndex].Cells["total"].Value.ToString();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTotal.Text))
            {
                MessageBox.Show("Por favor, complete el campo Total.");
                return;
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Ventas 
                                 (codigo_cliente, codigo_empleado, fecha_venta, total, estado_venta)
                                 VALUES (@cliente, @empleado, @fecha, @total, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cliente", 1); // valor fijo demo
                cmd.Parameters.AddWithValue("@empleado", 1); // valor fijo demo
                cmd.Parameters.AddWithValue("@fecha", dtpFecha.Value);
                cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(txtTotal.Text));
                cmd.Parameters.AddWithValue("@estado", "Activa"); // valor fijo demo
                cmd.ExecuteNonQuery();

                MessageBox.Show("Venta agregada correctamente.");
                CargarVentas();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvVentas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["codigo_venta"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Ventas 
                                 SET fecha_venta=@fecha, total=@total 
                                 WHERE codigo_venta=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fecha", dtpFecha.Value);
                cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(txtTotal.Text));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Venta actualizada correctamente.");
                CargarVentas();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvVentas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["codigo_venta"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Ventas WHERE codigo_venta=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Venta eliminada correctamente.");
                CargarVentas();
                LimpiarCampos();
            }
        }
        private void LimpiarCampos()
        {
            txtTotal.Clear();
            dtpFecha.Value = DateTime.Now;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();
            this.Hide();
        }
    }
}
