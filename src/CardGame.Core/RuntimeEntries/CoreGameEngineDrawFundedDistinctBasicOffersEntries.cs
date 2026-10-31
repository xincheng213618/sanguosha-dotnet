#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicOffersEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicOffersEntries(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicOffersEntries Create(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime> resolve) => new(resolve);
}
