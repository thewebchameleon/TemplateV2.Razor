using System;

namespace TemplateV2.Repositories.DatabaseRepos.UserRepo.Models
{
    public class LockoutUserRequest
    {
        public int Id { get; set; }

        public DateTime Lockout_End { get; set; }

        public int Updated_By { get; set; }
    }
}
