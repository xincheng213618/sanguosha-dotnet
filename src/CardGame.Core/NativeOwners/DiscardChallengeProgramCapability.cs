#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiscardChallengeProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineProgramDiscardChallengesRuntime>> getCoreGameEngineProgramDiscardChallengesRuntime) : IDiscardChallengeProgramHost
{
    public SkillProgramStepOutcome ExecuteDiscardChallenge(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineProgramDiscardChallengesRuntime().Value.ExecuteDiscardChallenge(effect, frame, targetSeat);
}
