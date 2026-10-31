#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAttributedMarkerQualificationsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerQualificationsRuntime> _resolve;
    private CoreGameEngineAttributedMarkerQualificationsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerQualificationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAttributedMarkerQualificationsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAttributedMarkerQualificationsRuntime> resolve) => new(resolve);
}
