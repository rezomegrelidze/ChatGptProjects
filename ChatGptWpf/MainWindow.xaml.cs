using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatGptWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string API_KEY = "YOUR_API_KEY";
        private const string MODEL_ID = "gpt-3.5-turbo";

        public MainWindow()
        {
            InitializeComponent();
        }

        private List<Message> messagesSoFar = new();

        private async void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create an HTTP client with the OpenAI API key
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", API_KEY);

                // Set up the request body with the user's prompt and other parameters
                messagesSoFar.Add(new(){content = txtPrompt.Text,role = "user"});
                var requestBody = new
                {
                    model = MODEL_ID,
                    messages = messagesSoFar
                };
                var requestBodyJson = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");

                // Send the request to the OpenAI API and parse the response
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<CompletionResponse>(responseJson);

                // Display the generated text in the text block
                var responseChoice = responseObject?.choices.FirstOrDefault();
                var responseMessage = responseChoice?.message;

                txtGeneratedText.Text = responseMessage?.content ?? "";
                if(responseMessage != null)
                    messagesSoFar.Add(responseMessage);
            }
            catch (Exception ex)
            {
                // Display an error message if something goes wrong
                MessageBox.Show("Error generating text: " + ex.Message);
            }
        }

        
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class Request
    {
        public string model { get; set; }
        public List<Message> Messages { get; set; }
    }
    
    public class CompletionResponse
    {
        public Choice[] choices { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
        public string? finish_reason { get; set; }
    }
}
