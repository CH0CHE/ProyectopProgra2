using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectopProgra2
{
    public partial class _17_PAGOSEMPLEADOS : Form
    {
        public _17_PAGOSEMPLEADOS()
        {
            InitializeComponent();
        }

        private void _17_PAGOSEMPLEADOS_Load(object sender, EventArgs e)
        {
            CargarPagosEmpleados();
        }

        private void CargarPagosEmpleados()
        {
            try
            {
                using (SqlConnection conn = ConexionBD.ObtenerConexion())
                {
                    conn.Open();
                    string query = "SELECT codigo_pago_empleado, codigo_empleado, fecha_pago, salario_base, pago_total FROM Pagos_Empleados";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPagosEmpleados.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar pagos de empleados: " + ex.Message);
            }
        }

        private void dgvPagosEmpleados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtSalario.Text = dgvPagosEmpleados.Rows[e.RowIndex].Cells["salario_base"].Value.ToString();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSalario.Text))
            {
                MessageBox.Show("Por favor, ingrese un salario base.");
                return;
            }

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                decimal salario = Convert.ToDecimal(txtSalario.Text);
                decimal pagoTotal = salario + 200; // bono fijo para demo

                string query = @"INSERT INTO Pagos_Empleados 
                                 (codigo_empleado, fecha_pago, año, mes, salario_base, horas_extras, bonos, descuentos, pago_total)
                                 VALUES (@empleado, @fecha, @anio, @mes, @salario, @extras, @bonos, @descuentos, @total)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@empleado", 1); // fijo para demo
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.Parameters.AddWithValue("@anio", DateTime.Now.Year);
                cmd.Parameters.AddWithValue("@mes", DateTime.Now.Month);
                cmd.Parameters.AddWithValue("@salario", salario);
                cmd.Parameters.AddWithValue("@extras", 0);
                cmd.Parameters.AddWithValue("@bonos", 200);
                cmd.Parameters.AddWithValue("@descuentos", 0);
                cmd.Parameters.AddWithValue("@total", pagoTotal);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago de empleado agregado correctamente.");
                CargarPagosEmpleados();
                LimpiarCampos();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvPagosEmpleados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para editar.");
                return;
            }

            int id = Convert.ToInt32(dgvPagosEmpleados.SelectedRows[0].Cells["codigo_pago_empleado"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = @"UPDATE Pagos_Empleados 
                                 SET salario_base=@salario, pago_total=@total 
                                 WHERE codigo_pago_empleado=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                decimal salario = Convert.ToDecimal(txtSalario.Text);
                decimal total = salario + 200;
                cmd.Parameters.AddWithValue("@salario", salario);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago de empleado actualizado correctamente.");
                CargarPagosEmpleados();
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvPagosEmpleados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
                return;
            }

            int id = Convert.ToInt32(dgvPagosEmpleados.SelectedRows[0].Cells["codigo_pago_empleado"].Value);

            using (SqlConnection conn = ConexionBD.ObtenerConexion())
            {
                conn.Open();
                string query = "DELETE FROM Pagos_Empleados WHERE codigo_pago_empleado=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Pago de empleado eliminado correctamente.");
                CargarPagosEmpleados();
                LimpiarCampos();
            }
        }
        private void LimpiarCampos()
        {
            txtSalario.Clear();
        }
        private void btnRegresar_Click(object sender, EventArgs e)
        {
            CRUD ventanaPrincipal = new CRUD();
            ventanaPrincipal.Show();
            this.Hide();
        }
    }
}
