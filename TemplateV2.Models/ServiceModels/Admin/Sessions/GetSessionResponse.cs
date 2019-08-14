using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Sessions
{
    public class GetSessionResponse : ServiceResponse
    {
        public UserEntity User { get; set; }

        public SessionEntity Session { get; set; }
    }
}
