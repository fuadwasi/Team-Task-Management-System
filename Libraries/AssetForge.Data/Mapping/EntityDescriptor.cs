namespace AssetForge.Data.Mapping
{
    public partial class EntityDescriptor
    {
        public EntityDescriptor()
        {
            Fields = new List<EntityFieldDescriptor>();
        }

        public string EntityName { get; set; }
        public string SchemaName { get; set; }
        public ICollection<EntityFieldDescriptor> Fields { get; set; }
    }
}