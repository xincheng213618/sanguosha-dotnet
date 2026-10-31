#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime))]
internal sealed partial class CoreGameEngineActorHandLimitTargetProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime> _resolve;
    private CoreGameEngineActorHandLimitTargetProgramsEntries(Func<EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActorHandLimitTargetProgramsEntries Create(Func<EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime> resolve) => new(resolve);
}
