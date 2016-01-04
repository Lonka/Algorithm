using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AisAlgorithm.DataAccess;

namespace AisAlgorithm.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        readonly EmployeeRepository _employeeRepository;

        ObservableCollection<ViewModelBase> _viewModel;

        public MainWindowViewModel()
        {
            _employeeRepository = new EmployeeRepository();
            EmployeeListViewModel viewModel = new EmployeeListViewModel(_employeeRepository);
            this.ViewModels.Add(viewModel);

            EmployeeListViewModel1 viewModel1 = new EmployeeListViewModel1(new EmployeeRepository1());
            this.ViewModels.Add(viewModel1);
        }

        public ObservableCollection<ViewModelBase> ViewModels
        {
            get
            {
                if(_viewModel == null)
                {
                    _viewModel = new ObservableCollection<ViewModelBase>();
                }
                return _viewModel;
            }
        }
    }
}
