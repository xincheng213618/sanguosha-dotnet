#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXueZongMobileEntries
{
    internal void AssertMobileFunanDeferredCompletions() => _resolve().AssertMobileFunanDeferredCompletions();
    internal bool BeginMobileFunanSettledCounterspellReward(global::CardGame.Core.NullificationWindowFrame parent) => _resolve().BeginMobileFunanSettledCounterspellReward(parent);
    internal void CaptureMobileFunanAcceptedAction(global::CardGame.Core.CardActionAcceptedEvent accepted) => _resolve().CaptureMobileFunanAcceptedAction(accepted);
    internal void DeferMobileFunanCounterspellReward(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> candidates) => _resolve().DeferMobileFunanCounterspellReward(action, window, candidates);
    internal bool HasMobileFunanUnansweredUse(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasMobileFunanUnansweredUse(candidate, context);
    internal bool IsMobileFunanActualUse(global::CardGame.Core.CardActionContext action) => _resolve().IsMobileFunanActualUse(action);
    internal bool MobileFunanAcceptsOtherCategoryUses(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().MobileFunanAcceptsOtherCategoryUses(query);
    internal bool PreserveMobileFunanPaidReward(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PreserveMobileFunanPaidReward(frame);
    internal bool QualifyMobileFunanCompletedUse(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyMobileFunanCompletedUse(candidate, context);
}
