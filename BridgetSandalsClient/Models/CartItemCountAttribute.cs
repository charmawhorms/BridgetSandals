using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BridgetSandalsClient.Utils;
using System.Reflection.Metadata;

namespace BridgetSandalsClient.Models
{
    public class CartItemCountAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;

            //Accesses the current controller from the ActionExecutingContext and checks if it's not null.
            if (controller != null)
            {
                //Retrieves the List<ShoppingCart> stored in the session
                var cart = controller.HttpContext.Session.Get<List<ShoppingCart>>("Cart");

                //Calculates the item count by getting the number of items in the cart
                //and assigning it to 0 if the cart is null
                int itemCount = cart?.Count ?? 0;

                //Setting the view bag with the item count
                controller.ViewBag.CartItemCount = itemCount;
            }

            base.OnActionExecuting(context);
        }
    }
}
