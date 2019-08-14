using System.Collections.Generic;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Models.ServiceModels.Admin.Users
{
    public class GetUserManagementResponse : ServiceResponse
    {
        public List<UserEntity> Users { get; set; }

        public GetUserManagementResponse()
        {
            Users = new List<UserEntity>();
        }
    }
}
