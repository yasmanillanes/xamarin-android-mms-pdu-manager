using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Com.Google.Mms.Pdu;
using System.IO;

namespace Android.Mms.PduManagement.Sample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private const string SampleImageFileName = "mms.png";
        private const string SamplePduFileName = "pdu.dat";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.activity_main);
            this.SendMmsSample();
            this.ReadMmsSample();
        }

        private void SendMmsSample()
        {
            // First instantiate the SendReq PDU we will be turning into a byte array that
            // will be ultimately sent to the MMS service provider.
            SendReq sendRequestPdu = new SendReq();
            // Set the recipient number of our MMS
            sendRequestPdu.AddTo(new EncodedStringValue("555-555-5555"));
            // Instantiate the body of our MMS
            PduBody pduBody = new PduBody();
            // Attach a sample image to our MMS body
            Stream sampleImageStream = this.Assets.Open(SampleImageFileName);
            MemoryStream imageMemoryStream = new MemoryStream();
            sampleImageStream.CopyTo(imageMemoryStream);
            byte[] sampleImageData = imageMemoryStream.ToArray();
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
            int pduDataLength = pduData.Length;
            Toast.MakeText(this, $"Successfully composed a PDU byte array of size: {pduDataLength.ToString("N")} bytes", ToastLength.Long).Show();
        }

        private void ReadMmsSample()
        {
            // First, load the bytes of our sample PDU that we usually get from the service
            // provider when receiving an MMS
            Stream samplePduStream = this.Assets.Open(SamplePduFileName);
            MemoryStream samplePduMemoryStream = new MemoryStream();
            samplePduStream.CopyTo(samplePduMemoryStream);
            byte[] samplePduData = samplePduMemoryStream.ToArray();
            // Parse the byte array into a PDU object we can process
            PduParser pduParser = new PduParser(samplePduData);
            GenericPdu genericPdu = pduParser.Parse();
            // In this case we know that our sample PDU is of type SendReq, so we can cast
            // it to that.
            // Note that in this case we are loading the same SendReq PDU from the
            // SendMmsSample() method, but normally what we get from MMS service providers
            // are NotificationInd PDUs, RetrieveConf PDUs, and so on.
            SendReq sendRequestPdu = (SendReq)genericPdu;
            Toast.MakeText(this, $"Successfully parsed a PDU with transaction ID: {sendRequestPdu.GetTransactionId()}", ToastLength.Long).Show();
        }

    }
}

