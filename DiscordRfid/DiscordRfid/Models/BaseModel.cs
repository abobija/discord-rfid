using System;

namespace DiscordRfid.Models
{
    public abstract class BaseModel : IModel
    {
        public virtual int Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        public virtual void Validate()
        {
            throw new NotImplementedException($"Validation not implemented for {GetType().Name} model");
        }
    }
}
