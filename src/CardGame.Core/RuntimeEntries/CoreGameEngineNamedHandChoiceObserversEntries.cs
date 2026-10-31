#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNamedHandChoiceObserversRuntime))]
internal sealed partial class CoreGameEngineNamedHandChoiceObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNamedHandChoiceObserversRuntime> _resolve;
    private CoreGameEngineNamedHandChoiceObserversEntries(Func<EngineOwners.CoreGameEngineNamedHandChoiceObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedHandChoiceObserversEntries Create(Func<EngineOwners.CoreGameEngineNamedHandChoiceObserversRuntime> resolve) => new(resolve);
}
