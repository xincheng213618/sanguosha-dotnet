#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhongYaoMobileEntries
{
    internal bool CanOfferMobileZhongYaoZuoding(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileZhongYaoZuoding(gate);
    internal global::System.Collections.Generic.IReadOnlyList<int> MobileZhongYaoZuodingTargets(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().MobileZhongYaoZuodingTargets(ownerSeat, context);
}
