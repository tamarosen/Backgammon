using BackgammonProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace BackgammonProject.Services
{
    class MessageDispatcher
    {
        private TalkBackAppContext context;
        private Task connectTask;

        public MessageDispatcher(TalkBackAppContext context)
        {
            this.context = context;
            this.context.Dispatcher = this;
        }

        public async void EstablishConnectionAsync()
        {
            while (context.MyWebSocket != null)
            {
                await Task.Delay(1000);

            }
            context.MyWebSocket = new MessageWebSocket();

            context.MyWebSocket.Control.MessageType = SocketMessageType.Utf8;
            context.MyWebSocket.MessageReceived += WebSocket_MessageReceived;
            context.MyWebSocket.Closed += WebSocket_Closed;

            try
            {
                connectTask = context.MyWebSocket.ConnectAsync(new Uri("ws://localhost:8123")).AsTask();
            }
            catch (Exception ex)
            {
                Windows.Web.WebErrorStatus webErrorStatus = WebSocketError.GetStatus(ex.GetBaseException().HResult);
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

        public async void SendListAsync(IList<AbstractXmlSerializable> list)
        {
            string serialized = ModelXmlMapper.GetAsXmlString(list);
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
            if (context.MyWebSocket != null)
            {
                context.MyWebSocket.Dispose();
                context.MyWebSocket = null;
            }
        }

        public void DispatchMessage(string serialized)
        {
            XDocument xmlDoc = null;
            try
            {
                xmlDoc = XDocument.Parse(serialized);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                return;
            }

            // First check if we got array:
            if (xmlDoc.Root.Name.LocalName.Equals("Array"))
            {

                DispatchListMsg(xmlDoc.Root);
            }
            else
            {
                DispatchSingleObjectMsg(xmlDoc.Root);
            }
        }

        private async void DispatchListMsg(XElement xmlList)
        {
            XElement first = (XElement)xmlList.FirstNode;
            ModelXmlMapper.MappedType typeInList = GetMappedType(first);

            IList<AbstractXmlSerializable> list = ModelXmlMapper.FromArrayXml(xmlList);

            switch (typeInList)
            {
                case ModelXmlMapper.MappedType.UNDEFINED:
                case ModelXmlMapper.MappedType.CONTACT:
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () =>
                        {
                            context.LoginWindow.OnContactsListReceived(list);
                        }
                    );
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        private static ModelXmlMapper.MappedType GetMappedType(XElement first)
        {
            ModelXmlMapper.MappedType typeInElement = ModelXmlMapper.MappedType.UNDEFINED;
            if (first != null)
            {
                XElement firstElementType = first.Element("Type");
                if (firstElementType != null)
                {
                    if (!ModelXmlMapper.map.TryGetValue(firstElementType.Value, out typeInElement))
                    {
                        typeInElement = ModelXmlMapper.MappedType.UNDEFINED;
                    }
                }
            }

            return typeInElement;
        }

        private async void DispatchSingleObjectMsg(XElement xmlObj)
        {
            ModelXmlMapper.MappedType typeInElement = GetMappedType(xmlObj);

            switch (typeInElement)
            {
                case ModelXmlMapper.MappedType.MESSAGE:
                    MessageModel msg = new MessageModel();
                    if (msg.FromXml(xmlObj))
                    {
                        string message = msg.From + ": " + msg.Content;
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            () =>
                            {
                                context.ChatWinodw.MessageReceived(message);
                            }
                        );
                    }
                    break;
                case ModelXmlMapper.MappedType.CHAT_REQUEST:
                    ChatRequest req = new ChatRequest();
                    if (req.FromXml(xmlObj))
                    {
                        // TODO: check if ongoing chat and return failure from here:
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            () =>
                            {
                                context.ContactsWindow.ChatRequestReceived(req);
                            }
                        );
                    }
                    break;
                case ModelXmlMapper.MappedType.CHAT_REQUEST_RESPONSE:
                    ChatRequestResponse resp = new ChatRequestResponse();
                    if (resp.FromXml(xmlObj))
                    {
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            () =>
                            {
                                context.ContactsWindow.ChatResponseReceived(resp);
                                // context.ChatWinodw.Disconnected(resp);
                            }
                        );
                    }
                    break;
                case ModelXmlMapper.MappedType.LOGIN_RESPONSE:
                    LoginResponse loginResp = new LoginResponse();
                    if (loginResp.FromXml(xmlObj))
                    {
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            () =>
                            {
                                context.LoginWindow.OnLoginResponseAsync(loginResp);
                            }
                        );
                    }
                    break;
                case ModelXmlMapper.MappedType.CONTACT:
                    Contact contact = new Contact();
                    if (contact.FromXml(xmlObj))
                    {
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal,
                            () =>
                            {
                                if (context.ContactsWindow != null)
                                    context.ContactsWindow.OnContactUpdate(contact);
                            }
                        );
                    }
                    break;
                default:
                    // do nothing
                    break;
            }
        }
    }
}
