#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTriggeredCardConversionsEntries
{
    internal bool CanOfferTriggeredSlashConversion(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferTriggeredSlashConversion(candidate, trigger, context);
    internal bool CanUseTriggeredSlashMaterial(int ownerSeat, int targetSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location, global::CardGame.Core.CardConversionSource source) => _resolve().CanUseTriggeredSlashMaterial(ownerSeat, targetSeat, card, location, source);
    internal bool MatchesTriggeredSlashMaterial(global::CardGame.Core.ProgramSkillFrame frame, string bind, int ownerSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => _resolve().MatchesTriggeredSlashMaterial(frame, bind, ownerSeat, card, location);
    internal bool TriggeredSlashSelectionAccepts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramEffect selection, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => _resolve().TriggeredSlashSelectionAccepts(owner, selection, card, location);
    internal global::CardGame.Core.SkillProgramStepOutcome UseTriggeredBoundCardsAsSlash(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().UseTriggeredBoundCardsAsSlash(supplied, effect);
}
