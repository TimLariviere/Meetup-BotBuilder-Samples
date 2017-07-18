using System;

namespace DemoPrompts
{
    [Serializable]
    public class User
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public long FlightNumber { get; set; }
    }
}