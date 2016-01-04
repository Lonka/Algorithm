using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AisAlgorithm.Model;

namespace AisAlgorithm.DataAccess
{
    public class EmployeeRepository
    {
        readonly List<Employee> _employee;
        public EmployeeRepository()
        {
            if(_employee == null)
            {
                _employee = new List<Employee>();
            }
            _employee.Add(Employee.CreateEmployee("A", "Aonka"));
            _employee.Add(Employee.CreateEmployee("B", "Bonka"));
            _employee.Add(Employee.CreateEmployee("C", "Conka"));
        }

        public List<Employee> GetEmployee()
        {
            return new List<Employee>(_employee);
        }
    }
}
