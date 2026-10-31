#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime))]
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime> _resolve;
    private CoreGameEngineSameNameHandDiscardOrDamageEntries(Func<EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSameNameHandDiscardOrDamageEntries Create(Func<EngineOwners.CoreGameEngineSameNameHandDiscardOrDamageRuntime> resolve) => new(resolve);
}
