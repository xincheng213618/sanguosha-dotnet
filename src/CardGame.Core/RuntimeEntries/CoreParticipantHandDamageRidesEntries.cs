#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreParticipantHandDamageRidesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _resolve;
    private CoreParticipantHandDamageRidesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreParticipantHandDamageRidesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> resolve) => new(resolve);
}
