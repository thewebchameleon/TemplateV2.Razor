namespace TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Models
{
    public class CreatePermissionRequest
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Group_Name { get; set; }

        public int Created_By { get; set; }
    }
}
