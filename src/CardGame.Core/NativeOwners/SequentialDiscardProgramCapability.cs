#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class SequentialDiscardProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialDiscardProgramsEntries> getCoreGameEngineSequentialDiscardProgramsRuntime) : ISequentialDiscardProgramHost
{
    public SkillProgramStepOutcome RunSequentialDiscard(ProgramSkillFrame f, SkillProgramEffect e, int target) => getCoreGameEngineSequentialDiscardProgramsRuntime().RunSequentialDiscard(f, e, target);
}
