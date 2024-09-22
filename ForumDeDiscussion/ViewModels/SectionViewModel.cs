namespace ForumDeDiscussion.ViewModels;

public class SectionViewModel
{
    public int SectionId { get; set; }
    public string SectionName { get; set; }
    public int TopicCount { get; set; }
    public int MessageCount { get; set; }
    public DateTime? LastMessageDate { get; set; }
    public int? LastMessageId { get; set; }
}