#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhuHuanPingkouHost(Func<RuntimeEntries.CharacterZhuHuanMobileEntries> getCharacterZhuHuanMobileRuntime) : IMobileZhuHuanPingkouHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCharacterZhuHuanMobileRuntime().BeginMobileZhuHuanPingkou(frame);
}
