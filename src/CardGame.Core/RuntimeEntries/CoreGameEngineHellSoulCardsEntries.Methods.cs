#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHellSoulCardsEntries
{
    internal void AddHellSoulActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> cards) => _resolve().AddHellSoulActions(actions, actor, cards);
    internal bool AllowsHellSoulVoluntaryActivation(ProgramActivationLegalityContext context) => _resolve().AllowsHellSoulVoluntaryActivation(context);
    internal SkillProgramStepOutcome BeginHellSoulRestoration(ProgramSkillFrame supplied) => _resolve().BeginHellSoulRestoration(supplied);
    internal bool CanContinueHellSoulProgram(ProgramSkillFrame frame) => _resolve().CanContinueHellSoulProgram(frame);
    internal void ContinueHellSoulRestoration(ProgramSkillFrame frame) => _resolve().ContinueHellSoulRestoration(frame);
    internal void DestroyHellReturnSoulIfDiscarded(Card card, CardLocation to) => _resolve().DestroyHellReturnSoulIfDiscarded(card, to);
    internal IReadOnlyList<int> HellReturnSoulTargets(int actorSeat) => _resolve().HellReturnSoulTargets(actorSeat);
    internal IReadOnlyList<int> HellReturnSoulUseTargets(CharacterState actor, Card card) => _resolve().HellReturnSoulUseTargets(actor, card);
    internal bool HellSoulFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _resolve().HellSoulFrameRidesOn(child, parent);
    internal string HellSoulGrantSource(long id) => _resolve().HellSoulGrantSource(id);
    internal HellWanghunGrantLostEvent[] HellSoulOutstandingLosses(int seat) => _resolve().HellSoulOutstandingLosses(seat);
    internal bool IsExactHellSoulProgram(ProgramSkillFrame frame) => _resolve().IsExactHellSoulProgram(frame);
    internal bool IsHellDestroyedSoulMaterial(long useId, int cardId) => _resolve().IsHellDestroyedSoulMaterial(useId, cardId);
    internal void ResolveHellReturnSoul(CharacterState actor, Card card) => _resolve().ResolveHellReturnSoul(actor, card);
    internal void ResolveHellReturnSoulEffect(NullificationWindowFrame pending) => _resolve().ResolveHellReturnSoulEffect(pending);
    internal bool ResumeHellSoulRestoration(long id) => _resolve().ResumeHellSoulRestoration(id);
    internal bool TryCompleteHellSoulProgram(ProgramSkillFrame frame, bool completed) => _resolve().TryCompleteHellSoulProgram(frame, completed);
}
