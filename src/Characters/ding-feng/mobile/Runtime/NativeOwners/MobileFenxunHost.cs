#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileFenxunHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDingFengMobileRuntime>> getCharacterDingFengMobileRuntime) : IMobileFenxunHost
{
    public SkillProgramStepOutcome Grant(ProgramSkillFrame frame) => getCharacterDingFengMobileRuntime().Value.GrantMobileFenxun(frame);
}
