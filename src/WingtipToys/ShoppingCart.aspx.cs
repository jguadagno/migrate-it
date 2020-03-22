using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Logic;
using System.Collections.Specialized;
using System.Collections;
using System.Web.ModelBinding;
using WingtipToys.Domain.Models;

namespace WingtipToys
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var cartManager = new Logic.CartManager();
            var cartId = ShoppingCartActions.GetCartId();
            var cartTotal = cartManager.GetCartTotal(cartId);

            if (cartTotal > 0)
            {
                // Display Total.
                lblTotal.Text = $"{cartTotal:c}";
            }
            else
            {
                LabelTotalText.Text = "";
                lblTotal.Text = "";
                ShoppingCartTitle.InnerText = "Shopping Cart is Empty";
                UpdateBtn.Visible = false;
                CheckoutImageBtn.Visible = false;
            }
        }

        public List<CartItem> GetShoppingCartItems()
        {
            var cartManager = new Logic.CartManager();
            return cartManager.GetCartItems(ShoppingCartActions.GetCartId());
        }

        public List<CartItem> UpdateCartItems()
        {
            var cartManager = new Logic.CartManager();
            var cartId = ShoppingCartActions.GetCartId();

            Domain.Models.ShoppingCartUpdates[] cartUpdates = new Domain.Models.ShoppingCartUpdates[CartList.Rows.Count];
            for (int i = 0; i < CartList.Rows.Count; i++)
            {
                IOrderedDictionary rowValues = new OrderedDictionary();
                rowValues = GetValues(CartList.Rows[i]);
                cartUpdates[i].ProductId = Convert.ToInt32(rowValues["ProductID"]);

                CheckBox cbRemove = new CheckBox();
                cbRemove = (CheckBox)CartList.Rows[i].FindControl("Remove");
                cartUpdates[i].RemoveItem = cbRemove.Checked;

                TextBox quantityTextBox = new TextBox();
                quantityTextBox = (TextBox)CartList.Rows[i].FindControl("PurchaseQuantity");
                cartUpdates[i].PurchaseQuantity = Convert.ToInt16(quantityTextBox.Text.ToString());
            }
            cartManager.UpdateShoppingCartDatabase(cartId, cartUpdates);
            CartList.DataBind();
            lblTotal.Text = $"{cartManager.GetCartTotal(cartId):c}";
            return cartManager.GetCartItems(cartId);

        }

        public static IOrderedDictionary GetValues(GridViewRow row)
        {
            IOrderedDictionary values = new OrderedDictionary();
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.Visible)
                {
                    // Extract values from the cell.
                    cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
                }
            }
            return values;
        }

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateCartItems();
        }

        protected void CheckoutBtn_Click(object sender, ImageClickEventArgs e)
        {
            var cartManager = new Logic.CartManager();
            var cartTotal = cartManager.GetCartTotal(ShoppingCartActions.GetCartId());

            Session["payment_amt"] = cartTotal;

            Response.Redirect("Checkout/CheckoutStart.aspx");
        }
    }
}