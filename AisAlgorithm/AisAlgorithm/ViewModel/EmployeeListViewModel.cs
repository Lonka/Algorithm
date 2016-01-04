using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AisAlgorithm.DataAccess;

namespace AisAlgorithm.ViewModel
{
    class EmployeeListViewModel : ViewModelBase
    {
        readonly EmployeeRepository _employeeRepository;

        public ObservableCollection<Model.Employee> AllEmployee
        {
            get;
            private set;
        }

        public EmployeeListViewModel(EmployeeRepository employeeRepository)
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
