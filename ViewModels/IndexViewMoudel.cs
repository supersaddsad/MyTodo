using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfApp1.Moudls;

namespace WpfApp1.ViewModels
{
    public class IndexViewMoudel : BindableBase
    {
        public IndexViewMoudel()
        {
            taskBars = new ObservableCollection<TaskBar>();
            CreateTaskBar();
            CreateTestDate();
        }



        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoDtos;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }


        void CreateTaskBar()
        {
            TaskBars.Add(new TaskBar()
            {
                Title = "汇总",
                Icon = "ClockFast",
                Content = "9",
                Color = 
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0CA0FF")),
                Target = ""
            });
            TaskBars.Add(new TaskBar()
            {
                Title = "已完成",
                Icon = "ClockCheckOutline",
                Content = "9",
                Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1ECA3A")),
                Target = ""
            });
            TaskBars.Add(new TaskBar()
            {
                Title = "完成率",
                Icon = "ChartLineVariant",
                Content = "100%",
                Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF02C6DC")),
                Target = ""
            });
            TaskBars.Add(new TaskBar()
            {
                Title = "备忘录",
                Icon = "PlaylistStar",
                Content = "9",
                Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFA000")),
             
                Target = ""
            });

        }


        void CreateTestDate()
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();

            MemoDtos = new ObservableCollection<MemoDto>();

            for (int i = 0; i < 10; i++)
            {
                ToDoDtos.Add(new ToDoDto() { Title = "测试" + i, Content = "内容" + i });

                MemoDtos.Add(new MemoDto() { Title = "测试" + i, Content = "内容" + i });
            }

        }
    }
}
