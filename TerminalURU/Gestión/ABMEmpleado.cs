﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gestión.ServicioWeb;

namespace Gestión
{
    public partial class ABMEmpleado : Form
    {
        private Empleado objEmpleado = null;
        private Empleado empLogueado;


        public ABMEmpleado(Empleado emp)
        {
            InitializeComponent();
            empLogueado = emp;
        }

        private void ABMEmpleado_Load(object sender, EventArgs e)
        {
            this.ActivoPorDefecto();
        }

        private void ActivoPorDefecto()
        {
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = false;
            btnModificar.Enabled = false;

            txtCI.Enabled = true;
            txtCI.Text = "";
            txtNombre.Text = "";
            txtContraseña.Text = "";
        }

        private void ActivoActualizacion()
        {
            btnAgregar.Enabled = false;
            btnEliminar.Enabled = true;
            btnModificar.Enabled = true;

            txtNombre.Text = objEmpleado.nombreCompleto;
            txtContraseña.Text = objEmpleado.contraseña;
        }

        private void ActivoAgregar()
        {
            btnAgregar.Enabled = true;
            btnEliminar.Enabled = false;
            btnModificar.Enabled = false;

            txtCI.Enabled = false;
            txtNombre.Text = "";
            txtContraseña.Text = "";
        }

        private void txtNombre_Validating(object sender, CancelEventArgs e)
        {
            if (txtNombre.Text == "")
                errorProvider1.SetError(txtNombre, "Debe rellenar este campo");
            else
                errorProvider1.Clear();
        }

        private void txtCI_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                WebService WS = new WebService();
                objEmpleado = WS.BuscarEmpleadosActivos(Convert.ToInt32(txtCI.Text));

                if (objEmpleado == null)
                    ActivoAgregar();
                else
                    ActivoActualizacion();
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void txtContraseña_Validating(object sender, CancelEventArgs e)
        {
            if (txtContraseña.Text == "")
                errorProvider1.SetError(txtContraseña, "Debe rellenar este campo");
            else
                errorProvider1.Clear();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                WebService WS = new WebService();
                objEmpleado = new Empleado();
                objEmpleado.ci = Convert.ToInt32(txtCI.Text);
                objEmpleado.contraseña = txtContraseña.Text;
                objEmpleado.nombreCompleto = txtNombre.Text;
                WS.AltaEmpleado(objEmpleado);

                lblError.Text = "Alta con éxito.";
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                WebService WS = new WebService();
                objEmpleado.ci = Convert.ToInt32(txtCI.Text);
                objEmpleado.contraseña = txtContraseña.Text;
                objEmpleado.nombreCompleto = txtNombre.Text;
                WS.ModificarEmpleado(objEmpleado);

                lblError.Text = "Modificación con éxito.";
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                WebService WS = new WebService();
                WS.BajaEmpleado(objEmpleado);

                lblError.Text = "Baja con éxito.";
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                lblError.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.ActivoPorDefecto();
        }

        private void ABMEmpleado_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
