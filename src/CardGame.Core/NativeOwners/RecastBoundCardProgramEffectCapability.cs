#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RecastBoundCardProgramEffectCapability(Func<RuntimeEntries.CoreRecastBoundCardProgramsEntries> getCoreRecastBoundCardProgramsRuntime) : IRecastBoundCardProgramEffectHost
{
    public SkillProgramStepOutcome RecastBoundCard(ProgramSkillFrame frame, string sourceBind) => getCoreRecastBoundCardProgramsRuntime().BeginProgramBoundCardRecast(frame.Id, sourceBind);
}
