using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSysICTSBM.API.Bll.Repository.Repository
{
    public interface IRepository
    {
        Task<string> LoginAsync(int AppId);
    }
}
