namespace LabsCourseManagement.Shared.Domain
{
    public class GradingInfoModel
    {
        public Guid Id { get; set; }
        public ExaminationTypeModel ExaminationType { get; set; }
        public bool IsMandatory { get; set; }
        public double MinGrade { get; set; }
        public double MaxGrade { get; set; }
        public string? Description { get; set; }
        public TimeAndPlaceModel? TimeAndPlace { get; set; }
    }
}
