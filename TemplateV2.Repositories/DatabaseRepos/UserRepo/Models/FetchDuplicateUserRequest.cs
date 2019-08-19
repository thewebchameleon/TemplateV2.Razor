namespace TemplateV2.Repositories.DatabaseRepos.UserRepo.Models
{
    public class FetchDuplicateUserRequest
    {
        public int User_Id { get; set; }

        public string Username { get; set; }

        public string Email_Address { get; set; }

        public string Mobile_Number { get; set; }
    }
}
