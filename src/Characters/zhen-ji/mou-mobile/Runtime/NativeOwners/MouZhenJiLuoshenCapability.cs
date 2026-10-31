#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhenJiLuoshenCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhenJiMouMobileRuntime>> getCharacterZhenJiMouMobileRuntime) : IMouZhenJiLuoshenHost
{
    public SkillProgramStepOutcome Start(ProgramSkillFrame f) => getCharacterZhenJiMouMobileRuntime().Value.StartMouZhenJiLuoshen(f);
}
