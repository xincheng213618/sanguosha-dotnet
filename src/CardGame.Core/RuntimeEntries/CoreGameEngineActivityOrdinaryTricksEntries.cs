#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime))]
internal sealed partial class CoreGameEngineActivityOrdinaryTricksEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> _resolve;
    private CoreGameEngineActivityOrdinaryTricksEntries(Func<EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActivityOrdinaryTricksEntries Create(Func<EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime> resolve) => new(resolve);
}
