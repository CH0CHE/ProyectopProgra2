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
    public partial class _13_ENVIOS : Form
    {
        public _13_ENVIOS()
        {
            InitializeComponent();
        }

        private void _13_ENVIOS_Load(object sender, EventArgs e)
        {
            cmbEstado.Items.Add("Pendiente");
            cmbEstado.Items.Add("Enviado");
            cmbEstado.Items.Add("Cancelado");
            CargarEnvios();
        }

        private void CargarEnvios()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_envio, codigo_venta, fecha_envio, direccion_envio, vehiculo_asignado, costo_envio, estado_envio FROM Envios";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvEnvios.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los envíos: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDireccion.Text) || string.IsNullOrWhiteSpace(txtVehiculo.Text) || string.IsNullOrWhiteSpace(txtCosto.Text))
            {
                MessageBox.Show("Por favor, complete los campos requeridos.");
                return;
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Envios 
                                 (codigo_venta, fecha_envio, direccion_envio, vehiculo_asignado, costo_envio, estado_envio)
                                 VALUES (@venta, @fecha, @direccion, @vehiculo, @costo, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@venta", 1); // Valor fijo demo
                cmd.Parameters.AddWithValue("@fecha", dtpFechaEnvio.Value);
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@vehiculo", txtVehiculo.Text);
                cmd.Parameters.AddWithValue("@costo", Convert.ToDecimal(txtCosto.Text));
                cmd.Parameters.AddWithValue("@estado", "Pendiente");
                cmd.ExecuteNonQuery();

                MessageBox.Show("Envío agregado correctamente.");
                CargarEnvios();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEnvios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvEnvios.SelectedRows[0].Cells["codigo_envio"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Envios 
                                 SET direccion_envio=@direccion, vehiculo_asignado=@vehiculo, costo_envio=@costo, estado_envio=@estado
                                 WHERE codigo_envio=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@vehiculo", txtVehiculo.Text);
                cmd.Parameters.AddWithValue("@costo", Convert.ToDecimal(txtCosto.Text));
                cmd.Parameters.AddWithValue("@estado", cmbEstado.SelectedItem?.ToString() ?? "Pendiente");
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Envío actualizado correctamente.");
                CargarEnvios();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEnvios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvEnvios.SelectedRows[0].Cells["codigo_envio"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Envios WHERE codigo_envio=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Envío eliminado correctamente.");
                CargarEnvios();
                LimpiarCampos();
            }
        }
        private void LimpiarCampos()
        {
            txtDireccion.Clear();
            txtVehiculo.Clear();
            txtCosto.Clear();
            cmbEstado.SelectedIndex = -1;
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();
            this.Hide();
        }

        private void dgvEnvios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtDireccion.Text = dgvEnvios.Rows[e.RowIndex].Cells["direccion_envio"].Value.ToString();
                txtVehiculo.Text = dgvEnvios.Rows[e.RowIndex].Cells["vehiculo_asignado"].Value.ToString();
                txtCosto.Text = dgvEnvios.Rows[e.RowIndex].Cells["costo_envio"].Value.ToString();
                cmbEstado.Text = dgvEnvios.Rows[e.RowIndex].Cells["estado_envio"].Value.ToString();
            }
        }
    }
}
