#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedDamagePointJudgmentsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOwnedDamagePointJudgment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginOwnedDamagePointJudgment(frame, effect);
    internal bool CanRunOwnedDamagePointJudgment(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanRunOwnedDamagePointJudgment(trigger, context, ownerSeat);
    internal bool HasOwnedDamagePointJudgmentObserver(long damageWindowId) => _resolve().HasOwnedDamagePointJudgmentObserver(damageWindowId);
    internal bool HasOwnedDamagePointJudgmentRide(long judgmentFrameId) => _resolve().HasOwnedDamagePointJudgmentRide(judgmentFrameId);
    internal bool IsExactOwnedDamagePointJudgmentFinalizedChildSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int windowIndex) => _resolve().IsExactOwnedDamagePointJudgmentFinalizedChildSubtree(window, judgment, windowIndex);
    internal bool IsOwnedDamagePointJudgmentProgramDying() => _resolve().IsOwnedDamagePointJudgmentProgramDying();
    internal bool IsOwnedDamagePointJudgmentResultClaimed(global::CardGame.Core.JudgmentFrame judgment) => _resolve().IsOwnedDamagePointJudgmentResultClaimed(judgment);
    internal bool OwnedDamagePointJudgmentEdge(int index, global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ProgramOwnedDamagePointJudgment receipt) => _resolve().OwnedDamagePointJudgmentEdge(index, root, receipt);
    internal global::CardGame.Core.ProgramSkillFrame? OwnedDamagePointJudgmentRoot(long damageWindowId) => _resolve().OwnedDamagePointJudgmentRoot(damageWindowId);
}
