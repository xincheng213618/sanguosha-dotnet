#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DrawAdviceProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDrawAdviceProgramsRuntime>> getCoreGameEngineDrawAdviceProgramsRuntime) : IDrawAdviceProgramHost
{
    public SkillProgramStepOutcome BeginDrawAdvice(ProgramSkillFrame frame, int recipientSeat) => getCoreGameEngineDrawAdviceProgramsRuntime().Value.BeginDrawAdvice(frame, recipientSeat);
}
