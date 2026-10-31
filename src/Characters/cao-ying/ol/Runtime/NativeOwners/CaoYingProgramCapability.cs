#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class CaoYingProgramCapability(Func<RuntimeEntries.CharacterCaoYingOlEntries> getCharacterCaoYingOlRuntime) : ICaoYingProgramHost
{
    public SkillProgramStepOutcome BeginCaoYing(ProgramSkillFrame f, SkillProgramEffect e) => getCharacterCaoYingOlRuntime().BeginCaoYing(f, e);
}
