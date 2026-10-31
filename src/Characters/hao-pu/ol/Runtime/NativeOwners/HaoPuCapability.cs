#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HaoPuCapability(Func<RuntimeEntries.CharacterHaoPuOlEntries> getCharacterHaoPuOlRuntime) : IHaoPuProgramHost
{
    public SkillProgramStepOutcome ResolveZhenying(ProgramSkillFrame f) => getCharacterHaoPuOlRuntime().BeginZhenying(f);
}
