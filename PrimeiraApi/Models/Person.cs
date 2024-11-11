namespace PrimeiraApi.Models
{
    public class Person
    {
        public Guid Id { get; init; }
        public string Name { get; private set; }
        public string Job { get; private set; }
        public bool Active { get; private set; }

        public Person( string name , string job )
        {
            Id = Guid.NewGuid();
            Name = name;
            Job = job;
            Active = true;
        }

        public void UpdateName( string name )
        {
            Name = name;
        }

        public void UpdateJob( string job )
        {
            Job = job;
        }

        public void Disable()
        {
            Active = false;
        }
    }
}
