#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiCaoShuangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoShuangMobileShiEntries> getCharacterCaoShuangMobileShiRuntime) : IShiCaoShuangHost
{
    public SkillProgramStepOutcome ResolveSheQuan(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().BeginShiCaoShuang(frame);
    public SkillProgramStepOutcome ResolveDianYi(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().BeginShiCaoShuang(frame);
    public SkillProgramStepOutcome ResolveJianZhuan(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().BeginShiCaoShuang(frame);
    public SkillProgramStepOutcome ResolveNiZunStash(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().BeginShiCaoShuang(frame);
    public SkillProgramStepOutcome ResolveNiZunUse(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().BeginShiCaoShuang(frame);
}
