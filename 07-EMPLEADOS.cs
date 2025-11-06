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
    public partial class _07_EMPLEADOS : Form
    {
        public _07_EMPLEADOS()
        {
            InitializeComponent();
        }

        private void _07_EMPLEADOS_Load(object sender, EventArgs e)
        {
            CargarEmpleados();
        }
        private void CargarEmpleados()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_empleado, nombres_empleado, direccion_empleado FROM Empleados";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvEmpleados.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los empleados: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreEmpleado.Text) || string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");
                return;
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"INSERT INTO Empleados 
                                 (nombres_empleado, direccion_empleado, telefono_empleado, dpi_empleado, fecha_nacimiento, puesto_empleado, estado_empleado)
                                 VALUES (@nombre, @direccion, @telefono, @dpi, @fecha, @puesto, @estado)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreEmpleado.Text);
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@telefono", "0000-0000"); // valor fijo demo
                cmd.Parameters.AddWithValue("@dpi", "0000000000000"); // valor fijo demo
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now); // valor fijo demo
                cmd.Parameters.AddWithValue("@puesto", "Vendedor"); // valor fijo demo
                cmd.Parameters.AddWithValue("@estado", "Activo"); // valor fijo demo
                cmd.ExecuteNonQuery();

                MessageBox.Show("Empleado agregado correctamente.");
                CargarEmpleados();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un empleado para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvEmpleados.SelectedRows[0].Cells["codigo_empleado"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "UPDATE Empleados SET nombres_empleado=@nombre, direccion_empleado=@direccion WHERE codigo_empleado=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", txtNombreEmpleado.Text);
                cmd.Parameters.AddWithValue("@direccion", txtDireccion.Text);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Empleado actualizado correctamente.");
                CargarEmpleados();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEmpleados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un empleado para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvEmpleados.SelectedRows[0].Cells["codigo_empleado"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Empleados WHERE codigo_empleado=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Empleado eliminado correctamente.");
                CargarEmpleados();
                LimpiarCampos();
            }
        }
        private void dgvEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreEmpleado.Text = dgvEmpleados.Rows[e.RowIndex].Cells["nombres_empleado"].Value.ToString();
                txtDireccion.Text = dgvEmpleados.Rows[e.RowIndex].Cells["direccion_empleado"].Value.ToString();
            }
        }

        private void LimpiarCampos()
        {
            txtNombreEmpleado.Clear();
            txtDireccion.Clear();
        }


        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD crudForm = new CRUD();
            crudForm.Show();
            this.Hide();
        }

        private void dgvEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreEmpleado.Text = dgvEmpleados.Rows[e.RowIndex].Cells["nombres_empleado"].Value.ToString();
                txtDireccion.Text = dgvEmpleados.Rows[e.RowIndex].Cells["direccion_empleado"].Value.ToString();
            }
        }
    }
}
