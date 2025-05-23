using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared.Dtos;
using WpfApp1.IService;
using Prism.Commands;
using Prism.Mvvm;

namespace WpfApp1.ViewModels
{
    public class MeMoViewMoudel : BindableBase
    {
        public MeMoViewMoudel(IMeMoService meMoService)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            NewMemo = new MemoDto();

            AddCommand = new DelegateCommand(Add);
            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);
            EditCommand = new DelegateCommand<MemoDto>(Edit);
            CloseDrawerCommand = new DelegateCommand(CloseDrawer);

            this.meMoService = meMoService;
            CreateToDoList();
        }

        private async void CreateToDoList()
        {
            var todoReast = await meMoService.GetAllListAsync(new MyToDo.Shared.Parameter.QueryParameter()
            {
                PageSize = 100,
                PageCount = 0,
            });

            if (todoReast.IsSuccess)
            {
                foreach (var todo in todoReast.Result.Items)
                {
                    MemoDtos.Add(todo);
                }
            }
        }

        private void Add()
        {
            NewMemo = new MemoDto();
            IsRightDrawerOpen = true;
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(NewMemo.Title) || string.IsNullOrWhiteSpace(NewMemo.Content))
                return;

            try
            {
                if (NewMemo.Id > 0)
                {
                    var updateResult = await meMoService.UpdateAsync(NewMemo);
                    if (updateResult.IsSuccess)
                    {
                        var memo = MemoDtos.FirstOrDefault(t => t.Id == NewMemo.Id);
                        if (memo != null)
                        {
                            memo.Title = NewMemo.Title;
                            memo.Content = NewMemo.Content;
                           
                        }
                    }
                }
                else
                {
                    var addResult = await meMoService.AddAsync(NewMemo);
                    if (addResult.IsSuccess)
                    {
                        MemoDtos.Add(addResult.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                // 处理异常
            }
            finally
            {
                IsRightDrawerOpen = false;
            }
        }

        private async void Delete(MemoDto dto)
        {
            try
            {
                var deleteResult = await meMoService.DeleteAsync(dto.Id);
                if (deleteResult.IsSuccess)
                {
                    var model = MemoDtos.FirstOrDefault(t => t.Id.Equals(dto.Id));
                    if (model != null)
                        MemoDtos.Remove(model);
                }
            }
            catch (Exception ex)
            {
                // 处理异常
            }
        }

        private void Edit(MemoDto dto)
        {
            NewMemo = new MemoDto
            {
                Id = dto.Id,
                Title = dto.Title,
                Content = dto.Content,
                 
            };
            IsRightDrawerOpen = true;
        }

        private void CloseDrawer()
        {
            IsRightDrawerOpen = false;
        }

        private bool isRightDrawerOpen;
        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }
        public DelegateCommand<MemoDto> EditCommand { get; private set; }
        public DelegateCommand CloseDrawerCommand { get; private set; }

        private ObservableCollection<MemoDto> memoDtos;
        private readonly IMeMoService meMoService;
        private MemoDto newMemo;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set
            {
                memoDtos = value;
                RaisePropertyChanged();
            }
        }

        public MemoDto NewMemo
        {
            get { return newMemo; }
            set
            {
                newMemo = value;
                RaisePropertyChanged();
            }
        }
    }
}