#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreParticipantHandProgramRescueRidesRuntime))]
internal sealed partial class CoreParticipantHandProgramRescueRidesEntries
{
    private readonly Func<EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> _resolve;
    private CoreParticipantHandProgramRescueRidesEntries(Func<EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreParticipantHandProgramRescueRidesEntries Create(Func<EngineOwners.CoreParticipantHandProgramRescueRidesRuntime> resolve) => new(resolve);
}
