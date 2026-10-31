#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepPoisonOutcomesEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ApplyProgramHpLoss(long frameId, string skillId, int targetSeat, int amount, global::System.Action<int, int, int>? afterPayment = default) => _resolve().ApplyProgramHpLoss(frameId, skillId, targetSeat, amount, afterPayment);
    internal bool ContinuePaidPrepPoison(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidPrepPoison(f);
    internal bool HasPrepPoisonParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasPrepPoisonParent(f);
    internal bool IsFirstPrepPoisonCandidate(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().IsFirstPrepPoisonCandidate(c);
    internal void RequirePrepPoisonOutcome(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequirePrepPoisonOutcome(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolvePrepPoisonOutcomeByRound(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolvePrepPoisonOutcomeByRound(supplied);
    internal bool ResumePrepPoisonOutcome(long id) => _resolve().ResumePrepPoisonOutcome(id);
}
