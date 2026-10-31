#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoSiOlEntries
{
    internal void AssertGuoSiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertGuoSiNativeUse(use);
    internal void BindGuoSiAcceptedUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindGuoSiAcceptedUse(use);
    internal bool CanContinueIssuedGuoSi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedGuoSi(f);
    internal bool CanOfferGuoSi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferGuoSi(c, t, w);
    internal void CaptureGuoSiCompletedUse(long id, global::CardGame.Core.CardActionContext a) => _resolve().CaptureGuoSiCompletedUse(id, a);
    internal void CaptureGuoSiFinalizedUse(long id, global::CardGame.Core.CardActionContext a) => _resolve().CaptureGuoSiFinalizedUse(id, a);
    internal bool CompleteGuoSiUseReturn(global::CardGame.Core.CardUseFrame use) => _resolve().CompleteGuoSiUseReturn(use);
    internal void ContinueGuoSiNativePayment(long id) => _resolve().ContinueGuoSiNativePayment(id);
    internal bool DistanceCardUseExemption100HasGuoSiTargetEffect(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption100HasGuoSiTargetEffect(q);
    internal bool DrainGuoSi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainGuoSi(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteGuoSi(global::CardGame.Core.SkillProgramEffectOp op, global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ExecuteGuoSi(op, supplied);
    internal void FinishGuoSi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishGuoSi(f);
    internal global::CardGame.Core.PromptChoice GuoSiAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GuoSiAiChoice(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> GuoSiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().GuoSiChoices(f);
    internal bool GuoSiDirectedTargetProhibited(global::CardGame.Core.ProgramDirectedTargetQuery query) => _resolve().GuoSiDirectedTargetProhibited(query);
    internal bool GuoSiFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().GuoSiFinalizedAfterCategoryTargetWindow(query);
    internal bool GuoSiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().GuoSiFirstChild(root, child);
    internal bool GuoSiNativePaymentEdge(int index) => _resolve().GuoSiNativePaymentEdge(index);
    internal bool GuoSiNativeRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use) => _resolve().GuoSiNativeRidesOn(f, use);
    internal bool GuoSiNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().GuoSiNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.ProgramSkillFrame? GuoSiObserverRoot() => _resolve().GuoSiObserverRoot();
    internal bool GuoSiObserverSuffix(int index) => _resolve().GuoSiObserverSuffix(index);
    internal bool GuoSiQualified(int seat) => _resolve().GuoSiQualified(seat);
    internal bool GuoSiResponseUse(global::CardGame.Core.CardActionContext a) => _resolve().GuoSiResponseUse(a);
    internal bool GuoSiTargetQuotaAllowed(global::CardGame.Core.ProgramTargetQuotaQuery query) => _resolve().GuoSiTargetQuotaAllowed(query);
    internal bool HasGuoSiBeforeDamageObserver(long id) => _resolve().HasGuoSiBeforeDamageObserver(id);
    internal bool HasGuoSiDamageObserver(long id) => _resolve().HasGuoSiDamageObserver(id);
    internal bool HasGuoSiTargetEffect(int actor, int target, global::CardGame.Core.DirectedTurnCardPolicyEffect effect) => _resolve().HasGuoSiTargetEffect(actor, target, effect);
    internal bool IsGuoSiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsGuoSiMovement(f, e, p);
    internal bool IsGuoSiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsGuoSiNativeUse(use);
    internal bool IsSuspendedGuoSiCompletedParent(global::CardGame.Core.CardUseFrame original) => _resolve().IsSuspendedGuoSiCompletedParent(original);
    internal void IssueSidao(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction action) => _resolve().IssueSidao(f, action);
    internal void ObserveGuoSiAcceptedUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveGuoSiAcceptedUse(payload);
    internal void ObserveGuoSiTurnExpiry(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveGuoSiTurnExpiry(payload);
    internal void PublishGuoSiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishGuoSiChoice(f);
    internal void RequireIssuedGuoSi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireIssuedGuoSi(f);
    internal void ResolveGuoSiChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveGuoSiChoice(choice);
    internal bool ResumeGuoSi(long id) => _resolve().ResumeGuoSi(id);
    internal bool ReturnGuoSiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnGuoSiMovement(f);
    internal void ReturnGuoSiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnGuoSiNativeUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> SidaoActions(int owner, global::CardGame.Core.GuoSiPair pair) => _resolve().SidaoActions(owner, pair);
    internal bool TryAdvanceGuoSiSubtree() => _resolve().TryAdvanceGuoSiSubtree();
    internal void ValidateGuoSiNativeUseReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateGuoSiNativeUseReturn(use);
    internal bool HasGuoSiCapability { get => _resolve().HasGuoSiCapability; }
}
