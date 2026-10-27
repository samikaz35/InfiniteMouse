using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;
using WPF = System.Windows;


namespace InfiniteMouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private App app = (App)WPF.Application.Current;

        public MainWindow()
        {
            InitializeComponent();

        }


        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            DessinerÉcran();

            string versionLocaleString =
                    Assembly.GetExecutingAssembly().GetName().Version!.ToString();

            lblVersion.Content = $"V.{versionLocaleString.Substring(0, versionLocaleString.Length - 2)}";

            if (app.InfiniteMouse.Parametre.DemarrageActiver == true)
            {
                ChangerEtat();
            }


            grdMain.DataContext = app.InfiniteMouse.Parametre;

        }
        /// <summary>
        /// Copilot ma générer sa car je savais vraiment pas comemnt m'y prendre
        /// </summary>
        private void DessinerÉcran()
        {
            ScreenCanvas.Children.Clear();

            List<Forms.Screen> screens = Forms.Screen.AllScreens.ToList();

            int minX = screens.Min(s => s.Bounds.Left);
            int minY = screens.Min(s => s.Bounds.Top);

            int maxX = screens.Max(s => s.Bounds.Right);
            int maxY = screens.Max(s => s.Bounds.Bottom);

            int totalWidth = maxX - minX;
            int totalHeight = maxY - minY;

            double canvasWidth = ScreenCanvas.Width;
            double canvasHeight = ScreenCanvas.Height;

            double scaleX = canvasWidth / totalWidth;
            double scaleY = canvasHeight / totalHeight;

            double scale = Math.Min(scaleX, scaleY) * 0.9;

            double dessinWidth = totalWidth * scale;
            double dessinHeight = totalHeight * scale;

            double offsetX = (canvasWidth - dessinWidth) / 2;
            double offsetY = (canvasHeight - dessinHeight) / 2;

            int index = 1;

            foreach (Forms.Screen screen in screens)
            {
                Border border = new()
                {
                    Width = screen.Bounds.Width * scale,
                    Height = screen.Bounds.Height * scale,
                    BorderBrush = WPF.Media.Brushes.White,
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(5),
                    Background = screen.Primary
                        ? new WPF.Media.SolidColorBrush(
                            WPF.Media.Color.FromRgb(0, 209, 255))
                        : new WPF.Media.SolidColorBrush(
                            WPF.Media.Color.FromRgb(45, 45, 45))
                };

                TextBlock text = new()
                {
                    Text = index.ToString(),
                    Foreground = WPF.Media.Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    HorizontalAlignment = WPF.HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center
                };

                Grid grid = new();
                grid.Children.Add(border);
                grid.Children.Add(text);

                Canvas.SetLeft(
                    grid,
                    offsetX + (screen.Bounds.Left - minX) * scale);

                Canvas.SetTop(
                    grid,
                    offsetY + (screen.Bounds.Top - minY) * scale);

                ScreenCanvas.Children.Add(grid);

                index++;
            }
        }
        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (/*!app.FermetureVolontaire ||*/ app.InfiniteMouse.Parametre.SystemeTray == true)
            {
                e.Cancel = true;
                Hide();
            }

        }

        private void btnChangerEtat_Click(object sender, RoutedEventArgs e)
        {
            ChangerEtat();
        }

        private void ChangerEtat()
        {

            ChangerEtatAffichage(app.InfiniteMouse.Actif);
            app.ChangerEtat();
        }

        private void frmMain_Closed(object sender, EventArgs e)
        {
            app.EnregistrerParam();
        }

        //private void chkWindows_Checked(object sender, RoutedEventArgs e)
        //{
        //    ConfigurerDemarrageWindows(app.InfiniteMouse.Parametre.DemarrageAvecWindows);
        //}


        //private void ConfigurerDemarrageWindows(bool Activer)
        //{
        //    RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey
        //        (RegistryKeyPath, true);

        //    if (registryKey == null || AppDirectory == null)
        //        throw new ArgumentException("Une erreur c'est produite");

        //    if (Activer)
        //    {
        //        registryKey.SetValue(AppName, AppDirectory);
        //    }
        //    else
        //    {
        //        registryKey.DeleteValue(AppName, false);
        //    }
        //}

        //readonly string? AppDirectory = Environment.ProcessPath;
        //readonly string RegistryKeyPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        //readonly string AppName = "InfiniteMouse";

        private void ChangerEtatAffichage(bool Actif)
        {
            if (Actif)
            {
                btnLED.NeonColor = WPF.Media.Colors.Red;
                btnLED.Background = WPF.Media.Brushes.Red;
                btnChangerEtat.Content = "Activer";
                lblEtat.Content = "Désactivé";
                btnLED.Neon = true;
            }
            else
            {
                btnLED.NeonColor = WPF.Media.Colors.Lime;
                btnLED.Background = WPF.Media.Brushes.Lime;
                btnChangerEtat.Content = "Désactiver";
                lblEtat.Content = "Activé";
                btnLED.Neon = true;

            }

        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            ChangerEtatAffichage(!app.InfiniteMouse.Actif);
            
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            app.InfiniteMouse.Reload();
            ChangerEtatAffichage(!app.InfiniteMouse.Actif);
            DessinerÉcran();
        }

        private void btnGitHub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/samikaz35/InfiniteMouse/releases",
                UseShellExecute = true
            });
        }
    }
}