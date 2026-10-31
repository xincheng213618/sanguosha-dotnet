#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouHxBojueCapability(Func<RuntimeEntries.CharacterHuaXiongMouOlEntries> getCharacterHuaXiongMouOlRuntime) : IMouHxBojueHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterHuaXiongMouOlRuntime().BeginMouHxBojue(f);
}
