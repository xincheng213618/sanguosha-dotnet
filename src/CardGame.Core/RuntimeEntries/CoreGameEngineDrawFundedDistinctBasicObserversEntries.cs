#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicObserversRuntime> resolve) => new(resolve);
}
