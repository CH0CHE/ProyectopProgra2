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
    public partial class _14_DETALLEENVIOS : Form
    {
        public _14_DETALLEENVIOS()
        {
            InitializeComponent();
        }

        private void _14_DETALLEENVIOS_Load(object sender, EventArgs e)
        {
            cmbEstado.Items.Add("Activo");
            cmbEstado.Items.Add("Inactivo");
            CargarDetalleEnvios();
        }

        private void CargarDetalleEnvios()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_detalle_envio, codigo_envio, codigo_material, cantidad, costo_unitario, costo_total, estado_detalle_envio FROM Detalle_Envios";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDetalleEnvios.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los detalles de envíos: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtCostoUnitario.Text))
            {
                MessageBox.Show("Por favor, complete los campos requeridos.");
                return;
            }

            int cantidad = Convert.ToInt32(txtCantidad.Text);
            decimal costoUnitario = Convert.ToDecimal(txtCostoUnitario.Text);
            decimal costoTotal = cantidad * costoUnitario;

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Detalle_Envios 
                                 (codigo_envio, codigo_material, cantidad, costo_unitario, costo_total, estado_detalle_envio)
                                 VALUES (@envio, @material, @cantidad, @costoU, @costoT, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@envio", 1); // valor fijo demo
                cmd.Parameters.AddWithValue("@material", 1); // valor fijo demo
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@costoU", costoUnitario);
                cmd.Parameters.AddWithValue("@costoT", costoTotal);
                cmd.Parameters.AddWithValue("@estado", "Activo");
                cmd.ExecuteNonQuery();

                MessageBox.Show("Detalle de envío agregado correctamente.");
                CargarDetalleEnvios();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDetalleEnvios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvDetalleEnvios.SelectedRows[0].Cells["codigo_detalle_envio"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Detalle_Envios 
                                 SET cantidad=@cantidad, costo_unitario=@costoU, costo_total=@costoT, estado_detalle_envio=@estado
                                 WHERE codigo_detalle_envio=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                int cantidad = Convert.ToInt32(txtCantidad.Text);
                decimal costoU = Convert.ToDecimal(txtCostoUnitario.Text);
                decimal costoT = cantidad * costoU;

                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@costoU", costoU);
                cmd.Parameters.AddWithValue("@costoT", costoT);
                cmd.Parameters.AddWithValue("@estado", cmbEstado.SelectedItem?.ToString() ?? "Activo");
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Detalle de envío actualizado correctamente.");
                CargarDetalleEnvios();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDetalleEnvios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvDetalleEnvios.SelectedRows[0].Cells["codigo_detalle_envio"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Detalle_Envios WHERE codigo_detalle_envio=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Detalle de envío eliminado correctamente.");
                CargarDetalleEnvios();
                LimpiarCampos();
            }
        }
        private void LimpiarCampos()
        {
            txtCantidad.Clear();
            txtCostoUnitario.Clear();
            cmbEstado.SelectedIndex = -1;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();
            this.Hide();

        }

        private void dgvDetalleEnvios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtCantidad.Text = dgvDetalleEnvios.Rows[e.RowIndex].Cells["cantidad"].Value.ToString();
                txtCostoUnitario.Text = dgvDetalleEnvios.Rows[e.RowIndex].Cells["costo_unitario"].Value.ToString();
                cmbEstado.Text = dgvDetalleEnvios.Rows[e.RowIndex].Cells["estado_detalle_envio"].Value.ToString();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
