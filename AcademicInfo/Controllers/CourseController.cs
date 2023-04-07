using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Text.Json.Serialization;

namespace AcademicInfo.Controllers
{
    public class CourseRequest
    {
        private string AccessToken;
    }
    public class CourseResponse
    {
        [JsonPropertyName("number")]
        public int Number { get; set; } // ���� ��ȣ

        [JsonPropertyName("year")]
        public int Year { get; set; } // ���� �⵵

        [JsonPropertyName("semester")]
        public string Semester { get; set; } // ���� �б�

        [JsonPropertyName("name")]
        public string Name { get; set; } // �������

        [JsonPropertyName("instructor")]
        public string Instructor { get; set; } // ��米��

        [JsonPropertyName("day")]
        public DayOfWeek Day { get; set; } // ���ǿ���

        [JsonPropertyName("startTime")]
        public int StartTime { get; set; } // ���� ���۽ð�

        [JsonPropertyName("endTime")]
        public int EndTime { get; set; } // ���� ����ð�

        [JsonPropertyName("classRoom")]
        public string ClassRoom { get; set; } // ���ǽ�
    }

    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {

        private readonly ILogger<CourseController> _logger;

        public CourseController(ILogger<CourseController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> POST()
        {
            //���� AccessToken�� �����ϴ� �κ��� �߰��� ����

            //Console.WriteLine("��û����");
            var connectionString = "Host=\r\nacademic-info-db.postgres.database.azure.com\r\n;Username=classhub;Password=ch55361!;Database=AcademicInfo";
            var connection = new NpgsqlConnection(connectionString);

            var query = "SELECT\r\n    Section.section_id AS number,\r\n    Section.year,\r\n    Section.semester,\r\n    Course.title AS name,\r\n    Instructor.name AS instructor,\r\n    Time_slot.day_of_week AS day,\r\n    Time_slot.start_time AS startTime,\r\n    Time_slot.end_time AS endTime,\r\n    Section.room_id AS classRoom\r\nFROM\r\n    Takes\r\n    JOIN Section ON Takes.course_id = Section.course_id\r\n        AND Takes.section_id = Section.section_id\r\n        AND Takes.semester = Section.semester\r\n        AND Takes.year = Section.year\r\n    JOIN Course ON Takes.course_id = Course.course_id\r\n    JOIN Teaches ON Takes.course_id = Teaches.course_id\r\n        AND Takes.section_id = Teaches.section_id\r\n        AND Takes.semester = Teaches.semester\r\n        AND Takes.year = Teaches.year\r\n    JOIN Instructor ON Teaches.instructor_id = Instructor.ID\r\n    JOIN Time_slot ON Section.section_time_slot_id = Time_slot.section_time_slot_id\r\nWHERE\r\n    Takes.student_id = 3;";
            var results = connection.Query<CourseResponse>(query);

            connection.Dispose();

            return Ok(results);
        }
    };
}