using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Users
{
    public class GetUserResponse : ServiceResponse
    {
        public UserEntity User { get; set; }

        public List<RoleEntity> Roles { get; set; }

        public GetUserResponse()
        {
            Roles = new List<RoleEntity>();
        }
    }
}
