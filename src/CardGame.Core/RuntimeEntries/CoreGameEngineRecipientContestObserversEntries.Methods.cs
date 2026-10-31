#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientContestObserversEntries
{
    internal bool HasUniqueHpPeerActualTargetObserver(long useId) => _resolve().HasUniqueHpPeerActualTargetObserver(useId);
    internal bool IsRecipientContestMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsRecipientContestMovement(f, effect, pending);
    internal bool IsRecipientContestProgramDying() => _resolve().IsRecipientContestProgramDying();
    internal bool RecipientContestFirstPaidChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().RecipientContestFirstPaidChild(f, child);
    internal ProgramSkillFrame? RecipientContestPaidObserverRoot() => _resolve().RecipientContestPaidObserverRoot();
    internal bool RecipientContestPaidPrefix(int rootIndex, int lastIndex) => _resolve().RecipientContestPaidPrefix(rootIndex, lastIndex);
    internal bool ReturnRecipientContestMovement(ProgramSkillFrame f) => _resolve().ReturnRecipientContestMovement(f);
}
