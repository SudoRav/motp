using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MOTP
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();

            if(false)
            {
                new MainWindow().Show();
                this.Close();
            }
        }

        private string Hash()
        {
            try
            {
                string str = $"{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}" + ")(w@)?f:RJM#pyFoRh0?n.5oYUsq7LN}#M*1YL*i";

                byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(new UTF8Encoding().GetBytes(str));
                string encoded = BitConverter.ToString(hash);
                string[] encodedspl = encoded.Split('-');
                string result = $"{encodedspl[2]}{encodedspl[4]}{encodedspl[encodedspl.Length - 5]}{encodedspl[encodedspl.Length - 3]}";

                return result;
            }
            catch { return null; }
        }

        private void TB_Pas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TB_Pas.Text == Hash())
                {
                    new MainWindow().Show();
                    this.Close();
                }
                else
                {
                    TB_Pas.Text = "";
                    System.Media.SystemSounds.Hand.Play();
                    //Close();
                }
            }
        }
    }
}
