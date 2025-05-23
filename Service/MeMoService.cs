using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToDo.Shared.Dtos;
using WpfApp1.IService;

namespace WpfApp1.Service
{
    public class MeMoService : BaseService<MemoDto>, IMeMoService
    {
        public MeMoService(HttpRestClient client) : base(client, "MeMo")
        {
        }
    }
}
