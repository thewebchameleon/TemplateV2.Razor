namespace TemplateV2.Repositories.DatabaseRepos.UserRepo.Models
{
    public class UpdateUserPasswordRequest
    {
        public int User_Id { get; set; }

        public string Password_Hash { get; set; }

        public int Updated_By { get; set; }
    }
}
