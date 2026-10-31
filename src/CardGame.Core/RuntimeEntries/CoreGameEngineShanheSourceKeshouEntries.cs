#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceKeshouEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceKeshouRuntime> _resolve;
    private CoreGameEngineShanheSourceKeshouEntries(Func<EngineOwners.CoreGameEngineShanheSourceKeshouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceKeshouEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceKeshouRuntime> resolve) => new(resolve);
}
