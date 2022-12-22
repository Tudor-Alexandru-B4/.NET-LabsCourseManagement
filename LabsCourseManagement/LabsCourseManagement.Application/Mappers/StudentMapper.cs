using AutoMapper;

namespace LabsCourseManagement.Application.Mappers
{
    public static class StudentMapper
    {
        private static Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p =>
                {
                    if(p.GetMethod != null)
                    {
                        return p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                    }
                    return false;
                };
                
                cfg.AddProfile<StudentMappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }
}
