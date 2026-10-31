#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

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
            return state.ResumeProgramFrameId == chained.Id && state.ResumeCardId == chained.CardId && state.ResumeCardKind == CardKind.IronChain && chained.TargetIndex >= 0 && chained.TargetIndex < chained.TargetSeats.Count && chained.TargetSeats.Skip(chained.TargetIndex).Contains(state.OwnerSeat) && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Count(e => e.Change.Id == state.Id && e.Change.ParentFrameId == chained.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == SkillProgramTriggerWindow.CharacterEnteredChain && e.Change.TurnedOver is null) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<IronChainStateChangedEvent>().Any(e => e.ResolutionId == chained.Id && e.SourceSeat == chained.SourceSeat && e.TargetSeat == state.OwnerSeat && e.IsChained) && _coreProgramEventHistoryOperations.ProgramEventHistory<IronChainResolvedEvent>().Any(e => e.ResolutionId == chained.Id && e.SourceSeat == chained.SourceSeat && e.TargetSeats.SequenceEqual(chained.TargetSeats));
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame original && IsIssuedMouLixianUse(original) && _resolutionStackOperations._resolutionStack[index] is TargetCardSelectionFrame selection)
            return selection.ParentFrameId == original.Id && selection.SourceSeat == original.SourceSeat && selection.EffectCardId == original.CardId && selection.EffectCardKind == original.CardKind && original.TargetIndex >= 0 && original.TargetIndex < original.TargetSeats.Count && selection.TargetSeat == original.TargetSeats[original.TargetIndex] && (original.CardKind == CardKind.Snatch && selection.ActionKind == LegalActionKind.Snatch || original.CardKind == CardKind.Dismantlement && selection.ActionKind == LegalActionKind.Dismantlement) && selection.Step == ResolutionFrameStep.AwaitingResponse && selection.CandidateSlots.Count > 0 && selection.CandidateSlots.SequenceEqual(Enumerable.Range(0, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[selection.TargetSeat]).Count)) && _coreProgramEventHistoryOperations.ProgramEventHistory<TargetCardSelectionRequestedEvent>().Any(e => e.ResolutionId == original.Id && e.SourceSeat == original.SourceSeat && e.TargetSeat == selection.TargetSeat && e.ActionKind == selection.ActionKind && e.CandidateCount == selection.CandidateSlots.Count);
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame use && IsIssuedMouLixianUse(use) && _resolutionStackOperations._resolutionStack[index] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId && window.Action.ActorSeat == use.SourceSeat && window.Action.EffectiveKind == use.CardKind;
        if (_resolutionStackOperations._resolutionStack[index - 1] is not ProgramCardTriggerWindowFrame parent || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { } native || !IsIssuedMouLixianUse(native) || parent.Action.ActionId != native.Action!.ActionId || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame child || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count)
            return false;
        var candidate = parent.Candidates[parent.CandidateIndex];
        return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(child, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(candidate)) && child.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(parent, candidate);
    }

    internal IReadOnlyList<Card> SeparateMouLixianDamageChildProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (processing.Count == 0 || MouHuangYueyingObserverRoot()is not { MouLixian: { Paid: true, Stage: MouLixianStage.NativeUse } r } parent || !ValidMouLixianStarted(parent) || parent.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.WindowId)is not DamageTriggerWindowFrame window || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || context.ParentFrameId != window.Id || !_coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(parent, window) || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window), attack) || _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window).ResolutionId != attack.ResolutionId || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action.PhysicalCards: [{ } cost] } use || use.MouLixianOrigin?.ProgramFrameId != parent.Id || !IsIssuedMouLixianUse(use) || cost.CardId != r.MaterialId || cost.CardKind != r.PrintedKind || cost.From != r.From || _cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.Processing || attack.PhysicalCards.Any(c => c.Id == cost.CardId) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianMaterialPaidEvent>().Count(p => p.FrameId == parent.Id && p.UseId == use.Id && p.CardId == cost.CardId && p.PrintedKind == cost.CardKind && p.From == cost.From && p.To == CardLocation.Processing && p.MovementSequence == r.MovementSequence) != 1)
            return processing;
        var wi = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        var pi = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id);
        var ui = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        var ai = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == attack.ResolutionId);
        if (ai < 0 || wi < 0 || pi != wi + 1 || ui != pi + 1 || ai >= wi)
            return processing;
        return processing.Where(c => c.Id != cost.CardId).ToArray();
    }

    internal ProgramSkillFrame? MouLixianDamageCursorRoot(DamageTriggerWindowFrame window, out long? ownedDyingId)
    {
        ownedDyingId = null;
        if (MouHuangYueyingObserverRoot()is not { MouLixian: { Paid: true } r } parent || !ValidMouLixianStarted(parent) || r.WindowId != window.Id || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || parent.WindowContext?.ParentFrameId != window.Id || !_coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(parent, window) || r.MovementSequence <= r.Before || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLixianMaterialPaidEvent>().Count(p => p.FrameId == parent.Id && p.UseId == r.UseId && p.CardId == r.MaterialId && p.PrintedKind == r.PrintedKind && p.From == r.From && p.MovementSequence == r.MovementSequence) != 1)
            return null;
        var wi = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        var pi = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id);
        if (wi < 0 || pi != wi + 1)
            return null;
        if (r.Stage == MouLixianStage.NativeUse)
        {
            if (_resolutionStackOperations._resolutionStack[pi + 1] is not CardUseFrame use || use.Id != r.UseId || use.MouLixianOrigin?.ProgramFrameId != parent.Id || !IsIssuedMouLixianUse(use))
                return null;
        }
        else if (!IsReturnedMouLixianUse(parent) || _resolutionStackOperations._resolutionStack[pi + 1] is not CardsMovedTriggerWindowFrame movement || movement.ResumeProgramFrameId != parent.Id || !IsMouLixianNativeSettlementBatch(parent, movement.Batch))
            return null;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is { } dying)
        {
            if (!IsMouHuangYueyingDying() || _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) <= pi)
                return null;
            ownedDyingId = dying.Id;
        }

        return parent;
    }

    internal IReadOnlyList<int>? MouLixianNativeAttackAncestorProcessing(long useId)
    {
        if (_coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { TriggerWindow: SkillProgramTriggerWindow.AfterDamageApplied } window || MouLixianDamageCursorRoot(window, out _)is not { MouLixian: { Stage: MouLixianStage.NativeUse } r } parent || r.UseId != useId || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { Action: { } childAction } use || !IsIssuedMouLixianUse(use))
            return null;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        var ai = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == attack.ResolutionId);
        var wi = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        if (ai < 0 || ai >= wi || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == window.ParentFrameId)is not DamageFrame damage || damage.ParentFrameId != attack.ResolutionId || damage.Id != parent.WindowContext?.DamageFrameId || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } cardAttack && !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(cardAttack, attack) || _coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel && !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(duel.Attack, attack) || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } group && group.ResolutionId != attack.ResolutionId || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is { } fire && fire.Id != attack.ResolutionId || attack.PhysicalCards.Select(c => c.Id).Distinct().Count() != attack.PhysicalCards.Count)
            return null;
        var originalUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == attack.ResolutionId);
        var ids = new List<int>();
        foreach (var card in attack.PhysicalCards)
        {
            if (_cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Processing)
                continue;
            var entry = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id);
            if (childAction.PhysicalCards.Any(c => c.CardId == card.Id) || entry is null || entry.To != CardLocation.Processing || entry.From == CardLocation.Processing || entry.CardKind != card.Kind || entry.Sequence > r.Before || originalUse is { Action: { } action } && !action.PhysicalCards.Any(c => c.CardId == card.Id && c.CardKind == card.Kind && c.From == entry.From))
                return null;
            ids.Add(card.Id);
        }

        return Array.AsReadOnly(ids.ToArray());
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouHuangYueyingLixian)]
    internal PromptChoice ChooseMouLixian(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") != "cancel") ?? decision.Choices[0];
}
