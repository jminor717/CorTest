using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORMobileApp.Helpers {
    public interface IHTTPs {
        Task<string> Post(string url,object body);

        Task<string> Get(string url, object body);
    }
}
