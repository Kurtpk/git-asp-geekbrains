using Microsoft.AspNetCore.Mvc;

namespace WebStore_Igor_Tonshev.ViewComponents
{
    public class LoginLogoutViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
