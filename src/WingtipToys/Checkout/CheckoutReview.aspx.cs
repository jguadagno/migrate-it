using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Domain.Models;
using WingtipToys.Logic;
using WingtipToys.Models;

namespace WingtipToys.Checkout
{
    public partial class CheckoutReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                NVPAPICaller payPalCaller = new NVPAPICaller();

                string retMsg = "";
                string token = "";
                string PayerID = "";
                NVPCodec decoder = new NVPCodec();
                token = Session["token"].ToString();

                bool ret = payPalCaller.GetCheckoutDetails(token, ref PayerID, ref decoder, ref retMsg);
                if (ret)
                {
                    Session["payerId"] = PayerID;

                    var myOrder = new Order();
                    myOrder.OrderDate = Convert.ToDateTime(decoder["TIMESTAMP"].ToString());
                    myOrder.Username = User.Identity.Name;
                    myOrder.FirstName = decoder["FIRSTNAME"].ToString();
                    myOrder.LastName = decoder["LASTNAME"].ToString();
                    myOrder.Address = decoder["SHIPTOSTREET"].ToString();
                    myOrder.City = decoder["SHIPTOCITY"].ToString();
                    myOrder.State = decoder["SHIPTOSTATE"].ToString();
                    myOrder.PostalCode = decoder["SHIPTOZIP"].ToString();
                    myOrder.Country = decoder["SHIPTOCOUNTRYCODE"].ToString();
                    myOrder.Email = decoder["EMAIL"].ToString();
                    myOrder.Total = Convert.ToDecimal(decoder["AMT"].ToString());

                    // Verify total payment amount as set on CheckoutStart.aspx.
                    try
                    {
                        decimal paymentAmountOnCheckout = Convert.ToDecimal(Session["payment_amt"].ToString());
                        decimal paymentAmourFromPayPal = Convert.ToDecimal(decoder["AMT"].ToString());
                        if (paymentAmountOnCheckout != paymentAmourFromPayPal)
                        {
                            Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                        }
                    }
                    catch (Exception)
                    {
                        Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                    }

                    var cartId = ShoppingCartActions.GetCartId();
                    var cartManager = new Logic.CartManager();
                    var orderSaved = cartManager.AddOrder(myOrder);

                    var cartItems = cartManager.GetCartItems(cartId);

                    // Get the shopping cart items and process them.


                    // Add OrderDetail information to the DB for each product purchased.
                    for (int i = 0; i < cartItems.Count; i++)
                    {
                        // Create a new OrderDetail object.
                        var myOrderDetail = new OrderDetail();
                        myOrderDetail.OrderId = myOrder.OrderId;
                        myOrderDetail.Username = User.Identity.Name;
                        myOrderDetail.ProductId = cartItems[i].ProductId;
                        myOrderDetail.Quantity = cartItems[i].Quantity;
                        myOrderDetail.UnitPrice = cartItems[i].Product.UnitPrice;

                        var cartItemSaved = cartManager.SaveOrderDetail(myOrderDetail);
                    }

                    // Set OrderId.
                    Session["currentOrderId"] = myOrder.OrderId;

                    // Display Order information.
                    List<Order> orderList = new List<Order> { myOrder };
                    ShipInfo.DataSource = orderList;
                    ShipInfo.DataBind();

                    // Display OrderDetails.
                    OrderItemList.DataSource = cartItems;
                    OrderItemList.DataBind();

                }
                else
                {
                    Response.Redirect("CheckoutError.aspx?" + retMsg);
                }
            }
        }

        protected void CheckoutConfirm_Click(object sender, EventArgs e)
        {
            Session["userCheckoutCompleted"] = "true";
            Response.Redirect("~/Checkout/CheckoutComplete.aspx");
        }
    }
}