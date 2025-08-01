

namespace Shared.Messages
{
    public class StudentCreated
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
    }

    public class CourseCreated
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
