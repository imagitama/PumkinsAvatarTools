namespace Pumkin.AvatarTools2.Settings
{
    public class PropertyGroup
    {
        public string Name { get; set; }
        public bool Enabled { get; set; } = true;
        public string[] PropertyNames { get; set; }


        public PropertyGroup(string name, bool enabled, params string[] propertyNames)
        {
            Name = name;
            Enabled = enabled;
            PropertyNames = propertyNames;
        }

        public PropertyGroup(string name, params string[] propertyNames)
            : this(name, true, propertyNames) { }
    }
}