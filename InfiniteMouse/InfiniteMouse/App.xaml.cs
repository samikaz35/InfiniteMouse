using InfiniteMouse.Model;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using Forms = System.Windows.Forms;

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
            HttpClient client;
            string json;
            JsonDocument doc;
            string versionGithub;
            string versionLocaleString;
            Version versionLocale;
            Version versionDistante;
            try
            {
                client = new();

                client.DefaultRequestHeaders.Add("User-Agent","InfiniteMouse");

                json = await client.GetStringAsync("https://api.github.com/repos/samikaz35/InfiniteMouse/releases/latest");
                doc = JsonDocument.Parse(json);

                versionGithub = doc.RootElement.GetProperty("tag_name").GetString()!.TrimStart('V', 'v');
                versionLocaleString = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
                versionLocale = new(versionLocaleString);
                versionDistante = new(versionGithub);


                if (versionDistante > versionLocale)
                {
                    if (
                    Forms.MessageBox.Show(
                    $"Une nouvelle version ({versionGithub}) est disponible sur GitHub. \n\rVoulez-vous le télécharger ?",
                    $"InfiniteMouse - {versionGithub}", MessageBoxButtons.YesNo)
                        == DialogResult.Yes)
                    {
                        JsonElement assets = doc.RootElement.GetProperty("assets");
                        if (assets.GetArrayLength() == 0)
                        {
                            Forms.MessageBox.Show("Une erreur c'est produite, le téléchargement est donc impossible.","Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();
                        }

                        JsonElement premierAsset = assets[0];

                        if (!premierAsset.TryGetProperty("browser_download_url", out JsonElement url))
                        {
                            Forms.MessageBox.Show("Une erreur c'est produite, le téléchargement est donc impossible.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new Exception();
                        }

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url.ToString(),
                            UseShellExecute = true
                        });
                    }
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
