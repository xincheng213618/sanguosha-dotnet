#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualPhaseEndsRuntime))]
internal sealed partial class CoreGameEngineActualPhaseEndsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualPhaseEndsRuntime> _resolve;
    private CoreGameEngineActualPhaseEndsEntries(Func<EngineOwners.CoreGameEngineActualPhaseEndsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualPhaseEndsEntries Create(Func<EngineOwners.CoreGameEngineActualPhaseEndsRuntime> resolve) => new(resolve);
}
