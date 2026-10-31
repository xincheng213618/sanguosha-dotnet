#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgmentOutcomeClaimsEntries
{
    internal void CaptureProgramJudgmentOutcomeClaim(ProgramSkillFrame claimant, JudgmentFrame judgment, Card card, int sequenceBefore) => _resolve().CaptureProgramJudgmentOutcomeClaim(claimant, judgment, card, sequenceBefore);
    internal bool HasExactJudgmentOutcomeClaimDisposition(JudgmentFrame judgment) => _resolve().HasExactJudgmentOutcomeClaimDisposition(judgment);
    internal ProgramSkillFrame? JudgmentOutcomeRoot(JudgmentFrame judgment) => _resolve().JudgmentOutcomeRoot(judgment);
}
