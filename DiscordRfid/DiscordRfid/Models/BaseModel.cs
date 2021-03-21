using System;

namespace DiscordRfid.Models
{
    public abstract class BaseModel : IModel
    {
        public virtual void Validate()
        {
            throw new NotImplementedException($"Validation not implemented for {GetType().Name} model");
        }
    }
}
