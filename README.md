# xamarin-android-mms-pdu-manager
A simple Xamarin.Android Bindings Library for parsing and composing PDUs

This is a wrapper library of the project at https://github.com/yasmanillanes/mms-pdu-manager

This repository also contains a sample project that demonstrates how to parse and compose PDUs from and into byte arrays respectively.

To compose a PDU into a byte array to send to an MMS Service Provider:

```c#
// First instantiate the PDU we will be turning into a byte array that
// will be ultimately sent to the MMS service provider.
// In this case, we are using a SendReq PDU type.
SendReq sendRequestPdu = new SendReq();
// Set the recipient number of our MMS
sendRequestPdu.AddTo(new EncodedStringValue("555-555-5555"));
// Instantiate the body of our MMS
PduBody pduBody = new PduBody();
// Attach a sample image to our MMS body
byte[] sampleImageData = LoadImageBytes();
PduPart sampleImagePduPart = new PduPart();
sampleImagePduPart.SetData(sampleImageData);
sampleImagePduPart.SetContentType(new EncodedStringValue("image/png").GetTextString());
sampleImagePduPart.SetFilename(new EncodedStringValue(SampleImageFileName).GetTextString());
pduBody.AddPart(sampleImagePduPart);
// Set the body of our MMS
sendRequestPdu.Body = pduBody;
// Finally, generate the byte array to send to the MMS provider
PduComposer composer = new PduComposer(sendRequestPdu);
byte[] pduData = composer.Make();
// Send pduData to service provider
```

To parse a PDU from a byte array we receive from an MMS Service Provider:
```c#
// First, load the bytes of our sample PDU that we usually get from the service provider
byte[] samplePduData = GetSampleData();
// Parse the byte array into a PDU object we can process
PduParser pduParser = new PduParser(samplePduData);
GenericPdu genericPdu = pduParser.Parse();
// Cast the PDU into a more specific type. In this case a NotificationInd type
NotificationInd notificationPdu = (NotificationInd)genericPdu;
```
