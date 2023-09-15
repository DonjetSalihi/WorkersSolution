using System;

namespace WorkersSolution.Models
{
    public class WorkerLocation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CreatingCompanyId { get; set; }
        public bool IsArchived { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
