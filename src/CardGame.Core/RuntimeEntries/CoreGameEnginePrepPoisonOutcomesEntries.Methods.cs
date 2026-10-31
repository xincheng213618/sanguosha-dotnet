#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepPoisonOutcomesEntries
{
    internal SkillProgramStepOutcome ApplyProgramHpLoss(long frameId, string skillId, int targetSeat, int amount, Action<int, int, int>? afterPayment = default) => _resolve().ApplyProgramHpLoss(frameId, skillId, targetSeat, amount, afterPayment);
    internal bool ContinuePaidPrepPoison(ProgramSkillFrame f) => _resolve().ContinuePaidPrepPoison(f);
    internal bool HasPrepPoisonParent(ProgramSkillFrame f) => _resolve().HasPrepPoisonParent(f);
    internal bool IsFirstPrepPoisonCandidate(ProgramTriggerLegalityContext c) => _resolve().IsFirstPrepPoisonCandidate(c);
    internal void RequirePrepPoisonOutcome(ProgramSkillFrame f) => _resolve().RequirePrepPoisonOutcome(f);
    internal SkillProgramStepOutcome ResolvePrepPoisonOutcomeByRound(ProgramSkillFrame supplied) => _resolve().ResolvePrepPoisonOutcomeByRound(supplied);
    internal bool ResumePrepPoisonOutcome(long id) => _resolve().ResumePrepPoisonOutcome(id);
}
