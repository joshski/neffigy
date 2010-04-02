using Neffigy.Mvc;

namespace Neffigy.Example.Views
{
    public class Master : NeffigyView
    {
        protected override void Transform()
        {
            Text("#header h1", "Neffigy: An ASP.NET MVC View Engine That Keeps Markup Pure");
        }
    }
}
