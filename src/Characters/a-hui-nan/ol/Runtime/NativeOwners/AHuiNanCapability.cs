#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class AHuiNanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterAHuiNanOlEntries> getCharacterAHuiNanOlRuntime) : IAHuiNanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterAHuiNanOlRuntime().BeginAHuiNan(f);
}
