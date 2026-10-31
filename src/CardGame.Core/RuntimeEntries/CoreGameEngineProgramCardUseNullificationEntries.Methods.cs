#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardUseNullificationEntries
{
    internal void AssertProgramUseNullification(global::CardGame.Core.CardUseFrame use) => _resolve().AssertProgramUseNullification(use);
    internal double GetWholeCardUseNullificationAiScore(int ownerSeat, global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().GetWholeCardUseNullificationAiScore(ownerSeat, window);
    internal bool HasBeforeTargetCardProgram(global::CardGame.Core.CardKind kind) => _resolve().HasBeforeTargetCardProgram(kind);
    internal bool IsExactSimpleBeforeTargetWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().IsExactSimpleBeforeTargetWindow(window);
    internal bool IsProgramUseIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsProgramUseIneffective(use);
    internal void NullifyCurrentProgramCardUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().NullifyCurrentProgramCardUse(frame);
    internal bool TryBeginSimpleBeforeTargetPrograms(long useId, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().TryBeginSimpleBeforeTargetPrograms(useId, continuation);
}
