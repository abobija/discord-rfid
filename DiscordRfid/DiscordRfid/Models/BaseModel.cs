using System;

namespace DiscordRfid.Models
{
    public abstract class BaseModel : IModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual void Validate()
        {
            throw new NotImplementedException($"Validation not implemented for {GetType().Name} model");
        }
    }
}
