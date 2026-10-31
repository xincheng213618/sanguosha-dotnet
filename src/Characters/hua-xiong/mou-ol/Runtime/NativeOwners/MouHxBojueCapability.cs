#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouHxBojueCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaXiongMouOlEntries> getCharacterHuaXiongMouOlRuntime) : IMouHxBojueHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterHuaXiongMouOlRuntime().BeginMouHxBojue(f);
}
