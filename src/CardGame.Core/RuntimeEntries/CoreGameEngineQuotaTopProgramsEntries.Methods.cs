#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineQuotaTopProgramsEntries
{
    internal void AssertQuotaTop(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect paused) => _resolve().AssertQuotaTop(frame, paused);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginQuotaTop(global::CardGame.Core.ProgramSkillFrame frame, int count) => _resolve().BeginQuotaTop(frame, count);
    internal int ComputeTurnCriterionQuota(int ownerSeat) => _resolve().ComputeTurnCriterionQuota(ownerSeat);
    internal void FinishQuotaTop(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishQuotaTop(frame);
    internal global::CardGame.Core.CardSnapshot[] GetQuotaPrivatelyViewedCards(int viewerSeat) => _resolve().GetQuotaPrivatelyViewedCards(viewerSeat);
    internal bool IsFirstTurnTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsFirstTurnTarget(candidate, context);
    internal void NullifyProgramFirstTurnTarget(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NullifyProgramFirstTurnTarget(frame);
    internal void PublishQuotaTop(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishQuotaTop(frame);
    internal bool QuotaTopSourceValid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().QuotaTopSourceValid(frame);
    internal void RegisterFirstTurnTargetActions(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().RegisterFirstTurnTargetActions(action, window);
    internal void ResolveQuotaTopChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveQuotaTopChoice(choice);
    internal void ResumeQuotaLoss(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResumeQuotaLoss(frame);
    internal bool TryResumeQuotaTop(long frameId) => _resolve().TryResumeQuotaTop(frameId);
}
