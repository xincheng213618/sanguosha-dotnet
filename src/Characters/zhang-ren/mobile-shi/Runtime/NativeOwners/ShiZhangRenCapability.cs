#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiZhangRenCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangRenMobileShiEntries> getCharacterZhangRenMobileShiRuntime) : IShiZhangRenHost
{
    public SkillProgramStepOutcome ResolveFuan(ProgramSkillFrame frame) => getCharacterZhangRenMobileShiRuntime().BeginShiZrFuan(frame);
    public SkillProgramStepOutcome ResolveFuanStrike(ProgramSkillFrame frame) => getCharacterZhangRenMobileShiRuntime().BeginShiZrFuanStrike(frame);
    public SkillProgramStepOutcome ResolveYinXian(ProgramSkillFrame frame) => getCharacterZhangRenMobileShiRuntime().ResolveShiZrYinXian(frame);
}
