#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActorHandLimitPenaltyCapability(Func<RuntimeEntries.CoreGameEngineActorHandLimitPenaltiesEntries> getCoreGameEngineActorHandLimitPenaltiesRuntime) : IActorHandLimitPenaltyHost
{
    public void GrantActorHandLimitPenalty(ProgramSkillFrame frame, int amount) => getCoreGameEngineActorHandLimitPenaltiesRuntime().GrantProgramActorHandLimitPenalty(frame, amount);
}
