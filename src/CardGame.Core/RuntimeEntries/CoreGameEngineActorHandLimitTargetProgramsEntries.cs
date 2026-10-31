#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime))]
internal sealed partial class CoreGameEngineActorHandLimitTargetProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime> _resolve;
    private CoreGameEngineActorHandLimitTargetProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActorHandLimitTargetProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitTargetProgramsRuntime> resolve) => new(resolve);
}
