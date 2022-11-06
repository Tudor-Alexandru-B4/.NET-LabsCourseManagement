﻿namespace LabsAndCourseManagement.Business
{
    public class Laboratory
    {
        public Guid Id { get; private set; }
        public String Name { get; private set; }
        public Course Course { get; private set; }
        public Bool IsActive { get; private set; }
        public Catalog LaboratoryCatalog { get; private set; }
        public Professor LaboratoryProfessor { get; private set; }
        public List<Student> LaboratoryStudents { get; private set; }
        public TimeAndPlace LaboratoryTimeAndPlace { get; private set; }
        public List<Announcement> LaboratoryAnnouncements { get; private set; }
        public List<GradingInfo> LaboratoryGradingInfo { get; private set; }

    }
}
