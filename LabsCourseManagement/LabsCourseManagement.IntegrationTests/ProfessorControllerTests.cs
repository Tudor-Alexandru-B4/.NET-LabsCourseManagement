using LabsCourseManagement.WebUI.Dtos;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace LabsCourseManagement.IntegrationTests
{
    public class ProfessorControllerTests:BaseIntegrationTests
    {
        private const string ApiUrl = "api/professors";
        [Fact]
        public async void When_CreatedProfessor_Then_ShouldReturnProfessorInTheGetRequest()
        {
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

        private static CreateProfessorDto CreateSUT()
        {
            return new CreateProfessorDto
            {
                Name = "Florin",
                Surname = "Olariu"
            };
        }
    }
}