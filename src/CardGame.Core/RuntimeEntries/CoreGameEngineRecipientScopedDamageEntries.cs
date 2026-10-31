#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientScopedDamageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> _resolve;
    private CoreGameEngineRecipientScopedDamageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientScopedDamageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> resolve) => new(resolve);
}
