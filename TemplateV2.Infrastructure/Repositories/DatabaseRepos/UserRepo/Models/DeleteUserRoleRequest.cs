using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Models
{
    public class DeleteUserRoleRequest
    {
        public int User_Id { get; set; }

        public int Role_Id { get; set; }

        public int Updated_By { get; set; }
    }
}
