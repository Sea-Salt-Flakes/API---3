namespace WebApp1.DTOs
{
    public class KeyValueDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public KeyValueDto() {}

        public KeyValueDto(int id, string key, string value, string description)
        {
            Id = id;
            Key = key;
            Value = value;
            Description = description;
        }
    }
}