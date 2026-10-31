#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiJueOlEntries
{
    internal SkillProgramStepOutcome BeginLiJue(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginLiJue(supplied, effect);
    internal bool CanContinueLiJue(ProgramSkillFrame f) => _resolve().CanContinueLiJue(f);
    internal bool CanOfferLiJue(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferLiJue(candidate, trigger, context);
    internal void CaptureLiJueCompletedNullificationUse(long windowId, CardActionContext action) => _resolve().CaptureLiJueCompletedNullificationUse(windowId, action);
    internal void CaptureLiJueNullificationInvoice(NullificationWindowFrame window, CardActionContext action, bool incomingAccepted, NullificationRespondedEvent? incomingResponse) => _resolve().CaptureLiJueNullificationInvoice(window, action, incomingAccepted, incomingResponse);
    internal LiJueUseDiscardInvoice? CaptureLiJueUseDiscard(CardMovementBatchContext batch) => _resolve().CaptureLiJueUseDiscard(batch);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectLiJueUseDiscardCandidates(CardMovementBatchContext batch) => _resolve().CollectLiJueUseDiscardCandidates(batch);
    internal void CollectWindow_CollectLiJueUseDiscardCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectLiJueUseDiscardCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureLiJueUseDiscard(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureLiJueUseDiscard(batch, completed);
    internal ProgramSkillWindowContext CreateLiJueMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateLiJueMovementContext(frame, candidate);
    internal void FinishLiJue(ProgramSkillFrame f) => _resolve().FinishLiJue(f);
    internal IReadOnlyList<PromptChoice> LangxiChoices(ProgramSkillFrame f) => _resolve().LangxiChoices(f);
    internal int[] LangxiTargets(int owner) => _resolve().LangxiTargets(owner);
    internal bool LiJueCurrentSource(ProgramSkillFrame f) => _resolve().LiJueCurrentSource(f);
    internal bool LiJueFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().LiJueFirstChild(root, child);
    internal ProgramSkillFrame? LiJueObserverRoot() => _resolve().LiJueObserverRoot();
    internal int[] MatchingLiJueUseDiscardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingLiJueUseDiscardIndexes(batch, candidate);
    internal void ObserveLiJueNativeEvent(IGameEvent payload) => _resolve().ObserveLiJueNativeEvent(payload);
    internal bool? ProveLiJueMovement(ProgramTriggerCandidate candidate, ProgramInstructionFeatures features, CardMovementBatchContext batch) => _resolve().ProveLiJueMovement(candidate, features, batch);
    internal void PublishLangxi(ProgramSkillFrame f) => _resolve().PublishLangxi(f);
    internal void RequireLiJue(ProgramSkillFrame f) => _resolve().RequireLiJue(f);
    internal void ResolveLangxiChoice(PromptChoice choice) => _resolve().ResolveLangxiChoice(choice);
    internal bool ResumeLiJue(long id) => _resolve().ResumeLiJue(id);
    internal bool ReturnLiJueMovement(ProgramSkillFrame f) => _resolve().ReturnLiJueMovement(f);
    internal bool TryAdvanceLiJueSubtree() => _resolve().TryAdvanceLiJueSubtree();
    internal ProgramSkillWindowContext? TryCreateLiJueMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateLiJueMovementContext(frame, candidate);
    internal bool ValidLiJueUseDiscard(LiJueUseDiscardInvoice invoice, CardMovementBatchContext batch, int owner, bool requireAvailable = true) => _resolve().ValidLiJueUseDiscard(invoice, batch, owner, requireAvailable);
    internal bool YisuanFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().YisuanFinalizedAfterCategoryTargetWindow(query);
    internal bool HasYisuanCapability { get => _resolve().HasYisuanCapability; }
}
