namespace TemplateV2.Models.DomainModels
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Is_Enabled { get; set; }
    }
}
