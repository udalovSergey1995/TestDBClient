namespace WebApplication1.Library
{
    public class Models
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public Passport Passport { get; set; }
        public Department Department { get; set; }
    }
    public class Passport
    {
        public string Type { get; set; }
        public string Number { get; set; }

    }
    public class Department
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
