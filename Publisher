using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAD
{
    class Publisher
    {
            public String PublisherName = "";
            public delegate void EventHandler(Publisher P, Message e);
            public event EventHandler EventTicked;
      
    }
    public void Send(Message newMessage, PublishSubscriberServer server )
    {
        server.buffer.Enqueue(newMessage);
    }
};
s
class Message
{
    public Message()
    {

    }
    public string topic;

}

class PublishSubscriberServer
{
    public PublishSubscriberServer()
    {

    }
    // buffererul de mesaje care vor fi primite de la publisher
    public Queue<Message> buffer = new Queue<Message>();
    // vor fi declarati noi subscriberi
}

