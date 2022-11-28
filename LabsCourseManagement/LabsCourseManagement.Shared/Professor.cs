using LabsCourseManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabsCourseManagement.Shared
{
	public class Professor
	{
		public Guid Id { get;  set; }
		public string? Name { get;  set; }
		public string? Surname { get;  set; }
		public Contact? ContactInfo { get;  set; }
		public bool IsActive { get;  set; }
		public List<Course>? Courses { get;  set; }
		public List<Laboratory>? Laboratories { get;  set; }
	}
}
