#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ParticipantHandProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramsEntries> getCoreParticipantHandProgramsRuntime) : IParticipantHandProgramHost
{
    public SkillProgramStepOutcome ViewAndTakeHand(ProgramSkillFrame frame, int sourceSeat, int count) => getCoreParticipantHandProgramsRuntime().BeginPrivateHandTake(frame.Id, sourceSeat, count);
    public SkillProgramStepOutcome RequestHandRepayment(ProgramSkillFrame frame, int sourceSeat) => getCoreParticipantHandProgramsRuntime().BeginHandRepayment(frame.Id, sourceSeat);
}
