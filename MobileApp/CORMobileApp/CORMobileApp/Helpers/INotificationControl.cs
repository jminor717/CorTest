using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORMobileApp.Helpers {
    public interface INotificationControl {

        void setHub(object hub);

         Task registerForTagsAsync(List<string> tags, string token);

        void unRegisterFromTags(List<string> tags);

        void setContext(Context context);
    }
}
