namespace StoreApi.Shared.Services.Cryptography;

public interface ICryptography{
    
    public string Encrypt(string data);

    public string Decrypt(string data);
    
}