namespace ForumDeDiscussion.ViewModels
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}  