#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecipientScopedDamageRuntime))]
internal sealed partial class CoreGameEngineRecipientScopedDamageEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> _resolve;
    private CoreGameEngineRecipientScopedDamageEntries(Func<EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecipientScopedDamageEntries Create(Func<EngineOwners.CoreGameEngineRecipientScopedDamageRuntime> resolve) => new(resolve);
}
