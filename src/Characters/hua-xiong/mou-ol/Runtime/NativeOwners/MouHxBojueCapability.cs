#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouHxBojueCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuaXiongMouOlRuntime>> getCharacterHuaXiongMouOlRuntime) : IMouHxBojueHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f) => getCharacterHuaXiongMouOlRuntime().Value.BeginMouHxBojue(f);
}
