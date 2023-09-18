using System;
using System.Web;
using System.Web.UI;

namespace CourseRequest__.Net_Framework_
{
    public partial class ErrorPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                ErrorDetails.Text = exception.ToString();
                Server.ClearError();
            }
        }
    }
}
