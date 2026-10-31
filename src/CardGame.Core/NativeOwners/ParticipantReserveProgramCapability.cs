#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ParticipantReserveProgramCapability(Func<RuntimeEntries.CoreGameEngineParticipantAndReserveProgramsEntries> getCoreGameEngineParticipantAndReserveProgramsRuntime) : IParticipantReserveProgramHost
{
    public SkillProgramStepOutcome ExecuteParticipantReserve(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineParticipantAndReserveProgramsRuntime().ExecuteParticipantReserve(effect, frame);
}
