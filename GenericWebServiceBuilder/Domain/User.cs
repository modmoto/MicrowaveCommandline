using GenericWebServiceBuilder;

namespace Domain
{
    public partial class User
    {
        public UserCreateEvent Create(string Name)
        {
            return null;
        }

        public UserCreateEvent UpdateAge(int Age)
        {
            throw new System.NotImplementedException();
        }
    }
}