#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HandComparisonProgramCapability(Func<RuntimeEntries.CoreGameEngineHandComparisonEntries> getCoreGameEngineHandComparisonRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IHandComparisonProgramHost
{
    public SkillProgramStepOutcome CompareSelectedHandWithHpHand(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineHandComparisonRuntime().CompareSelectedHandWithHpHand(f, e);
    public void AdjustPersistentHandLimit(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => getCoreGameEngineHandComparisonRuntime().AdjustPersistentHandLimit(f, targetSeat, amount, stateId);
    public void ProhibitSelfCardTargetsForTurn(ProgramSkillFrame f)
    {
        getCoreGameEngineCardUseModulesRuntime().ValidateProgramTurnEffectGrant(f);
        advanceEventRulesAndQueueFact(new ProgramSelfCardTargetsProhibitedEvent(f.OwnerSeat, f.SkillId, f.SkillInstanceId, actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToCurrentSeat));
    }
}
