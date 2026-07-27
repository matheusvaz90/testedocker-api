namespace TodoApi.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Done { get; set; }
    public string Urgency { get; set; } = "Baixa";
    public int UserId { get; set; }
}
