#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime> _resolve;
    private CoreGameEngineLastDamageSourceReciprocityEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLastDamageSourceReciprocityEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastDamageSourceReciprocityRuntime> resolve) => new(resolve);
}
