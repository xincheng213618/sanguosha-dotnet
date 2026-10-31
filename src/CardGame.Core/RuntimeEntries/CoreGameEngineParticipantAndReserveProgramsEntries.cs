#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime))]
internal sealed partial class CoreGameEngineParticipantAndReserveProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> _resolve;
    private CoreGameEngineParticipantAndReserveProgramsEntries(Func<EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineParticipantAndReserveProgramsEntries Create(Func<EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> resolve) => new(resolve);
}
