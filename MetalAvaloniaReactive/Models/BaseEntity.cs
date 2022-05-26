using System;
using AvaloniaClientMVVM.Interfaces;

namespace AvaloniaClientMVVM.Models;

public class BaseEntity : IEntity
{
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
}