#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceYanyuEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceYanyuRuntime> _resolve;
    private CoreGameEngineShanheSourceYanyuEntries(Func<EngineOwners.CoreGameEngineShanheSourceYanyuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceYanyuEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceYanyuRuntime> resolve) => new(resolve);
}
