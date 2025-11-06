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
    public partial class _09_MATERIALES : Form
    {
        public _09_MATERIALES()
        {
            InitializeComponent();
        }

        private void _09_MATERIALES_Load(object sender, EventArgs e)
        {
            CargarMateriales();
        }

        private void CargarMateriales()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_material, nombre_material, precio FROM Materiales";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMateriales.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los materiales: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("Por favor, complete los campos requeridos.");
                return;
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Materiales 
                                 (nombre_material, precio, stock, categoria, estado_material)
                                 VALUES (@nombre, @precio, @stock, @categoria, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@stock", 50); // valor fijo demo
                cmd.Parameters.AddWithValue("@categoria", "General"); // valor fijo demo
                cmd.Parameters.AddWithValue("@estado", "Activo"); // valor fijo demo
                cmd.ExecuteNonQuery();

                MessageBox.Show("Material agregado correctamente.");
                CargarMateriales();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvMateriales.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un material para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvMateriales.SelectedRows[0].Cells["codigo_material"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "UPDATE Materiales SET nombre_material=@nombre, precio=@precio WHERE codigo_material=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@precio", Convert.ToDecimal(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Material actualizado correctamente.");
                CargarMateriales();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvMateriales.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un material para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvMateriales.SelectedRows[0].Cells["codigo_material"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Materiales WHERE codigo_material=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Material eliminado correctamente.");
                CargarMateriales();
                LimpiarCampos();
            }
        }

        private void dgvRoles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombre.Text = dgvMateriales.Rows[e.RowIndex].Cells["nombre_material"].Value.ToString();
                txtPrecio.Text = dgvMateriales.Rows[e.RowIndex].Cells["precio"].Value.ToString();
            }
        }
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();
            this.Hide();
        }
    }
}
