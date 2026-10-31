#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PublicPileCollectionProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileCollectionEntries> getCoreGameEnginePublicPileCollectionRuntime) : IPublicPileCollectionProgramHost
{
    public SkillProgramStepOutcome ExecutePublicPileCollection(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEnginePublicPileCollectionRuntime().ExecutePublicPileCollection(effect, frame);
}
