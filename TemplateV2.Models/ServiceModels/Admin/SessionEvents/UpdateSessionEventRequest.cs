using System.ComponentModel.DataAnnotations;

namespace TemplateV2.Models.ServiceModels.Admin.SessionEvents
{
    public class UpdateSessionEventRequest
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
