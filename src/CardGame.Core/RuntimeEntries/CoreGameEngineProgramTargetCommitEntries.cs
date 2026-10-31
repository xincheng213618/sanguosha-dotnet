#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime))]
internal sealed partial class CoreGameEngineProgramTargetCommitEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime> _resolve;
    private CoreGameEngineProgramTargetCommitEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTargetCommitEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime> resolve) => new(resolve);
}
