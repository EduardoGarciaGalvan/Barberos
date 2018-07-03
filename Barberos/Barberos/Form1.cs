using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Barberos
{
    public partial class Form1 : Form
    {
        Thread[] Barberas = new Thread[5];
        
        private int barberos = 0, tiempo, Clientes_Esperando=0;
        private PictureBox[] PicBarbero;
        private PictureBox[] picCliente_Esperando;
        private PictureBox[] picCliente_Satisfecho;

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 5; i++)
            {
                Barberas[i] = new Thread(new ParameterizedThreadStart(stBarbera_Cortando));
            }
            PicBarbero = new PictureBox[]
            {
                picBarbero1,
                picBarbero2,
                picBarbero3,
                picBarbero4,
                picBarbero5
            };
            picCliente_Esperando = new PictureBox[]
            {
                picCliente_Esperando1,
                picCliente_Esperando2,
                picCliente_Esperando3,
                picCliente_Esperando4,
                picCliente_Esperando5
            };
            picCliente_Satisfecho = new PictureBox[]
            {
                picCliente_Satisfecho1,
                picCliente_Satisfecho2,
                picCliente_Satisfecho3,
                picCliente_Satisfecho4,
                picCliente_Satisfecho5
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Clientes.Text) < 5)
            {
                picCliente_Esperando[Convert.ToInt32(Clientes.Text)].Image = Properties.Resources.Cliente;
            }
            Clientes.Text = (Convert.ToInt32(Clientes.Text) + 1).ToString();
            Clientes_Esperando++;
            var v = new { form = this, index = barberos };
            Barberas[barberos].Start(v);
        }

        private void Tiempo_Scroll(object sender, EventArgs e)
        {
            tiempo = Tiempo.Value * 1000;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value>barberos)
            {
                while(barberos<numericUpDown1.Value)
                {
                    var v = new { form = this, index = barberos };
                    Barberas[barberos].Start(v);
                    PicBarbero[barberos].Image = Properties.Resources.Barbera;
                    barberos++;
                }
            }
            else if (numericUpDown1.Value < barberos)
            {
                while(barberos > numericUpDown1.Value)
                {
                    Barberas[barberos].Interrupt();
                    barberos--;
                    PicBarbero[barberos].Image = null;
                }
            }
        }

        //var v = new { form = this, index = barberos }
        //thread.Start(v);

        static void stBarbera_Cortando(object anon)
        {
            var a = new { form = (Form1)null, index = 0 };
            a = Cast(a, anon);
            a.form.Barbera_Cortando(a.index);
        }

        private static T Cast<T>(T typeHolder, Object x)
        {
            // typeHolder above is just for compiler magic
            // to infer the type to cast x to
            return (T)x;
        }

        void Barbera_Cortando(int index)
        {
            while (Clientes_Esperando>0)
            {
                Thread.Sleep(1000);
                if (Clientes_Esperando < 5) picCliente_Esperando[Clientes_Esperando].Image = Properties.Resources.Silla_de_espera;
                Clientes_Esperando--;
                PicBarbero[barberos].Image = Properties.Resources.Barbera_cortando;
                Thread.Sleep(tiempo);
                PicBarbero[barberos].Image = Properties.Resources.Barbera;
                picCliente_Satisfecho[barberos].Image = Properties.Resources.Cliente_Satisfecho;
                Thread.Sleep(1000);
                picCliente_Satisfecho[barberos].Image = null;
            }
        }
    }
}
