#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime))]
internal sealed partial class CoreGameEngineSoleIncomingRangeDrawEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> _resolve;
    private CoreGameEngineSoleIncomingRangeDrawEntries(Func<EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleIncomingRangeDrawEntries Create(Func<EngineOwners.CoreGameEngineSoleIncomingRangeDrawRuntime> resolve) => new(resolve);
}
