namespace Identity.Models
{
    public class ProjectAndCad
    {
        public Project project { get; set; }
        public IEnumerable<CADFile> file { get; set; }
        public int Id { get; set; }

        public ProjectAndCad(Project _p, IEnumerable<CADFile> _f, int id)
        {
            project = _p;
            file = _f;
            Id = id;
        }
    }
}
