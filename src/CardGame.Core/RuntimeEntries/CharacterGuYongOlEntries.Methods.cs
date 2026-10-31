#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuYongOlEntries
{
    internal bool AdvanceOrdinaryBingyiChildren() => _resolve().AdvanceOrdinaryBingyiChildren();
    internal void AnswerOrdinaryBingyi(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerOrdinaryBingyi(selected);
    internal void AssertOrdinaryBingyi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertOrdinaryBingyi(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOrdinaryBingyi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginOrdinaryBingyi(supplied);
    internal bool CanContinueIssuedOrdinaryGuYong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedOrdinaryGuYong(f);
    internal bool CanOfferOrdinaryBingyi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferOrdinaryBingyi(c, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseOrdinaryBingyiAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseOrdinaryBingyiAi(decision, f);
    internal void ContinueOrdinaryBingyi(long id) => _resolve().ContinueOrdinaryBingyi(id);
    internal bool DrainOrdinaryBingyi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainOrdinaryBingyi(f);
    internal void FinishOrdinaryBingyi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishOrdinaryBingyi(f);
    internal bool HasOrdinaryGuYongBeforeDamageObserver(long id) => _resolve().HasOrdinaryGuYongBeforeDamageObserver(id);
    internal bool HasOrdinaryGuYongDamageObserver(long id) => _resolve().HasOrdinaryGuYongDamageObserver(id);
    internal bool IsOrdinaryBingyiDiscard(global::CardGame.Core.CardMovementRecord m, int owner) => _resolve().IsOrdinaryBingyiDiscard(m, owner);
    internal bool IsOrdinaryBingyiDrawMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsOrdinaryBingyiDrawMovement(f, m);
    internal bool IsOrdinaryGuYongBingyiTrigger(global::CardGame.Core.SkillProgramTrigger t) => _resolve().IsOrdinaryGuYongBingyiTrigger(t);
    internal bool IsOrdinaryGuYongDying() => _resolve().IsOrdinaryGuYongDying();
    internal bool IsOrdinaryGuYongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsOrdinaryGuYongMovement(f, effect, p);
    internal bool IsOrdinaryGuYongRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsOrdinaryGuYongRecoveryParent(f, recovery);
    internal bool MatchesOrdinaryBingyiRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MatchesOrdinaryBingyiRoot(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OrdinaryBingyiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OrdinaryBingyiChoices(f);
    internal bool OrdinaryBingyiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().OrdinaryBingyiFirstChild(f, child);
    internal bool OrdinaryBingyiMovementRoot(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().OrdinaryBingyiMovementRoot(c, context);
    internal global::CardGame.Core.ProgramSkillFrame? OrdinaryBingyiObserverRoot() => _resolve().OrdinaryBingyiObserverRoot();
    internal bool OrdinaryBingyiObserverSuffix(int i) => _resolve().OrdinaryBingyiObserverSuffix(i);
    internal bool OrdinaryBingyiPhaseSpent(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().OrdinaryBingyiPhaseSpent(owner, phase);
    internal bool PermitPaidOrdinaryBingyi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitPaidOrdinaryBingyi(f);
    internal void PublishOrdinaryBingyi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishOrdinaryBingyi(f);
    internal bool ResumeOrdinaryBingyi(long id) => _resolve().ResumeOrdinaryBingyi(id);
    internal void ReturnOrdinaryBingyiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnOrdinaryBingyiDraw(f, outcome);
    internal bool ReturnOrdinaryBingyiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOrdinaryBingyiMovement(f);
    internal bool HasOrdinaryGuYongCapability { get => _resolve().HasOrdinaryGuYongCapability; }
}
