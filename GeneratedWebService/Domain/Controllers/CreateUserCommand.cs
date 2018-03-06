namespace GeneratedWebService.Controllers
{
    public class CreateUserCommand
    {
        public CreateUserCommand(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }
}