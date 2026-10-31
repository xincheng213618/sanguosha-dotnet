#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RecastBoundCardProgramEffectCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreRecastBoundCardProgramsEntries> getCoreRecastBoundCardProgramsRuntime) : IRecastBoundCardProgramEffectHost
{
    public SkillProgramStepOutcome RecastBoundCard(ProgramSkillFrame frame, string sourceBind) => getCoreRecastBoundCardProgramsRuntime().BeginProgramBoundCardRecast(frame.Id, sourceBind);
}
