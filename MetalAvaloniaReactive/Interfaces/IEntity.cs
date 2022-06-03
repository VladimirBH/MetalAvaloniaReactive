using System;
using NodaTime;

namespace AvaloniaClientMVVM.Interfaces;

public interface IEntity
{
    int Id { get; set; }
    Instant CreationDate { get; set; }
}