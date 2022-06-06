using System;
using AvaloniaClientMVVM.Interfaces;
using NodaTime;

namespace AvaloniaClientMVVM.Models;

public class BaseEntity : IEntity
{
    public int Id { get; set; }

    public DateTimeOffset CreationDate { get; set; }
    
    public DateTimeOffset? UpdatedDate { get; set; }
}