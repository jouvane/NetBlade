using System;
using System.Collections.Generic;

namespace NetBlade.Scheduling
{
    public class JobOptions
    {
        public string CronnExpression { get; set; }

        public Dictionary<string, string> Data { get; set; }

        public string Name { get; set; }

        public string Queue { get; set; }

        public virtual Type Type
        {
            get => !string.IsNullOrEmpty(this.TypeName) ? Type.GetType(this.TypeName) : null;
        }

        public string TypeName { get; set; }
    }
}
