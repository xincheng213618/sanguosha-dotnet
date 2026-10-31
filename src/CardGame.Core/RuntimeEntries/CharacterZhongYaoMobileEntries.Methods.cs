#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhongYaoMobileEntries
{
    internal bool CanOfferMobileZhongYaoZuoding(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileZhongYaoZuoding(gate);
    internal IReadOnlyList<int> MobileZhongYaoZuodingTargets(int ownerSeat, ProgramSkillWindowContext? context) => _resolve().MobileZhongYaoZuodingTargets(ownerSeat, context);
}
