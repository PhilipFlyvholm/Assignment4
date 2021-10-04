using System.Collections.Generic;

namespace Assignment4.Core
{
    public record TaskDetailsDTO : TaskDTO
    {
        public string AssignedToName { get; init; }
        public string AssignedToEmail { get; init; }
    }
}
