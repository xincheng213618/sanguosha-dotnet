#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal bool HasWuXiongmuFirstDamageCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuLuXunXiongmuFirstDamage);

    internal int WuXiongmuChainCursor(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation ? -1 : attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex;
    internal XiongmuDamageReceipt? WuXiongmuDamageState(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        CardUseFrame c => c.XiongmuDamage,
        ProgramSkillFrame p => p.XiongmuDamage,
        JudgmentFrame j => j.XiongmuDamage,
        _ => null
    };
    internal void SetWuXiongmuDamageState(long id, XiongmuDamageReceipt? r)
    {
        var f = _resolutionStackOperations._resolutionStack.Single(f => f.Id == id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f switch
        {
            CardUseFrame c => c with { XiongmuDamage = r },
            ProgramSkillFrame p => p with { XiongmuDamage = r },
            JudgmentFrame j => j with { XiongmuDamage = r },
            _ => throw new InvalidOperationException("Xiongmu needs the actual native damage owning frame.")});
    }

    internal bool WuXiongmuHasReceivedDamage(int owner) => _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Any(e => e.Amount > 0 && e.TargetSeat == owner) || _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuFirstDamageReducedEvent>().Any(e => e.Receipt.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.Receipt.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Receipt.TargetSeat == owner);
    internal bool WuXiongmuFirstDamageEligible(int owner, long windowId) => _coreMatchSetupSetupCompleteAccess._setupComplete && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].HasHpValue && !WuXiongmuHasReceivedDamage(owner) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count <= _coreMatchParticipantsPlayersAccess._players[owner].Hp && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == windowId)is BeforeDamageProgramWindowFrame { Prevented: false, RedirectedTargetSeat: null, Amount: > 0 } w && w.TargetSeat == owner && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { DamageAmountFinalized: true } attack && attack.TargetSeat == owner && (w.ContinuationAttackResolutionId ?? w.ParentFrameId) == attack.ResolutionId && attack.DamageAmount == w.Amount;
    // This memo is global while the capability is present, independent of who
    // currently owns the skill. The ordinary native DamageApplied history is
    // also consulted, so receiving the skill later cannot erase an earlier hit.
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 29001)]
    internal void ObserveWuXiongmuNativeDamage(IGameEvent payload)
    {
        if (payload is ProgramDamageTransferredEvent transferred)
        {
            TransferWuXiongmuDamage(transferred);
            return;
        }

        if (!HasWuXiongmuFirstDamageCapability || !_coreMatchSetupSetupCompleteAccess._setupComplete || payload is not DamageAppliedEvent { Amount: > 0 } fact || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame native || native.ParentFrameId != attack.ResolutionId || native.TargetSeat != fact.TargetSeat || native.Amount != fact.Amount || attack.TargetSeat != fact.TargetSeat || attack.SourceSeat != fact.SourceSeat || attack.IsSourceLess != fact.SourceLess || _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuFirstPositiveDamageEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.TargetSeat == fact.TargetSeat))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuFirstPositiveDamageEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, attack.ResolutionId, fact.SourceSeat, fact.TargetSeat, fact.Amount, fact.Nature, fact.SourceLess));
    }

    internal SkillProgramStepOutcome ResolveWuXiongmuFirstDamage(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != WuXiongmuSkill || f.TriggerId != "first-damage" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !WuXiongmuNativeCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context) || !WuXiongmuFirstDamageEligible(f.OwnerSeat, context.ParentFrameId) || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack)
            throw new InvalidOperationException("Xiongmu cannot reduce another native damage, repeat its consumed first hit or invent a late-granted first hit.");
        var receipt = new XiongmuDamageReceipt(f.Id, WuXiongmuSource(f), f.GameplayHash, window.Id, attack.ResolutionId, f.OwnerSeat, WuXiongmuChainCursor(attack), window.Amount, Math.Max(0, window.Amount - 1), _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat);
        var count = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
        var hp = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp;
        _coreProgramDamageReductionOperations.ReduceProgramCurrentDamage(f, 1);
        SetWuXiongmuDamageState(attack.ResolutionId, receipt);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuFirstDamageReducedEvent(receipt, count, hp));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool ValidWuXiongmuDamage(XiongmuDamageReceipt r) => r.Source.SkillId == WuXiongmuSkill && r.Source.BindingId == "first-damage" && r.Source.OwnerSeat == r.TargetSeat && r.AmountBefore > 0 && r.AmountAfter == Math.Max(0, r.AmountBefore - 1) && r.AttackId > 0 && r.WindowId > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TurnOwner) && r.ChainCursor >= -1 && _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash == r.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuFirstDamageReducedEvent>().Count(e => e.Receipt.ProgramFrameId == r.ProgramFrameId && e.Receipt == (r with { TransferredTargetSeat = null, TransferWindowId = null }) && e.HandCount <= e.Hp) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<XiongmuFirstDamageReducedEvent>().Count(e => e.Receipt.ActualTurn == r.ActualTurn && e.Receipt.TurnOwner == r.TurnOwner && e.Receipt.TargetSeat == r.TargetSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramDamageReducedEvent>().Count(e => e.FrameId == r.ProgramFrameId && e.SkillId == WuXiongmuSkill && e.OwnerSeat == r.TargetSeat && e.TargetSeat == r.TargetSeat && e.AmountBefore == r.AmountBefore && e.AmountAfter == r.AmountAfter) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == r.ProgramFrameId && e.SkillId == r.Source.SkillId && e.BindingId == r.Source.BindingId && e.SkillInstanceId == r.Source.SkillInstanceId && e.OwnerSeat == r.TargetSeat && e.Window == SkillProgramTriggerWindow.BeforeDamageApplied) == 1;
    internal void SetWuXiongmuChainAmount(GameEngine.IDamageAttempt attack, int amount)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || amount < 0)
            throw new InvalidOperationException("Xiongmu cannot rebase a foreign native attack.");
        if (attack is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, s => s!with { DamageAmount = amount });
    }

    internal void PrepareWuXiongmuChainAdvance(GameEngine.IDamageAttempt attack)
    {
        if (WuXiongmuDamageState(attack.ResolutionId)is not { } r)
            return;
        if (!ValidWuXiongmuDamage(r))
            throw new InvalidOperationException("Xiongmu lost its exact first-damage invoice before native chain advance.");
        var remaining = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats;
        var cursor = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex;
        if (remaining is null || !remaining.Skip(cursor).Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive))
        {
            ValidateWuXiongmuCurrentAttempt(attack, r);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuDamageRetiredEvent(r.AttackId, r.WindowId, r.ProgramFrameId, r.TargetSeat, r.ChainCursor));
            SetWuXiongmuDamageState(attack.ResolutionId, null);
            return;
        }

        // Initial damage establishes the native chain base. A reduction on a
        // later recipient is local and must not leak into the next recipient.
        if (attack.IsChainPropagation && r.ChainCursor == WuXiongmuChainCursor(attack) && (r.TransferredTargetSeat ?? r.TargetSeat) == attack.TargetSeat)
        {
            SetWuXiongmuChainAmount(attack, r.AmountBefore);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuChainRebasedEvent(r.AttackId, r.WindowId, attack.TargetSeat, r.ChainCursor, r.AmountBefore));
        }

        SetWuXiongmuDamageState(attack.ResolutionId, null);
    }

    internal bool ValidWuXiongmuCangxinReduction(LuYuShengDamageReceipt receipt, out LuYuShengDamageAdjustedEvent adjusted)
    {
        adjusted = null !;
        var original = receipt with
        {
            TransferredTargetSeat = null,
            TransferWindowId = null
        };
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<LuYuShengDamageAdjustedEvent>().Where(e => e.Applied && e.Receipt == original).ToArray()is not [var fact] || fact.Hearts < 0 || fact.Hearts > 3 || original.Before <= 0 || original.After != Math.Max(0, original.Before - fact.Hearts) || _coreProgramEventHistoryOperations.ProgramEventHistory<LuYuShengStartedEvent>().Where(e => e.FrameId == fact.FrameId).ToArray()is not [var start] || start.Operation != SkillProgramEffectOp.DiscardBottomHeartsForDamageReduction || start.WindowId != original.WindowId || start.AttackId != original.AttackId || start.Payer != original.TargetSeat || start.Source.OwnerSeat != original.TargetSeat || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(start.Source.SkillId)?.Program?.GameplayHash != start.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.FrameId && e.SkillId == start.Source.SkillId && e.SkillInstanceId == start.Source.SkillInstanceId && e.BindingId == start.Source.BindingId && e.OwnerSeat == original.TargetSeat && e.Window == SkillProgramTriggerWindow.BeforeDamageApplied) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<LuYuShengMovementIssuedEvent>().Where(e => e.FrameId == fact.FrameId && e.Stage == LuYuShengStage.BottomDiscardChildren).ToArray()is not [var paid] || paid.Payer != original.TargetSeat || paid.Before < 0 || paid.After < paid.Before || paid.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.From == CardLocation.DrawPile && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.lu-yu-sheng.cangxin.discard") != paid.Count || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.From == CardLocation.DrawPile && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.lu-yu-sheng.cangxin.discard" && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Suit == Suit.Heart) != fact.Hearts)
            return false;
        if (receipt.TransferredTargetSeat is { } recipient && (receipt.TransferWindowId is not { } window || _coreProgramEventHistoryOperations.ProgramEventHistory<LuYuShengDamageTransferredEvent>().Count(e => e.WindowId == receipt.WindowId && e.AttackId == receipt.AttackId && e.TargetSeat == recipient && e.OriginalTargetSeat == receipt.TargetSeat && e.ChainCursor == receipt.ChainCursor && e.TransferWindowId == window) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramDamageTransferredEvent>().Any(e => e.ResolutionId == receipt.AttackId && e.TargetSeat == recipient)))
            return false;
        adjusted = fact;
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 10)]
    internal void PrepareWuXiongmuCangxinChainAdvance(GameEngine.IDamageAttempt attack)
    {
        var xiongmu = WuXiongmuDamageState(attack.ResolutionId);
        var cangxin = _characterLuYuShengOlOperations.LuYuShengDamageState(attack.ResolutionId);
        if (xiongmu is null || cangxin is null)
        {
            _characterLuYuShengOlOperations.PrepareLuYuShengChainAdvance(attack);
            PrepareWuXiongmuChainAdvance(attack);
            return;
        }

        var cursor = WuXiongmuChainCursor(attack);
        var remaining = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats;
        var nextIndex = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex;
        if (!attack.IsChainPropagation || xiongmu.AttackId != attack.ResolutionId || cangxin.AttackId != attack.ResolutionId || xiongmu.ChainCursor != cursor || cangxin.ChainCursor != cursor || (xiongmu.TransferredTargetSeat ?? xiongmu.TargetSeat) != attack.TargetSeat || (cangxin.TransferredTargetSeat ?? cangxin.TargetSeat) != attack.TargetSeat || remaining is null || !remaining.Skip(nextIndex).Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive))
        {
            _characterLuYuShengOlOperations.PrepareLuYuShengChainAdvance(attack);
            PrepareWuXiongmuChainAdvance(attack);
            return;
        }

        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !ValidWuXiongmuDamage(xiongmu) || !ValidWuXiongmuCangxinReduction(cangxin, out var cangxinFact))
            throw new InvalidOperationException("The combined first-damage rebase lost one of its exact paid native recipient invoices.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var xIndex = Array.FindIndex(history, e => e is XiongmuFirstDamageReducedEvent fact && fact.Receipt.ProgramFrameId == xiongmu.ProgramFrameId && fact.Receipt == (xiongmu with { TransferredTargetSeat = null, TransferWindowId = null }));
        var cIndex = Array.FindIndex(history, e => e is LuYuShengDamageAdjustedEvent fact && fact == cangxinFact);
        // Each before-damage effect retains its own actual before/after invoice.
        // Whichever ran first observed this recipient's original local base.
        // Restore that base once, instead of letting two setters overwrite it
        // in an unrelated fixed skill order. Other native rebase producers are
        // still dispatched by their existing chain-advance hooks.
        var firstXiongmu = xIndex < cIndex;
        var originalAmount = firstXiongmu ? xiongmu.AmountBefore : cangxin.Before;
        SetWuXiongmuChainAmount(attack, originalAmount);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuCangxinChainRebasedEvent(attack.ResolutionId, attack.TargetSeat, cursor, xiongmu.ProgramFrameId, xiongmu.WindowId, cangxinFact.FrameId, cangxin.WindowId, firstXiongmu, originalAmount));
        _characterLuYuShengOlOperations.SetLuYuShengDamageState(attack.ResolutionId, null);
        SetWuXiongmuDamageState(attack.ResolutionId, null);
    }

    internal void TransferWuXiongmuDamage(ProgramDamageTransferredEvent transfer)
    {
        if (_coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || WuXiongmuDamageState(attack.ResolutionId)is not { TransferredTargetSeat: null } r || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^1] is not ProgramSkillFrame f || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame w || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.RedirectCurrentDamage || f.WindowContext?.ParentFrameId != w.Id || w.Id != r.WindowId || (w.ContinuationAttackResolutionId ?? w.ParentFrameId) != attack.ResolutionId || w.TargetSeat != r.TargetSeat || transfer.ResolutionId != attack.ResolutionId || transfer.SourceSeat != attack.SourceSeat || transfer.TargetSeat != attack.TargetSeat || w.RedirectedTargetSeat != attack.TargetSeat || transfer.DamageAmount != w.Amount || transfer.OwnerSeat != f.OwnerSeat || transfer.SkillId != f.SkillId || !attack.DamageRedirected || r.ChainCursor != WuXiongmuChainCursor(attack))
            return;
        if (!ValidWuXiongmuDamage(r))
            throw new InvalidOperationException("Xiongmu's transferred damage lost its original consumed first-hit reduction.");
        SetWuXiongmuDamageState(attack.ResolutionId, r with { TransferredTargetSeat = attack.TargetSeat, TransferWindowId = w.Id });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuDamageTransferredEvent(r.AttackId, r.WindowId, r.TargetSeat, attack.TargetSeat, r.ChainCursor, w.Id));
    }

    internal void ValidateWuXiongmuCurrentAttempt(GameEngine.IDamageAttempt attack, XiongmuDamageReceipt receipt)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || receipt.AttackId != attack.ResolutionId || (receipt.TransferredTargetSeat ?? receipt.TargetSeat) != attack.TargetSeat || receipt.ChainCursor != WuXiongmuChainCursor(attack) || !ValidWuXiongmuDamage(receipt))
            throw new InvalidOperationException("Xiongmu cannot retire a different native damage attempt's reduction invoice.");
    }

    internal void ValidateWuXiongmuCangxinCurrentAttempt(GameEngine.IDamageAttempt attack, LuYuShengDamageReceipt receipt, out LuYuShengDamageAdjustedEvent adjusted)
    {
        adjusted = null !;
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || receipt.AttackId != attack.ResolutionId || (receipt.TransferredTargetSeat ?? receipt.TargetSeat) != attack.TargetSeat || receipt.ChainCursor != WuXiongmuChainCursor(attack) || !ValidWuXiongmuCangxinReduction(receipt, out adjusted))
            throw new InvalidOperationException("The composed native damage return lost Cangxin's exact completed attempt.");
    }

    internal ProgramSkillFrame CaptureWuXiongmuProgramDamageCompleted(ProgramSkillFrame frame, GameEngine.IDamageAttempt attack)
    {
        if (frame.XiongmuDamage is null && (!HasWuXiongmuFirstDamageCapability || frame.LuYuShengDamage is null))
            return frame;
        if (attack is not NativeOwners.ProgramAttackHandle || frame.Id != attack.ResolutionId || frame.AttackAttempt is null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || !CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt))
            throw new InvalidOperationException("Xiongmu's native program damage return lost its actual completed producer.");
        if (frame.XiongmuDamage is { } xiongmu)
        {
            ValidateWuXiongmuCurrentAttempt(attack, xiongmu);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuDamageRetiredEvent(xiongmu.AttackId, xiongmu.WindowId, xiongmu.ProgramFrameId, xiongmu.TargetSeat, xiongmu.ChainCursor));
        }

        if (HasWuXiongmuFirstDamageCapability && frame.LuYuShengDamage is { } cangxin)
        {
            ValidateWuXiongmuCangxinCurrentAttempt(attack, cangxin, out var fact);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuCangxinDamageRetiredEvent(cangxin.AttackId, cangxin.WindowId, fact.FrameId, cangxin.TargetSeat, cangxin.ChainCursor));
            frame = frame with
            {
                LuYuShengDamage = null
            };
        }

        return frame with
        {
            XiongmuDamage = null
        };
    }

    internal void RetireWuXiongmuCardDamageCompleted(GameEngine.IDamageAttempt attack)
    {
        var xiongmu = WuXiongmuDamageState(attack.ResolutionId);
        var cangxin = HasWuXiongmuFirstDamageCapability ? _characterLuYuShengOlOperations.LuYuShengDamageState(attack.ResolutionId) : null;
        if (xiongmu is null && cangxin is null)
            return;
        if (attack is not NativeOwners.CardAttackHandle || !CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt))
            throw new InvalidOperationException("Xiongmu cannot clear another native card attack's completed damage state.");
        if (xiongmu is not null)
        {
            ValidateWuXiongmuCurrentAttempt(attack, xiongmu);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuDamageRetiredEvent(xiongmu.AttackId, xiongmu.WindowId, xiongmu.ProgramFrameId, xiongmu.TargetSeat, xiongmu.ChainCursor));
            SetWuXiongmuDamageState(attack.ResolutionId, null);
        }

        if (cangxin is not null)
        {
            ValidateWuXiongmuCangxinCurrentAttempt(attack, cangxin, out var fact);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiongmuCangxinDamageRetiredEvent(cangxin.AttackId, cangxin.WindowId, fact.FrameId, cangxin.TargetSeat, cangxin.ChainCursor));
            _characterLuYuShengOlOperations.SetLuYuShengDamageState(attack.ResolutionId, null);
        }
    }
}
