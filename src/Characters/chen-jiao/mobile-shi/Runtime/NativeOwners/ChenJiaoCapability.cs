#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ChenJiaoCapability(Func<RuntimeEntries.CharacterChenJiaoMobileShiEntries> getCharacterChenJiaoMobileShiRuntime) : IChenJiaoHost
{
    public SkillProgramStepOutcome ResolveQingYan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().BeginChenJiao(frame);
    public SkillProgramStepOutcome ResolveCeDuan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().BeginChenJiao(frame);
    public SkillProgramStepOutcome SettleCeDuan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().SettleChenJiaoCeDuan(frame);
}
