using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Domain.Entities
{
    public class Lawyer
    {
        public int LawyerId { get; set; }
        public string? LawyerName { get; set; }
        public string? LawyerType { get; set; }
        public decimal Rating { get; set; }
    }
}
