#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineIncomingDistanceCourtesyEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime> _resolve;
    private CoreGameEngineIncomingDistanceCourtesyEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineIncomingDistanceCourtesyEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIncomingDistanceCourtesyRuntime> resolve) => new(resolve);
}
