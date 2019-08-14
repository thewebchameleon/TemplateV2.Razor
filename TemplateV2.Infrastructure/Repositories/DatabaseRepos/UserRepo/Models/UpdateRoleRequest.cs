namespace TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Models
{
    public class UpdateRoleRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Updated_By { get; set; }
    }
}
