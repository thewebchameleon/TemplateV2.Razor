namespace TemplateV2.Repositories.DatabaseRepos.SessionRepo.Models
{
    public class CreateSessionRequest
    {
        public string User_Agent { get; set; }

        public int Created_By { get; set; }
    }
}
