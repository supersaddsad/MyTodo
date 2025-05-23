using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using Prism.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WpfApp1.Moudls; // 注意：此处可能存在拼写错误，应为"Models"？

namespace WpfApp1.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private string _currentDate;
        private ObservableCollection<TaskBar> _taskBars;
        private ObservableCollection<ToDoDto> _toDoDtos;
        private ObservableCollection<MemoDto> _memoDtos;
        private DispatcherTimer _timer;

        public IndexViewModel(IRegionManager regionManager, IContainerProvider containerProvider)
            : base(containerProvider)
        {
            InitializeCommands();
            InitializeData();
            InitializeTimer(); // 初始化定时器
        }

        public string CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        public ObservableCollection<TaskBar> TaskBars
        {
            get => _taskBars;
            set => SetProperty(ref _taskBars, value);
        }

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get => _toDoDtos;
            set => SetProperty(ref _toDoDtos, value);
        }

        public ObservableCollection<MemoDto> MemoDtos
        {
            get => _memoDtos;
            set => SetProperty(ref _memoDtos, value);
        }

        public DelegateCommand AddToDoCommand { get; private set; }
        public DelegateCommand AddMemoCommand { get; private set; }

        private void InitializeCommands()
        {
            AddToDoCommand = new DelegateCommand(AddToDo);
            AddMemoCommand = new DelegateCommand(AddMemo);
        }

        private void InitializeData()
        {
            // 初始设置日期时间
            UpdateCurrentDate();

            TaskBars = new ObservableCollection<TaskBar>();
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();

            LoadTaskBars();
            LoadToDos();
            LoadMemos();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1) // 设置每秒触发一次
            };
            _timer.Tick += Timer_Tick;
            _timer.Start(); // 启动定时器
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCurrentDate(); // 每秒更新一次日期时间
        }

        private void UpdateCurrentDate()
        {
            // 使用HH（24小时制）替代hh（12小时制）
            CurrentDate = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        // 其他方法保持不变...
        private void LoadTaskBars()
        {
            TaskBars.Add(new TaskBar
            {
                Icon = "FormatListChecks",
                Title = "待办事项",
                Content = "9",
                Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"))
            });
            TaskBars.Add(new TaskBar
            {
                Icon = "NotebookOutline",
                Title = "备忘录",
                Content = "6",
                Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"))
            });
            TaskBars.Add(new TaskBar
            {
                Icon = "CheckCircle",
                Title = "已完成",
                Content = "12",
                Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"))
            });
            TaskBars.Add(new TaskBar
            {
                Icon = "ChartLineVariant",
                Title = "完成比例",
                Content = "57%",
                Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9C27B0"))
            });
        }

        private void LoadToDos()
        {
            ToDoDtos.Add(new ToDoDto { Title = "完成WPF项目", Content = "实现一个待办事项管理应用" });
            ToDoDtos.Add(new ToDoDto { Title = "学习Blazor", Content = "掌握Blazor开发Web应用" });
            ToDoDtos.Add(new ToDoDto { Title = "复习Docker", Content = "复习Docker容器化技术" });
        }

        private void LoadMemos()
        {
            MemoDtos.Add(new MemoDto
            {
                Title = "技术笔记",
                Content = "记录学习过程中的重要知识点",
                CreateDate = DateTime.Now
            });
            MemoDtos.Add(new MemoDto
            {
                Title = "会议记录",
                Content = "项目进度讨论会议要点",
                CreateDate = DateTime.Now.AddDays(-1)
            });
            MemoDtos.Add(new MemoDto
            {
                Title = "想法收集",
                Content = "新功能开发的创意和想法",
                CreateDate = DateTime.Now.AddDays(-2)
            });
        }

        private void AddToDo()
        {
            // 待实现
        }

        private void AddMemo()
        {
            // 待实现
        }
    }
}