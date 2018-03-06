using System;

namespace GeneratedWebService.Controllers
{
    public class UpdateUserNameCommand
    {
        public UpdateUserNameCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}