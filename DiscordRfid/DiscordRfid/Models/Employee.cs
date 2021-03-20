using System;
using System.Activities;

namespace DiscordRfid.Models
{
    public class Employee : BaseModel
    {
        public int Id;
        public DateTime CreatedAt;
        public string FirstName;
        public string LastName;
        public bool Present;

        public override void Validate()
        {
            if(string.IsNullOrWhiteSpace(LastName))
            {
                throw new ValidationException("Last name cannot be empty");
            }
        }
    }
}
