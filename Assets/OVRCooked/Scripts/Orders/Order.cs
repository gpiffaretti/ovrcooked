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
        public event Action<Order> OrderDelivered;

        public Recipe recipe;
        float totalTime;
        float timeLeft;

        public float TotalTime { get { return totalTime; } }
        public float TimeLeft { get { return timeLeft; } }

        public Order(Recipe recipe, float totalTime)
        {
            this.recipe = recipe;
            this.totalTime = totalTime;

            this.timeLeft = this.totalTime;
        }

        public void UpdateOrderTime(float deltaTime) 
        {
            timeLeft -= deltaTime;

            if (timeLeft <= 0f) 
            {
                ExpireOrder();
            }
        }

        internal void ExpireOrder()
        {
            OrderExpired?.Invoke(this);
        }

        public void DeliverOrder() 
        {
            OrderDelivered?.Invoke(this);
        }
    }
}
