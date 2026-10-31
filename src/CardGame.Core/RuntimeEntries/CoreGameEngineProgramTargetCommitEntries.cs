#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramTargetCommitRuntime))]
internal sealed partial class CoreGameEngineProgramTargetCommitEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramTargetCommitRuntime> _resolve;
    private CoreGameEngineProgramTargetCommitEntries(Func<EngineOwners.CoreGameEngineProgramTargetCommitRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTargetCommitEntries Create(Func<EngineOwners.CoreGameEngineProgramTargetCommitRuntime> resolve) => new(resolve);
}
