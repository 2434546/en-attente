using System.ComponentModel;

namespace ForumDeDiscussion.Areas.Admin.ViewModels
{
    public class SectionListViewModel
    {
        public int SectionId { get; set; }

        [DisplayName("Name")] public string Name { get; set; } = string.Empty;
    }
}
