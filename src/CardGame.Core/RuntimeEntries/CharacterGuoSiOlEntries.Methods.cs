#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoSiOlEntries
{
    internal void AssertGuoSiNativeUse(CardUseFrame use) => _resolve().AssertGuoSiNativeUse(use);
    internal void BindGuoSiAcceptedUse(CardUseFrame use) => _resolve().BindGuoSiAcceptedUse(use);
    internal bool CanContinueIssuedGuoSi(ProgramSkillFrame f) => _resolve().CanContinueIssuedGuoSi(f);
    internal bool CanOfferGuoSi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferGuoSi(c, t, w);
    internal void CaptureGuoSiCompletedUse(long id, CardActionContext a) => _resolve().CaptureGuoSiCompletedUse(id, a);
    internal void CaptureGuoSiFinalizedUse(long id, CardActionContext a) => _resolve().CaptureGuoSiFinalizedUse(id, a);
    internal bool CompleteGuoSiUseReturn(CardUseFrame use) => _resolve().CompleteGuoSiUseReturn(use);
    internal void ContinueGuoSiNativePayment(long id) => _resolve().ContinueGuoSiNativePayment(id);
    internal bool DistanceCardUseExemption100HasGuoSiTargetEffect(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption100HasGuoSiTargetEffect(q);
    internal bool DrainGuoSi(ProgramSkillFrame f) => _resolve().DrainGuoSi(f);
    internal SkillProgramStepOutcome ExecuteGuoSi(SkillProgramEffectOp op, ProgramSkillFrame supplied) => _resolve().ExecuteGuoSi(op, supplied);
    internal void FinishGuoSi(ProgramSkillFrame f) => _resolve().FinishGuoSi(f);
    internal PromptChoice GuoSiAiChoice(ProgramSkillFrame f) => _resolve().GuoSiAiChoice(f);
    internal IReadOnlyList<PromptChoice> GuoSiChoices(ProgramSkillFrame f) => _resolve().GuoSiChoices(f);
    internal bool GuoSiDirectedTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().GuoSiDirectedTargetProhibited(query);
    internal bool GuoSiFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().GuoSiFinalizedAfterCategoryTargetWindow(query);
    internal bool GuoSiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().GuoSiFirstChild(root, child);
    internal bool GuoSiNativePaymentEdge(int index) => _resolve().GuoSiNativePaymentEdge(index);
    internal bool GuoSiNativeRidesOn(ProgramSkillFrame f, CardUseFrame use) => _resolve().GuoSiNativeRidesOn(f, use);
    internal bool GuoSiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().GuoSiNormalizeImplicitDrawTwoTargetWindow(query);
    internal ProgramSkillFrame? GuoSiObserverRoot() => _resolve().GuoSiObserverRoot();
    internal bool GuoSiObserverSuffix(int index) => _resolve().GuoSiObserverSuffix(index);
    internal bool GuoSiQualified(int seat) => _resolve().GuoSiQualified(seat);
    internal bool GuoSiResponseUse(CardActionContext a) => _resolve().GuoSiResponseUse(a);
    internal bool GuoSiTargetQuotaAllowed(ProgramTargetQuotaQuery query) => _resolve().GuoSiTargetQuotaAllowed(query);
    internal bool HasGuoSiTargetEffect(int actor, int target, DirectedTurnCardPolicyEffect effect) => _resolve().HasGuoSiTargetEffect(actor, target, effect);
    internal bool IsGuoSiNativeUse(CardUseFrame use) => _resolve().IsGuoSiNativeUse(use);
    internal bool IsSuspendedGuoSiCompletedParent(CardUseFrame original) => _resolve().IsSuspendedGuoSiCompletedParent(original);
    internal void IssueSidao(ProgramSkillFrame f, LegalAction action) => _resolve().IssueSidao(f, action);
    internal void ObserveGuoSiAcceptedUse(IGameEvent payload) => _resolve().ObserveGuoSiAcceptedUse(payload);
    internal void ObserveGuoSiTurnExpiry(IGameEvent payload) => _resolve().ObserveGuoSiTurnExpiry(payload);
    internal void PublishGuoSiChoice(ProgramSkillFrame f) => _resolve().PublishGuoSiChoice(f);
    internal void RequireIssuedGuoSi(ProgramSkillFrame f) => _resolve().RequireIssuedGuoSi(f);
    internal void ResolveGuoSiChoice(PromptChoice choice) => _resolve().ResolveGuoSiChoice(choice);
    internal bool ResumeGuoSi(long id) => _resolve().ResumeGuoSi(id);
    internal bool ReturnGuoSiMovement(ProgramSkillFrame f) => _resolve().ReturnGuoSiMovement(f);
    internal void ReturnGuoSiNativeUse(CardUseFrame use) => _resolve().ReturnGuoSiNativeUse(use);
    internal IReadOnlyList<LegalAction> SidaoActions(int owner, GuoSiPair pair) => _resolve().SidaoActions(owner, pair);
    internal bool TryAdvanceGuoSiSubtree() => _resolve().TryAdvanceGuoSiSubtree();
    internal void ValidateGuoSiNativeUseReturn(CardUseFrame use) => _resolve().ValidateGuoSiNativeUseReturn(use);
    internal bool HasGuoSiCapability { get => _resolve().HasGuoSiCapability; }
}
