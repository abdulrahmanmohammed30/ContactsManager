using CrudProject.Enums;
using ServiceContracts.DTO;

namespace CrudProject.Mappers
{
    public static class FilterablePropertiesMapper
    {
        public static readonly Dictionary<FilterableProperties, string> PropertyMappings = new()
            {
                { FilterableProperties.FirstName, "First Name" },
                { FilterableProperties.LastName, "Last Name" },     
                { FilterableProperties.Email, "Email" }
            };
    }
}

