#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ShajueEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShajueRuntime> _resolve;
    private CoreGameEngineShanheS5ShajueEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShajueRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5ShajueEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5ShajueRuntime> resolve) => new(resolve);
}
