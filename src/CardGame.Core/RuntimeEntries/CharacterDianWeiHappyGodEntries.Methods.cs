#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiHappyGodEntries
{
    internal void AnswerHappyGuard(PromptChoice choice) => _resolve().AnswerHappyGuard(choice);
    internal bool BeginHappyGuardRemovedWindow(HappyGuardRemovalReturn ret) => _resolve().BeginHappyGuardRemovedWindow(ret);
    internal bool CanOfferHappyDianWei(ProgramTriggerLegalityContext g) => _resolve().CanOfferHappyDianWei(g);
    internal void CaptureHappyGuardDeathRemoval(PlayerMarkerChangedEvent fact) => _resolve().CaptureHappyGuardDeathRemoval(fact);
    internal bool CompleteHappyGuardRemovedWindow(ProgramLifecycleTriggerWindowFrame w) => _resolve().CompleteHappyGuardRemovedWindow(w);
    internal bool ContinuePaidHappyGuard(ProgramSkillFrame f) => _resolve().ContinuePaidHappyGuard(f);
    internal void FinishHappyGuardTransfer(long id) => _resolve().FinishHappyGuardTransfer(id);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> HappyDianWeiKuangxi(GameEngine.IDamageAttempt attack) => _resolve().HappyDianWeiKuangxi(attack);
    internal bool HappyDianWeiStructuralEdge(ResolutionFrame child, ResolutionFrame parent) => _resolve().HappyDianWeiStructuralEdge(child, parent);
    internal PromptChoice HappyGuardAi(PendingDecision p, ProgramSkillFrame f) => _resolve().HappyGuardAi(p, f);
    internal GameEngine.IDamageAttempt? HappyGuardAttack(long id) => _resolve().HappyGuardAttack(id);
    internal IReadOnlyList<PromptChoice> HappyGuardChoices(ProgramSkillFrame f) => _resolve().HappyGuardChoices(f);
    internal int HappyGuardCount(int seat) => _resolve().HappyGuardCount(seat);
    internal bool HappyGuardDamageOffer(int owner, ProgramSkillWindowContext? context) => _resolve().HappyGuardDamageOffer(owner, context);
    internal bool HappyGuardOnField() => _resolve().HappyGuardOnField();
    internal bool HappyGuardOriginalCandidate(ProgramSkillFrame f) => _resolve().HappyGuardOriginalCandidate(f);
    internal bool HappyGuardRemovalIsPaid(HappyGuardRemovalReturn r) => _resolve().HappyGuardRemovalIsPaid(r);
    internal CardConversionSource HappyGuardSource(ProgramSkillFrame f) => _resolve().HappyGuardSource(f);
    internal bool IsHappyDianWeiElaiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsHappyDianWeiElaiCandidate(c, context);
    internal bool IsHappyDianWeiElaiContext(ProgramSkillFrame f) => _resolve().IsHappyDianWeiElaiContext(f);
    internal bool IsOriginalDyingSuspendedByHappyGuardElai(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByHappyGuardElai(dying);
    internal void PublishHappyGuardChoice(ProgramSkillFrame f) => _resolve().PublishHappyGuardChoice(f);
    internal SkillProgramStepOutcome ResolveHappyDianWei(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ResolveHappyDianWei(supplied, op);
    internal void ReturnHappyGuardElai(ProgramSkillFrame f, bool completed) => _resolve().ReturnHappyGuardElai(f, completed);
    internal void SetHappyGuard(ProgramSkillFrame f, int target, int count) => _resolve().SetHappyGuard(f, target, count);
    internal bool TryBeginHappyDianWeiDeathRemovals(DeathFrame supplied) => _resolve().TryBeginHappyDianWeiDeathRemovals(supplied);
    internal bool ValidHappyGuardFrame(ProgramSkillFrame f) => _resolve().ValidHappyGuardFrame(f);
}
