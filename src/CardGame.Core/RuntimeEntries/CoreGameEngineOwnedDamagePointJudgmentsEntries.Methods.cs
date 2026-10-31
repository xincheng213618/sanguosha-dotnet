#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedDamagePointJudgmentsEntries
{
    internal SkillProgramStepOutcome BeginOwnedDamagePointJudgment(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BeginOwnedDamagePointJudgment(frame, effect);
    internal bool CanRunOwnedDamagePointJudgment(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanRunOwnedDamagePointJudgment(trigger, context, ownerSeat);
    internal bool IsOwnedDamagePointJudgmentProgramDying() => _resolve().IsOwnedDamagePointJudgmentProgramDying();
    internal bool IsOwnedDamagePointJudgmentResultClaimed(JudgmentFrame judgment) => _resolve().IsOwnedDamagePointJudgmentResultClaimed(judgment);
    internal bool OwnedDamagePointJudgmentEdge(int index, ProgramSkillFrame root, ProgramOwnedDamagePointJudgment receipt) => _resolve().OwnedDamagePointJudgmentEdge(index, root, receipt);
    internal ProgramSkillFrame? OwnedDamagePointJudgmentRoot(long damageWindowId) => _resolve().OwnedDamagePointJudgmentRoot(damageWindowId);
}
