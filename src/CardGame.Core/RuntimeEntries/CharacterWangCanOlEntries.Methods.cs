#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangCanOlEntries
{
    internal void AssertDenglouNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertDenglouNativeUse(use);
    internal void BeginDenglouLease(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginDenglouLease(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildDenglouNativeBasicUseOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BuildDenglouNativeBasicUseOptions(f);
    internal bool CanContinueIssuedWangCan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedWangCan(f);
    internal bool CanOfferWangCan(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferWangCan(c, t, w);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SanwenGainProof>? CaptureSanwenGainQualifications(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureSanwenGainQualifications(batch);
    internal void CompleteDenglouNativeAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteDenglouNativeAttack(completion);
    internal bool CompleteDenglouUseReturn(global::CardGame.Core.CardUseFrame use) => _resolve().CompleteDenglouUseReturn(use);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureSanwenGainQualifications(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureSanwenGainQualifications(batch, completed);
    internal void CompleteSanwenNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof) => _resolve().CompleteSanwenNativeDrawQualification(proof);
    internal void ContinueDenglou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueDenglou(f);
    internal void ContinueDenglouNativePayment(long id) => _resolve().ContinueDenglouNativePayment(id);
    internal void ContinueQiai(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueQiai(f);
    internal void ContinueSanwenAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueSanwenAfterPublicReveal(f);
    internal bool DenglouNativePaymentEdge(int index) => _resolve().DenglouNativePaymentEdge(index);
    internal bool DenglouNativeRidesOn(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.CardUseFrame use) => _resolve().DenglouNativeRidesOn(parent, use);
    internal global::CardGame.Core.CardSnapshot[] DenglouPrivateCards(int viewer) => _resolve().DenglouPrivateCards(viewer);
    internal bool DrainWangCan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainWangCan(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteWangCan(global::CardGame.Core.SkillProgramEffectOp op, global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ExecuteWangCan(op, supplied);
    internal void FinishDenglouNativeReturn(global::CardGame.Core.WangCanNativeReceipt n) => _resolve().FinishDenglouNativeReturn(n);
    internal void FinishWangCan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWangCan(f);
    internal bool IsDenglouNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsDenglouNativeUse(use);
    internal void IssueDenglouNativeUse(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.LegalAction action) => _resolve().IssueDenglouNativeUse(f, action);
    internal void IssueWangCanSegment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueWangCanSegment(f);
    internal void PaySanwenDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaySanwenDraw(f);
    internal void PayWangCanMove(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.WangCanStage stage) => _resolve().PayWangCanMove(f, ids, destination, stage);
    internal void PublishWangCan(global::CardGame.Core.ProgramSkillFrame f, int seat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string text) => _resolve().PublishWangCan(f, seat, choices, text);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> QiaiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().QiaiChoices(f);
    internal void RequireIssuedWangCan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireIssuedWangCan(f);
    internal void ResolveWangCanChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveWangCanChoice(choice);
    internal bool ResumeWangCan(long id) => _resolve().ResumeWangCan(id);
    internal void ReturnDenglouNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnDenglouNativeUse(use);
    internal bool ReturnRegisteredCompleteDenglouNativeAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteDenglouNativeAttack(completion);
    internal bool ReturnWangCanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWangCanMovement(f);
    internal global::CardGame.Core.SanwenGainProof? SanwenContextFor(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().SanwenContextFor(batch, owner);
    internal bool SanwenIncomingBelongs(global::CardGame.Core.CardMovementBatchContext batch, int owner, int id) => _resolve().SanwenIncomingBelongs(batch, owner, id);
    internal global::System.Collections.Generic.IReadOnlyList<int> SanwenPayable(global::CardGame.Core.SanwenGainProof q) => _resolve().SanwenPayable(q);
    internal bool TryAdvanceWangCanSubtree() => _resolve().TryAdvanceWangCanSubtree();
    internal global::CardGame.Core.ProgramSkillWindowContext? TrySanwenGain(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TrySanwenGain(frame, candidate);
    internal bool ValidSanwenProof(global::CardGame.Core.SanwenGainProof q, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().ValidSanwenProof(q, batch);
    internal void ValidateDenglouNativeUseReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateDenglouNativeUseReturn(use);
    internal global::CardGame.Core.PromptChoice WangCanAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangCanAiChoice(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WangCanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangCanChoices(f);
    internal bool WangCanFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().WangCanFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? WangCanObserverRoot() => _resolve().WangCanObserverRoot();
    internal bool WangCanObserverSuffix(int index) => _resolve().WangCanObserverSuffix(index);
    internal bool WangCanSegmentMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.WangCanReceipt r) => _resolve().WangCanSegmentMatches(f, r);
    internal bool HasDenglouCapability { get => _resolve().HasDenglouCapability; }
    internal bool HasSanwenCapability { get => _resolve().HasSanwenCapability; }
}
