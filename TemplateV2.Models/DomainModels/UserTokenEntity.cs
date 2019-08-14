using System;

namespace TemplateV2.Models.DomainModels
{
    public class UserTokenEntity : BaseEntity
    {
        public int User_Id { get; set; }

        public Guid Guid { get; set; }

        public TokenTypeEnum Type_Id { get; set; }

        public bool Processed { get; set; }
    }
}
