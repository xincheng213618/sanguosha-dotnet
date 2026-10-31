#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXueZongMobileEntries
{
    internal void AssertMobileFunanDeferredCompletions() => _resolve().AssertMobileFunanDeferredCompletions();
    internal bool BeginMobileFunanSettledCounterspellReward(NullificationWindowFrame parent) => _resolve().BeginMobileFunanSettledCounterspellReward(parent);
    internal void CaptureMobileFunanAcceptedAction(CardActionAcceptedEvent accepted) => _resolve().CaptureMobileFunanAcceptedAction(accepted);
    internal void DeferMobileFunanCounterspellReward(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> candidates) => _resolve().DeferMobileFunanCounterspellReward(action, window, candidates);
    internal bool HasMobileFunanUnansweredUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().HasMobileFunanUnansweredUse(candidate, context);
    internal bool IsMobileFunanActualUse(CardActionContext action) => _resolve().IsMobileFunanActualUse(action);
    internal bool MobileFunanAcceptsOtherCategoryUses(ProgramTargetCardWindowQuery query) => _resolve().MobileFunanAcceptsOtherCategoryUses(query);
    internal bool PreserveMobileFunanPaidReward(ProgramSkillFrame frame) => _resolve().PreserveMobileFunanPaidReward(frame);
    internal bool QualifyMobileFunanCompletedUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().QualifyMobileFunanCompletedUse(candidate, context);
}
