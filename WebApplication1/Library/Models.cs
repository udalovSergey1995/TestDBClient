using System.Text.Json.Serialization;
using WebApplication1.Library.DB;

namespace WebApplication1.Library
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public int PassportId { get; set; }
        public int DepartmentId { get; set; }
        public PassportModel Passport 
        {
            get 
            {
                if (_passport == null)
                {
                    GetPassportModel();
                }
                return _passport;
            }
            set {
                _passport = value;
            } 
        }
        public DepartmentModel Department
        {
            get
            {
                if (_department == null)
                {
                    GetDepartmentModel();
                }
                return _department;
            }
            set
            {
                _department = value;
            }
        }


        private PassportModel _passport = null;
        private DepartmentModel _department = null;

        private async void GetPassportModel()
        {
            _passport = await ((new GetObjectById<PassportModel>("passport")).Get(PassportId.ToString()));
        }
        private async void GetDepartmentModel() => _department = await ((new GetObjectById<DepartmentModel>("department")).Get(DepartmentId.ToString()));
    }
    public class PassportModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }

    }
    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
