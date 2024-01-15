namespace BiddingService.Swagger
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerHeaderAttribute: Attribute
    {
        public SwaggerHeaderAttribute(string name, 
            string description = "", 
            string type = "", 
            bool required = false, 
            bool deprecated = false
        )
        {
            Name = name;
            Type = type;
            Description = description;
            Required = required;
            Deprecated = deprecated;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Deprecated { get; set; }
    }
}
