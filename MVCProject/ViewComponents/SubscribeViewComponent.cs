using Microsoft.AspNetCore.Mvc;

namespace MVCProject.ViewComponents
{
    public class SubscribeViewComponent : ViewComponent
    {
        
        public async Task<IViewComponentResult> InvokeAsync()
        {            
            return View();
        }      
    }
}
