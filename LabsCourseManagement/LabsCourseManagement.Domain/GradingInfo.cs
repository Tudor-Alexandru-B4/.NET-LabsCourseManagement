using LabsCourseManagement.Domain.Helpers;
using System.Xml.Linq;

namespace LabsCourseManagement.Domain
{
    public class GradingInfo
    {
        public Guid Id { get; private set; }
        public ExaminationType ExaminationType { get; private set; }
        public bool IsMandatory { get; private set; }
        public double MinGrade { get; private set; }
        public double MaxGrade { get; private set; }
        public string? Description { get; private set; }
        public TimeAndPlace? TimeAndPlace { get; private set; }
        public static Result<GradingInfo> Create(ExaminationType type, double minGrade, double maxGrade, bool isMandatory, string Description, TimeAndPlace timeAndPlace)
        {
            if ( minGrade <= 0 || maxGrade <= 0 || Description==null)
            {
                return Result<GradingInfo>.Failure("Min grade / max grade cannot be under 0 or description cannot be null");
            }
            var gradingInfo = new GradingInfo
            {
                Id=Guid.NewGuid(),
                ExaminationType = type,
                MinGrade = minGrade,
                MaxGrade = maxGrade,
                Description = Description,
                IsMandatory = isMandatory,
                TimeAndPlace = timeAndPlace,
            };
            return Result<GradingInfo>.Success(gradingInfo);

        }
    }
}