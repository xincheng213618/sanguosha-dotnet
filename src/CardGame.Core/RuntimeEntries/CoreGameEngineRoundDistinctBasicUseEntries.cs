#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime))]
internal sealed partial class CoreGameEngineRoundDistinctBasicUseEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> _resolve;
    private CoreGameEngineRoundDistinctBasicUseEntries(Func<EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundDistinctBasicUseEntries Create(Func<EngineOwners.CoreGameEngineRoundDistinctBasicUseRuntime> resolve) => new(resolve);
}
