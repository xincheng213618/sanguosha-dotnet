#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouCangBoundaryMobileEntries
{
    internal bool AdvanceBoundaryZhongyong() => _resolve().AdvanceBoundaryZhongyong();
    internal int[] AvailableBoundaryZhongyong(ProgramSkillFrame f, bool slash) => _resolve().AvailableBoundaryZhongyong(f, slash);
    internal SkillProgramStepOutcome BeginBoundaryZhongyong(ProgramSkillFrame supplied) => _resolve().BeginBoundaryZhongyong(supplied);
    internal IReadOnlyList<PromptChoice> BoundaryZhongyongChoices(ProgramSkillFrame f) => _resolve().BoundaryZhongyongChoices(f);
    internal bool BoundaryZhongyongEntityAvailable(long action, int id) => _resolve().BoundaryZhongyongEntityAvailable(action, id);
    internal bool BoundaryZhongyongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BoundaryZhongyongFirstChild(f, child);
    internal bool BoundaryZhongyongLegalBan(ProgramLegalActionQuery q) => _resolve().BoundaryZhongyongLegalBan(q);
    internal bool BoundaryZhongyongMaterialBan(ProgramCardMaterialQuery q) => _resolve().BoundaryZhongyongMaterialBan(q);
    internal bool BoundaryZhongyongNativeResponseBan(ProgramNativeResponseQuery q) => _resolve().BoundaryZhongyongNativeResponseBan(q);
    internal ProgramSkillFrame? BoundaryZhongyongObserverRoot() => _resolve().BoundaryZhongyongObserverRoot();
    internal bool BoundaryZhongyongOriginalTurn(BoundaryZhongyongReceipt r) => _resolve().BoundaryZhongyongOriginalTurn(r);
    internal int[] BoundaryZhongyongRecipients(ProgramSkillFrame f) => _resolve().BoundaryZhongyongRecipients(f);
    internal bool BoundaryZhongyongResponseIsUse(int actor, CardKind kind, bool dying = false) => _resolve().BoundaryZhongyongResponseIsUse(actor, kind, dying);
    internal CardActionContext[] BoundaryZhongyongResponses(CardUseFrame use) => _resolve().BoundaryZhongyongResponses(use);
    internal void BoundaryZhongyongSubmittedBan(ProgramSubmittedUseQuery q) => _resolve().BoundaryZhongyongSubmittedBan(q);
    internal CardUseFrame? BoundaryZhongyongUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().BoundaryZhongyongUse(c, context);
    internal bool BoundaryZhongyongUseForbidden(int actor, IReadOnlyList<int> ids) => _resolve().BoundaryZhongyongUseForbidden(actor, ids);
    internal bool BoundaryZhongyongViewAsBan(ProgramViewAsLegalityQuery q) => _resolve().BoundaryZhongyongViewAsBan(q);
    internal bool CanOfferBoundaryZhongyong(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanOfferBoundaryZhongyong(c, context);
    internal void ContinueBoundaryZhongyong(long id) => _resolve().ContinueBoundaryZhongyong(id);
    internal bool DrainBoundaryZhongyong(ProgramSkillFrame f) => _resolve().DrainBoundaryZhongyong(f);
    internal void FinishBoundaryZhongyong(ProgramSkillFrame f) => _resolve().FinishBoundaryZhongyong(f);
    internal bool HasBoundaryZhouCangResponseObserver(CardActionContext a, ProgramCardContinuation continuation) => _resolve().HasBoundaryZhouCangResponseObserver(a, continuation);
    internal bool IsBoundaryZhouCangPhysicalClaim(long useId, int cardId) => _resolve().IsBoundaryZhouCangPhysicalClaim(useId, cardId);
    internal bool KeepBoundaryZhongyongPaidFrame(ProgramSkillFrame f) => _resolve().KeepBoundaryZhongyongPaidFrame(f);
    internal void MoveBoundaryZhongyong(ProgramSkillFrame f, BoundaryZhongyongStage stage, int recipient, IReadOnlyList<int> ids) => _resolve().MoveBoundaryZhongyong(f, stage, recipient, ids);
    internal void ObserveBoundaryZhongyongOrigin(IGameEvent payload) => _resolve().ObserveBoundaryZhongyongOrigin(payload);
    internal void PublishBoundaryZhongyong(ProgramSkillFrame f) => _resolve().PublishBoundaryZhongyong(f);
    internal void RecordBoundaryZhongyongEntities(CardActionContext a) => _resolve().RecordBoundaryZhongyongEntities(a);
    internal CardUseFrame RequireBoundaryZhongyongUse(ProgramSkillFrame f) => _resolve().RequireBoundaryZhongyongUse(f);
    internal void ResolveBoundaryZhongyongChoice(PromptChoice selected) => _resolve().ResolveBoundaryZhongyongChoice(selected);
    internal bool ResumeBoundaryZhongyong(long id) => _resolve().ResumeBoundaryZhongyong(id);
    internal bool ReturnBoundaryZhongyongMovement(ProgramSkillFrame f) => _resolve().ReturnBoundaryZhongyongMovement(f);
    internal PromptChoice SelectAiBoundaryZhongyong(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiBoundaryZhongyong(decision, frame);
    internal bool HasBoundaryZhouCang { get => _resolve().HasBoundaryZhouCang; }
}
