namespace TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Models
{
    public class UpdatePermissionRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Group_Name { get; set; }

        public int Updated_By { get; set; }
    }
}
