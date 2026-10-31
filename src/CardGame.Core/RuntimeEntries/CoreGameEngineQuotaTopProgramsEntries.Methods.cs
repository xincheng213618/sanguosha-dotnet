#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineQuotaTopProgramsEntries
{
    internal SkillProgramStepOutcome BeginQuotaTop(ProgramSkillFrame frame, int count) => _resolve().BeginQuotaTop(frame, count);
    internal int ComputeTurnCriterionQuota(int ownerSeat) => _resolve().ComputeTurnCriterionQuota(ownerSeat);
    internal void FinishQuotaTop(ProgramSkillFrame frame) => _resolve().FinishQuotaTop(frame);
    internal CardSnapshot[] GetQuotaPrivatelyViewedCards(int viewerSeat) => _resolve().GetQuotaPrivatelyViewedCards(viewerSeat);
    internal bool IsFirstTurnTarget(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsFirstTurnTarget(candidate, context);
    internal void NullifyProgramFirstTurnTarget(ProgramSkillFrame frame) => _resolve().NullifyProgramFirstTurnTarget(frame);
    internal void PublishQuotaTop(ProgramSkillFrame frame) => _resolve().PublishQuotaTop(frame);
    internal bool QuotaTopSourceValid(ProgramSkillFrame frame) => _resolve().QuotaTopSourceValid(frame);
    internal void RegisterFirstTurnTargetActions(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().RegisterFirstTurnTargetActions(action, window);
    internal void ResolveQuotaTopChoice(PromptChoice choice) => _resolve().ResolveQuotaTopChoice(choice);
    internal void ResumeQuotaLoss(ProgramSkillFrame frame) => _resolve().ResumeQuotaLoss(frame);
    internal bool TryResumeQuotaTop(long frameId) => _resolve().TryResumeQuotaTop(frameId);
}
