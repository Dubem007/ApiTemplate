using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee : AuditableEntity
    {
        public decimal Amount { get; set; }
        public DateTime DateInitiated { get; set; }
        public Guid EmployeeId{ get; set; }
        public Guid OrganizationId { get; set; }
        public Guid eCategoryId { get; set; }
    }
}
