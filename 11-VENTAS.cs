using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

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
            try
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
                    cmd.Parameters.AddWithValue("@cliente", 99); // valor inexistente para probar error FK
                    cmd.Parameters.AddWithValue("@empleado", 99); // valor inexistente para probar error FK
                    cmd.Parameters.AddWithValue("@fecha", dtpFecha.Value);

                    // Validar que total sea numérico y positivo
                    if (!decimal.TryParse(txtTotal.Text, out decimal total) || total < 0)
                    {
                        MessageBox.Show("El valor del total no es válido. Debe ser un número positivo.");
                        return;
                    }

                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@estado", "Activa");
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Venta agregada correctamente.");
                    CargarVentas();
                    LimpiarCampos();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // violación de FK
                    MessageBox.Show("Error: El cliente o empleado no existe (violación de clave foránea).", "Error de relación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error al agregar la venta: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado al agregar la venta: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
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

                    if (!decimal.TryParse(txtTotal.Text, out decimal total) || total < 0)
                    {
                        MessageBox.Show("El valor del total no es válido. Debe ser un número positivo.");
                        return;
                    }

                    cmd.Parameters.AddWithValue("@total", total);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Venta actualizada correctamente.");
                    CargarVentas();
                    LimpiarCampos();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error de base de datos al editar la venta: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado al editar la venta: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
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
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("Error: No se puede eliminar la venta porque está relacionada con otros registros (violación de clave foránea).", "Error de relación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error al eliminar la venta: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado al eliminar la venta: " + ex.Message);
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
