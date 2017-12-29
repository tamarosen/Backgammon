using BackgammonProject.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace BackgammonProject.Services
{
    class MessageDispatcher
    {
        private AppContext context;
        private Task connectTask;

        public MessageDispatcher(AppContext context)
        {
            this.context = context;
            this.context.Dispatcher = this;
        }

        public void EstablishConnection()
        {
            context.MyWebSocket = new MessageWebSocket();

            context.MyWebSocket.Control.MessageType = SocketMessageType.Utf8;
            context.MyWebSocket.MessageReceived += WebSocket_MessageReceived;
            context.MyWebSocket.Closed += WebSocket_Closed;

            try
            {
                connectTask = context.MyWebSocket.ConnectAsync(new Uri("ws://localhost:36453")).AsTask();
                connectTask.ContinueWith(_ => SendMessageUsingMessageWebSocketAsync(ModelXmlMapper.GetAsXmlString(new User(context.CurrentUser))));
                // connectTask.Wait();
            }
            catch (Exception ex)
            {
                Windows.Web.WebErrorStatus webErrorStatus = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                // Add additional code here to handle exceptions.
            }
        }

        private async Task SendMessageUsingMessageWebSocketAsync(string message)
        {
            using (var dataWriter = new DataWriter(context.MyWebSocket.OutputStream))
            {
                dataWriter.WriteString(message);
                try
                {
                    await dataWriter.StoreAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                }
                dataWriter.DetachStream();
            }
            Debug.WriteLine("Sending message using MessageWebSocket: " + message);
        }

        public async void SendObjectAsync(AbstractXmlSerializable obj)
        {
            string serialized = ModelXmlMapper.GetAsXmlString(obj);
            await connectTask.ContinueWith(_ => SendMessageUsingMessageWebSocketAsync(serialized));
        }

        private void WebSocket_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            string serialized = "";
            try
            {
                using (DataReader dataReader = args.GetDataReader())
                {
                    dataReader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    serialized = dataReader.ReadString(dataReader.UnconsumedBufferLength);
                    Debug.WriteLine("Message received from MessageWebSocket: " + serialized);
                }
            }
            catch (Exception ex)
            {
                Windows.Web.WebErrorStatus webErrorStatus = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                // Add additional code here to handle exceptions.
            }

            DispatchMessage(serialized);
        }

        private void WebSocket_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            Debug.WriteLine("WebSocket_Closed; Code: " + args.Code + ", Reason: \"" + args.Reason + "\"");
            context.MyWebSocket.Dispose();
            context.MyWebSocket = null;
        }

        public async void DispatchMessage(string serialized)
        {
            XDocument xmlDoc = null;
            try
            {
                xmlDoc = XDocument.Parse(serialized);
            } catch (Exception ex)
            {
                return;
            }

            // initially assume the recieved message is of type Message:
            Message msg = new Message();
            if (msg.FromXml(xmlDoc.Root))
            {
                string message = msg.From + ": " + msg.Content;
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        context.ChatWinodw.MessageReceived(message);
                    }
                );
            }
        }
    }
}
