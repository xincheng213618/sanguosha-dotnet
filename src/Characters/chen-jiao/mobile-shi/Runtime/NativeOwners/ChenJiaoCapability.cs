#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ChenJiaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenJiaoMobileShiEntries> getCharacterChenJiaoMobileShiRuntime) : IChenJiaoHost
{
    public SkillProgramStepOutcome ResolveQingYan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().BeginChenJiao(frame);
    public SkillProgramStepOutcome ResolveCeDuan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().BeginChenJiao(frame);
    public SkillProgramStepOutcome SettleCeDuan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().SettleChenJiaoCeDuan(frame);
}
