using System;
using System.Windows.Forms;

namespace ProyectopProgra2
{
    public partial class CRUD : Form
    {
        public CRUD()
        {
            InitializeComponent();
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            _03_ROLES rolesForm = new _03_ROLES();
            rolesForm.Show();
            this.Hide();
        }

        private void btnSucursales_Click(object sender, EventArgs e)
        {
            _04_SUCURSALES sucursalForm = new _04_SUCURSALES();
            sucursalForm.Show();
            this.Hide();
        }

        private void btnDistribuidores_Click(object sender, EventArgs e)
        {
            _05_DISTRIBUIDORES distribuidoresForm = new _05_DISTRIBUIDORES();
            distribuidoresForm.Show();
            this.Hide();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            _06_CLIENTES clientesForm = new _06_CLIENTES();
            clientesForm.Show();
            this.Hide();
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            _07_EMPLEADOS empleadosForm = new _07_EMPLEADOS();
            empleadosForm.Show();
            this.Hide();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            _08_USUARIOS usuariosForm = new _08_USUARIOS();
            usuariosForm.Show();
            this.Hide();
        }

        private void btnMateriales_Click(object sender, EventArgs e)
        {
            _09_MATERIALES materialesForm = new _09_MATERIALES();
            materialesForm.Show();
            this.Hide();
        }

        private void btnInventarios_Click(object sender, EventArgs e)
        {
            _10_INVENTARIOS inventariosForm = new _10_INVENTARIOS();
            inventariosForm.Show();
            this.Hide();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            _11_VENTAS ventasForm = new _11_VENTAS();
            ventasForm.Show();
            this.Hide();
        }

        private void btnDetalleVentas_Click(object sender, EventArgs e)
        {
            _12_DETALLEVENTAS detalleVentasForm = new _12_DETALLEVENTAS();
            detalleVentasForm.Show();
            this.Hide();
        }

        private void btnEnvios_Click(object sender, EventArgs e)
        {
            _13_ENVIOS enviosForm = new _13_ENVIOS();
            enviosForm.Show();
            this.Hide();
        }

        private void btnDetalleEnvios_Click(object sender, EventArgs e)
        {
            _14_DETALLEENVIOS detalleEnviosForm = new _14_DETALLEENVIOS();
            detalleEnviosForm.Show();
            this.Hide();
        }

        private void btnPagosClientes_Click(object sender, EventArgs e)
        {
            _15_PAGOCLIENTES pagosClientesForm = new _15_PAGOCLIENTES();
            pagosClientesForm.Show();
            this.Hide();
        }

        private void btnPagosDistribuidores_Click(object sender, EventArgs e)
        {
            _16_PAGOCONTRIBUIDORES pagosDistribuidoresForm = new _16_PAGOCONTRIBUIDORES();
            pagosDistribuidoresForm.Show();
            this.Hide();
        }

        private void btnPagosEmpleados_Click(object sender, EventArgs e)
        {
            _17_PAGOSEMPLEADOS pagosEmpleadosForm = new _17_PAGOSEMPLEADOS();
            pagosEmpleadosForm.Show();
            this.Hide();
        }


        private void CRUD_Load(object sender, EventArgs e)
        {
            // Aquí puedes inicializar cosas si quieres
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Evento opcional para el label
        }

        private void btnDistribuidores_Click_1(object sender, EventArgs e)
        {
            _05_DISTRIBUIDORES distribuidoresForm = new _05_DISTRIBUIDORES();
            distribuidoresForm.Show();
            this.Hide();
        }

        private void btnClientes_Click_1(object sender, EventArgs e)
        {
            _06_CLIENTES clientesForm = new _06_CLIENTES();
            clientesForm.Show();
            this.Hide();
        }

        private void btnEmpleados_Click_1(object sender, EventArgs e)
        {
            _07_EMPLEADOS empleadosForm = new _07_EMPLEADOS();
            empleadosForm.Show();
            this.Hide();
        }

        private void btnUsuarios_Click_1(object sender, EventArgs e)
        {
            _08_USUARIOS usuariosForm = new _08_USUARIOS();
            usuariosForm.Show();
            this.Hide();
        }

        private void btnMateriales_Click_1(object sender, EventArgs e)
        {
            _09_MATERIALES materialesForm = new _09_MATERIALES();
            materialesForm.Show();
            this.Hide();
        }

        private void btnInventarios_Click_1(object sender, EventArgs e)
        {
            _10_INVENTARIOS inventariosForm = new _10_INVENTARIOS();
            inventariosForm.Show();
            this.Hide();
        }

        private void btnVentas_Click_1(object sender, EventArgs e)
        {
            _11_VENTAS ventasForm = new _11_VENTAS();
            ventasForm.Show();
            this.Hide();
        }

        private void btnDetalleVentas_Click_1(object sender, EventArgs e)
        {
            _12_DETALLEVENTAS detalleVentasForm = new _12_DETALLEVENTAS();
            detalleVentasForm.Show();
            this.Hide();
        }

        private void btnEnvios_Click_1(object sender, EventArgs e)
        {
            _13_ENVIOS enviosForm = new _13_ENVIOS();
            enviosForm.Show();
            this.Hide();
        }

        private void btnDetalleEnvios_Click_1(object sender, EventArgs e)
        {
            _14_DETALLEENVIOS detalleEnviosForm = new _14_DETALLEENVIOS();
            detalleEnviosForm.Show();
            this.Hide();
        }

        private void btnPagosClientes_Click_1(object sender, EventArgs e)
        {
            _15_PAGOCLIENTES pagosClientesForm = new _15_PAGOCLIENTES();
            pagosClientesForm.Show();
            this.Hide();
        }

        private void btnPagosDistribuidores_Click_1(object sender, EventArgs e)
        {
            _16_PAGOCONTRIBUIDORES pagosDistribuidoresForm = new _16_PAGOCONTRIBUIDORES();
            pagosDistribuidoresForm.Show();
            this.Hide();
        }

        private void btnPagosEmpleados_Click_1(object sender, EventArgs e)
        {
            _17_PAGOSEMPLEADOS pagosEmpleadosForm = new _17_PAGOSEMPLEADOS();
            pagosEmpleadosForm.Show();
            this.Hide();
        }

        

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
