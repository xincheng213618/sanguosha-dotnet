#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileZhuHuanPingkouHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhuHuanMobileEntries> getCharacterZhuHuanMobileRuntime) : IMobileZhuHuanPingkouHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame frame) => getCharacterZhuHuanMobileRuntime().BeginMobileZhuHuanPingkou(frame);
}
