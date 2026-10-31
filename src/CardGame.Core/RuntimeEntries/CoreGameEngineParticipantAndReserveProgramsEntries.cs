#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime))]
internal sealed partial class CoreGameEngineParticipantAndReserveProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> _resolve;
    private CoreGameEngineParticipantAndReserveProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineParticipantAndReserveProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineParticipantAndReserveProgramsRuntime> resolve) => new(resolve);
}
