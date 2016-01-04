using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AisAlgorithm.Model;

namespace AisAlgorithm.DataAccess
{
    public class EmployeeRepository1
    {
        readonly List<Employee> _employee;
        public EmployeeRepository1()
        {
            if(_employee == null)
            {
                _employee = new List<Employee>();
            }
            _employee.Add(Employee.CreateEmployee("D", "Donka"));
            _employee.Add(Employee.CreateEmployee("E", "Eonka"));
            _employee.Add(Employee.CreateEmployee("F", "Fonka"));
        }

        public List<Employee> GetEmployee()
        {
            return new List<Employee>(_employee);
        }
    }
}
