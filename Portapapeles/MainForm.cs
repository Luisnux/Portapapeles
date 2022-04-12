namespace Portapapeles
{
    public partial class MainForm : Form
    {

        SettingsForm opciones;  //Ventana de opciones.

        //configuracion.
        static int maximo;
        static double nTransparencia = 50;
        static int tTransparencia;
        static int tiempo = 10;

        static bool bHerramientas = false;
        static bool desvanecer = true;

        static bool cPortapapeles = true;
        static bool cAplicacion = true;

        //variables
        String actual = "";
        String[] elementos;


        public MainForm()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_NOACTIVATE = 0x08000000;
                CreateParams param = base.CreateParams;
                param.ExStyle |= WS_EX_NOACTIVATE;
                return param;
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            configuracion();

            //Ajustar Ventana.
            this.Location = new Point(Properties.Settings.Default.X, Properties.Settings.Default.Y);
            this.ShowInTaskbar = bHerramientas;

            maximo = Properties.Settings.Default.maximo;
            elementos = new String[maximo];

            //Cargar los elementos a la lista.
            String linea;

            try
            {
                if (File.Exists("lista.txt"))
                {
                    StreamReader lector = new StreamReader("lista.txt");
                    linea = lector.ReadLine();
                    int contador = 0;

                    while (linea != null && contador < maximo)
                    {
                        if (linea.Equals("ENDEND"))
                            contador++;
                        else
                            elementos[contador] += linea;

                        linea = lector.ReadLine();
                    }
                    lector.Close();
                    actual = elementos[0];

                    if (actual == null)
                        actual = "";

                    agregar();
                    textBox1.Text = actual;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message.ToString(), "Error al cargar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void configuracion()
        {
            try
            {
                this.TopMost = Properties.Settings.Default.visible;
                nTransparencia = Properties.Settings.Default.nTransparencia;
                tTransparencia = Properties.Settings.Default.tTransparencia;
                tiempo = Properties.Settings.Default.tiempo;
                bHerramientas = Properties.Settings.Default.bHerramientas;
                desvanecer = Properties.Settings.Default.desvanecer;

                cPortapapeles = Properties.Settings.Default.cPortapapeles;
                cAplicacion = Properties.Settings.Default.cAplicacion;

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message.ToString(), "Error de configuracion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            iniciarTransparencia();
        }

        private void agregar() //agregar la lista de elementos a la listbox.
        {
            listBox1.Items.Clear();
            for (int i = 0; i < maximo; i++)
            {
                if (elementos[i] != null)
                    listBox1.Items.Add(elementos[i]);
            }
        }

        private void iniciarTransparencia()
        {
            switch (tTransparencia)
            {
                case 1: //ninguna
                    this.Opacity = 1;
                    timer_transparencia.Enabled = false;
                    Transparencia_gradual.Enabled = false;
                    break;

                case 2: //Fija
                    this.Opacity = nTransparencia / 100;
                    timer_transparencia.Enabled = false;
                    Transparencia_gradual.Enabled = false;
                    break;

                case 3: //gradual
                    Transparencia_gradual.Enabled = false;
                    this.Opacity = 1;
                    timer_transparencia.Enabled = false;
                    timer_transparencia.Interval = tiempo * 1000;
                    timer_transparencia.Enabled = true;
                    break;
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StreamWriter archivo = new StreamWriter("lista.txt");

                for (int i = 0; i < elementos.Length; i++)
                {
                    if (elementos[i] != null && elementos[i] != "")
                    {
                        archivo.WriteLine(elementos[i]);
                        archivo.WriteLine("ENDEND");
                    }
                }
                archivo.Close();

                //Actualizar la configuracion con el ultimo punto de posicion.
                Properties.Settings.Default.X = this.Location.X;
                Properties.Settings.Default.Y = this.Location.Y;

                Properties.Settings.Default.Save();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message.ToString(), "Error guardando elementos de la lista.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            iniciarTransparencia();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Minimized)
            {
                if (!bHerramientas)
                {
                    this.Visible = false;
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(2000);
                }

            }

            if (this.WindowState == FormWindowState.Normal)
            {
                this.Visible = true;
                notifyIcon1.Visible = false;

            }

        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            iniciarTransparencia();
        }

        private void Transparencia_gradual_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > (nTransparencia / 100))
                this.Opacity -= .01;
            else
                Transparencia_gradual.Enabled = false;
        }

        private void timer_transparencia_Tick(object sender, EventArgs e)
        {
            timer_transparencia.Enabled = false;

            if (desvanecer)
                Transparencia_gradual.Enabled = true;
            else
                this.Opacity = nTransparencia / 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            opciones = new SettingsForm();

            opciones.ShowDialog(); //Desplegar ventana de configuracion.
            //Esperando.

            if (opciones.cargar)//si la salida fue aceptar. recargar la configuracion.
                configuracion();

            this.TopMost = Properties.Settings.Default.visible;
            this.ShowInTaskbar = bHerramientas;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                String temp = temp = textBox1.Text = actual = listBox1.SelectedItem.ToString();

                for (int i = listBox1.SelectedIndex; i > 0; i--)
                    elementos[i] = elementos[i - 1];

                elementos[0] = temp;
                agregar();
                listBox1.SetSelected(0, false);

                try
                {
                    if (cPortapapeles)
                    {
                        Clipboard.SetText(actual);
                        if (cAplicacion)
                        {
                            SendKeys.Send("+{INS}");
                        }
                    }

                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Error al pegar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            iniciarTransparencia();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (Clipboard.ContainsText())
            {
                if (!actual.Equals(Clipboard.GetText()))
                {
                    for (int i = (maximo - 1); i > 0; i--)
                    {
                        elementos[i] = elementos[i - 1];
                    }
                    actual = elementos[0] = textBox1.Text = Clipboard.GetText();
                    agregar();
                }
            }
        }
    }
}