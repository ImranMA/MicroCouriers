using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.MicroCouriers.BuildingBlocks.EventBus.Abstractions;
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
        const string ServiceBusConnectionString = "";
        const string TopicName = "microcourier";
        static ITopicClient topicClient;       
        private const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";

        public Shipping()
        {
            InitializeComponent();
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);        
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks!");
        }

        private void btn_UpdateStatus_Click(object sender, EventArgs e)
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
                var orderPicked = new OrderDeliveredIntegrationEvent(txtBookingID.Text, txtDesc.Text,"");
                Publish(orderPicked);
            }
        }

        private void btn_getBooking_Click(object sender, EventArgs e)
        {
            try
            {
                var bookingID = txtBookingID.Text;
                string URL = "http://localhost:5002/api/tracking/" + bookingID;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.ContentType = "application/json; charset=utf-8";
                //request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("username:password"));
                // request.PreAuthenticate = true;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    // Console.WriteLine();
                    txtBooingStatus.Text = reader.ReadToEnd();
                }
            }
            catch(Exception ex) { }
        }
    }
}
