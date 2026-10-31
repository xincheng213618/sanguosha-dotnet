#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime))]
internal sealed partial class CoreGameEngineIncomingDistanceCourtesyEntries
{
    private readonly Func<EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime> _resolve;
    private CoreGameEngineIncomingDistanceCourtesyEntries(Func<EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIncomingDistanceCourtesyEntries Create(Func<EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime> resolve) => new(resolve);
}
