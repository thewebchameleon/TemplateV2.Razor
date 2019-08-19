namespace TemplateV2.Repositories.DatabaseRepos.SessionRepo.Models
{
    public class CreateSessionLogEventRequest
    {
        public int Session_Log_Id { get; set; }

        public int Event_Id { get; set; }

        public string InfoDictionary_JSON { get; set; }

        public int Created_By { get; set; }
    }
}
