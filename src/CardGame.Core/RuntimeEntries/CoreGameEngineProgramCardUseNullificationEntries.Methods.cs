#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramCardUseNullificationEntries
{
    internal void AssertProgramUseNullification(CardUseFrame use) => _resolve().AssertProgramUseNullification(use);
    internal double GetWholeCardUseNullificationAiScore(int ownerSeat, ProgramCardTriggerWindowFrame window) => _resolve().GetWholeCardUseNullificationAiScore(ownerSeat, window);
    internal bool HasBeforeTargetCardProgram(CardKind kind) => _resolve().HasBeforeTargetCardProgram(kind);
    internal bool IsProgramUseIneffective(CardUseFrame use) => _resolve().IsProgramUseIneffective(use);
    internal void NullifyCurrentProgramCardUse(ProgramSkillFrame frame) => _resolve().NullifyCurrentProgramCardUse(frame);
    internal bool TryBeginSimpleBeforeTargetPrograms(long useId, ProgramSimpleCardContinuation continuation) => _resolve().TryBeginSimpleBeforeTargetPrograms(useId, continuation);
}
