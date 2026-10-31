#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal bool TryAdvanceMouLixianAiNullification()
    {
        // An after-damage Lixian trick retains the ancestral damage window.
        // Its exact native AI response must run before that suspended ancestor.
        if (!_coreNativePromptBoundaryOperations.IsAiNullificationPending() || _resolutionStackOperations._resolutionStack.Count < 3 || _resolutionStackOperations._resolutionStack[^1] is not NullificationWindowFrame counterspell || _resolutionStackOperations._resolutionStack[^2] is not CardUseFrame { MouLixianOrigin: { } origin } use || _resolutionStackOperations._resolutionStack[^3] is not ProgramSkillFrame { MouLixian: { Stage: MouLixianStage.NativeUse, Paid: true } receipt } parent || origin.ProgramFrameId != parent.Id || receipt.UseId != use.Id || receipt.ActionId != use.Action?.ActionId || !MouHuangYueyingFirstChild(parent, use) || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow?.Id != counterspell.Id || counterspell.ParentFrameId != use.Id || counterspell.SourceSeat != use.SourceSeat || counterspell.EffectCardId != use.CardId || counterspell.EffectCardKind != use.CardKind || counterspell.ActionKind != (use.CardKind switch
        {
            CardKind.DrawTwo => LegalActionKind.DrawTwo,
            CardKind.Snatch => LegalActionKind.Snatch,
            CardKind.Dismantlement => LegalActionKind.Dismantlement,
            CardKind.IronChain => LegalActionKind.IronChain,
            _ => (LegalActionKind? )null
        }) || !(counterspell.TargetSeats.SequenceEqual(use.TargetSeats) || counterspell.TargetSeats is [var target] && use.TargetIndex >= 0 && use.TargetIndex < use.TargetSeats.Count && target == use.TargetSeats[use.TargetIndex]))
            return false;
        _coreNativeAiDecisionsOperations.ResolvePendingAiNullification();
        return true;
    }

    internal bool MouLixianCardWindowEdge(int index)
    {
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame { CardKind: CardKind.IronChain } chained && IsIssuedMouLixianUse(chained) && _resolutionStackOperations._resolutionStack[index] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.CharacterEnteredChain, Continuation: ProgramLifecycleContinuation.ResumeCharacterStateChange, CharacterStateContinuation: CharacterStateContinuation.CardUse } state)
            // IronChain resolves its whole target set before draining these windows;
            // its target cursor does not advance once for each entered-chain child.
            return state.ResumeProgramFrameId == chained.Id && state.ResumeCardId == chained.CardId && state.ResumeCardKind == CardKind.IronChain && chained.TargetIndex >= 0 && chained.TargetIndex < chained.TargetSeats.Count && chained.TargetSeats.Skip(chained.TargetIndex).Contains(state.OwnerSeat) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Count(e => e.Change.Id == state.Id && e.Change.ParentFrameId == chained.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == SkillProgramTriggerWindow.CharacterEnteredChain && e.Change.TurnedOver is null) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<IronChainStateChangedEvent>().Any(e => e.ResolutionId == chained.Id && e.SourceSeat == chained.SourceSeat && e.TargetSeat == state.OwnerSeat && e.IsChained) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<IronChainResolvedEvent>().Any(e => e.ResolutionId == chained.Id && e.SourceSeat == chained.SourceSeat && e.TargetSeats.SequenceEqual(chained.TargetSeats));
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame original && IsIssuedMouLixianUse(original) && _resolutionStackOperations._resolutionStack[index] is TargetCardSelectionFrame selection)
            return selection.ParentFrameId == original.Id && selection.SourceSeat == original.SourceSeat && selection.EffectCardId == original.CardId && selection.EffectCardKind == original.CardKind && original.TargetIndex >= 0 && original.TargetIndex < original.TargetSeats.Count && selection.TargetSeat == original.TargetSeats[original.TargetIndex] && (original.CardKind == CardKind.Snatch && selection.ActionKind == LegalActionKind.Snatch || original.CardKind == CardKind.Dismantlement && selection.ActionKind == LegalActionKind.Dismantlement) && selection.Step == ResolutionFrameStep.AwaitingResponse && selection.CandidateSlots.Count > 0 && selection.CandidateSlots.SequenceEqual(Enumerable.Range(0, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[selection.TargetSeat]).Count)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TargetCardSelectionRequestedEvent>().Any(e => e.ResolutionId == original.Id && e.SourceSeat == original.SourceSeat && e.TargetSeat == selection.TargetSeat && e.ActionKind == selection.ActionKind && e.CandidateCount == selection.CandidateSlots.Count);
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame use && IsIssuedMouLixianUse(use) && _resolutionStackOperations._resolutionStack[index] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId && window.Action.ActorSeat == use.SourceSeat && window.Action.EffectiveKind == use.CardKind;
        if (_resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame parent || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { } native || !IsIssuedMouLixianUse(native) || parent.Action.ActionId != native.Action!.ActionId || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame child || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count)
            return false;
        var candidate = parent.Candidates[parent.CandidateIndex];
        return CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(child, CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate)) && child.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, candidate);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouHuangYueyingLixian)]
    internal PromptChoice ChooseMouLixian(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") != "cancel") ?? decision.Choices[0];
}
