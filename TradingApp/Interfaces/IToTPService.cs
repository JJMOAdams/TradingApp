namespace TradingApp.Interfaces
{
    public interface IToTPService
    {
        string GenerateSecret();
        string GenerateQrCodeUri(string username, string secret);
        byte[] GenerateQrCode(string optUri);  
        bool ValidateTotp(string secret, string code);      
    }
}