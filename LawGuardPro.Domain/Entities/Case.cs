﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawGuardPro.Domain.Entities
{
    public class Case
    {
        public int CaseId { get; set; }
        public string? CaseNumber { get; set; }
        public string? CaseName { get; set; }
        public string? CaseType { get; set; }
        public string? Description { get; set; }
        public bool IsAttachmentAvailable { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsLawyerAssigned { get; set; }
        public string? ApplicationUserId { get; set; }
        public int? LawyerId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
        public Lawyer? Lawyer { get; set; }
    }
}
