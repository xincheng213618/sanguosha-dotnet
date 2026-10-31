#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ShanheXiashengHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheXiashengRuntime>> getCoreGameEngineShanheXiashengRuntime) : IShanheXiashengHost
{
    public SkillProgramStepOutcome ResolveXiasheng(ProgramSkillFrame frame, string stage) => getCoreGameEngineShanheXiashengRuntime().Value.BeginShanheXiasheng(frame, stage);
}
