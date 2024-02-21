

namespace DataAccess.Entity
{
    public class BBDAllocation
    {


        int ID { get; set; }
        decimal Budget { get; set; }
        DateTime DateCreated { get; set; }
        public BBDAllocation(int _id, decimal _budget, DateTime _dateCreated)
        {
            ID = _id;
            Budget = _budget;
            DateCreated = _dateCreated;
        }
        public BBDAllocation(decimal budget, DateTime dateCreated)
        {
            Budget = budget;
            DateCreated = dateCreated;
        }

        public decimal getBudget() => Budget;

        public int getID() => ID;

        public DateTime getDate() => DateCreated;



    }
}
