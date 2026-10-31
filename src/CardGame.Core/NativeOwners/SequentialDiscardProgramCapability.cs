#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SequentialDiscardProgramCapability(Func<RuntimeEntries.CoreGameEngineSequentialDiscardProgramsEntries> getCoreGameEngineSequentialDiscardProgramsRuntime) : ISequentialDiscardProgramHost
{
    public SkillProgramStepOutcome RunSequentialDiscard(ProgramSkillFrame f, SkillProgramEffect e, int target) => getCoreGameEngineSequentialDiscardProgramsRuntime().RunSequentialDiscard(f, e, target);
}
