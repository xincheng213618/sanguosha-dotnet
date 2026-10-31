#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ActorHandLimitPenaltyCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActorHandLimitPenaltiesEntries> getCoreGameEngineActorHandLimitPenaltiesRuntime) : IActorHandLimitPenaltyHost
{
    public void GrantActorHandLimitPenalty(ProgramSkillFrame frame, int amount) => getCoreGameEngineActorHandLimitPenaltiesRuntime().GrantProgramActorHandLimitPenalty(frame, amount);
}
