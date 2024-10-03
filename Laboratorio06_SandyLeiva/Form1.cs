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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection cn;
        DataView dv;
        Form2 f2;

        void cargaAutomoviles()
        {
            try
            {
                using (cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    SqlDataAdapter da = new SqlDataAdapter("pa_listaAutomovil", cn);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "auto");
                    dv = new DataView(ds.Tables["auto"]);
                    dataGridView1.DataSource = dv;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        void buscar(string dato)
        {
            dv.RowFilter = "Placa_Auto like '%" + dato + "%' or Nom_Modelo like '%" + dato + "%'";
            dataGridView1.DataSource = dv;
        }



        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            buscar(txtBuscar.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargaAutomoviles();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            f2 = new Form2();
            f2.ShowDialog();
            cargaAutomoviles();
        }


        private int? retornaID()
        {
            try
            {
                return int.Parse(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }
        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = retornaID();
            if (id != null)
            {
                f2 = new Form2(id);
                f2.ShowDialog();
            }
            cargaAutomoviles();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = retornaID();
            if (id != null)
            {
                if (MessageBox.Show("¿Está seguro de eliminar el registro?", "Eliminar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (cn = new SqlConnection(Properties.Settings.Default.cnx))
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("pa_eliminaAutomovil", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("id", id);

                        cmd.ExecuteNonQuery();
                        cargaAutomoviles();
                        MessageBox.Show("Automovil Eliminado");
                    }
                }
            }
        }
    }
}
