#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicUsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicUsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicUsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> resolve) => new(resolve);
}
