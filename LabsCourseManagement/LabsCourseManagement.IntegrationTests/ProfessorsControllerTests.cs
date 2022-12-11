using LabsCourseManagement.WebUI.Dtos;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LabsCourseManagement.IntegrationTests
{
    [Collection("Sequential")]
    public class ProfessorsControllerTests:BaseIntegrationTests
    {
        private const string ApiUrl = "v1/api/professors";
        [Fact]
        public async void When_CreatedProfessor_Then_ShouldReturnProfessorInTheGetRequest()
        {
            CleanDatabases();
            CreateProfessorDto professorDto = CreateSUT();
            //ACT
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync(ApiUrl, professorDto);
            var getProfessorResult = await HttpClientProfessor.GetAsync(ApiUrl);

            //assert
            createProfessorResponse.EnsureSuccessStatusCode();
            createProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            getProfessorResult.EnsureSuccessStatusCode();
            var professors = await getProfessorResult.Content.ReadFromJsonAsync<List<CreateProfessorDto>>();
            professors.Count.Should().Be(1);
            professors.Should().HaveCount(1);
            professors.Should().NotBeNull();

        }

        [Fact]
        public async void When_DeleteProfessor_Then_ShouldNotReturnProfessorInGetRequest()
        {
            CleanDatabases();
            //Arrange
            CreateProfessorDto professorDto = CreateSUT();

            //Act
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync(ApiUrl, professorDto);
            var getProfessorResponse = await HttpClientProfessor.GetAsync(ApiUrl);
            var professors = await getProfessorResponse.Content.ReadFromJsonAsync<List<ProfessorDto>>();
            var deleteProfessorResponse = await HttpClientProfessor.DeleteAsync($"{ApiUrl}/{professors[professors.Count - 1].Id}");
            var getProfessorAfterDeleteResponse = await HttpClientProfessor.GetAsync($"{ApiUrl}/{professors[professors.Count - 1].Id}");

            //Assert
            deleteProfessorResponse.EnsureSuccessStatusCode();
            deleteProfessorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            getProfessorAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async void When_AddCoursesToProfessor_Then_ShouldBeAdded()
        {
            CleanDatabases();
            //Arrange
            CreateProfessorDto professorDto = CreateSUT();

            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync(ApiUrl, professorDto);
            var getProfessorResponse = await HttpClientProfessor.GetAsync(ApiUrl);
            var professors = await getProfessorResponse.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var courseDto = new CreateCourseDto()
            {
                Name = "AddedProfessorName",
                ProfessorId = professors[professors.Count - 1].Id
            };

            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            //Act
            var addCourseResponse = await HttpClientProfessor.PostAsJsonAsync($"{ApiUrl}/{professors[professors.Count - 1].Id}/courses", new List<Guid>
            {
                courses[courses.Count - 1].Id
            });

            var getProfessorsAfterAddResponse = await HttpClientProfessor.GetAsync(ApiUrl);
            var professorsAfterAdd = await getProfessorsAfterAddResponse.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Assert

            addCourseResponse.EnsureSuccessStatusCode();
            addCourseResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            professorsAfterAdd[professorsAfterAdd.Count - 1].Courses.Count.Should().Be(1);
            professorsAfterAdd[professorsAfterAdd.Count - 1].Courses[0].Id.Should().Be(courses[courses.Count - 1].Id);
            professorsAfterAdd[professorsAfterAdd.Count - 1].Courses[0].Name.Should().Be(courses[courses.Count - 1].Name);
        }

        [Fact]
        public async void When_AddLaboratoriesToProfessor_Then_ShouldBeAdded()
        {
            CleanDatabases();
            //Arrange
            CreateProfessorDto professorDto = CreateSUT();

            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync(ApiUrl, professorDto);
            var getProfessorResponse = await HttpClientProfessor.GetAsync(ApiUrl);
            var professors = await getProfessorResponse.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            var courseDto = new CreateCourseDto()
            {
                Name = "AddedProfessorName",
                ProfessorId = professors[professors.Count - 1].Id
            };

            var createCourseResponse = await HttpClientCourses.PostAsJsonAsync("v1/api/courses", courseDto);
            var getCourseResponse = await HttpClientCourses.GetAsync("v1/api/courses");
            var courses = await getCourseResponse.Content.ReadFromJsonAsync<List<CourseDto>>();

            var laboratoryDto = new CreateLaboratoryDto()
            {
                Name = "LaboratoryName",
                CourseId = courses[0].Id,
                ProfessorId = professors[0].Id,
                DateTime = "20-11-22 3:55",
                Place = "C409"
            };

            var createLaboratoryResponse = await HttpClientLaboratories.PostAsJsonAsync("v1/api/laboratories", laboratoryDto);
            var getLaboratoryResponse = await HttpClientLaboratories.GetAsync("v1/api/laboratories");
            var laboratories = await getLaboratoryResponse.Content.ReadFromJsonAsync<List<LaboratoryDto>>();

            //Act
            var addLaboratoryResponse = await HttpClientProfessor.PostAsJsonAsync($"{ApiUrl}/{professors[professors.Count - 1].Id}/laboratories", new List<Guid>
            {
                laboratories[laboratories.Count - 1].Id
            });
            var getProfessorsAfterAddResponse = await HttpClientProfessor.GetAsync(ApiUrl);
            var professorsAfterAdd = await getProfessorsAfterAddResponse.Content.ReadFromJsonAsync<List<ProfessorDto>>();

            //Assert

            addLaboratoryResponse.EnsureSuccessStatusCode();
            addLaboratoryResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            professorsAfterAdd[professorsAfterAdd.Count - 1].Laboratories.Count.Should().Be(1);
            professorsAfterAdd[professorsAfterAdd.Count - 1].Laboratories[0].Id.Should().Be(laboratories[laboratories.Count - 1].Id);
            professorsAfterAdd[professorsAfterAdd.Count - 1].Laboratories[0].Name.Should().Be(laboratories[laboratories.Count - 1].Name);
        }

        [Fact]
        public async void When_GetByIdProfessor_Then_ShouldGetProfessor()
        {
            CleanDatabases();
            //Arrange
            CreateProfessorDto professorDto = CreateSUT();

            //Act
            var createProfessorResponse = await HttpClientProfessor.PostAsJsonAsync(ApiUrl, professorDto);
            var getProfessorResponse = await HttpClientProfessor.GetAsync(ApiUrl);
            var professors = await getProfessorResponse.Content.ReadFromJsonAsync<List<ProfessorDto>>();
            var getProfessorByIdResponse = await HttpClientProfessor.GetAsync($"{ApiUrl}/{professors[professors.Count - 1].Id}");

            //Assert
            getProfessorByIdResponse.EnsureSuccessStatusCode();
            getProfessorByIdResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var professor = await getProfessorByIdResponse.Content.ReadFromJsonAsync<ProfessorDto>();

            professor.Should().NotBeNull();
            professor.Id.Should().Be(professors[professors.Count - 1].Id);
        }

        private static CreateProfessorDto CreateSUT()
        {
            return new CreateProfessorDto
            {
                Name = "Florin",
                Surname = "Olariu",
                PhoneNumber = "0733156778",
            };
        }
    }
}