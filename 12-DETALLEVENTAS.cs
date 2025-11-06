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
    public partial class _12_DETALLEVENTAS : Form
    {
        public _12_DETALLEVENTAS()
        {
            InitializeComponent();
        }

        private void _12_DETALLEVENTAS_Load(object sender, EventArgs e)
        {
            CargarDetalleVentas();
        }

        private void CargarDetalleVentas()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_detalle, codigo_venta, codigo_material, cantidad, precio_unitario, subtotal FROM DetalleVentas";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDetalleVentas.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los detalles de venta: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Por favor, complete los campos requeridos.");
                return;
            }

            int cantidad = Convert.ToInt32(txtCantidad.Text);
            decimal precio = Convert.ToDecimal(txtPrecio.Text);
            decimal subtotal = cantidad * precio;

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO DetalleVentas 
                                 (codigo_venta, codigo_material, cantidad, precio_unitario, subtotal)
                                 VALUES (@venta, @material, @cantidad, @precio, @subtotal)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@venta", 1); // valor fijo demo
                cmd.Parameters.AddWithValue("@material", 1); // valor fijo demo
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@subtotal", subtotal);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Detalle de venta agregado correctamente.");
                CargarDetalleVentas();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDetalleVentas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvDetalleVentas.SelectedRows[0].Cells["codigo_detalle"].Value);
            int cantidad = Convert.ToInt32(txtCantidad.Text);
            decimal precio = Convert.ToDecimal(txtPrecio.Text);
            decimal subtotal = cantidad * precio;

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE DetalleVentas 
                                 SET cantidad=@cantidad, precio_unitario=@precio, subtotal=@subtotal
                                 WHERE codigo_detalle=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@subtotal", subtotal);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Detalle de venta actualizado correctamente.");
                CargarDetalleVentas();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDetalleVentas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvDetalleVentas.SelectedRows[0].Cells["codigo_detalle"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM DetalleVentas WHERE codigo_detalle=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Detalle de venta eliminado correctamente.");
                CargarDetalleVentas();
                LimpiarCampos();
            }
        }
        private void LimpiarCampos()
        {
            txtCantidad.Clear();
            txtPrecio.Clear();
        }
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();
            this.Hide();
        }

        private void dgvDetalleVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtCantidad.Text = dgvDetalleVentas.Rows[e.RowIndex].Cells["cantidad"].Value.ToString();
                txtPrecio.Text = dgvDetalleVentas.Rows[e.RowIndex].Cells["precio_unitario"].Value.ToString();
            }
        }
    }
}
