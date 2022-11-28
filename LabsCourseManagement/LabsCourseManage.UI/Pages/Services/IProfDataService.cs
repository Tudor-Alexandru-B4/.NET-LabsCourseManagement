using LabsCourseManagement.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabsCourseManage.UI.Pages.Services
{
    public interface IProfDataService
    {
        Task<IEnumerable<Professor>> GetAllProfessors();
        Task<Professor> GetProfessorDetail(Guid professorId);
    }
}
