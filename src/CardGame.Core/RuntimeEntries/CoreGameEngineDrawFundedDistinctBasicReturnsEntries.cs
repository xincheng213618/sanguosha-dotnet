#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime))]
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> _resolve;
    private CoreGameEngineDrawFundedDistinctBasicReturnsEntries(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawFundedDistinctBasicReturnsEntries Create(Func<EngineOwners.CoreGameEngineDrawFundedDistinctBasicReturnsRuntime> resolve) => new(resolve);
}
