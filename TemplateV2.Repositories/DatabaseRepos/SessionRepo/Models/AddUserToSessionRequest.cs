namespace TemplateV2.Repositories.DatabaseRepos.SessionRepo.Models
{
    public class AddUserToSessionRequest
    {
        public int Id { get; set; }

        public int User_Id { get; set; }

        public int Updated_By { get; set; }
    }
}
