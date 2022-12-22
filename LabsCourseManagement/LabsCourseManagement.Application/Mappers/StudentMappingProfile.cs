using AutoMapper;
using LabsCourseManagement.Application.Queries;
using LabsCourseManagement.Application.Response;
using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Mappers
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<Student, StudentResponse>().ReverseMap();
            CreateMap<Student, CreateStudentRequest>().ReverseMap();
        }
    }
}
