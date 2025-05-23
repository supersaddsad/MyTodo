using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared.Dtos;

namespace WpfApp1.IService
{
    public interface IToDoService : IBaseService<ToDoDto>
    {
    }
}
