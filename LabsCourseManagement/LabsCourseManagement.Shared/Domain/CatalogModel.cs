namespace LabsCourseManagement.Shared.Domain
{
    public class CatalogModel
    {
        public Guid Id { get; set; }
        public List<StudentGradesModel>? StudentGrades { get; set; }
    }
}
