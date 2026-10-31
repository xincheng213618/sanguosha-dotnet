#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime))]
internal sealed partial class CoreGameEngineRoundDistinctBasicPlayEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> _resolve;
    private CoreGameEngineRoundDistinctBasicPlayEntries(Func<EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundDistinctBasicPlayEntries Create(Func<EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> resolve) => new(resolve);
}
