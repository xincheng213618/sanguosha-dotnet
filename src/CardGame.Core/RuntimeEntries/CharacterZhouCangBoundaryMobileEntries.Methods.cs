#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouCangBoundaryMobileEntries
{
    internal bool AdvanceBoundaryZhongyong() => _resolve().AdvanceBoundaryZhongyong();
    internal void AssertBoundaryZhongyong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBoundaryZhongyong(f);
    internal int[] AvailableBoundaryZhongyong(global::CardGame.Core.ProgramSkillFrame f, bool slash) => _resolve().AvailableBoundaryZhongyong(f, slash);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBoundaryZhongyong(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginBoundaryZhongyong(supplied);
    internal bool BoundaryZhongyongBeforeDamageObserver(long id) => _resolve().BoundaryZhongyongBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryZhongyongChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryZhongyongChoices(f);
    internal bool BoundaryZhongyongDamageObserver(long id) => _resolve().BoundaryZhongyongDamageObserver(id);
    internal bool BoundaryZhongyongDyingObserver() => _resolve().BoundaryZhongyongDyingObserver();
    internal bool BoundaryZhongyongEntityAvailable(long action, int id) => _resolve().BoundaryZhongyongEntityAvailable(action, id);
    internal bool BoundaryZhongyongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryZhongyongFirstChild(f, child);
    internal bool BoundaryZhongyongLegalBan(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().BoundaryZhongyongLegalBan(q);
    internal bool BoundaryZhongyongMaterialBan(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().BoundaryZhongyongMaterialBan(q);
    internal bool BoundaryZhongyongNativeResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().BoundaryZhongyongNativeResponseBan(q);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryZhongyongObserverRoot() => _resolve().BoundaryZhongyongObserverRoot();
    internal bool BoundaryZhongyongOriginalTurn(global::CardGame.Core.BoundaryZhongyongReceipt r) => _resolve().BoundaryZhongyongOriginalTurn(r);
    internal int[] BoundaryZhongyongRecipients(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryZhongyongRecipients(f);
    internal bool BoundaryZhongyongResponseIsUse(int actor, global::CardGame.Core.CardKind kind, bool dying = false) => _resolve().BoundaryZhongyongResponseIsUse(actor, kind, dying);
    internal global::CardGame.Core.CardActionContext[] BoundaryZhongyongResponses(global::CardGame.Core.CardUseFrame use) => _resolve().BoundaryZhongyongResponses(use);
    internal void BoundaryZhongyongSubmittedBan(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().BoundaryZhongyongSubmittedBan(q);
    internal global::CardGame.Core.CardUseFrame? BoundaryZhongyongUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().BoundaryZhongyongUse(c, context);
    internal bool BoundaryZhongyongUseForbidden(int actor, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().BoundaryZhongyongUseForbidden(actor, ids);
    internal bool BoundaryZhongyongViewAsBan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().BoundaryZhongyongViewAsBan(q);
    internal bool CanOfferBoundaryZhongyong(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferBoundaryZhongyong(c, context);
    internal void ContinueBoundaryZhongyong(long id) => _resolve().ContinueBoundaryZhongyong(id);
    internal bool DrainBoundaryZhongyong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainBoundaryZhongyong(f);
    internal void FinishBoundaryZhongyong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishBoundaryZhongyong(f);
    internal bool HasBoundaryZhouCangCompletedUseObserver(long useId) => _resolve().HasBoundaryZhouCangCompletedUseObserver(useId);
    internal bool HasBoundaryZhouCangResponseObserver(global::CardGame.Core.CardActionContext a, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().HasBoundaryZhouCangResponseObserver(a, continuation);
    internal bool IsBoundaryZhongyongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsBoundaryZhongyongMovement(f, e, p);
    internal bool IsBoundaryZhouCangPhysicalClaim(long useId, int cardId) => _resolve().IsBoundaryZhouCangPhysicalClaim(useId, cardId);
    internal bool KeepBoundaryZhongyongPaidFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryZhongyongPaidFrame(f);
    internal void MoveBoundaryZhongyong(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.BoundaryZhongyongStage stage, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().MoveBoundaryZhongyong(f, stage, recipient, ids);
    internal void ObserveBoundaryZhongyongOrigin(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveBoundaryZhongyongOrigin(payload);
    internal void PublishBoundaryZhongyong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryZhongyong(f);
    internal void RecordBoundaryZhongyongEntities(global::CardGame.Core.CardActionContext a) => _resolve().RecordBoundaryZhongyongEntities(a);
    internal global::CardGame.Core.CardUseFrame RequireBoundaryZhongyongUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireBoundaryZhongyongUse(f);
    internal void ResolveBoundaryZhongyongChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveBoundaryZhongyongChoice(selected);
    internal bool ResumeBoundaryZhongyong(long id) => _resolve().ResumeBoundaryZhongyong(id);
    internal bool ReturnBoundaryZhongyongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryZhongyongMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiBoundaryZhongyong(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiBoundaryZhongyong(decision, frame);
    internal bool HasBoundaryZhouCang { get => _resolve().HasBoundaryZhouCang; }
}
