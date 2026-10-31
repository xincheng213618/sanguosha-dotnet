#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceZirunEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceZirunRuntime> _resolve;
    private CoreGameEngineShanheSourceZirunEntries(Func<EngineOwners.CoreGameEngineShanheSourceZirunRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceZirunEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceZirunRuntime> resolve) => new(resolve);
}
