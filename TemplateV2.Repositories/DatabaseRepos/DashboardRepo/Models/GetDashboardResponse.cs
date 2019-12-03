namespace TemplateV2.Repositories.DatabaseRepos.DashboardRepo.Models
{
    public class GetDashboardResponse
    {
        public int TotalSessions { get; set; }

        public int TotalUsers { get; set; }

        public int TotalRoles { get; set; }

        public int TotalConfigItems { get; set; }
    }
}
