using System;
using System.Movieslections.Generic;
using System.Text;

namespace Movies.CrossCutting.Comum
{
    public class TransactionID
    {
        public TransactionID(string id)
        {
            this.ID = id;
        }

        public string ID { get; }
    }
}
