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
    public partial class _06_CLIENTES : Form
    {
        public _06_CLIENTES()
        {
            InitializeComponent();
        }

        private void Clientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void CargarClientes()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_cliente, nombres_cliente, direccion_cliente FROM Clientes";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvClientes.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes: " + ex.Message);
            }
        }
        private void _06_CLIENTES_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void dgvRoles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreCliente.Text = dgvClientes.Rows[e.RowIndex].Cells["nombres_cliente"].Value.ToString();
                txtDireccion.Text = dgvClientes.Rows[e.RowIndex].Cells["direccion_cliente"].Value.ToString();
            }
        }
        private void LimpiarCampos()
        {
            txtNombreCliente.Clear();
            txtDireccion.Clear();
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreCliente.Text) || string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");
                return;
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Clientes 
                                 (nombres_cliente, nit_cliente, telefono_cliente, direccion_cliente, departamento_cliente, tipo_cliente, estado_cliente)
                                 VALUES (@nombre, @nit, @telefono, @direccion, @departamento, @tipo, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreCliente.Text);
                cmd.Parameters.AddWithValue("@nit", "CF"); // valor fijo demo
                cmd.Parameters.AddWithValue("@telefono", "0000-0000"); // valor fijo demo
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@departamento", "Guatemala"); // valor fijo demo
                cmd.Parameters.AddWithValue("@tipo", "Particular"); // valor fijo demo
                cmd.Parameters.AddWithValue("@estado", "Activo"); // valor fijo demo
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente agregado correctamente.");
                CargarClientes();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvClientes.SelectedRows[0].Cells["codigo_cliente"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "UPDATE Clientes SET nombres_cliente=@nombre, direccion_cliente=@direccion WHERE codigo_cliente=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreCliente.Text);
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente actualizado correctamente.");
                CargarClientes();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvClientes.SelectedRows[0].Cells["codigo_cliente"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Clientes WHERE codigo_cliente=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente eliminado correctamente.");
                CargarClientes();
                LimpiarCampos();
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();

            // Ocultar o cerrar el login
            this.Hide();
            // this.Close(); 
        }
    }
}
