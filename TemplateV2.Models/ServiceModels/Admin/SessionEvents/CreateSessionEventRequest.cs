using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels.Admin.SessionEvents
{
    public class CreateSessionEventRequest
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
