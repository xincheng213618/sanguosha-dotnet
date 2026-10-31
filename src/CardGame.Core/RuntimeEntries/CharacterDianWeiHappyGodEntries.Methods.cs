#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiHappyGodEntries
{
    internal void AnswerHappyGuard(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHappyGuard(choice);
    internal bool BeginHappyGuardRemovedWindow(global::CardGame.Core.HappyGuardRemovalReturn ret) => _resolve().BeginHappyGuardRemovedWindow(ret);
    internal bool CanOfferHappyDianWei(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanOfferHappyDianWei(g);
    internal void CaptureHappyGuardDeathRemoval(global::CardGame.Core.PlayerMarkerChangedEvent fact) => _resolve().CaptureHappyGuardDeathRemoval(fact);
    internal bool CompleteHappyGuardRemovedWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame w) => _resolve().CompleteHappyGuardRemovedWindow(w);
    internal bool ContinuePaidHappyGuard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHappyGuard(f);
    internal void FinishHappyGuardTransfer(long id) => _resolve().FinishHappyGuardTransfer(id);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> HappyDianWeiKuangxi(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HappyDianWeiKuangxi(attack);
    internal bool HappyDianWeiStructuralEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().HappyDianWeiStructuralEdge(child, parent);
    internal global::CardGame.Core.PromptChoice HappyGuardAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyGuardAi(p, f);
    internal global::CardGame.Core.GameEngine.IDamageAttempt? HappyGuardAttack(long id) => _resolve().HappyGuardAttack(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyGuardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyGuardChoices(f);
    internal int HappyGuardCount(int seat) => _resolve().HappyGuardCount(seat);
    internal bool HappyGuardDamageOffer(int owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().HappyGuardDamageOffer(owner, context);
    internal bool HappyGuardOnField() => _resolve().HappyGuardOnField();
    internal bool HappyGuardOriginalCandidate(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyGuardOriginalCandidate(f);
    internal bool HappyGuardRemovalIsPaid(global::CardGame.Core.HappyGuardRemovalReturn r) => _resolve().HappyGuardRemovalIsPaid(r);
    internal global::CardGame.Core.CardConversionSource HappyGuardSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyGuardSource(f);
    internal bool IsHappyDianWeiElaiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsHappyDianWeiElaiCandidate(c, context);
    internal bool IsHappyDianWeiElaiContext(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsHappyDianWeiElaiContext(f);
    internal bool IsOriginalDyingSuspendedByHappyGuardElai(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByHappyGuardElai(dying);
    internal void PublishHappyGuardChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHappyGuardChoice(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveHappyDianWei(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveHappyDianWei(supplied, op);
    internal void ReturnHappyGuardElai(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().ReturnHappyGuardElai(f, completed);
    internal void SetHappyGuard(global::CardGame.Core.ProgramSkillFrame f, int target, int count) => _resolve().SetHappyGuard(f, target, count);
    internal bool TryBeginHappyDianWeiDeathRemovals(global::CardGame.Core.DeathFrame supplied) => _resolve().TryBeginHappyDianWeiDeathRemovals(supplied);
    internal bool ValidHappyGuardFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHappyGuardFrame(f);
}
