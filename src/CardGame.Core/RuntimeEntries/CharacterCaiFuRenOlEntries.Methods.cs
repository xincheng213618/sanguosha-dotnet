#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiFuRenOlEntries
{
    internal bool AdvanceOrdinaryQietingChildren() => _resolve().AdvanceOrdinaryQietingChildren();
    internal void AnswerOrdinaryQieting(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerOrdinaryQieting(selected);
    internal void AssertOrdinaryQieting(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertOrdinaryQieting(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOrdinaryQieting(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginOrdinaryQieting(supplied);
    internal bool CanActivateOrdinaryCaiFuRenXianzhou(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateOrdinaryCaiFuRenXianzhou(c);
    internal bool CanContinueIssuedOrdinaryCaiFuRen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedOrdinaryCaiFuRen(f);
    internal bool CanMoveOrdinaryQietingEquipment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Card card) => _resolve().CanMoveOrdinaryQietingEquipment(f, card);
    internal bool CaptureOrdinaryQietingFinalUse(global::CardGame.Core.CardUseFrame use) => _resolve().CaptureOrdinaryQietingFinalUse(use);
    internal global::CardGame.Core.PromptChoice ChooseOrdinaryQietingAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseOrdinaryQietingAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseOrdinaryXianzhouAi(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseOrdinaryXianzhouAi(d, f);
    internal void ContinueOrdinaryQieting(long id) => _resolve().ContinueOrdinaryQieting(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateOrdinaryCaiFuRenQietingContext(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateOrdinaryCaiFuRenQietingContext(parent, candidate, context);
    internal bool DrainOrdinaryQieting(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainOrdinaryQieting(f);
    internal void FinishOrdinaryQieting(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishOrdinaryQieting(f);
    internal bool HasOrdinaryCaiFuRenBeforeDamageObserver(long id) => _resolve().HasOrdinaryCaiFuRenBeforeDamageObserver(id);
    internal bool HasOrdinaryCaiFuRenDamageObserver(long id) => _resolve().HasOrdinaryCaiFuRenDamageObserver(id);
    internal bool IsEligibleOrdinaryCaiFuRenQietingCandidate(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsEligibleOrdinaryCaiFuRenQietingCandidate(parent, candidate, context);
    internal bool IsIssuedOrdinaryCaiFuRenXianzhou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsIssuedOrdinaryCaiFuRenXianzhou(f);
    internal bool IsOrdinaryCaiFuRenDying() => _resolve().IsOrdinaryCaiFuRenDying();
    internal bool IsOrdinaryCaiFuRenMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsOrdinaryCaiFuRenMovement(f, effect, p);
    internal bool IsOrdinaryCaiFuRenQietingTrigger(global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().IsOrdinaryCaiFuRenQietingTrigger(trigger);
    internal bool IsOrdinaryCaiFuRenRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsOrdinaryCaiFuRenRecoveryParent(f, recovery);
    internal bool IsOrdinaryQietingPaidMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsOrdinaryQietingPaidMovement(f, m);
    internal bool MatchesOrdinaryQietingRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MatchesOrdinaryQietingRoot(f);
    internal void ObserveOrdinaryQietingUses(global::CardGame.Core.IGameEvent e) => _resolve().ObserveOrdinaryQietingUses(e);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OrdinaryQietingChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OrdinaryQietingChoices(f);
    internal bool OrdinaryQietingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().OrdinaryQietingFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? OrdinaryQietingObserverRoot() => _resolve().OrdinaryQietingObserverRoot();
    internal bool OrdinaryQietingObserverSuffix(int index) => _resolve().OrdinaryQietingObserverSuffix(index);
    internal bool OrdinaryQietingUsedOnAnother(global::CardGame.Core.OrdinaryQietingTurnContext c) => _resolve().OrdinaryQietingUsedOnAnother(c);
    internal bool PermitPaidOrdinaryCaiFuRen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitPaidOrdinaryCaiFuRen(f);
    internal void PublishOrdinaryQieting(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishOrdinaryQieting(f);
    internal void RecordOrdinaryQietingUse(global::CardGame.Core.CardUseFrame use) => _resolve().RecordOrdinaryQietingUse(use);
    internal bool ResumeOrdinaryQieting(long id) => _resolve().ResumeOrdinaryQieting(id);
    internal void ReturnOrdinaryQietingDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnOrdinaryQietingDraw(f, outcome);
    internal bool ReturnOrdinaryQietingMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOrdinaryQietingMovement(f);
    internal bool ReturnPaidOrdinaryXianzhouMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnPaidOrdinaryXianzhouMovement(f);
    internal bool HasOrdinaryCaiFuRenCapability { get => _resolve().HasOrdinaryCaiFuRenCapability; }
}
