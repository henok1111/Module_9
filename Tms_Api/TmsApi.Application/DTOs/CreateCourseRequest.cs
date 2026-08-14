namespace TmsApi.Application.DTOs;

public class CreateCourseRequest
{
    public string Code { get; set; } = "";
    public string Title { get; set; } = "";
    public int MaxCapacity { get; set; }
}