using AvaloniaClientMVVM.Models;
using WebServer.DataAccess.Implementations.Entities;

namespace WebServer.Classes
{

    public class CalculationHistory : BaseEntity
    {
        public int UserId { set; get; }
        
        public int FurnaceId { set; get; }
        
        public decimal? Ag { set; get; }
        
        public decimal? Al { set; get; }
        
        public decimal? Au { set; get; }
        
        public decimal? Ca { set; get; }
        
        public decimal? Cr { set; get; }
        
        public decimal? Cu { set; get; }
        
        public decimal? Fe { set; get; }
        
        public decimal? Ni { set; get; }
        
        public decimal? Pb { set; get; }
        
        public decimal? Si { set; get; }
        
        public decimal? Sn { set; get; }
        
        public decimal? Zn { set; get; }

        public User? User { set; get; }
        
        public Furnace? Furnace { set; get; }
    }
}
