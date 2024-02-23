using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace DataAccess.Entity
{
    public class University
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProvinceID { get; set; }

        [AllowedValues("INACTIVE","ACTIVE")]
        public string? Status { get; set; }
        
        
        public University(string name, int provinceID)
        {
            Name = name;
            ProvinceID = provinceID;
            

        }
        public University(int _id, string _name, int _provinceID)
        {
            ID = _id;
            Name = _name;
            ProvinceID = _provinceID;

        }

        public int GetID() => ID;
        public string GetName() => Name;
        public int GetProvinceID() => ProvinceID;



    }
}
