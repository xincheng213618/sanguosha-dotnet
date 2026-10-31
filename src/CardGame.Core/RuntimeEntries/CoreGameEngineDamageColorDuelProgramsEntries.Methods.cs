#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageColorDuelProgramsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDamageAppearanceDraw(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginDamageAppearanceDraw(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSelectedActorDuel(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginSelectedActorDuel(input);
    internal bool CanIssueSelectedActorDuel(int ownerSeat, int actorSeat) => _resolve().CanIssueSelectedActorDuel(ownerSeat, actorSeat);
    internal global::CardGame.Core.ProgramSkillFrame? DamageAppearanceDrawObserverRoot(long windowId) => _resolve().DamageAppearanceDrawObserverRoot(windowId);
    internal global::CardGame.Core.Card GetTrickRepresentation(long frameId, int cardId, bool requireProcessing = false) => _resolve().GetTrickRepresentation(frameId, cardId, requireProcessing);
    internal bool HasDamageAppearanceDrawObserver(long windowId) => _resolve().HasDamageAppearanceDrawObserver(windowId);
    internal bool IsSelectedActorDuelUse(long id) => _resolve().IsSelectedActorDuelUse(id);
    internal bool MatchesSelectedActorDuelAction(long frameId, global::CardGame.Core.CardActionContext action, int cardId) => _resolve().MatchesSelectedActorDuelAction(frameId, action, cardId);
    internal void ReturnSelectedActorDuel(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnSelectedActorDuel(use);
    internal global::CardGame.Core.Card UnrepresentedZeroTrick(long frameId) => _resolve().UnrepresentedZeroTrick(frameId);
}
