using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Portapapeles
{
    public partial class SettingsForm : Form
    {

        public bool cargar = false;
        private int m;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = m = Properties.Settings.Default.maximo;
            checkBox1.Checked = Properties.Settings.Default.visible;
            checkBox2.Checked = Properties.Settings.Default.bHerramientas;

            checkBox3.Checked = Properties.Settings.Default.cPortapapeles;
            checkBox4.Checked = Properties.Settings.Default.cAplicacion;

            switch (Properties.Settings.Default.tTransparencia)
            {
                case 1:
                    radioButton1.Select();
                    break;
                case 2:
                    radioButton2.Select();
                    break;
                case 3:
                    radioButton3.Select();
                    break;
            }

            trackBar1.Value = (int)Properties.Settings.Default.nTransparencia;
            label6.Text = trackBar1.Value.ToString() + "%";

            numericUpDown2.Value = Properties.Settings.Default.tiempo;
            checkBox5.Checked = Properties.Settings.Default.desvanecer;

            checkBox4.Enabled = checkBox3.Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label6.Text = trackBar1.Value.ToString() + "%";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.maximo = (int)numericUpDown1.Value;
            Properties.Settings.Default.visible = checkBox1.Checked;
            Properties.Settings.Default.bHerramientas = checkBox2.Checked;
            Properties.Settings.Default.cPortapapeles = checkBox3.Checked;
            Properties.Settings.Default.cAplicacion = checkBox4.Checked;

            if (radioButton1.Checked)
                Properties.Settings.Default.tTransparencia = 1;
            if (radioButton2.Checked)
                Properties.Settings.Default.tTransparencia = 2;
            if (radioButton3.Checked)
                Properties.Settings.Default.tTransparencia = 3;

            Properties.Settings.Default.nTransparencia = trackBar1.Value;
            Properties.Settings.Default.tiempo = (int)numericUpDown2.Value;
            Properties.Settings.Default.desvanecer = checkBox5.Checked;

            Properties.Settings.Default.Save();

            cargar = true;
            if (m != numericUpDown1.Value)
                MessageBox.Show("El cambio del numero maximo de elementps solo tendra efecto \nhasta que se reinicie el programa.",
                    "Error al pegar", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                trackBar1.Enabled = false;
                numericUpDown2.Enabled = false;
                checkBox5.Enabled = false;
            }
            else
                trackBar1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                trackBar1.Enabled = true;
                numericUpDown2.Enabled = false;
                checkBox5.Enabled = false;
            }
            else
                trackBar1.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                trackBar1.Enabled = true;
                numericUpDown2.Enabled = true;
                checkBox5.Enabled = true;
            }
            else
            {
                trackBar1.Enabled = false;
                numericUpDown2.Enabled = false;
                checkBox5.Enabled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox4.Enabled = checkBox3.Enabled;
        }
    }
}
