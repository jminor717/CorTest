using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.Helpers {
    public interface INotificationControl {

        void setHub(object hub);

        void registerForTags(List<string> tags);

        void unRegisterFromTags(List<string> tags);
    }
}
