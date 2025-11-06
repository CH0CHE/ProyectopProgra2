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
    public partial class _15_PAGOCLIENTES : Form
    {
        public _15_PAGOCLIENTES()
        {
            InitializeComponent();
        }

        private void _15_PAGOCLIENTES_Load(object sender, EventArgs e)
        {
            cmbEstado.Items.Add("Pagado");
            cmbEstado.Items.Add("Pendiente");
            cmbEstado.Items.Add("Cancelado");

            cmbMetodo.Items.Add("Efectivo");
            cmbMetodo.Items.Add("Transferencia");
            cmbMetodo.Items.Add("Tarjeta");

            CargarPagosClientes();
        }

        private void CargarPagosClientes()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_pago_cliente, codigo_cliente, fecha_pago, monto, metodo_pago, estado_pago FROM Pagos_Clientes";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPagosClientes.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los pagos de clientes: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMonto.Text) || cmbMetodo.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, complete los campos requeridos.");
                return;
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Pagos_Clientes 
                                 (codigo_cliente, fecha_pago, monto, metodo_pago, estado_pago)
                                 VALUES (@cliente, @fecha, @monto, @metodo, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cliente", 1); // valor fijo demo
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@monto", Convert.ToDecimal(txtMonto.Text));
                cmd.Parameters.AddWithValue("@metodo", cmbMetodo.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@estado", "Pagado");
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago de cliente agregado correctamente.");
                CargarPagosClientes();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvPagosClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvPagosClientes.SelectedRows[0].Cells["codigo_pago_cliente"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Pagos_Clientes 
                                 SET monto=@monto, metodo_pago=@metodo, estado_pago=@estado
                                 WHERE codigo_pago_cliente=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@monto", Convert.ToDecimal(txtMonto.Text));
                cmd.Parameters.AddWithValue("@metodo", cmbMetodo.SelectedItem?.ToString() ?? "Efectivo");
                cmd.Parameters.AddWithValue("@estado", cmbEstado.SelectedItem?.ToString() ?? "Pagado");
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago de cliente actualizado correctamente.");
                CargarPagosClientes();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPagosClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvPagosClientes.SelectedRows[0].Cells["codigo_pago_cliente"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Pagos_Clientes WHERE codigo_pago_cliente=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago de cliente eliminado correctamente.");
                CargarPagosClientes();
                LimpiarCampos();
            }
        }
        private void LimpiarCampos()
        {
            txtMonto.Clear();
            cmbMetodo.SelectedIndex = -1;
            cmbEstado.SelectedIndex = -1;
        }
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();
            this.Hide();
        }

        private void dgvPagosClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMonto.Text = dgvPagosClientes.Rows[e.RowIndex].Cells["monto"].Value.ToString();
                cmbMetodo.Text = dgvPagosClientes.Rows[e.RowIndex].Cells["metodo_pago"].Value.ToString();
                cmbEstado.Text = dgvPagosClientes.Rows[e.RowIndex].Cells["estado_pago"].Value.ToString();
            }
        }
    }
}
