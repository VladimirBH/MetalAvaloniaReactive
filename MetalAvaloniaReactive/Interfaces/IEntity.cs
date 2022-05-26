using System;

namespace AvaloniaClientMVVM.Interfaces;

public interface IEntity
{
    int Id { get; set; }
    DateTime CreationDate { get; set; }
}