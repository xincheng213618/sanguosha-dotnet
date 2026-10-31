#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamagePreventionDrawEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionDrawRuntime> _resolve;
    private CoreGameEngineDamagePreventionDrawEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionDrawRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamagePreventionDrawEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamagePreventionDrawRuntime> resolve) => new(resolve);
}
