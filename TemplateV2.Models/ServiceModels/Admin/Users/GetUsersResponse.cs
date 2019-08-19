using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Users
{
    public class GetUsersResponse : ServiceResponse
    {
        public List<UserEntity> Users { get; set; }

        public GetUsersResponse()
        {
            Users = new List<UserEntity>();
        }
    }
}
