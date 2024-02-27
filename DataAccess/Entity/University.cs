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
        public string Status { get; set; }
        
        
        public University(string name, int provinceID, string status)
        {
            Name = name;
            ProvinceID = provinceID;
            Status = status;
        }
        public University(int _id, string _name, int _provinceID, string _status)
        {
            ID = _id;
            Name = _name;
            ProvinceID = _provinceID;
            Status = _status;
        }

        public int GetID() => ID;
        public string GetName() => Name;
        public int GetProvinceID() => ProvinceID;



    }
}
