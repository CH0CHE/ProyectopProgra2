using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProyectopProgra2
{
    public partial class _10_INVENTARIOS : Form
    {
        public _10_INVENTARIOS()
        {
            InitializeComponent();
        }

        private void _10_INVENTARIOS_Load(object sender, EventArgs e)
        {
            CargarInventarios();
        }

        private void CargarInventarios()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_inventario, cantidad, stock_minimo FROM Inventarios";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvInventarios.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar inventarios: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtStockMinimo.Text))
                {
                    MessageBox.Show("Por favor, complete los campos requeridos.");
                    return;
                }

                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"INSERT INTO Inventarios 
                                     (codigo_sucursal, codigo_material, cantidad, fecha_ultima_actualizacion, stock_minimo, estado_inventario)
                                     VALUES (@sucursal, @material, @cantidad, @fecha, @stockMinimo, @estado)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@sucursal", 99); // valor inexistente para provocar error FK
                    cmd.Parameters.AddWithValue("@material", 99); // valor inexistente para provocar error FK
                    cmd.Parameters.AddWithValue("@cantidad", Convert.ToInt32(txtCantidad.Text));
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@stockMinimo", Convert.ToInt32(txtStockMinimo.Text));
                    cmd.Parameters.AddWithValue("@estado", "Activo");
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Inventario agregado correctamente.");
                    CargarInventarios();
                    LimpiarCampos();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547) // Error 547 = violación de clave foránea
                    MessageBox.Show("Error: El código de sucursal o material no existe (violación de clave foránea).", "Error de relación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error al agregar el inventario: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvInventarios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un registro para editar.");
                    return;
                }

                int id = Convert.ToInt32(dgvInventarios.SelectedRows[0].Cells["codigo_inventario"].Value);

                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"UPDATE Inventarios 
                                     SET cantidad=@cantidad, stock_minimo=@stockMinimo, fecha_ultima_actualizacion=@fecha 
                                     WHERE codigo_inventario=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cantidad", Convert.ToInt32(txtCantidad.Text));
                    cmd.Parameters.AddWithValue("@stockMinimo", Convert.ToInt32(txtStockMinimo.Text));
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Inventario actualizado correctamente.");
                    CargarInventarios();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el inventario: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvInventarios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione un registro para eliminar.");
                    return;
                }

                int id = Convert.ToInt32(dgvInventarios.SelectedRows[0].Cells["codigo_inventario"].Value);

                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "DELETE FROM Inventarios WHERE codigo_inventario=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Inventario eliminado correctamente.");
                    CargarInventarios();
                    LimpiarCampos();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("Error: No se puede eliminar porque el registro está relacionado con otra tabla (violación de clave foránea).", "Error de relación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Error al eliminar el inventario: " + ex.Message);
            }
        }

        private void dgvInventarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtCantidad.Text = dgvInventarios.Rows[e.RowIndex].Cells["cantidad"].Value.ToString();
                txtStockMinimo.Text = dgvInventarios.Rows[e.RowIndex].Cells["stock_minimo"].Value.ToString();
            }
        }

        private void LimpiarCampos()
        {
            txtCantidad.Clear();
            txtStockMinimo.Clear();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();
            this.Hide();
        }
    }
}
