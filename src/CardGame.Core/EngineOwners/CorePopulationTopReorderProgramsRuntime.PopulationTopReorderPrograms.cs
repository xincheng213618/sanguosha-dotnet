#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CorePopulationTopReorderProgramsRuntime
{
    SkillProgramStepOutcome IPopulationTopReorderProgramEffectHost.ReorderTopCardsByPopulation(ProgramSkillFrame frame, int maximumCards, ProgramPopulationThresholdCount population, string? allBottomStateId) => _coreProgramTopReorderOperations.BeginProgramTopReorder(frame, maximumCards, null, population: population, allBottomStateId: allBottomStateId);
    internal CardSnapshot[] GetPopulationTopPrivatelyViewedCards(int viewerSeat) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { TopReorder.Population: not null } frame && frame.OwnerSeat == viewerSeat && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: var seat } && seat == viewerSeat ? frame.TopReorder!.ViewedCardIds.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(card => card.Id == id))).ToArray() : [];
    internal void ValidatePopulationTopReorder(ProgramSkillFrame frame)
    {
        var draft = frame.TopReorder!;
        var owning = draft.Population!;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var selected = draft.TopCardIds.Concat(draft.BottomCardIds).ToArray();
        if (effect.Op != SkillProgramEffectOp.ReorderTopCards || effect.PopulationThresholdCount is not { } policy || effect.ExactTopCount is not null || effect.NumberExpression is not null || draft.RequiredTopCount is not null || owning.InstructionIndex != frame.InstructionIndex || owning.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || owning.TurnSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || owning.AllBottomStateId != effect.AllBottomStateId || owning.RequestedCount != effect.Amount && owning.RequestedCount != policy.BelowAmount || draft.ViewedCardIds.Count < 1 || draft.ViewedCardIds.Count > owning.RequestedCount || draft.ViewedCardIds.Distinct().Count() != draft.ViewedCardIds.Count || selected.Distinct().Count() != selected.Length || selected.Any(id => !draft.ViewedCardIds.Contains(id)) || !draft.ChoosingBottom && draft.BottomCardIds.Count != 0 || owning.AllBottomStateId is not null && frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)
            throw new InvalidOperationException("Population top ordering lost its owning instruction, actual turn or partition.");
    }
}
