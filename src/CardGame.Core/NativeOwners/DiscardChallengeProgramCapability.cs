#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiscardChallengeProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardChallengesEntries> getCoreGameEngineProgramDiscardChallengesRuntime) : IDiscardChallengeProgramHost
{
    public SkillProgramStepOutcome ExecuteDiscardChallenge(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineProgramDiscardChallengesRuntime().ExecuteDiscardChallenge(effect, frame, targetSeat);
}
