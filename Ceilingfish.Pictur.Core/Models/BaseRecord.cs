using System;

namespace Ceilingfish.Pictur.Core.Models
{
    public abstract class BaseRecord
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
