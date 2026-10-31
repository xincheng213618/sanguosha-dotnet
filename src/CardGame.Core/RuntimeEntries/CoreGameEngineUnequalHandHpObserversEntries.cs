#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUnequalHandHpObserversRuntime))]
internal sealed partial class CoreGameEngineUnequalHandHpObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUnequalHandHpObserversRuntime> _resolve;
    private CoreGameEngineUnequalHandHpObserversEntries(Func<EngineOwners.CoreGameEngineUnequalHandHpObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUnequalHandHpObserversEntries Create(Func<EngineOwners.CoreGameEngineUnequalHandHpObserversRuntime> resolve) => new(resolve);
}
