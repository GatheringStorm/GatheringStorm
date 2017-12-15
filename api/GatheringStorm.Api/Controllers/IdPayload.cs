using System;
using System.Collections.Generic;
using System.Text;

namespace GatheringStorm.Api.Controllers
{
    public class IdPayload
    {
        public Guid Id { get; set; }
    }

    public class IdPayload<T> : IdPayload
    {
        public T Value { get; set; }
    }
}
