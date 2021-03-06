using System;

namespace K94Warriors.Models
{
    public class DogNotesReport
    {
        public int NoteID { get; set; }
        public int DogProfileID { get; set; }
        public string Note { get; set; }
        public bool IsCritical { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public int? Age { get; set; }
        public string Color { get; set; }
        public DateTime? PickedUpDate { get; set; }
        public int? DonorID { get; set; }
        public bool IsFixed { get; set; }
        public DateTime? GraduationDate { get; set; }
        public int? SponsorID { get; set; }
        public int? WarriorID { get; set; }
        public DateTime CreatedTimeUTC { get; set; }
        public Guid CreatedByUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}