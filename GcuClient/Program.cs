using GcuClient.Wdr.ServiceReference;
using System.ServiceModel;

Console.WriteLine("Hello, Gcu!");

string userName = "1234"; // Format 1234 (API User, not your personal user)
string password = "Topsecret";

string fullUic = "378058406646";
string carAvvCode = "4133";
string trainnumber = "47112";

string reportId = "123456"; //Guid.NewGuid().ToString();

await SendDamageProtocoll();

//await GetRsds();

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
                MessageType = "5502", // Unclear
                MessageTypeVersion = "2.1.8" // Unclear
            },
            //SenderReference = userName//senderReference
        };

        GcuClient.Wdr.ServiceReference.AdministrativeContactInformation administrativeContactInformation = new()
        {
            Name = "Mickmouse GmbH",
            eMail = "info@test.de",
            FreeTextField = "Micky Mouse Street, 666125 Musterhausen",
        };

        GcuClient.Wdr.ServiceReference.WagonDamageReportMessageContactUserRU contactUserRU = new()
        {
            AdministrativeContactInformation = administrativeContactInformation,
        };

        GcuClient.Wdr.ServiceReference.ConsignmentIdent consignmentIdent = new()
        {
            Value = "AB 123 456 789", //Sendungsnummer

        };

        GcuClient.Wdr.ServiceReference.CountryCodeISO originCountryCodeISO = new()
        {
            Value = "XX",
        };
        GcuClient.Wdr.ServiceReference.CountryCodeISO destinationCountryCodeISO = new()
        {
            Value = "FR",
        };
        GcuClient.Wdr.ServiceReference.CountryCodeISO preditorCountryCodeISO = new()
        {
            Value = "FR",
        };

        GcuClient.Wdr.ServiceReference.LocationSubsidiaryCode originLocationSubsidiaryCode = new()
        {
            Value = "42",
        };



        GcuClient.Wdr.ServiceReference.LocationSubsidiaryIdentification originLocationSubsidiaryIdentification = new()
        {
            LocationSubsidiaryCode = originLocationSubsidiaryCode,
        };

        GcuClient.Wdr.ServiceReference.TrainIdentifier trainIdentifier = new()
        {
            Item = trainnumber,
        };
        GcuClient.Wdr.ServiceReference.CompositIdentifierPlannedType trainIdentifier3 = new()
        {
            StartDate = DateTime.Now,
        };

        GcuClient.Wdr.ServiceReference.TransportOperationalIdentifiers transportOperationalIdentifiers = new()
        {
            StartDate = DateTime.UtcNow,
            TimetableYear = "2024",
            Company = "TMPC",
            Variant = "545",
        };

        GcuClient.Wdr.ServiceReference.LocationIdent origin = new()
        {
            LocationPrimaryCode = "99999",
            PrimaryLocationName = "Frankfurt",
            //CountryCodeISO = originCountryCodeISO,
            //LocationSubsidiaryIdentification = originLocationSubsidiaryIdentification,
        };



        GcuClient.Wdr.ServiceReference.LocationIdent destination = new()
        {
            LocationPrimaryCode = "99999",
            PrimaryLocationName = "Idstein",
            //CountryCodeISO = destinationCountryCodeISO,
        };

        GcuClient.Wdr.ServiceReference.LocationIdent locationDamageDetection = new()
        {
            LocationPrimaryCode = "99999",
            PrimaryLocationName = "Niedernhausen",
            //CountryCodeISO = destinationCountryCodeISO,
        };
        GcuClient.Wdr.ServiceReference.HandingOverCompany handingOverCompany = new()
        {
            Item = "DB Cargo",
            ItemElementName = ItemChoiceType.CompanyName,
            CompanyType = HandingOverCompanyCompanyType.NonGCUsignatoryRU,
            CompanyTypeSpecified = true,
        };



        GcuClient.Wdr.ServiceReference.TransportInformation transportInfo = new()
        {
            ConsignmentNumber = consignmentIdent,
            DepartureDate = System.DateTime.Today,
            LoadingStatus = LoadingStatus.Item1,
            Destination = destination,
            TrainIdentifier = trainIdentifier,
            Origin = origin,
        };


        GcuClient.Wdr.ServiceReference.DamageDetection damageDetection = new()
        {
            Location = locationDamageDetection,
            DetectedUponHandover = false,
            DetectionDate = System.DateTime.Today,
            HandingOverCompany = handingOverCompany,
        };

        GcuClient.Wdr.ServiceReference.ExistingLabelsReportingRU existingLabelsReportingRU = new()
        {
            Item = "DB Cargo",
            ItemElementName = ItemChoiceType1.CompanyName,
        };

        GcuClient.Wdr.ServiceReference.ExistingLabels existingLabels = new()
        {
            DamageLabel = [LabelType.K],
            DateSpecified = true,
            Date = System.DateTime.Today,
            ReportingRU = existingLabelsReportingRU,
        };
        GcuClient.Wdr.ServiceReference.NewLabels newLabels = new()
        {
            DamageLabel = [LabelType.R1],
            WagonDetached = false,
            DispatchedToWorkshop = NewLabelsDispatchedToWorkshop.AfterUnloading,
        };
        GcuClient.Wdr.ServiceReference.AddressInformation adressInfomationPerpetrator = new()
        {
            Address = "Musterstraße 36",
            CityTown = "Musterhausen",
            PostalCode = "65894",
            CompanyName = "BRD GmbH",
            CountryCodeISO = preditorCountryCodeISO,
        };

        GcuClient.Wdr.ServiceReference.DamageDescriptionCategoryPerpetrator damageDescriptionCategoryPerpetrator = new()
        {
            Remarks = "this is a free text",
            AddressInformation = adressInfomationPerpetrator,
            AdministrativeContactInformation = administrativeContactInformation,



        };
        GcuClient.Wdr.ServiceReference.DamageDescriptionCategory damageDescriptionCategory = new()
        {
            DamageCategory = DamageDescriptionCategoryDamageCategory.Wear,
            Perpetrator = damageDescriptionCategoryPerpetrator,

        };
        GcuClient.Wdr.ServiceReference.Damage codeDigits = new()
        {
            CodeDigit1 = "1",
            CodeDigit2 = "2",
            CodeDigit3 = "3",
            CodeDigit4 = "4",
            OldDamage = false,
            OldDamageSpecified = false,
        };
        GcuClient.Wdr.ServiceReference.DamageDescription damageDescription = new()
        {
            Remarks = "free Stuff as Comment",
            Category = damageDescriptionCategory,
            Damage = [codeDigits]

        };

        bool attachments = false;

        GcuClient.Wdr.ServiceReference.wdrRequest request = new(msgHeader,
                                                                reportId,
                                                                userName,
                                                                contactUserRU,
                                                                carAvvCode,
                                                                fullUic,
                                                                transportInfo,
                                                                damageDetection,
                                                                existingLabels,
                                                                newLabels,
                                                                damageDescription,
                                                                attachments);

        EndpointAddress ea = new("https://stage.gcubroker.org/wdr"); // https://prod.gcubroker.org/wdr

        GcuClient.Wdr.ServiceReference.WdrEndpointClient client = new(httpBinding, ea);
        client.ClientCredentials.UserName.UserName = userName;
        client.ClientCredentials.UserName.Password = password;

        GcuClient.Wdr.ServiceReference.wdrResponse response = await client.wdrAsync(request);

        /*
         * Work with the resonse from here.
         */

        string Test = "Test";
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