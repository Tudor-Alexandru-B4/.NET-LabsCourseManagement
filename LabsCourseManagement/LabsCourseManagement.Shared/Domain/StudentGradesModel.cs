namespace LabsCourseManagement.Shared.Domain
{
    public class StudentGradesModel
    {
        public Guid Id { get; set; }
        public StudentModel? Student { get; set; }
        public List<GradeModel>? Grades { get; set; }
        public GradeModel? FinalGrade { get; set; } = null;

    }
}
