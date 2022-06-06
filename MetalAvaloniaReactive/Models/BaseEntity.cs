using System;
using AvaloniaClientMVVM.Interfaces;
using NodaTime;

namespace AvaloniaClientMVVM.Models;

public class BaseEntity : IEntity
{
    public int Id { get; set; }

    public Instant CreationDate { get; set; }
    
    public Instant? UpdatedDate { get; set; }
}