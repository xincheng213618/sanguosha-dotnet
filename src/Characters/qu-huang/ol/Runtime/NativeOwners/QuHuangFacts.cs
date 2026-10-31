#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QuHuangFacts(global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState,global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState,global::CardGame.Core.EngineStates.CoreMatchOutcomeState matchOutcomeState,global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries> getCoreResolutionIdentityBoundaryRuntime,global::CardGame.Core.EngineStates.CoreGameEngineCardActionsState gameEngineCardActionsState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime,global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime,global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSkillsEntries> getCoreGameEngineEquipmentSkillsRuntime,global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry) : IQuHuangFacts
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
