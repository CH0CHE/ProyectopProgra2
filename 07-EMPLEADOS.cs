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
                    string query = "SELECT codigo_empleado, nombres_empleado, telefono_empleado, correo_empleado, cargo_empleado, salario_base, fecha_ingreso, estado_empleado FROM Empleados";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvEmpleados.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombreEmpleado.Text))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");
                return;
            }

            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"INSERT INTO Empleados 
                                     (nombres_empleado, telefono_empleado, correo_empleado, cargo_empleado, salario_base, fecha_ingreso, estado_empleado)
                                     VALUES (@nombre, @telefono, @correo, @cargo, @salario, @fecha, @estado)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombreEmpleado.Text);
                    cmd.Parameters.AddWithValue("@telefono", "0000-0000"); // valor fijo demo
                    cmd.Parameters.AddWithValue("@correo", "correo@demo.com"); // valor fijo demo
                    cmd.Parameters.AddWithValue("@cargo", "Vendedor"); // valor fijo demo
                    cmd.Parameters.AddWithValue("@salario", 3500.00m); // valor fijo demo
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@estado", "Activo");
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Empleado agregado correctamente.");
                    CargarEmpleados();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = @"UPDATE Empleados 
                                     SET nombres_empleado=@nombre, telefono_empleado=@telefono, correo_empleado=@correo, cargo_empleado=@cargo, salario_base=@salario, estado_empleado=@estado 
                                     WHERE codigo_empleado=@id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombre", txtNombreEmpleado.Text);
                    cmd.Parameters.AddWithValue("@telefono", "0000-0000"); // valor demo
                    cmd.Parameters.AddWithValue("@correo", "correo@demo.com"); // valor demo
                    cmd.Parameters.AddWithValue("@cargo", "Vendedor");
                    cmd.Parameters.AddWithValue("@salario", 3500.00m);
                    cmd.Parameters.AddWithValue("@estado", "Activo");
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Empleado actualizado correctamente.");
                    CargarEmpleados();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNombreEmpleado.Text = dgvEmpleados.Rows[e.RowIndex].Cells["nombres_empleado"].Value.ToString();
            }
        }

        private void LimpiarCampos()
        {
            txtNombreEmpleado.Clear();
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
            }
        }
    }
}
