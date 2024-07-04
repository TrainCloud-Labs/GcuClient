using System.ServiceModel;

Console.WriteLine("Hello, Gcu!");

string userName = "1234"; // Format 1234 (API User, not your personal user)
string password = "Top$ecret123!";

await SendDamageProtocoll();

await GetRsds();

/// Sends a "Schadwagen-Protokoll"
async Task SendDamageProtocoll()
{
    try
    {
        string messageIdentifier = Guid.NewGuid().ToString();
        string senderReference = Guid.NewGuid().ToString();

        BasicHttpBinding httpBinding = new();
        httpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
        httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
        httpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

        GcuClient.Wdr.ServiceReference.MessageHeader msgHeader = new()
        {
            MessageRoutingID = "1",
            Sender = new GcuClient.Wdr.ServiceReference.Sender()
            {
                CI_InstanceNumber = "1",
                Value = userName // Sender = your username
            },
            Recipient = new GcuClient.Wdr.ServiceReference.Recipient()
            {
                CI_InstanceNumber = "1",
                Value = "4000" // 4000 = GCU Broker 
            },
            MessageReference = new GcuClient.Wdr.ServiceReference.MessageReference()
            {
                MessageDateTime = DateTime.Now,
                MessageIdentifier = messageIdentifier,
                MessageType = "6004", // Unclear
                MessageTypeVersion = "RSRDM0100" // Unclear
            },
            SenderReference = senderReference
        };

        GcuClient.Wdr.ServiceReference.WagonDamageReportMessageContactUserRU contactUserRU = new();
        GcuClient.Wdr.ServiceReference.TransportInformation transportInfo = new();
        GcuClient.Wdr.ServiceReference.DamageDetection damageDetection = new();
        GcuClient.Wdr.ServiceReference.ExistingLabels existingLabels = new();
        GcuClient.Wdr.ServiceReference.NewLabels newLabels = new();
        GcuClient.Wdr.ServiceReference.DamageDescription damageDescription = new();
        bool attachments = false;

        GcuClient.Wdr.ServiceReference.wdrRequest request = new(msgHeader,
                                                                "reportId",
                                                                "UserRU",
                                                                contactUserRU,
                                                                "Keeper",
                                                                "WagonNumberFreight",
                                                                transportInfo,
                                                                damageDetection,
                                                                existingLabels,
                                                                newLabels,
                                                                damageDescription,
                                                                attachments);

        EndpointAddress ea = new("https://prod.gcubroker.org/wdr"); // https://stage.gcubroker.org/wdr

        GcuClient.Wdr.ServiceReference.WdrEndpointClient client = new(httpBinding, ea);
        client.ClientCredentials.UserName.UserName = userName;
        client.ClientCredentials.UserName.Password = password;

        GcuClient.Wdr.ServiceReference.wdrResponse response = await client.wdrAsync(request);

        /*
         * Work with the resonse from here.
         */
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}


/// Gets RSDS² from GCU Broker
async Task GetRsds()
{
    try
    {
        string messageIdentifier = Guid.NewGuid().ToString();
        string senderReference = Guid.NewGuid().ToString();

        BasicHttpBinding httpBinding = new();
        httpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
        httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
        httpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

        GcuClient.Rsds.ServiceReference.MessageHeader msgHeader = new()
        {
            MessageRoutingID = "1",
            Sender = new GcuClient.Rsds.ServiceReference.Sender()
            {
                CI_InstanceNumber = "1",
                Value = userName // Sender = your username
            },
            Recipient = new GcuClient.Rsds.ServiceReference.Recipient()
            {
                CI_InstanceNumber = "1",
                Value = "4000" // 4000 = GCU Broker 
            },
            MessageReference = new GcuClient.Rsds.ServiceReference.MessageReference()
            {
                MessageDateTime = DateTime.Now,
                MessageIdentifier = messageIdentifier,
                MessageType = "6004", // Unclear
                MessageTypeVersion = "RSRDM0100" // Unclear
            },
            SenderReference = senderReference
        };

        GcuClient.Rsds.ServiceReference.rsdsRequest request = new(msgHeader, new string[] { "378058406646", "378449605864", "378449609650", "378449609676", "218007311770" });

        EndpointAddress ea = new("https://prod.gcubroker.org/rsds"); // https://stage.gcubroker.org/rsds

        GcuClient.Rsds.ServiceReference.RsdsEndpointClient client = new(httpBinding, ea);
        client.ClientCredentials.UserName.UserName = userName;
        client.ClientCredentials.UserName.Password = password;

        GcuClient.Rsds.ServiceReference.rsdsResponse response = await client.rsdsAsync(request);

        /*
         * Work with the resonse from here.
         */
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}