using Newtonsoft.Json;

namespace KhumbulaProcessUploader
{
    public partial class Form1 : Form
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string _baseUrl = "https://us-central1-khumbula.cloudfunctions.net";

        private string _processFile;

        public Form1()
        {
            InitializeComponent();
        }

        void ClearMessage()
        {
            lblError.Text = "";
        }

        void ShowMessage(string message)
        {
            lblError.Text = message;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            ShowMessage("Busy...");

            try
            {
                var code = tbxCode.Text;

                if (String.IsNullOrWhiteSpace(code))
                {
                    ShowMessage("Code is required");
                    return;
                }

                var response = await _client.GetAsync($"{_baseUrl}/process/{code}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Could not download process ({response.StatusCode})");
                }

                var sfd = new SaveFileDialog();
                sfd.Filter = "Process File | *.json";
                var saveResponse = sfd.ShowDialog();
                if (saveResponse != DialogResult.OK)
                {
                    return;
                }

                File.WriteAllText(sfd.FileName, await response.Content.ReadAsStringAsync());
                ShowMessage("Process downloaded");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Process File | *.json";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _processFile = ofd.FileName;
            lblPath.Text = _processFile;
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            ShowMessage("Busy...");

            try
            {
                var code = tbxCode.Text;
                var secret = tbxSecret.Text;

                if (String.IsNullOrWhiteSpace(code))
                {
                    throw new Exception("Code is required");
                }

                if (String.IsNullOrWhiteSpace(secret))
                {
                    throw new Exception("Secret is required");
                }

                if (String.IsNullOrWhiteSpace(_processFile))
                {
                    throw new Exception("Process file is required");
                }

                var process = JsonConvert.DeserializeObject(File.ReadAllText(_processFile));

                var request = new Dictionary<string, object?>()
                {
                    { "secret", secret },
                    { "process", process }
                };

                var body = new StringContent(
                    JsonConvert.SerializeObject(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PutAsync($"{_baseUrl}/process/{code}", body);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Could not create process ({response.StatusCode})");
                }

                ShowMessage("Process created successfully");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);

            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            ShowMessage("Busy...");

            try
            {
                var code = tbxCode.Text;
                var secret = tbxSecret.Text;

                if (String.IsNullOrWhiteSpace(code))
                {
                    throw new Exception("Code is required");
                }

                if (String.IsNullOrWhiteSpace(secret))
                {
                    throw new Exception("Secret is required");
                }

                if (String.IsNullOrWhiteSpace(_processFile))
                {
                    throw new Exception("Process file is required");
                }

                var process = JsonConvert.DeserializeObject(File.ReadAllText(_processFile));

                var request = new Dictionary<string, object?>()
                {
                    { "secret", secret },
                    { "process", process }
                };

                var body = new StringContent(
                    JsonConvert.SerializeObject(request),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _client.PostAsync($"{_baseUrl}/process/{code}", body);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Could not update process ({response.StatusCode})");
                }

                ShowMessage("Process updated successfully");
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);

            }
        }
    }
}