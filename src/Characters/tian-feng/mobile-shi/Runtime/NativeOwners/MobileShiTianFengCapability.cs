#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileShiTianFengCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterTianFengMobileShiRuntime>> getCharacterTianFengMobileShiRuntime) : IMobileShiTianFengHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => getCharacterTianFengMobileShiRuntime().Value.BeginMobileShiTianFeng(frame);
}
