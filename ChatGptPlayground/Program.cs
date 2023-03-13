using System.Text.Json;

var apiKey = "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";

var question = "What is the meaning of life?";
var model = "text-davinci-002"; // replace with your desired model

var client = new HttpClient();
client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

var requestBody = new
{
    model = model,
    prompt = question,
    max_tokens = 10000,
    temperature = 0.5
};

var requestUrl = "https://api.openai.com/v1/completions";
var requestBodyJson = JsonSerializer.Serialize(requestBody);
var content = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");

var response = await client.PostAsync(requestUrl, content);

var responseJson = await response.Content.ReadAsStringAsync();
var responseObject = JsonSerializer.Deserialize<CompletionResponse>(responseJson);

Console.WriteLine(responseObject.choices[0].text);

class CompletionResponse
{
    public Choice[] choices { get; set; }
}

class Choice
{
    public string text { get; set; }
    public string logprobs { get; set; }
    public string finish_reason { get; set; }
}