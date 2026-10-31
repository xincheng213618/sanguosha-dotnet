#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgmentOutcomeClaimsEntries
{
    internal void CaptureProgramJudgmentOutcomeClaim(global::CardGame.Core.ProgramSkillFrame claimant, global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, int sequenceBefore) => _resolve().CaptureProgramJudgmentOutcomeClaim(claimant, judgment, card, sequenceBefore);
    internal bool HasExactJudgmentOutcomeClaimDisposition(global::CardGame.Core.JudgmentFrame judgment) => _resolve().HasExactJudgmentOutcomeClaimDisposition(judgment);
    internal global::CardGame.Core.ProgramSkillFrame? JudgmentOutcomeRoot(global::CardGame.Core.JudgmentFrame judgment) => _resolve().JudgmentOutcomeRoot(judgment);
}
