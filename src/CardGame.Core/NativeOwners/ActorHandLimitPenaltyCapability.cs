#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActorHandLimitPenaltyCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineActorHandLimitPenaltiesRuntime>> getCoreGameEngineActorHandLimitPenaltiesRuntime) : IActorHandLimitPenaltyHost
{
    public void GrantActorHandLimitPenalty(ProgramSkillFrame frame, int amount) => getCoreGameEngineActorHandLimitPenaltiesRuntime().Value.GrantProgramActorHandLimitPenalty(frame, amount);
}
