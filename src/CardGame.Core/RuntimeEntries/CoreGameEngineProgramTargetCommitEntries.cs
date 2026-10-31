#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramTargetCommitEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime> _resolve;
    private CoreGameEngineProgramTargetCommitEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTargetCommitEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetCommitRuntime> resolve) => new(resolve);
}
