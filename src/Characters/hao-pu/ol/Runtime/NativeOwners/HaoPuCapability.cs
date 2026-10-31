#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HaoPuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHaoPuOlEntries> getCharacterHaoPuOlRuntime) : IHaoPuProgramHost
{
    public SkillProgramStepOutcome ResolveZhenying(ProgramSkillFrame f) => getCharacterHaoPuOlRuntime().BeginZhenying(f);
}
