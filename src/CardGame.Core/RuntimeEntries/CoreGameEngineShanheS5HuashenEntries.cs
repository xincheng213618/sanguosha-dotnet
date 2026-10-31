#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5HuashenEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheS5HuashenRuntime> _resolve;
    private CoreGameEngineShanheS5HuashenEntries(Func<EngineOwners.CoreGameEngineShanheS5HuashenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5HuashenEntries Create(Func<EngineOwners.CoreGameEngineShanheS5HuashenRuntime> resolve) => new(resolve);
}
