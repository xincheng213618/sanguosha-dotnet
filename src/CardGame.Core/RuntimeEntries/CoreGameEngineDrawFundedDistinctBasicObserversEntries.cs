#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicObserversEntries(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicObserversEntries Create(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> resolve) => new(resolve);
}
