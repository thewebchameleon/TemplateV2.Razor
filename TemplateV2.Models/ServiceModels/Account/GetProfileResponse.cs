using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Account
{
    public class GetProfileResponse : ServiceResponse
    {
        public UserEntity User { get; set; }

        public List<RoleEntity> Roles { get; set; }

        public GetProfileResponse()
        {
            Roles = new List<RoleEntity>();
        }
    }
}
