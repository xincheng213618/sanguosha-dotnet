#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime))]
internal sealed partial class CoreGameEnginePrivateTurnHoldsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> _resolve;
    private CoreGameEnginePrivateTurnHoldsEntries(Func<EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateTurnHoldsEntries Create(Func<EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> resolve) => new(resolve);
}
