#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PublicPersistentPileProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEnginePublicPersistentPilesRuntime>> getCoreGameEnginePublicPersistentPilesRuntime) : IPublicPersistentPileProgramHost
{
    public SkillProgramStepOutcome ExecutePublicPile(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEnginePublicPersistentPilesRuntime().Value.ExecutePublicPile(effect, frame);
}
