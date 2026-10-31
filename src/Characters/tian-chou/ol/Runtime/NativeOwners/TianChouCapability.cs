#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class TianChouCapability(Func<RuntimeEntries.CharacterTianChouOlEntries> getCharacterTianChouOlRuntime) : ITianChouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterTianChouOlRuntime().BeginTianChou(f);
}
