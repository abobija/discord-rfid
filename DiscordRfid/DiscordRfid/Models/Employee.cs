﻿using System;
using System.Activities;

namespace DiscordRfid.Models
{
    public class Employee : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Present { get; set; }
        public RfidTag[] RfidTags { get; set; }

        public override void Validate()
        {
            if(string.IsNullOrWhiteSpace(LastName))
            {
                throw new ValidationException("Last name cannot be empty");
            }
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} (Id: {Id})";
        }
    }
}
