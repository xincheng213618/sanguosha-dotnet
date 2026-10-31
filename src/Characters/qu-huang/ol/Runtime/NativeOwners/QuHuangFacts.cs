#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QuHuangFacts(EngineStates.CoreActualTurnProgressState actualTurnProgressState,EngineStates.CoreActualRoundProgressState actualRoundProgressState,EngineStates.CoreMatchOutcomeState matchOutcomeState,EngineStates.CoreCardMovementJournalState cardMovementJournalState,Func<RuntimeEntries.CoreResolutionIdentityBoundaryEntries> getCoreResolutionIdentityBoundaryRuntime,EngineStates.CoreGameEngineCardActionsState gameEngineCardActionsState,Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime,Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime,Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime,EngineStates.CoreMatchParticipantsState matchParticipantsState,Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime,Func<RuntimeEntries.CoreGameEngineEquipmentSkillsEntries> getCoreGameEngineEquipmentSkillsRuntime,Func<EquipmentContentRegistry> getEquipmentContentRegistry) : IQuHuangFacts
{
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public int TurnOwnerSeat => actualTurnProgressState.ReferenceToTurnProgression.OwnerSeat;
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public int MovementSequence => cardMovementJournalState.ReferenceToMovementSequence;
    public long ResolutionSequence => getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence;
    public long CardActionSequence => gameEngineCardActionsState.ReferenceToCardActionSequence;

    public Card Card(int id) => getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id);
    public SkillProgramTrigger Trigger(ProgramSkillFrame frame) => getCoreGameEngineProgramLifecycleRuntime().GetProgramTrigger(frame);
    public bool HasRuntimeSkill(int seat, string skill) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkill(matchParticipantsState.ReferenceToPlayers[seat], skill);
    public SkillProgramTriggerFacts MovementFacts(int owner, CardMovementSourceCount count, CardMovementTiming? timing) => getCoreGameEngineCardMovementProgramsRuntime().CaptureCardsMovedTriggerFacts(matchParticipantsState.ReferenceToPlayers[owner], 1, count, SkillProgramTriggerWindow.CardsMoved, timing);
    public bool IsActiveSourceEquipment(int owner, string skill, string instance, Card card) => getCoreGameEngineEquipmentSkillsRuntime().IsActiveProgramSourceEquipmentCard(owner, skill, instance, card);
    public bool IsEquipment(CardKind kind) => getEquipmentContentRegistry().IsEquipment(kind);
    public EquipmentSlot EquipmentSlot(CardKind kind) => getEquipmentContentRegistry().Get(kind).Slot;
}
