using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Shared.Domain
{
    public class LaboratoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public Course Course { get; set; }
        public bool IsActive { get; set; }
        //public Catalog LaboratoryCatalog { get; set; }
        //public Professor LaboratoryProfessor { get; set; }
        //public List<StudentModel> LaboratoryStudents { get; set; }
        //public TimeAndPlace LaboratoryTimeAndPlace { get; set; }
        //public List<Announcement> LaboratoryAnnouncements { get; set; }
        //public List<GradingInfo> LaboratoryGradingInfo { get; set; }
    }
}
