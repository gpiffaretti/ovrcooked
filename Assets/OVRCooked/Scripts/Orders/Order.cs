using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.OVRCooked.Scripts.Orders
{
    public class Order
    {
        public event Action<Order> OrderExpired;

        public Recipe recipe;
        float timeLeftSeconds;

        public Order(Recipe recipe, float timeLeftSeconds)
        {
            this.recipe = recipe;
            this.timeLeftSeconds = timeLeftSeconds;
        }

        internal void ExpireOrder()
        {
            OrderExpired?.Invoke(this);
        }
    }
}
