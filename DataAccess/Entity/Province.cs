

namespace DataAccess.Entity

{
    public class Province
    {
        public int ID { get; set; }
        public string ProvinceName { get; set; }

        public Province(int _id, string _provinceName)
        {
            ID = _id;
            ProvinceName = _provinceName;
        }

        public Province(string _provinceName)
        {
            ProvinceName = _provinceName;
        }

        public void Save()
        {
            // save to the database using the DBManager

        }
    }
}
