
public class PlayerData
{
    private static string _nickname;
    private static bool isContinue;

    public static void SetNickname(string nickname) => _nickname = nickname;

    public static string GetNickname() => _nickname;

    public static bool IsContinue() => isContinue;

    public static void SetContinue(bool state) => isContinue = state;
}
