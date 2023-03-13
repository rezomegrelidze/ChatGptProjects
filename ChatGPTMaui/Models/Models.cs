namespace ChatGPTMaui.Models;

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