#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConditionalDiscardDuelObserversEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? ConditionalDuelPaidObserverRoot() => _resolve().ConditionalDuelPaidObserverRoot();
    internal bool IsConditionalDiscardDuelProgramDying() => _resolve().IsConditionalDiscardDuelProgramDying();
    internal bool IsConditionalDuelAwaitedMovement(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsConditionalDuelAwaitedMovement(root, effect, pending);
}
