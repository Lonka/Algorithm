using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AisAlgorithm.DataAccess;

namespace AisAlgorithm.ViewModel
{
    class EmployeeListViewModel1 : ViewModelBase
    {
        readonly EmployeeRepository1 _employeeRepository;

        public ObservableCollection<Model.Employee> AllEmployee
        {
            get;
            private set;
        }

        public EmployeeListViewModel1(EmployeeRepository1 employeeRepository)
        {
            if(employeeRepository == null)
            {
                throw new ArgumentNullException("employeeRepository");
            }
            _employeeRepository = employeeRepository;
            this.AllEmployee = new ObservableCollection<Model.Employee>(_employeeRepository.GetEmployee());
        }

        protected override void OnDispose()
        {
            this.AllEmployee.Clear();
        }
    }
}
