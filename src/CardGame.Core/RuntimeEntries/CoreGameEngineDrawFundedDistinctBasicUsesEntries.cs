#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicUsesEntries(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicUsesEntries Create(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicUsesRuntime> resolve) => new(resolve);
}
