﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EntidadesCompartidas;
using Logica;

public partial class ConsultaViajes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                Session["viajeElegido"] = null;

                List<Viajes> v = FabricaLogica.GetLogicaViajes().ListarViajes();
                RepeaterCViajes.DataSource = v;
                RepeaterCViajes.DataBind();

                Session["TodosLosViajes"] = v;
                List<Terminal> terminales = new List<Terminal>();
                List<Compania> companias = new List<Compania>();

                foreach (Viajes viajes in v)
                {
                    terminales.Add(viajes.t);
                    companias.Add(viajes.c);
                }

                var resultadoT = ((from t in terminales group t by t.codigo into b select b.Key)).ToList<object>();

                foreach (var j in resultadoT)
                {
                    DDLDestino.Items.Add(j.ToString());
                }

                var resultadoC = ((from c in companias group c by c.nombre into a select a.Key)).ToList<object>();

                foreach (var q in resultadoC)
                {
                    DDLCompania.Items.Add(q.ToString());
                }
                DDLDestino.SelectedIndex = -1;
                DDLCompania.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }

    protected void RepeaterCViajes_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        List<Viajes> v = (List<Viajes>)Session["TodosLosViajes"];
        if (e.CommandName == "Seleccionar")
        {
            foreach (Viajes j in v)
            {
                if (j.numero == Convert.ToInt32(((Label)(e.Item.Controls[1])).Text))
                {
                    Session["viajeElegido"] = j;
                    Response.Redirect("ConsultaIndividualDeViaje.aspx");
                }
            }
        }
    }

    protected void bntLimpiarFiltros_Click(object sender, EventArgs e)
    {
        DDLCompania.ClearSelection();
        DDLDestino.ClearSelection();
        Años1.ClearSelection();
        RepeaterCViajes.DataSource = (List<Viajes>)Session["TodosLosViajes"];
        RepeaterCViajes.DataBind();
    }
    protected void btnFiltro_Click(object sender, EventArgs e)
    {
        try
        {
            List<Viajes> viajes = (List<Viajes>)Session["TodosLosViajes"];
            DateTime fecha = new DateTime(Años1.SelectedAños, Meses1.SeleccionMes, Dias1.SelectedDia);

            string a = DDLDestino.SelectedValue.ToString();
            string c = DDLCompania.SelectedValue.ToString();
            List<Viajes> resultado = (from v in viajes
                                      where v.t.codigo.Contains(a) && v.c.nombre.Contains(c) &&
                                      Convert.ToDateTime(v.partida.ToString("yyyy/MM/dd")) == fecha
                                      select v).ToList();

            RepeaterCViajes.DataSource = resultado;
            RepeaterCViajes.DataBind();
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}