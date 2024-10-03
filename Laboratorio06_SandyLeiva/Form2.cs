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

namespace Laboratorio06_SandyLeiva
{
    public partial class Form2 : Form
    {
        public int? id;

        public Form2(int? id = null)
        {
            this.id = id;
            InitializeComponent();

        }

        SqlConnection cn;

        void cargaModelo()
        {
            try
            {
                using (cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    SqlDataAdapter da = new SqlDataAdapter("Select * from modelo", cn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "modelo");
                    cboModelo.DataSource = ds.Tables["modelo"];
                    cboModelo.DisplayMember = "Nom_Modelo";
                    cboModelo.ValueMember = "ModeloID";
                    cboModelo.SelectedIndex = -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        void cargaProp()
        {
            try
            {
                using (cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    SqlDataAdapter da = new SqlDataAdapter("Select * from PROPIETARIO", cn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "propietario");
                    cboPropietario.DataSource = ds.Tables["propietario"];
                    cboPropietario.DisplayMember = "Nom_Pro";
                    cboPropietario.ValueMember = "PropietarioID";
                    cboPropietario.SelectedIndex = -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }


        void grabar()
        {
            int añoSeleccionado = dtAño.Value.Year; // Obtén solo el año

            try
            {
                using (cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    cn.Open();

                    if (id == null)
                    {
                        SqlCommand cmd = new SqlCommand("pa_nuevoAutomovil", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("placa", txtPlaca.Text.ToUpper());
                        cmd.Parameters.AddWithValue("color", txtColor.Text.ToUpper());
                        cmd.Parameters.AddWithValue("anio", añoSeleccionado);
                        cmd.Parameters.AddWithValue("modeloid", (int)cboModelo.SelectedValue);
                        cmd.Parameters.AddWithValue("propietarioID", (int)cboPropietario.SelectedValue);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Automóvil Registrado");
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("pa_modificaAutomovil", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.Parameters.AddWithValue("placa", txtPlaca.Text.ToUpper());
                        cmd.Parameters.AddWithValue("color", txtColor.Text.ToUpper());
                        cmd.Parameters.AddWithValue("anio", añoSeleccionado);
                        cmd.Parameters.AddWithValue("modeloid", (int)cboModelo.SelectedValue);
                        cmd.Parameters.AddWithValue("propietarioID", (int)cboPropietario.SelectedValue);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Automovil Modificado");
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            cargaModelo();
            cargaProp();  
            if (id != null)
            {
                cargaDatos();
                return;
            }
            cboModelo.SelectedIndex = 0;
            cboPropietario.SelectedIndex = 0;
        }


        void cargaDatos()
        {
            try
            {
                using (cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("pa_buscaAutomovil", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id", id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "auto");
                    txtPlaca.Text = ds.Tables["auto"].Rows[0]["Placa_Auto"].ToString();
                    txtColor.Text = ds.Tables["auto"].Rows[0]["Color_Auto"].ToString();
                    dtAño.Value = Convert.ToDateTime(ds.Tables["auto"].Rows[0]["Año_Auto"].ToString());
                    cboModelo.Text = ds.Tables["auto"].Rows[0]["Nom_Modelo"].ToString();
                    cboPropietario.Text = ds.Tables["auto"].Rows[0]["Nom_Pro"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            grabar();

        }

        private void cboPropietario_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtAño_ValueChanged(object sender, EventArgs e)
        {
            int añoSeleccionado = dtAño.Value.Year;

        }
    }
}
