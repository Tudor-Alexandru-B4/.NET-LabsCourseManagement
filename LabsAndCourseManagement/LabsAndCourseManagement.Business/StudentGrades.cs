namespace LabsAndCourseManagement.Business
{
    public class StudentGrades
    {
        public Guid Id { get; private set; }
        public Student Student { get; private set; }
        public List<Grade> Grades { get; private set; }
        public Grade FinalGrade { get; private set; }
    }
}
