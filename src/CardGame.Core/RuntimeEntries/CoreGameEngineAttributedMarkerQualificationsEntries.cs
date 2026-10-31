#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAttributedMarkerQualificationsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAttributedMarkerQualificationsRuntime> _resolve;
    private CoreGameEngineAttributedMarkerQualificationsEntries(Func<EngineOwners.CoreGameEngineAttributedMarkerQualificationsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAttributedMarkerQualificationsEntries Create(Func<EngineOwners.CoreGameEngineAttributedMarkerQualificationsRuntime> resolve) => new(resolve);
}
