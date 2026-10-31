#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicResponsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicResponsesEntries(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicResponsesEntries Create(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> resolve) => new(resolve);
}
