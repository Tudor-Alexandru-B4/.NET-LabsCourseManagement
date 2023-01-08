namespace LabsCourseManagement.Shared.Domain
{
    public class CourseModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public List<ProfessorModel>? Professors { get; set; }
        public List<StudentModel>? Students { get; set; }
        public List<AnnouncementModel>? CourseAnnouncements { get; set;} = new List<AnnouncementModel>();
        public CatalogModel? CourseCatalog { get; set; }
        public List<LaboratoryModel>? Laboratories { get; set; }
        public List<TimeAndPlaceModel>? CourseProgram { get; set; }
        public List<GradingInfoModel>? CourseGradingInfo { get; set; }
        public List<InformationStringModel>? HelpfulMaterials { get; set; }
    }
}
