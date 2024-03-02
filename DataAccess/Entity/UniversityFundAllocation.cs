

namespace DataAccess.Entity
{
    public class UniversityFundAllocation
    {
        public int ID { get; set; }
        decimal Budget { get; set; }
        DateTime DateAllocated { get; set; }
        int UniversityID { get; set; }
        int BBDAllocationID { get; set; }

        public UniversityFundAllocation(decimal budget, DateTime dateAllocated, int universityID, int bbdAllocationID)
        {

            Budget = budget;
            DateAllocated = dateAllocated;
            UniversityID = universityID;
            BBDAllocationID = bbdAllocationID;
        }

        public void save()
        {
            //new DBManager().SaveUniversityFundAllocation(this);

        }

        //gettes for all the attributes
        public decimal getBudget() => Budget;

        public DateTime getDateAllocated() => DateAllocated;

        public int getUniversityID() => UniversityID;

        public int getBBDAllocationID() => BBDAllocationID;

        public int getID() => ID;


    }
}
