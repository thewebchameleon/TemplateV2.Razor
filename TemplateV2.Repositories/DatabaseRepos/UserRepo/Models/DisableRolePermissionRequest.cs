namespace TemplateV2.Repositories.DatabaseRepos.UserRepo.Models
{
    public class DisableRolePermissionRequest
    {
        public int Role_Id { get; set; }

        public int Permission_Id { get; set; }

        public int Updated_By { get; set; }
    }
}
