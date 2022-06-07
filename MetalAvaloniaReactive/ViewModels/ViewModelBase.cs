using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Helpers;

namespace MetalAvaloniaReactive.ViewModels
{
    public class ViewModelBase :  ReactiveObject
    {
 
        public ValidationContext ValidationContext { get; } = new ValidationContext();
    }
}