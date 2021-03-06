using System;

namespace K94Warriors.Models
{
    public class DogMedicalRecord
    {
        public int RecordID { get; set; }
        public int DogProfileID { get; set; }
        public string RecordType { get; set; }
        public string Title { get; set; }
        public DateTime? RecordExpirationDate { get; set; }
        public string RecordURL { get; set; }
        public virtual DogProfile DogProfile { get; set; }
    }
}