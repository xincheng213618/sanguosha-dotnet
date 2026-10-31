#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicOffersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicOffersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicOffersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> resolve) => new(resolve);
}
