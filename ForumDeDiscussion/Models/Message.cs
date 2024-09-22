using System.ComponentModel.DataAnnotations;

namespace ForumDeDiscussion.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public DateTime Date { get; set; }

        public Message()
        {
        }
    }
}
