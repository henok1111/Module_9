namespace TmsApi.Application.DTOs;

public record StudentOptionDto(int Id, string RegistrationNumber, string Name);
public record CourseOptionDto(int Id, string Code, string Title);