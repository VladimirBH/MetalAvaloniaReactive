using System;

namespace AvaloniaClientMVVM.Models;

public class User : BaseEntity
{
    public string Name { get; set; }
    
    public string Surname { get; set; }
    
    
    /*public string Patronymic { get; set; }
    
    public DateTime DateBirth { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public int RoleId { get; set; }
    
    public Role? Role { get; set; }*/

}