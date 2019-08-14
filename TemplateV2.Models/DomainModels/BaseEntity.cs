using System;

namespace TemplateV2.Models.DomainModels
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public int Created_By { get; set; }

        public DateTime Created_Date { get; set; }

        public int Updated_By { get; set; }

        public DateTime Updated_Date { get; set; }

        public bool Is_Deleted { get; set; }
    }
}
