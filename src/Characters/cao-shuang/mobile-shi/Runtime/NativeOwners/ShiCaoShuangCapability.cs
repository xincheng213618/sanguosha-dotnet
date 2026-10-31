#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiCaoShuangCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoShuangMobileShiRuntime>> getCharacterCaoShuangMobileShiRuntime) : IShiCaoShuangHost
{
    public SkillProgramStepOutcome ResolveSheQuan(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().Value.BeginShiCaoShuang(frame);
    public SkillProgramStepOutcome ResolveDianYi(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().Value.BeginShiCaoShuang(frame);
    public SkillProgramStepOutcome ResolveJianZhuan(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().Value.BeginShiCaoShuang(frame);
    public SkillProgramStepOutcome ResolveNiZunStash(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().Value.BeginShiCaoShuang(frame);
    public SkillProgramStepOutcome ResolveNiZunUse(ProgramSkillFrame frame) => getCharacterCaoShuangMobileShiRuntime().Value.BeginShiCaoShuang(frame);
}
