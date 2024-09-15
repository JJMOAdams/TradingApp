using OtpNet;
using QRCoder;
using TradingApp.Interfaces;

namespace TradingApp.Services
{
    public class ToTPService : IToTPService
    {
        public string GenerateSecret()
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        public string GenerateQrCodeUri(string username, string secret)
        {
            // This is the URI format that the authenticator app will understand
            string otpUri = $"otpauth://totp/{Uri.EscapeDataString("Jake's Trading App")}:{Uri.EscapeDataString(username)}?secret={secret}&issuer={Uri.EscapeDataString("Jake's Trading App")}&digits=6";
            return otpUri;
        }

        public byte[] GenerateQrCode(string otpUri)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q);
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(20);
                }
            }
        }

        public bool ValidateTotp(string secret, string code)
        {
            // Convert the secret back from Base32 encoding
            var key = Base32Encoding.ToBytes(secret);

            // Create a new TOTP instance using the secret key
            var totp = new Totp(key);

            // Verify the TOTP code provided by the user
            return totp.VerifyTotp(code, out long timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);
        }
    }
}