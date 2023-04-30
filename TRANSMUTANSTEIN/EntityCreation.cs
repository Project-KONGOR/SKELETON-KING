namespace TRANSMUTANSTEIN;

public class EntityCreation
{
    /// <summary>
    ///     Create a fake user with fairly complete dummy data.
    /// </summary>
    public static ElementUser CreateFakeElementUser()
        => new(
            // Precomputed Values: https://pastebin.com/c0TTziD6
            salt: "5dc38946324a5f866d4fcaf5f5c2777d52e168e8601f01bd7861d957455b6bee3f6e9617c0f5e958daff3ddc36f1f3ad075052ee13db0a9d77bbf6683786b9ffccdfbdf849370648f527442d66752ba07e1bfad5a5387c3be7f4df7d41425c78fbd2762502453fa491c9045bbe71fdce33d9d5afe1cc19d0d2515c389708cf3d2c9069b69e39de1580a567b651848ea7fdbcade3157b5c69caa53f886e36363e82fefce1a7f06ec333028d183c67da9aeef1cb6237774b85af230213e3159d34990cfdcabba62bfda64ccdcac45f278925044f834e38c5b271d48f652b6350bd5a82efac1e591ad2645c96c1652625d917a14753061670779afd69a5721a31ce",
            passwordSalt: "b79bd4a832b46152f09ac9",
            hashedPassword: "45b9467c354f712fa8b6175f6b6e82da5cb09a90fcfbda1e1d053dfc6aa41f5a"
        );

    public static Account CreateFakeAccountAndSave(string name, BountyContext bountyContext)
    {
        ElementUser user = CreateFakeElementUser();

        Account account = new Account()
        {
            Name = name,
            User = user,
            Cookie = "cookie",
        };

        user.Accounts.Add(account);

        bountyContext.Users.Add(user);
        bountyContext.SaveChanges();

        return account;
    }
}