using System.Text.Json;
using ChatGPTMaui.Models;


namespace ChatGPTMaui
{
    public partial class MainPage : ContentPage
    {
        private const string API_KEY = "YOUR_API_KEY";
        private const string MODEL_ID = "gpt-3.5-turbo";
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }
        

        
        private readonly List<Message> messagesSoFar = new();

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            // Create an HTTP client with the OpenAI API key
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", API_KEY);

            // Set up the request body with the user's prompt and other parameters
            messagesSoFar.Add(new() {content = txtPrompt.Text, role = "user"});
            var requestBody = new
            {
                model = MODEL_ID,
                messages = messagesSoFar
            };
            var requestBodyJson = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");

            // Send the request to the OpenAI API and parse the response
            activityIndicator.IsRunning = true;
            try
            {
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<CompletionResponse>(responseJson);

                // Display the generated text in the text block
                var responseChoice = responseObject?.choices.FirstOrDefault();
                var responseMessage = responseChoice?.message;

                txtGeneratedText.Text = responseMessage?.content ?? "";

                if (responseMessage != null)
                    messagesSoFar.Add(responseMessage);
                else messagesSoFar.Clear();
            }
            catch
            {

            }

            activityIndicator.IsRunning = false;
        }
    }
}