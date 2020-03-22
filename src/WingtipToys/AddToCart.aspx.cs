using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using WingtipToys.Logic;

namespace WingtipToys
{
  public partial class AddToCart : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      string rawId = Request.QueryString["ProductID"];
      if (!String.IsNullOrEmpty(rawId) && int.TryParse(rawId, out var productId))
      {
          var cartId = ShoppingCartActions.GetCartId();
          var userCart = new Logic.CartManager();
          userCart.AddToCart(cartId, productId);
      }
      else
      {
        Debug.Fail("ERROR : We should never get to AddToCart.aspx without a ProductId.");
        throw new Exception("ERROR : It is illegal to load AddToCart.aspx without setting a ProductId.");
      }
      Response.Redirect("ShoppingCart.aspx");
    }
  }
}