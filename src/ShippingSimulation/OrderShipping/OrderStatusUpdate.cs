using System;
using System.Text;
using System.Windows.Forms;
using Microsoft.Azure.ServiceBus;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Events;
using Newtonsoft.Json;
using OrderShipping.Events;
using System.Net;
using System.IO;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace OrderShipping
{
    public partial class Shipping : Form
    {
        //We will use this app to simulate the Order Progression
        //e.g. if order is moved from booking to Order picked or order in transit
        const string ServiceBusConnectionString = "";
        const string TopicName = "microcouriers-topic";
        static ITopicClient topicClient;       
        private const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";

        public Shipping()
        {
            InitializeComponent();
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);        
        }


        //Events are published
        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFIX, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Microsoft.Azure.ServiceBus.Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName,
            };

            //var topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            topicClient.SendAsync(message)
                .GetAwaiter()
                .GetResult();
        }


        //Update the order status
        private void btn_UpdateStatus_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Please Wait !";
            try
            {
                if (cmbOrderStatus.Text.ToLower().Contains("picked"))
                {
                    var orderPicked = new OrderPickedIntegrationEvent(txtBookingID.Text, txtDesc.Text);
                    Publish(orderPicked);
                }

                if (cmbOrderStatus.Text.ToLower().Contains("transit"))
                {
                    var orderPicked = new OrderTransitIntegrationEvent(txtBookingID.Text, txtDesc.Text);
                    Publish(orderPicked);
                }


                if (cmbOrderStatus.Text.ToLower().Contains("delivered"))
                {
                    var orderPicked = new OrderDeliveredIntegrationEvent(txtBookingID.Text, txtDesc.Text, txtSignedBy.Text);
                    Publish(orderPicked);
                }

                lblStatus.Text = "Order Updated";

            }
            catch(Exception ex)
            {
                lblStatus.Text = "Some Problem " + ex.Message;
            }
           
        }

        private void Shipping_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
             
        }
    }
}
