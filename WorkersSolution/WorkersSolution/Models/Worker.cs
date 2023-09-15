using System;

namespace WorkersSolution.Models
{
    public class Worker
    {
        public string Id { get; set; }
        public bool Active { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string EmployerId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateHired { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmergencyContact1 { get; set; }
        public string EmergencyContact2 { get; set; }
        public string EmergencyNotes { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string PictureId { get; set; }
        public string Email { get; set; }
    }
}
