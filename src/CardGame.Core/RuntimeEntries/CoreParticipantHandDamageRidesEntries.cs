#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed partial class CoreParticipantHandDamageRidesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> _resolve;
    private CoreParticipantHandDamageRidesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreParticipantHandDamageRidesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandDamageRidesRuntime> resolve) => new(resolve);
}
