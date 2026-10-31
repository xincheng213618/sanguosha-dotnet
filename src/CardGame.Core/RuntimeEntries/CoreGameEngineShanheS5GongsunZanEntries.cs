#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5GongsunZanEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5GongsunZanRuntime> _resolve;
    private CoreGameEngineShanheS5GongsunZanEntries(Func<EngineOwners.CoreGameEngineShanheS5GongsunZanRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5GongsunZanEntries Create(Func<EngineOwners.CoreGameEngineShanheS5GongsunZanRuntime> resolve) => new(resolve);
}
