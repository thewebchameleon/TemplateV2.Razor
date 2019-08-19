namespace TemplateV2.Repositories.DatabaseRepos.UserRepo.Models
{
    public class CreateRolePermissionRequest
    {
        public int Role_Id { get; set; }

        public int Permission_Id { get; set; }

        public int Created_By { get; set; }
    }
}
