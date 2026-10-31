#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTurnDrawDebtsRuntime))]
internal sealed partial class CoreGameEngineTurnDrawDebtsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> _resolve;
    private CoreGameEngineTurnDrawDebtsEntries(Func<EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnDrawDebtsEntries Create(Func<EngineOwners.CoreGameEngineTurnDrawDebtsRuntime> resolve) => new(resolve);
}
