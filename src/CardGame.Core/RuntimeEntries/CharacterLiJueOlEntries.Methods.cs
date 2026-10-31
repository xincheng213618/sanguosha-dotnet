#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiJueOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiJue(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginLiJue(supplied, effect);
    internal bool CanContinueLiJue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueLiJue(f);
    internal bool CanOfferLiJue(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLiJue(candidate, trigger, context);
    internal void CaptureLiJueCompletedNullificationUse(long windowId, global::CardGame.Core.CardActionContext action) => _resolve().CaptureLiJueCompletedNullificationUse(windowId, action);
    internal void CaptureLiJueNullificationInvoice(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardActionContext action, bool incomingAccepted, global::CardGame.Core.NullificationRespondedEvent? incomingResponse) => _resolve().CaptureLiJueNullificationInvoice(window, action, incomingAccepted, incomingResponse);
    internal global::CardGame.Core.LiJueUseDiscardInvoice? CaptureLiJueUseDiscard(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureLiJueUseDiscard(batch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectLiJueUseDiscardCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectLiJueUseDiscardCandidates(batch);
    internal void CollectWindow_CollectLiJueUseDiscardCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectLiJueUseDiscardCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureLiJueUseDiscard(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureLiJueUseDiscard(batch, completed);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateLiJueMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateLiJueMovementContext(frame, candidate);
    internal void FinishLiJue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLiJue(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LangxiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LangxiChoices(f);
    internal int[] LangxiTargets(int owner) => _resolve().LangxiTargets(owner);
    internal bool LiJueCurrentSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiJueCurrentSource(f);
    internal bool LiJueFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().LiJueFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? LiJueObserverRoot() => _resolve().LiJueObserverRoot();
    internal int[] MatchingLiJueUseDiscardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingLiJueUseDiscardIndexes(batch, candidate);
    internal void ObserveLiJueNativeEvent(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveLiJueNativeEvent(payload);
    internal bool? ProveLiJueMovement(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramInstructionFeatures features, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().ProveLiJueMovement(candidate, features, batch);
    internal void PublishLangxi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLangxi(f);
    internal void RequireLiJue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireLiJue(f);
    internal void ResolveLangxiChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveLangxiChoice(choice);
    internal bool ResumeLiJue(long id) => _resolve().ResumeLiJue(id);
    internal bool ReturnLiJueMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLiJueMovement(f);
    internal bool TryAdvanceLiJueSubtree() => _resolve().TryAdvanceLiJueSubtree();
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateLiJueMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateLiJueMovementContext(frame, candidate);
    internal bool ValidLiJueUseDiscard(global::CardGame.Core.LiJueUseDiscardInvoice invoice, global::CardGame.Core.CardMovementBatchContext batch, int owner, bool requireAvailable = true) => _resolve().ValidLiJueUseDiscard(invoice, batch, owner, requireAvailable);
    internal bool YisuanFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().YisuanFinalizedAfterCategoryTargetWindow(query);
    internal bool HasYisuanCapability { get => _resolve().HasYisuanCapability; }
}
