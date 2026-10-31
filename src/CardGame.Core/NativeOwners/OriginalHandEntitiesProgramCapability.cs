#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class OriginalHandEntitiesProgramCapability(Func<RuntimeEntries.CoreGameEngineOriginalHandEntitiesEntries> getCoreGameEngineOriginalHandEntitiesRuntime, Func<RuntimeEntries.CoreGameEngineOriginalHandAwakeningEntries> getCoreGameEngineOriginalHandAwakeningRuntime) : IOriginalHandEntitiesProgramHost
{
    public SkillProgramStepOutcome ExecuteOriginalHandEntityOperation(ProgramSkillFrame f, SkillProgramEffect e) => e.Op switch
    {
        SkillProgramEffectOp.InitializeOriginalHandEntities => getCoreGameEngineOriginalHandEntitiesRuntime().InitializeOriginalHandEntities(f, e.StateId!),
        SkillProgramEffectOp.OfferOriginalHandLossBenefit => getCoreGameEngineOriginalHandEntitiesRuntime().BeginOriginalHandBenefit(f, e.StateId!),
        SkillProgramEffectOp.InheritOriginalHandBonuses => getCoreGameEngineOriginalHandEntitiesRuntime().BeginOriginalHandInheritance(f, e.StateId!),
        SkillProgramEffectOp.AwakenWhenOriginalHandEmpty => getCoreGameEngineOriginalHandAwakeningRuntime().BeginOriginalHandAwakening(f, e),
        _ => throw new InvalidOperationException("Unknown original-hand operation.")};
}
