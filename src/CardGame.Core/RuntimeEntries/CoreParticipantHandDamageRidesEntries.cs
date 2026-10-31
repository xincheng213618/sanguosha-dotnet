#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreParticipantHandDamageRidesRuntime))]
internal sealed partial class CoreParticipantHandDamageRidesEntries
{
    private readonly Func<EngineOwners.CoreParticipantHandDamageRidesRuntime> _resolve;
    private CoreParticipantHandDamageRidesEntries(Func<EngineOwners.CoreParticipantHandDamageRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreParticipantHandDamageRidesEntries Create(Func<EngineOwners.CoreParticipantHandDamageRidesRuntime> resolve) => new(resolve);
}
