using System;
using Hanssens.Net;

namespace AvaloniaClientMetal.Models;

public static class PreparedLocalStorage
{
    private static readonly LocalStorageConfiguration Config = new LocalStorageConfiguration()
    {
        EnableEncryption = true,
        EncryptionSalt = "salt",
        AutoLoad = true,
        AutoSave = true
    };

    private static LocalStorage _encryptedStorage = new LocalStorage(Config, "superP@ssw0rd");

    public static void PutTokenPairFromLocalStorage(TokenPair tokenPair)
    {
        _encryptedStorage.Store("TokenPair", tokenPair);
        SaveLocalStorage();
    }

    public static TokenPair GetTokenPairFromLocalStorage()
    {
        TokenPair tokenPair = _encryptedStorage.Get<TokenPair>("TokenPair") ?? throw new InvalidOperationException();
        return tokenPair;
    }

    public static void LoadLocalStorage()
    {
        _encryptedStorage.Load();
    }

    public static void SaveLocalStorage()
    {
        _encryptedStorage.Persist();
    }

    public static void ClearLocalStorage()
    {
        _encryptedStorage.Clear();
    }

    public static bool CheckValidTokenInLocalStorage()
    {
        return (DateTime.Now - GetTokenPairFromLocalStorage().CreationDateTime).TotalSeconds > GetTokenPairFromLocalStorage().ExpiredInAccessToken;
    }
}