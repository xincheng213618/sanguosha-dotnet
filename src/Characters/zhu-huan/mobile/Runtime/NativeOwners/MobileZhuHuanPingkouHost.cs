#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhuHuanPingkouHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhuHuanMobileRuntime>> getCharacterZhuHuanMobileRuntime) : IMobileZhuHuanPingkouHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCharacterZhuHuanMobileRuntime().Value.BeginMobileZhuHuanPingkou(frame);
}
