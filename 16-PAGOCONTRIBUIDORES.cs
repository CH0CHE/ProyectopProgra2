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
    public partial class _16_PAGOCONTRIBUIDORES : Form
    {
        public _16_PAGOCONTRIBUIDORES()
        {
            InitializeComponent();
        }

        private void _16_PAGOCONTRIBUIDORES_Load(object sender, EventArgs e)
        {
            cmbMetodo.Items.Add("Efectivo");
            cmbMetodo.Items.Add("Transferencia");
            cmbMetodo.Items.Add("Cheque");

            cmbEstado.Items.Add("Pagado");
            cmbEstado.Items.Add("Pendiente");
            cmbEstado.Items.Add("Cancelado");

            CargarPagosDistribuidores();
        }

        private void CargarPagosDistribuidores()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_pago_distribuidor, codigo_distribuidor, fecha_pago, monto, metodo_pago, estado_pago FROM Pagos_Distribuidores";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPagosDistribuidores.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pagos de distribuidores: " + ex.Message);
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
                string query = @"INSERT INTO Pagos_Distribuidores 
                                 (codigo_distribuidor, fecha_pago, monto, metodo_pago, estado_pago)
                                 VALUES (@distribuidor, @fecha, @monto, @metodo, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@distribuidor", 1); // fijo para demo
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@monto", Convert.ToDecimal(txtMonto.Text));
                cmd.Parameters.AddWithValue("@metodo", cmbMetodo.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@estado", "Pagado");
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago a distribuidor agregado correctamente.");
                CargarPagosDistribuidores();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPagosDistribuidores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvPagosDistribuidores.SelectedRows[0].Cells["codigo_pago_distribuidor"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Pagos_Distribuidores WHERE codigo_pago_distribuidor=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago de distribuidor eliminado correctamente.");
                CargarPagosDistribuidores();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvPagosDistribuidores.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvPagosDistribuidores.SelectedRows[0].Cells["codigo_pago_distribuidor"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Pagos_Distribuidores 
                                 SET monto=@monto, metodo_pago=@metodo, estado_pago=@estado 
                                 WHERE codigo_pago_distribuidor=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@monto", Convert.ToDecimal(txtMonto.Text));
                cmd.Parameters.AddWithValue("@metodo", cmbMetodo.SelectedItem?.ToString() ?? "Efectivo");
                cmd.Parameters.AddWithValue("@estado", cmbEstado.SelectedItem?.ToString() ?? "Pagado");
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago de distribuidor actualizado correctamente.");
                CargarPagosDistribuidores();
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

        private void dgvRoles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMonto.Text = dgvPagosDistribuidores.Rows[e.RowIndex].Cells["monto"].Value.ToString();
                cmbMetodo.Text = dgvPagosDistribuidores.Rows[e.RowIndex].Cells["metodo_pago"].Value.ToString();
                cmbEstado.Text = dgvPagosDistribuidores.Rows[e.RowIndex].Cells["estado_pago"].Value.ToString();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
