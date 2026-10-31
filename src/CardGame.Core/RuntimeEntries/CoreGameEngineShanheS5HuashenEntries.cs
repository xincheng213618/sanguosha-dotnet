#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5HuashenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5HuashenRuntime> _resolve;
    private CoreGameEngineShanheS5HuashenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5HuashenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5HuashenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5HuashenRuntime> resolve) => new(resolve);
}
