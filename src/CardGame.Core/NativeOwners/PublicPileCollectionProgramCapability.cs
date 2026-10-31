#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PublicPileCollectionProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEnginePublicPileCollectionRuntime>> getCoreGameEnginePublicPileCollectionRuntime) : IPublicPileCollectionProgramHost
{
    public SkillProgramStepOutcome ExecutePublicPileCollection(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEnginePublicPileCollectionRuntime().Value.ExecutePublicPileCollection(effect, frame);
}
