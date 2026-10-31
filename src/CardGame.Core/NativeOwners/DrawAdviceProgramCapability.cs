#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DrawAdviceProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawAdviceProgramsEntries> getCoreGameEngineDrawAdviceProgramsRuntime) : IDrawAdviceProgramHost
{
    public SkillProgramStepOutcome BeginDrawAdvice(ProgramSkillFrame frame, int recipientSeat) => getCoreGameEngineDrawAdviceProgramsRuntime().BeginDrawAdvice(frame, recipientSeat);
}
