namespace TemplateV2.Repositories.DatabaseRepos.UserRepo.Models
{
    public class CreateUserRoleRequest
    {
        public int User_Id { get; set; }

        public int Role_Id { get; set; }

        public int Created_By { get; set; }
    }
}
