namespace LabsCourseManagement.Shared.Domain
{
    public class LaboratoryModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public CourseModel? Course { get; set; }
        public bool IsActive { get; set; }
        public CatalogModel? LaboratoryCatalog { get; set; }
        public ProfessorModel? LaboratoryProfessor { get; set; }
        public List<StudentModel>? LaboratoryStudents { get; set; }
        public TimeAndPlaceModel? LaboratoryTimeAndPlace { get; set; }
        public List<AnnouncementModel>? LaboratoryAnnouncements { get; set; }
        public List<GradingInfoModel>? LaboratoryGradingInfo { get; set; }
    }
}
