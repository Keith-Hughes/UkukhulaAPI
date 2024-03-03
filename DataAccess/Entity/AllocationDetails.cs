namespace DataAccess.Entity
{
    public class AllocationDetails
    {
        /*
         SELECT 
    University.[Name] AS University, 
    Provinces.ProvinceName AS Province, 
    UniversityFundAllocation.Budget,
    UniversityFundAllocation.DateAllocated,
    ISNULL(SUM(StudentFundAllocation.Amount),0) AS TotalAllocated
FROM 
    University 
INNER JOIN 
    UniversityFundAllocation ON University.ID = UniversityFundAllocation.UniversityID
INNER JOIN 
    Provinces ON University.ProvinceID = Provinces.ID
LEFT JOIN 
    StudentFundAllocation ON UniversityFundAllocation.ID = StudentFundAllocation.UniversityFundID
WHERE 
    DATEDIFF(YEAR, UniversityFundAllocation.DateAllocated, GETDATE()) = 0
GROUP BY
    University.[Name], 
    Provinces.ProvinceName, 
    UniversityFundAllocation.Budget,
    UniversityFundAllocation.DateAllocated;*/   

    public string University { get; set; }
    public string Province { get; set; }
    public decimal Budget { get; set; }
    public DateTime DateAllocated { get; set; }
    public decimal TotalAllocated { get; set; }

    public AllocationDetails(string university, string province, decimal budget, DateTime dateAllocated, decimal totalAllocated)
    {
        University = university;
        Province = province;
        Budget = budget;
        DateAllocated = dateAllocated;
        TotalAllocated = totalAllocated;
    }
}
    }

 
