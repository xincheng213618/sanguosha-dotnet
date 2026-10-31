#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicResponsesRuntime> resolve) => new(resolve);
}
