#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ChenJiaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterChenJiaoMobileShiRuntime>> getCharacterChenJiaoMobileShiRuntime) : IChenJiaoHost
{
    public SkillProgramStepOutcome ResolveQingYan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().Value.BeginChenJiao(frame);
    public SkillProgramStepOutcome ResolveCeDuan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().Value.BeginChenJiao(frame);
    public SkillProgramStepOutcome SettleCeDuan(ProgramSkillFrame frame) => getCharacterChenJiaoMobileShiRuntime().Value.SettleChenJiaoCeDuan(frame);
}
