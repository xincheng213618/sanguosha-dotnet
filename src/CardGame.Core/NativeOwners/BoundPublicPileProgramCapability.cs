#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundPublicPileProgramCapability(Func<RuntimeEntries.CoreGameEngineBoundPublicPileProgramsEntries> getCoreGameEngineBoundPublicPileProgramsRuntime) : IBoundPublicPileProgramHost
{
    public SkillProgramStepOutcome SelectPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineBoundPublicPileProgramsRuntime().SelectProgramPublicPile(effect, frame);
    public SkillProgramStepOutcome DistributeBoundCards(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineBoundPublicPileProgramsRuntime().DistributeProgramBoundPublicPile(effect, frame);
}
