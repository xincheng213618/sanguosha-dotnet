#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PublicPersistentPileProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPersistentPilesEntries> getCoreGameEnginePublicPersistentPilesRuntime) : IPublicPersistentPileProgramHost
{
    public SkillProgramStepOutcome ExecutePublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEnginePublicPersistentPilesRuntime().ExecutePublicPile(effect, frame);
}
