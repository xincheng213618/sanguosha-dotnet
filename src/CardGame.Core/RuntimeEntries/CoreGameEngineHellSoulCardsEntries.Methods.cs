#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHellSoulCardsEntries
{
    internal void AddHellSoulActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> cards) => _resolve().AddHellSoulActions(actions, actor, cards);
    internal bool AllowsHellSoulVoluntaryActivation(global::CardGame.Core.ProgramActivationLegalityContext context) => _resolve().AllowsHellSoulVoluntaryActivation(context);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHellSoulRestoration(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHellSoulRestoration(supplied);
    internal bool CanContinueHellSoulProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueHellSoulProgram(frame);
    internal void ContinueHellSoulRestoration(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueHellSoulRestoration(frame);
    internal void DestroyHellReturnSoulIfDiscarded(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation to) => _resolve().DestroyHellReturnSoulIfDiscarded(card, to);
    internal global::System.Collections.Generic.IReadOnlyList<int> HellReturnSoulTargets(int actorSeat) => _resolve().HellReturnSoulTargets(actorSeat);
    internal global::System.Collections.Generic.IReadOnlyList<int> HellReturnSoulUseTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => _resolve().HellReturnSoulUseTargets(actor, card);
    internal bool HellSoulFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().HellSoulFrameRidesOn(child, parent);
    internal string HellSoulGrantSource(long id) => _resolve().HellSoulGrantSource(id);
    internal global::CardGame.Core.HellWanghunGrantLostEvent[] HellSoulOutstandingLosses(int seat) => _resolve().HellSoulOutstandingLosses(seat);
    internal bool IsExactHellSoulProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsExactHellSoulProgram(frame);
    internal bool IsHellDestroyedSoulMaterial(long useId, int cardId) => _resolve().IsHellDestroyedSoulMaterial(useId, cardId);
    internal void ResolveHellReturnSoul(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => _resolve().ResolveHellReturnSoul(actor, card);
    internal void ResolveHellReturnSoulEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveHellReturnSoulEffect(pending);
    internal bool ResumeHellSoulRestoration(long id) => _resolve().ResumeHellSoulRestoration(id);
    internal bool TryCompleteHellSoulProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().TryCompleteHellSoulProgram(frame, completed);
}
