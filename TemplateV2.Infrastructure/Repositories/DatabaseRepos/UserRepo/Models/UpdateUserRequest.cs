using System;

namespace TemplateV2.Infrastructure.Repositories.DatabaseRepos.UserRepo.Models
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email_Address { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Mobile_Number { get; set; }

        public int Updated_By { get; set; }
    }
}
