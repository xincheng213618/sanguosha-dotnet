#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreParticipantHandProgramRescueRidesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> _resolve;
    private CoreParticipantHandProgramRescueRidesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreParticipantHandProgramRescueRidesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> resolve) => new(resolve);
}
