using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameter;
using Prism.Dialogs;
using WpfApp1.DialogHostcs;
using WpfApp1.Extensions;
using WpfApp1.IService;
using WpfApp1.Moudls;
using WpfApp1.Views.Dialogs;
using ToDoDto = MyToDo.Shared.Dtos.ToDoDto;

namespace WpfApp1.ViewModels
{
    public class ToDoViewMoudel : NavigationViewModel
    {
		public ToDoViewMoudel(IToDoService toDoService, IContainerProvider provider)
            : base(provider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
			
;          ExecutCommand = new DelegateCommand<string>(excutm);
            SelectedCommand = new DelegateCommand<ToDoDto>(Selected);
            CloseDrawerCommand = new DelegateCommand(colse);
                  DeleteCommand = new DelegateCommand<ToDoDto>(Delete);


            this.toDoService = toDoService;
            m_Dialog =  provider.Resolve<IDialogHostService>();
        }

        private async void Delete(ToDoDto dto)
        {
            try
            {
                var dialogResult = await m_Dialog.Question("温馨提示", $"确认删除待办事项:{dto.Title} ?");
                if (dialogResult.Result != Prism.Dialogs.ButtonResult.OK) return;

                UpdateLoading(true);
                var deleteResult = await toDoService.DeleteAsync(dto.Id);
                if (deleteResult.IsSuccess)
                {
                    var model = ToDoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
                    if (model != null)
                        ToDoDtos.Remove(model);
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private async void Selected(ToDoDto dto)
        {
            try
            {
                UpdateLoading(true);
                var TodoRusert = await toDoService.GetFirstOfDefaultAsync(dto.Id);
                if (TodoRusert.IsSuccess) 
                {
                    CurrToDto = TodoRusert.Result;
                    IsRightDrawerOpen = true;
                }

            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private void colse()
        {
            IsRightDrawerOpen = false;
        }

    
        private void excutm(string obj)
        {
            switch (obj)
            {
                case "新增":Add();break;
                case "查询": GetDateList(); break;
                case "修改": Save(); break;
                case "新增DTO": AddDtoAsync(); break;

            }
        }

        private async Task AddDtoAsync()
        {
            DialogParameters param = new DialogParameters();
            var dialogResult = await m_Dialog.ShowDialog("AddToDoView", param); 
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrToDto.Title) ||
                  string.IsNullOrWhiteSpace(CurrToDto.Content))
                return;

            UpdateLoading(true);

            try
            {
                if (CurrToDto.Id > 0)
                {
                    
                   
                    var updateResult = await toDoService.UpdateAsync(CurrToDto);
                    if (updateResult.IsSuccess)
                    {
                        var todo = ToDoDtos.FirstOrDefault(t => t.Id == CurrToDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrToDto.Title;
                            todo.Content = CurrToDto.Content;
                            todo.Status = CurrToDto.Status;
                        }
                    }
                    IsRightDrawerOpen = false;
                }
                else
                {
                    var addResult = await toDoService.AddAsync(CurrToDto);
                    if (addResult.IsSuccess)
                    {
                        ToDoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UpdateLoading(false);
            }

        }

        private ToDoDto currToDto;

        public ToDoDto CurrToDto
        {
            get { return currToDto; }
            set { currToDto = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }
        public DelegateCommand CloseDrawerCommand { get; private set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }

        private string search;

        /// <summary>
        /// 搜索条件
        /// </summary>
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // 基础实现
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // 带参数校验的增强实现
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return;
            storage = value;
            OnPropertyChanged(propertyName);
        }

        private int _status;
        public int Status
        {
            get => _status;
            set
            {
                if (_status == value) return;
                _status = value;
                OnPropertyChanged();

                // 触发相关属性的更新
                OnPropertyChanged(nameof(StatusDisplay));
                OnPropertyChanged(nameof(IsCompleted));
            }
        }

        // 只读计算属性
        public bool IsCompleted => Status == 1;

        // 显示用格式化属性
        public string StatusDisplay => Status == 1 ? "✅ 已完成" : "⏳ 进行中";

        // 集合更新示例
        private ObservableCollection<string> _items = new();
        public ObservableCollection<string> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        private void Add()
        {
            currToDto=new ToDoDto();

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                IsRightDrawerOpen = true;
            });



        }

        private bool _isRightDrawerOpen;
        public bool IsRightDrawerOpen
        {
            get => _isRightDrawerOpen;
            set
            {
                _isRightDrawerOpen = value;
                RaisePropertyChanged(nameof(IsRightDrawerOpen)); // 明确传递属性名
            }
        }

        private int? _selectedStatus;
        public int? SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }
        private string selectedIndex { get;set; }

        /// <summary>
        /// 下拉列表选中状态值
        /// </summary>
        public string SelectedIndex
        {
            get { return selectedIndex; }
            set {
                selectedIndex = value;
                SelectedStatus = value switch
                {
                    "待办" => 0,
                    "已完成" => 1,
                    _ => null
                };
                RaisePropertyChanged();
            }
        }


        public ICommand ToggleStatusCommand => new DelegateCommand(() =>
        {
            Status = Status == 0 ? 1 : 0;
        });

        public DelegateCommand<string> ExecutCommand { get;private set; }

		private ObservableCollection<ToDoDto> toDoDtos;
        private readonly IToDoService toDoService;
        private readonly IDialogHostService m_Dialog;

        public ObservableCollection<ToDoDto> ToDoDtos
		{
			get { return toDoDtos; }
			set
			{
				toDoDtos = value; RaisePropertyChanged();
			}

		}

		async void GetDateList()
        {
            ToDoDtos.Clear();
            UpdateLoading(true);
            var todoReast = await toDoService.GetAllListAsync(new MyToDo.Shared.Parameter.QueryParameter()
            {
                PageSize = 100,
                PageCount = 0,
                Search=search
            });

            if (todoReast.IsSuccess) 
            {
                foreach (var todo in todoReast.Result.Items) 
                {

                    if (SelectedStatus == null)
                    {
                        ToDoDtos.Add(todo);
                    }
                    else
                    {
                        if (todo.Status == SelectedStatus) { ToDoDtos.Add(todo); }
                    }
                  
                }
            }
            UpdateLoading(false);
        }



        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetDateList();
        }
    }
}
