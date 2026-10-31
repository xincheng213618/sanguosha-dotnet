#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal bool HasWuLuXunZhangcaiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuLuXunZhangcai);

    SkillProgramStepOutcome IWuLuXunZhangcaiHost.Zhangcai(ProgramSkillFrame frame) => BeginWuLuXunZhangcai(frame);
    SkillProgramStepOutcome IWuLuXunZhangcaiHost.Ruxian(ProgramSkillFrame frame) => ActivateWuLuXunRuxian(frame);
}
