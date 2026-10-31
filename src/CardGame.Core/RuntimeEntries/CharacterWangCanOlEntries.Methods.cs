#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangCanOlEntries
{
    internal void AssertDenglouNativeUse(CardUseFrame use) => _resolve().AssertDenglouNativeUse(use);
    internal void BeginDenglouLease(ProgramSkillFrame f) => _resolve().BeginDenglouLease(f);
    internal IReadOnlyList<LegalAction> BuildDenglouNativeBasicUseOptions(ProgramSkillFrame f) => _resolve().BuildDenglouNativeBasicUseOptions(f);
    internal bool CanContinueIssuedWangCan(ProgramSkillFrame f) => _resolve().CanContinueIssuedWangCan(f);
    internal bool CanOfferWangCan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferWangCan(c, t, w);
    internal IReadOnlyList<SanwenGainProof>? CaptureSanwenGainQualifications(CardMovementBatchContext batch) => _resolve().CaptureSanwenGainQualifications(batch);
    internal void CompleteDenglouNativeAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteDenglouNativeAttack(completion);
    internal bool CompleteDenglouUseReturn(CardUseFrame use) => _resolve().CompleteDenglouUseReturn(use);
    internal CardMovementBatchContext CompleteMovementBatchCaptureSanwenGainQualifications(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureSanwenGainQualifications(batch, completed);
    internal void CompleteSanwenNativeDrawQualification(NativeDrawInvocationProof proof) => _resolve().CompleteSanwenNativeDrawQualification(proof);
    internal void ContinueDenglou(ProgramSkillFrame f) => _resolve().ContinueDenglou(f);
    internal void ContinueDenglouNativePayment(long id) => _resolve().ContinueDenglouNativePayment(id);
    internal void ContinueQiai(ProgramSkillFrame f) => _resolve().ContinueQiai(f);
    internal void ContinueSanwenAfterPublicReveal(ProgramSkillFrame f) => _resolve().ContinueSanwenAfterPublicReveal(f);
    internal bool DenglouNativePaymentEdge(int index) => _resolve().DenglouNativePaymentEdge(index);
    internal bool DenglouNativeRidesOn(ProgramSkillFrame parent, CardUseFrame use) => _resolve().DenglouNativeRidesOn(parent, use);
    internal CardSnapshot[] DenglouPrivateCards(int viewer) => _resolve().DenglouPrivateCards(viewer);
    internal bool DrainWangCan(ProgramSkillFrame f) => _resolve().DrainWangCan(f);
    internal SkillProgramStepOutcome ExecuteWangCan(SkillProgramEffectOp op, ProgramSkillFrame supplied) => _resolve().ExecuteWangCan(op, supplied);
    internal void FinishDenglouNativeReturn(WangCanNativeReceipt n) => _resolve().FinishDenglouNativeReturn(n);
    internal void FinishWangCan(ProgramSkillFrame f) => _resolve().FinishWangCan(f);
    internal bool IsDenglouNativeUse(CardUseFrame use) => _resolve().IsDenglouNativeUse(use);
    internal void IssueDenglouNativeUse(ProgramSkillFrame f, LegalAction action) => _resolve().IssueDenglouNativeUse(f, action);
    internal void IssueWangCanSegment(ProgramSkillFrame f) => _resolve().IssueWangCanSegment(f);
    internal void PaySanwenDraw(ProgramSkillFrame f) => _resolve().PaySanwenDraw(f);
    internal void PayWangCanMove(ProgramSkillFrame f, IReadOnlyList<int> ids, CardLocation destination, WangCanStage stage) => _resolve().PayWangCanMove(f, ids, destination, stage);
    internal void PublishWangCan(ProgramSkillFrame f, int seat, IReadOnlyList<PromptChoice> choices, string text) => _resolve().PublishWangCan(f, seat, choices, text);
    internal IReadOnlyList<PromptChoice> QiaiChoices(ProgramSkillFrame f) => _resolve().QiaiChoices(f);
    internal void RequireIssuedWangCan(ProgramSkillFrame f) => _resolve().RequireIssuedWangCan(f);
    internal void ResolveWangCanChoice(PromptChoice choice) => _resolve().ResolveWangCanChoice(choice);
    internal bool ResumeWangCan(long id) => _resolve().ResumeWangCan(id);
    internal void ReturnDenglouNativeUse(CardUseFrame use) => _resolve().ReturnDenglouNativeUse(use);
    internal bool ReturnRegisteredCompleteDenglouNativeAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteDenglouNativeAttack(completion);
    internal bool ReturnWangCanMovement(ProgramSkillFrame f) => _resolve().ReturnWangCanMovement(f);
    internal SanwenGainProof? SanwenContextFor(CardMovementBatchContext batch, int owner) => _resolve().SanwenContextFor(batch, owner);
    internal bool SanwenIncomingBelongs(CardMovementBatchContext batch, int owner, int id) => _resolve().SanwenIncomingBelongs(batch, owner, id);
    internal IReadOnlyList<int> SanwenPayable(SanwenGainProof q) => _resolve().SanwenPayable(q);
    internal bool TryAdvanceWangCanSubtree() => _resolve().TryAdvanceWangCanSubtree();
    internal ProgramSkillWindowContext? TrySanwenGain(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TrySanwenGain(frame, candidate);
    internal bool ValidSanwenProof(SanwenGainProof q, CardMovementBatchContext batch) => _resolve().ValidSanwenProof(q, batch);
    internal void ValidateDenglouNativeUseReturn(CardUseFrame use) => _resolve().ValidateDenglouNativeUseReturn(use);
    internal PromptChoice WangCanAiChoice(ProgramSkillFrame f) => _resolve().WangCanAiChoice(f);
    internal IReadOnlyList<PromptChoice> WangCanChoices(ProgramSkillFrame f) => _resolve().WangCanChoices(f);
    internal bool WangCanFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().WangCanFirstChild(root, child);
    internal ProgramSkillFrame? WangCanObserverRoot() => _resolve().WangCanObserverRoot();
    internal bool WangCanObserverSuffix(int index) => _resolve().WangCanObserverSuffix(index);
    internal bool WangCanSegmentMatches(ProgramSkillFrame f, WangCanReceipt r) => _resolve().WangCanSegmentMatches(f, r);
    internal bool HasDenglouCapability { get => _resolve().HasDenglouCapability; }
    internal bool HasSanwenCapability { get => _resolve().HasSanwenCapability; }
}
