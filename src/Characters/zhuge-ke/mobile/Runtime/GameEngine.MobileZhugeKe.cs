namespace CardGame.Core;

public sealed partial class GameEngine
{
    // These edition qualifications parameterize the original issued request and
    // damage invoices; neither skill owns a parallel pending state or payment.
    private static bool IsMobileZhugeKeAocai(string skillId) => skillId == "mobile:aocai";
    private static bool IsMobileZhugeKeDuwu(string skillId) => skillId == "mobile:duwu";
}
