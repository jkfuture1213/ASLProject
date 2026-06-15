using System;
using System.Runtime.Serialization;

namespace SmartAccountBook
{
    [DataContract]
    public class Transaction
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
