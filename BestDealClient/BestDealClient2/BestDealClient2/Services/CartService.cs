using BestDealClient2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestDealClient2.Services
{
    /// <summary>
    /// A service class responsible for managing a shopping cart.
    /// </summary>
    public class CartService
    {

        /// <summary>
        /// The current cart instance.
        /// </summary>
        private static Cart _currentCart;

        /// <summary>
        /// Property to get or set the current cart.
        /// If there is no current cart when trying to get it, a new cart will be created.
        /// </summary>
        public static Cart CurrentCart
        {
            get
            {
                // If the cart does not exist, return a new cart.
                if (_currentCart == null)
                {
                    return new Cart();
                }

                // If the cart exists, return the current cart.
                return _currentCart;
            }
            set
            {
                // Update the current cart.
                _currentCart = value;
            }
        }
    }
}
