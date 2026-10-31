#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundPublicPileProgramsEntries
{
    internal void BindSelectedPublicPile(ProgramSkillFrame frame, SkillProgramEffect effect, PublicPersistentPileSource? source) => _resolve().BindSelectedPublicPile(frame, effect, source);
    internal bool ContinueBoundPublicPileDistribution(ProgramSkillFrame frame) => _resolve().ContinueBoundPublicPileDistribution(frame);
    internal SkillProgramStepOutcome DistributeProgramBoundPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().DistributeProgramBoundPublicPile(effect, frame);
    internal IReadOnlyList<int> PublicPileDistributionTargets(ProgramSkillFrame frame) => _resolve().PublicPileDistributionTargets(frame);
    internal SkillProgramStepOutcome SelectProgramPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().SelectProgramPublicPile(effect, frame);
}
