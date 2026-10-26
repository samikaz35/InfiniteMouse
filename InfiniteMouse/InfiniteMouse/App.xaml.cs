using InfiniteMouse.Model;
using System.IO;
using System.Text.Json;
using System.Windows;
using Forms = System.Windows.Forms;
using System.Net.Http;
using System.Reflection;

namespace InfiniteMouse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
      


        const string FICHIERPARAM = @"\InfiniteMouse\Parametre.json";

        public bool FermetureVolontaire { get; set; } = false;
        public InfiniteMouseService InfiniteMouse { get; } = new();
        public Forms.NotifyIcon NotificationIcon { get; set; } = new Forms.NotifyIcon();
        private Forms.ToolStripMenuItem _toggleItem = null!;
        protected override async void OnStartup(StartupEventArgs e)
        {
            await VerifierMiseAJour();

            //MainWindow =  new MainWindow();
            //MainWindow.Show();
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\InfiniteMouse");

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + FICHIERPARAM))
            {
                StreamReader Lecture = null!;
                string sJson = "";

                Lecture = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + FICHIERPARAM);
                sJson = Lecture.ReadToEnd();
                Lecture.Close();
                InfiniteMouse.Parametre = JsonSerializer.Deserialize<Parametre>(sJson)!;
            }


            _toggleItem = new Forms.ToolStripMenuItem("Activé", global::InfiniteMouse.Properties.Resources.Activer, OnToggleClicked);

            NotificationIcon.Icon = global::InfiniteMouse.Properties.Resources.InfiniteMouse;
            NotificationIcon.Text = "InfiniteMouse - Désactivé";
            NotificationIcon.MouseClick += NotifyIcon_MouseClick;
            NotificationIcon.ContextMenuStrip = new Forms.ContextMenuStrip();
            NotificationIcon.ContextMenuStrip.Items.Add("Ouvrir", global::InfiniteMouse.Properties.Resources.Ouvrir, OnOuvrirClicked);
            NotificationIcon.ContextMenuStrip.Items.Add(_toggleItem);
            NotificationIcon.ContextMenuStrip.Items.Add("Reload", global::InfiniteMouse.Properties.Resources.ReloadNoir, OnReloadClicked);
            NotificationIcon.ContextMenuStrip.Items.Add("Quitter", global::InfiniteMouse.Properties.Resources.PowerButton, OnQuitterClicked);
            NotificationIcon.Visible = true;
            InfiniteMouse.Initialiser();

            base.OnStartup(e);
        }

        private async Task VerifierMiseAJour()
        {
            try
            {
                HttpClient client = new();

                client.DefaultRequestHeaders.Add(
                    "User-Agent",
                    "InfiniteMouse");

                string json =
                    await client.GetStringAsync(
                        "https://api.github.com/repos/samikaz35/InfiniteMouse/releases/latest");

                JsonDocument doc =
                    JsonDocument.Parse(json);


                string versionGithub =
                    doc.RootElement
                        .GetProperty("tag_name")
                        .GetString()!
                        .TrimStart('V', 'v');

                string versionLocaleString =
                    Assembly.GetExecutingAssembly()
                        .GetName()
                        .Version!
                        .ToString();

                Version versionLocale =
                    new(versionLocaleString);

                Version versionDistante =
                    new(versionGithub);

                Forms.MessageBox.Show($"Version local : {versionLocale} \n\rVersion Distance : {versionDistante}");

                if (versionDistante > versionLocale)
                {
                    Forms.MessageBox.Show(
                        $"Une nouvelle version ({versionGithub}) est disponible.",
                        "InfiniteMouse");
                }
            }
            catch
            {
                // Ignore en cas d'erreur internet
            }
        }
        private void OnReloadClicked(object? sender, EventArgs e)
        {

            InfiniteMouse.Reload();
           
        }

        private void OnToggleClicked(object? sender, EventArgs e)
        {

            ChangerEtat();



        }


        private void OnOuvrirClicked(object? sender, EventArgs e)
        {
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
            MainWindow.Show();
        }

        private void OnQuitterClicked(object? sender, EventArgs e)
        {


            FermetureVolontaire = true;
            NotificationIcon.Dispose();
            Shutdown();
        }

        private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MainWindow.WindowState = WindowState.Normal;
                MainWindow.Activate();
            }
        }
        public void EnregistrerParam()
        {
            string sJson = "";
            StreamWriter Ecrire = null!;

            sJson = JsonSerializer.Serialize(InfiniteMouse.Parametre);
            Ecrire = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + FICHIERPARAM);
            Ecrire.WriteLine(sJson);
            Ecrire.Close();
        }

        protected override void OnExit(ExitEventArgs e)
        {

            NotificationIcon.Dispose();
            base.OnExit(e);
        }

        public void ChangerEtat()
        {
            if (InfiniteMouse.Actif)
            {
                InfiniteMouse.Desactiver();

                NotificationIcon.Text = "InfiniteMouse - Désactivé";
                _toggleItem.Text = "Activer";
                _toggleItem.Image = global::InfiniteMouse.Properties.Resources.Activer;
            }
            else
            {
                InfiniteMouse.Activer();

                NotificationIcon.Text = "InfiniteMouse - Activé";
                _toggleItem.Text = "Désactiver";
                _toggleItem.Image = global::InfiniteMouse.Properties.Resources.Stop;

            }
        }
    }

}
