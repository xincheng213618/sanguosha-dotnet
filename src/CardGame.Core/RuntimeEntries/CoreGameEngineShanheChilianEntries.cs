#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheChilianEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheChilianRuntime> _resolve;
    private CoreGameEngineShanheChilianEntries(Func<EngineOwners.CoreGameEngineShanheChilianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheChilianEntries Create(Func<EngineOwners.CoreGameEngineShanheChilianRuntime> resolve) => new(resolve);
}
