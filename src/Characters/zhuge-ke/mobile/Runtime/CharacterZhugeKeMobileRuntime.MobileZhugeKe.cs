#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeKeMobileRuntime
{
    internal static bool IsMobileZhugeKeAocai(string skillId) => skillId == "mobile:aocai";
    internal static bool IsMobileZhugeKeDuwu(string skillId) => skillId == "mobile:duwu";
}
