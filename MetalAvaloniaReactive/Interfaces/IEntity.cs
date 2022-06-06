using System;
using NodaTime;

namespace AvaloniaClientMVVM.Interfaces;

public interface IEntity
{
    int Id { get; set; }
    DateTimeOffset CreationDate { get; set; }
    
    DateTimeOffset? UpdatedDate { get; set; }
}