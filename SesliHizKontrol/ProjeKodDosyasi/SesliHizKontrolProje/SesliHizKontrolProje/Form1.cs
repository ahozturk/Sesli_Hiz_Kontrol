using System;
using System.Windows.Forms;
using System.IO.Ports;

namespace SesliHizKontrolProje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] ports = SerialPort.GetPortNames();
        PictureBox[] ibre;
        PictureBox[] ibreRpm;
        int AcikIbre = 0;
        int AcikIbreRpm = 0;
        string sesYer;
        sesKonumu ses = new sesKonumu();
        private void Form1_Load(object sender, EventArgs e)
        {
            sesYer = ses.konum;
            ibre = new PictureBox[7] { pictureBox1, pictureBox6, pictureBox3, pictureBox8, pictureBox4, pictureBox7, pictureBox5};
            ibreRpm = new PictureBox[7] { pictureBox15, pictureBox10, pictureBox14, pictureBox11, pictureBox12, pictureBox9, pictureBox13 };
            foreach (string port in ports)
            {
                comboBox2.Items.Add(port);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.BaudRate = 9600;
                serialPort1.PortName = comboBox2.Text;
                serialPort1.Open();
                groupBox1.Visible = false;
                button2.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Porta Bağlanılamadı");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        string data;

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data = serialPort1.ReadLine().ToString();
            this.Invoke(new EventHandler(VeriGelince));
        }
        float hiz;
        float rpm;

        bool hizli = true;
        void VeriGelince(object sender, EventArgs e)
        {
            try
            {
                hiz = Convert.ToInt32(float.Parse(data.Remove(0, 4)) / 10);
                rpm = Convert.ToInt32(float.Parse(data.Remove(0, 4)) * 10);
                label10.Text = hiz + " km/h";
                label11.Text = rpm + " rpm";
            }
            catch(Exception ex)
            {
            }

            if (data.Remove(3).ToString().Replace(" ", "") == "KMH")
            {
                hiz = hiz / 10;
                if (hiz > 5.8f)
                {
                    IbreOynat(6);
                }
                else if (hiz > 5)
                {
                    IbreOynat(5);
                    if (hizli)
                    {
                        hizli = false;
                        if (checkBox1.Checked)
                        {
                            axWindowsMediaPlayer1.URL = sesYer;
                            axWindowsMediaPlayer1.Ctlcontrols.play();
                        }
                    }
                }
                else if (hiz > 4)
                {
                    IbreOynat(4);
                }
                else if (hiz > 3)
                {
                    IbreOynat(3);
                    hizli = true;

                }
                else if (hiz > 2)
                {
                    IbreOynat(2);
                    hizli = true;

                }
                else if (hiz > 0)
                {
                    IbreOynat(1);
                    hizli = true;

                }
                else if (hiz == 0)
                {
                    hizli = true;

                    IbreOynat(0);
                }
            }
            else if(data.Remove(3).ToString().Replace(" ", "") == "RPM")
            {
                rpm = rpm / 50;
                
                if (rpm > 6000)
                {
                    IbreOynatRpm(6);
                }
                else if (rpm > 5000)
                {
                    IbreOynatRpm(5);
                }
                else if (rpm > 4000)
                {
                    IbreOynatRpm(4);
                }
                else if (rpm > 3000)
                {
                    IbreOynatRpm(3);
                }
                else if (rpm > 2000)
                {
                    IbreOynatRpm(2);
                }
                else if (rpm > 0)
                {
                    IbreOynatRpm(1);
                }
                else if (rpm == 0)
                {
                    IbreOynatRpm(0);
                }
            }
        }

        void IbreOynat(int sayi)
        {
            if(AcikIbre > sayi)
            {
                ibre[AcikIbre].Visible = false;
                AcikIbre--;
                ibre[AcikIbre].Visible = true;
                //System.Threading.Thread.Sleep(50);
            }
            else if(AcikIbre < sayi)
            {
                ibre[AcikIbre].Visible = false;
                AcikIbre++;
                ibre[AcikIbre].Visible = true;
            }
        }

        void IbreOynatRpm(int sayi)
        {
            if (AcikIbreRpm > sayi)
            {
                ibreRpm[AcikIbreRpm].Visible = false;
                AcikIbreRpm--;
                ibreRpm[AcikIbreRpm].Visible = true;
                //System.Threading.Thread.Sleep(50);
            }
            else if (AcikIbreRpm < sayi)
            {
                ibreRpm[AcikIbreRpm].Visible = false;
                AcikIbreRpm++;
                ibreRpm[AcikIbreRpm].Visible = true;
            }
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox2.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox2.Items.Add(port);
            }
        }
    }
}
 