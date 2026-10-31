#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheS5BaiyinCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheS5SimayiRuntime>> getCoreGameEngineShanheS5SimayiRuntime) : IShanheS5BaiyinHost
{
    public SkillProgramStepOutcome GrantShanheS5Baiyin(ProgramSkillFrame frame) => getCoreGameEngineShanheS5SimayiRuntime().Value.GrantShanheS5Baiyin(frame);
}
