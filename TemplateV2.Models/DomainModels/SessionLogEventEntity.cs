namespace TemplateV2.Models.DomainModels
{
    public class SessionLogEventEntity : BaseEntity
    {
        public int Session_Log_Id { get; set; }

        public int Event_Id { get; set; }

        public string InfoDictionary_JSON { get; set; }
    }
}
