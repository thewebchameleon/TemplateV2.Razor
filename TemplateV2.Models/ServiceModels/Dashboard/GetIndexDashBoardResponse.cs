namespace TemplateV2.Models.ServiceModels.Dashboard
{
    public class GetIndexDashBoardResponse : ServiceResponse
    {
        public int TotalSessions { get; set; }

        public int TotalUsers { get; set; }

        public int TotalRoles { get; set; }

        public int TotalConfigItems { get; set; }
    }
}
