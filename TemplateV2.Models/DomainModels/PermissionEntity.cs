namespace TemplateV2.Models.DomainModels
{
    public class PermissionEntity : BaseEntity
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Group_Name { get; set; }

        public string Description { get; set; }
    }
}
