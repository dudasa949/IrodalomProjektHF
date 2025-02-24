using IrodalomProjekt.Modells;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IrodalomProjekt
{
    public partial class MainWindow : Window
    {
        private List<Kerdes> kerdesek = new();
        private int aktualisKerdesIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BetoltesClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    kerdesek.Clear();
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        while (!sr.EndOfStream)
                        {
                            string[] adatok = sr.ReadLine().Split(';');
                            kerdesek.Add(new Kerdes(adatok[0], adatok[1], adatok[2], adatok[3], adatok[4]));
                        }
                    }
                    MessageBox.Show("Kérdések sikeresen betöltve!");
                    aktualisKerdesIndex = 0;
                    KerdesMegjelenit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a fájl beolvasásakor: " + ex.Message);
                }
            }
        }



        private void KerdesMegjelenit()
        {
            if (kerdesek.Count == 0)
            {
                MessageBox.Show("Nincsenek betöltött kérdések!");
                return;
            }

            Kerdes kerdes = kerdesek[aktualisKerdesIndex];
            tbkKerdeszSzoveg.Text = kerdes.KerdesSzoveg;

            ValaszA.Content = kerdes.ValaszA;
            ValaszB.Content = kerdes.ValaszB;
            ValaszC.Content = kerdes.ValaszC;

            ValaszA.IsChecked = false;
            ValaszB.IsChecked = false;
            ValaszC.IsChecked = false;
        }

        private void ElozoClick(object sender, RoutedEventArgs e)
        {
            if (aktualisKerdesIndex > 0)
            {
                aktualisKerdesIndex--;
                KerdesMegjelenit();
            }
            else
            {
                MessageBox.Show("Nincs előző kérdés!");
            }
        }

        private void KovetkezoClick(object sender, RoutedEventArgs e)
        {
            if (aktualisKerdesIndex < kerdesek.Count - 1)
            {
                aktualisKerdesIndex++;
                KerdesMegjelenit();
            }
            else
            {
                MessageBox.Show("Nincs több kérdés!");
            }
        }
        private void KiertekelesClick(object sender, RoutedEventArgs e)
        {
            if (kerdesek.Count == 0)
            {
                MessageBox.Show("Nincsenek betöltött kérdések!");
                return;
            }

            int helyesValaszokSzama = 0;

            foreach (Kerdes kerdes in kerdesek)
            {
                if (kerdes.ValaszEllenorzes())
                {
                    helyesValaszokSzama++;
                }
            }

            MessageBox.Show($"Helyes válaszok száma: {helyesValaszokSzama} / {kerdesek.Count}");
        }

        private void KilepesClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MegerositesClick(object sender, RoutedEventArgs e)
        {
            if (kerdesek.Count == 0)
            {
                MessageBox.Show("Nincsenek betöltött kérdések!");
                return;
            }

            Kerdes kerdes = kerdesek[aktualisKerdesIndex];

            if (ValaszA.IsChecked == true)
            {
                kerdes.UserValasz = kerdes.ValaszA;
            }
            else if (ValaszB.IsChecked == true)
            {
                kerdes.UserValasz = kerdes.ValaszB;
            }
            else if (ValaszC.IsChecked == true)
            {
                kerdes.UserValasz = kerdes.ValaszC;
            }
            else
            {
                MessageBox.Show("Kérjük, válasszon egy választ!");
                return;
            }

            MessageBox.Show("Válasz rögzítve!");
        }
    }
}