#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AHuiNanCapability(Func<RuntimeEntries.CharacterAHuiNanOlEntries> getCharacterAHuiNanOlRuntime) : IAHuiNanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterAHuiNanOlRuntime().BeginAHuiNan(f);
}
