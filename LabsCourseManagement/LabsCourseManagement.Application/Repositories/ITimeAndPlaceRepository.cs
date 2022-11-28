﻿using LabsCourseManagement.Domain;

namespace LabsCourseManagement.Application.Repositories
{
    public interface ITimeAndPlaceRepository
    {
        void Add(TimeAndPlace timeAndPlace);
        void Delete(TimeAndPlace timeAndPlace);
        TimeAndPlace Get(Guid id);
        List<TimeAndPlace> GetAll();
        Boolean Exists(DateTime time, string place);
        void Save();
    }
}