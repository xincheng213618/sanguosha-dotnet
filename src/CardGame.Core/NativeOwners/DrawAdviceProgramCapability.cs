#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DrawAdviceProgramCapability(Func<RuntimeEntries.CoreGameEngineDrawAdviceProgramsEntries> getCoreGameEngineDrawAdviceProgramsRuntime) : IDrawAdviceProgramHost
{
    public SkillProgramStepOutcome BeginDrawAdvice(ProgramSkillFrame frame, int recipientSeat) => getCoreGameEngineDrawAdviceProgramsRuntime().BeginDrawAdvice(frame, recipientSeat);
}
