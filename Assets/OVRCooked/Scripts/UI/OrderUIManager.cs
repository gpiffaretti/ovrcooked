using Assets.OVRCooked.Scripts.Orders;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.OVRCooked.Scripts.UI
{
    public class OrderUIManager : Singleton<OrderUIManager>
    {
        [SerializeField]
        OrderDisplay orderDisplayPrefab;

        [SerializeField]
        GameObject ordersParent;

        List<OrderDisplay> activeOrders = new List<OrderDisplay>();

        private void Start()
        {
            OrderManager.Instance.OrderCreated += OnOrderCreated;
        }

        private void OnOrderCreated(Order order)
        {
            // Instantiate OrderDisplay with proper parameter
            order.OrderExpired += OnOrderExpired;
            order.OrderDelivered += OnOrderDelivered;
            var orderDisplay = Instantiate<OrderDisplay>(orderDisplayPrefab, ordersParent.transform);
            orderDisplay.order = order;
            orderDisplay.Initialize(order);

            activeOrders.Add(orderDisplay);
        }

        private void OnOrderExpired(Order order)
        {
            var activeOrder = activeOrders.Single(x => x.order == order);
            activeOrders.Remove(activeOrder);
            Destroy(activeOrder.gameObject);
        }

        private void OnOrderDelivered(Order order)
        {
            var activeOrder = activeOrders.Single(x => x.order == order);
            activeOrders.Remove(activeOrder);
            Destroy(activeOrder.gameObject);
        }

        private void Update()
        {
            
        }
    }
}
