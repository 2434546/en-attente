namespace ForumDeDiscussion.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public string UserId { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();

        public Subject()
        {
        }
    }
}
