#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HandComparisonProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries> getCoreGameEngineHandComparisonRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState) : IHandComparisonProgramHost
{
    public SkillProgramStepOutcome CompareSelectedHandWithHpHand(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineHandComparisonRuntime().CompareSelectedHandWithHpHand(f, e);
    public void AdjustPersistentHandLimit(ProgramSkillFrame f, int targetSeat, int amount, string stateId) => getCoreGameEngineHandComparisonRuntime().AdjustPersistentHandLimit(f, targetSeat, amount, stateId);
    public void ProhibitSelfCardTargetsForTurn(ProgramSkillFrame f)
    {
        getCoreGameEngineCardUseModulesRuntime().ValidateProgramTurnEffectGrant(f);
        advanceEventRulesAndQueueFact(new ProgramSelfCardTargetsProhibitedEvent(f.OwnerSeat, f.SkillId, f.SkillInstanceId, actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToCurrentSeat));
    }
}
