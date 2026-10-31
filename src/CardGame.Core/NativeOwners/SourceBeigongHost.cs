#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class SourceBeigongHost(Func<RuntimeEntries.CoreGameEngineSourceDingxiEntries> getCoreGameEngineSourceDingxiRuntime, Func<RuntimeEntries.CoreGameEngineSourceBeigongEntries> getCoreGameEngineSourceBeigongRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSourceHuojieRuntime>> getCoreGameEngineSourceHuojieRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSourceXingxiaRuntime>> getCoreGameEngineSourceXingxiaRuntime) : ISourceBeigongHost
{
    public SkillProgramStepOutcome Run(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveSourceDingxiUsedCard => getCoreGameEngineSourceDingxiRuntime().StartSourceDingxi(f),
        SkillProgramEffectOp.DrawSourceDingxiPile => getCoreGameEngineSourceBeigongRuntime().DrawSourceDingxi(f),
        SkillProgramEffectOp.ResolveSourceHuojie => getCoreGameEngineSourceHuojieRuntime().Value.StartSourceHuojie(f),
        SkillProgramEffectOp.ResolveSourceXingxia => getCoreGameEngineSourceXingxiaRuntime().Value.StartSourceXingxia(f),
        _ => throw new InvalidOperationException("Unrecognized original Beigong instruction.")};
}
