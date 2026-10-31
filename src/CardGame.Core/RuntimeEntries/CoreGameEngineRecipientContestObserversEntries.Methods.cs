#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientContestObserversEntries
{
    internal bool HasUniqueHpPeerActualTargetObserver(long useId) => _resolve().HasUniqueHpPeerActualTargetObserver(useId);
    internal bool IsRecipientContestMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsRecipientContestMovement(f, effect, pending);
    internal bool IsRecipientContestProgramDying() => _resolve().IsRecipientContestProgramDying();
    internal bool RecipientContestFirstPaidChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().RecipientContestFirstPaidChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? RecipientContestPaidObserverRoot() => _resolve().RecipientContestPaidObserverRoot();
    internal bool RecipientContestPaidPrefix(int rootIndex, int lastIndex) => _resolve().RecipientContestPaidPrefix(rootIndex, lastIndex);
    internal bool ReturnRecipientContestMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnRecipientContestMovement(f);
}
