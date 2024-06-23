﻿namespace LawGuardPro.Domain.Entities;

public class Lawyer
{
    public Guid LawyerId { get; set; }
    public string? LawyerName { get; set; }
    public string? LawyerType { get; set; }
    public decimal Rating { get; set; }

    public ICollection<Case> Cases { get; set; } = new List<Case>();
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();

}
