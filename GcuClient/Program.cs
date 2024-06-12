// See https://aka.ms/new-console-template for more information
using System.ServiceModel;

Console.WriteLine("Hello, Gcu!");

string userName = "1234"; // Format 1234 (API User, not your personal user)
string password = "Top$ecret123!"; 

string messageIdentifier = Guid.NewGuid().ToString();
string senderReference = Guid.NewGuid().ToString();

try
{
    BasicHttpBinding httpBinding = new();
    httpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
    httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
    httpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

    MessageHeader msgHeader = new()
    {
        MessageRoutingID = "1",
        Sender = new Sender()
        {
            CI_InstanceNumber = "1", 
            Value = userName // Sender = your username
        },
        Recipient = new Recipient()
        {
            CI_InstanceNumber = "1",
            Value = "4000" // 4000 = GCU Broker 
        },
        MessageReference = new MessageReference()
        {
            MessageDateTime = DateTime.Now,
            MessageIdentifier = messageIdentifier,
            MessageType = "6004", // Unclear
            MessageTypeVersion = "RSRDM0100" // Unclear
        },
        SenderReference = senderReference
    };
    
    rsdsRequest request = new(msgHeader, new string[] { "378058406646", "378449605864", "378449609650", "378449609676", "218007311770" });
 
    EndpointAddress ea = new("https://prod.gcubroker.org/rsds"); // https://stage.gcubroker.org/rsds

    RsdsEndpointClient client = new(httpBinding, ea);
    client.ClientCredentials.UserName.UserName = userName;
    client.ClientCredentials.UserName.Password = password;

    rsdsResponse response = await client.rsdsAsync(request);

    /*
     * Work with the resonse from here.
     */    
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
