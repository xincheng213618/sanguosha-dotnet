#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingRescueContinuationsEntries
{
    internal bool ProgramPaidDyingRescue(ProgramSkillFrame frame, DyingFrame dying) => _resolve().ProgramPaidDyingRescue(frame, dying);
}
