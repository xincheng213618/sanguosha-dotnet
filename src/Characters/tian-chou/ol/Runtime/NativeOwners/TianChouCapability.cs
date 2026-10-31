#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class TianChouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterTianChouOlEntries> getCharacterTianChouOlRuntime) : ITianChouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterTianChouOlRuntime().BeginTianChou(f);
}
