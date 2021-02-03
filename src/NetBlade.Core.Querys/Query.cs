using System;

namespace NetBlade.Core.Querys
{
    public class Query : IQuery
    {
        protected Query()
        {
            this.TimestampQuery = DateTime.Now;
            this.IdQuery = Guid.NewGuid().ToString();
        }

        protected string IdQuery { get; }

        protected DateTime TimestampQuery { get; }
    }
}
