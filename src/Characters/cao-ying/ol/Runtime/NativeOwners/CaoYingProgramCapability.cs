#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class CaoYingProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoYingOlEntries> getCharacterCaoYingOlRuntime) : ICaoYingProgramHost
{
    public SkillProgramStepOutcome BeginCaoYing(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterCaoYingOlRuntime().BeginCaoYing(f, e);
}
