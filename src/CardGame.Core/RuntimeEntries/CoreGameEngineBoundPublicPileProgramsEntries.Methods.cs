#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundPublicPileProgramsEntries
{
    internal void AssertSelectedPublicPileBindings(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertSelectedPublicPileBindings(frame, plan);
    internal void BindSelectedPublicPile(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.PublicPersistentPileSource? source) => _resolve().BindSelectedPublicPile(frame, effect, source);
    internal bool ContinueBoundPublicPileDistribution(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueBoundPublicPileDistribution(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome DistributeProgramBoundPublicPile(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DistributeProgramBoundPublicPile(effect, frame);
    internal global::System.Collections.Generic.IReadOnlyList<int> PublicPileDistributionTargets(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublicPileDistributionTargets(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome SelectProgramPublicPile(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectProgramPublicPile(effect, frame);
}
