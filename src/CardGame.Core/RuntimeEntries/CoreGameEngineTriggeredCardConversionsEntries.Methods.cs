#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTriggeredCardConversionsEntries
{
    internal bool CanOfferTriggeredSlashConversion(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferTriggeredSlashConversion(candidate, trigger, context);
    internal bool CanUseTriggeredSlashMaterial(int ownerSeat, int targetSeat, Card card, CardLocation location, CardConversionSource source) => _resolve().CanUseTriggeredSlashMaterial(ownerSeat, targetSeat, card, location, source);
    internal bool MatchesTriggeredSlashMaterial(ProgramSkillFrame frame, string bind, int ownerSeat, Card card, CardLocation location) => _resolve().MatchesTriggeredSlashMaterial(frame, bind, ownerSeat, card, location);
    internal bool TriggeredSlashSelectionAccepts(CharacterState owner, SkillProgramEffect selection, Card card, CardLocation location) => _resolve().TriggeredSlashSelectionAccepts(owner, selection, card, location);
    internal SkillProgramStepOutcome UseTriggeredBoundCardsAsSlash(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().UseTriggeredBoundCardsAsSlash(supplied, effect);
}
