#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramVirtualDuelRuntime))]
internal sealed partial class CoreGameEngineProgramVirtualDuelEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> _resolve;
    private CoreGameEngineProgramVirtualDuelEntries(Func<EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramVirtualDuelEntries Create(Func<EngineOwners.CoreGameEngineProgramVirtualDuelRuntime> resolve) => new(resolve);
}
