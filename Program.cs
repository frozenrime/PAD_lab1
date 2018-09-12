using System;

class Program
{
    static void main(string[] args)
    {
        Publisher Publisher1 = new Publisher();
        Publisher Publisher2 = new Publisher();

        PublishSubscriberServer server = new PublishSubscriberServer();

        Message publisher1Message = new Message();
        publisher1Message.topic = "Test";

        Mesage publisher2Message = new Mesage();
        publisher2Message.topic = "Test2";

        Publisher1.Send(publisher1Message, server);
        Publisher2.Send(publisher2Message, server);
    }
}
