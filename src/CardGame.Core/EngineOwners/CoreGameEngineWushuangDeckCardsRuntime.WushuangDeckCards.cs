#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangDeckCardsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IWushuangDeckCardsHost.ObtainWushuangDeckCards(ProgramSkillFrame frame, SkillProgramEffect effect) => ObtainWushuangDeckCards(frame, effect);
    internal SkillProgramStepOutcome ObtainWushuangDeckCards(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || f.WushuangDeckGain is not null || effect.Amount is < 1 or > 4 || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
            throw new InvalidOperationException("A pile gain requires its active single-duel program instruction.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_corePhysicalDrawFacadesOperations.EnsureDrawPile())
            return SkillProgramStepOutcome.Continue;
        var cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Where(c => (effect.OutputKind is null || c.Kind == effect.OutputKind) && (effect.CardCategories.Count == 0 || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(c.Kind, effect.CardCategories))).Take(effect.Amount).Select(c => c.Id).ToArray();
        if (cards.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var receipt = new WushuangDeckGainReceipt(f.InstructionIndex, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, effect.Amount, effect.OutputKind, effect.CardCategories.Count == 0 ? null : effect.CardCategories[0], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            CardIds = cards
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WushuangDeckGain = receipt });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(f.OwnerSeat), new("wushuang.obtain-deck-cards"), (batch, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { WushuangDeckGain = receipt with { BatchId = batch, SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangDeckCardsObtainedEvent(f.Id, receipt.InstructionIndex, f.OwnerSeat, records.Count, batch, receipt.SequenceBefore, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        });
        var outcome = _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
        if (outcome == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { WushuangDeckGain = null });
        return outcome;
    }

    internal bool ValidWushuangDeckGain(ProgramSkillFrame f)
    {
        if (f.WushuangDeckGain is not { } r || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || r.BatchId <= 0 || r.SequenceAfter <= r.SequenceBefore || r.CardIds.Count is < 1 or > 4 || r.CardIds.Count > r.Requested || r.CardIds.Distinct().Count() != r.CardIds.Count)
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        return moves.Length == r.CardIds.Count && moves.Select(m => m.CardId).SequenceEqual(r.CardIds) && moves.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "wushuang.obtain-deck-cards" && (r.Kind is null || m.CardKind == r.Kind) && (r.Category is null || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(m.CardKind, [r.Category.Value]))) && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangDeckCardsObtainedEvent>().Count(e => e.ProgramFrameId == f.Id && e.InstructionIndex == r.InstructionIndex && e.BatchId == r.BatchId && e.Count == moves.Length && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 62000)]
    internal void AssertWushuangDeckGain(ProgramSkillFrame f, ProgramExecutionPlan plan)
    {
        if (f.WushuangDeckGain is not { } r)
            return;
        if (f.InstructionIndex < 1 || f.InstructionIndex > plan.Instructions.Count || plan.Instructions[f.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.ObtainWushuangDeckCards } effect || !_coreWushuangModeProjectionOperations.IsWushuangMode || effect.Amount != r.Requested || effect.OutputKind != r.Kind || (effect.CardCategories.Count == 0 ? null : (SkillProgramCardCategory? )effect.CardCategories[0]) != r.Category || r.CardIds is not System.Collections.IList { IsReadOnly: true } || !ValidWushuangDeckGain(f))
            throw new InvalidOperationException("The pile gain lost its instruction, immutable payment or physical movement.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 62000)]
    internal bool IsWushuangDeckGainMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.ObtainWushuangDeckCards && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidWushuangDeckGain(f);
    [ProgramContinuation(ProgramContinuationKind.Movement, 62000)]
    internal bool ReturnWushuangDeckGain(ProgramSkillFrame f)
    {
        if (f.WushuangDeckGain is null)
            return false;
        if (!ValidWushuangDeckGain(f))
            throw new InvalidOperationException("A pile gain lost its exact issued physical movement.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WushuangDeckGain = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}
