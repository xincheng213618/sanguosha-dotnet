#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceLiuSandaoHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceLiuSandaoRuntime>> getCoreGameEngineShanheSourceLiuSandaoRuntime) : IShanheSourceLiuSandaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineShanheSourceLiuSandaoRuntime().Value.ResolveShanheSourceLiuSandao(frame, operation);
}
