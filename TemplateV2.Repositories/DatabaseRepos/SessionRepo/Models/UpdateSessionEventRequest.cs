namespace TemplateV2.Repositories.DatabaseRepos.SessionRepo.Models
{
    public class UpdateSessionEventRequest
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int Updated_By { get; set; }
    }
}
