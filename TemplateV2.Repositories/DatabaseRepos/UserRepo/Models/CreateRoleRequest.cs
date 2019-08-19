namespace TemplateV2.Repositories.DatabaseRepos.UserRepo.Models
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int Created_By { get; set; }
    }
}
