using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudProject.Filters
{
    public class SkipFilter: Attribute, IFilterMetadata
    {
    }
}
