#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundPublicPileProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundPublicPileProgramsEntries> getCoreGameEngineBoundPublicPileProgramsRuntime) : IBoundPublicPileProgramHost
{
    public SkillProgramStepOutcome SelectPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineBoundPublicPileProgramsRuntime().SelectProgramPublicPile(effect, frame);
    public SkillProgramStepOutcome DistributeBoundCards(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineBoundPublicPileProgramsRuntime().DistributeProgramBoundPublicPile(effect, frame);
}
