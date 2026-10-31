#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuYuShengOlRuntime
{
    internal int LuYuShengChainCursor(GameEngine.IDamageAttempt attack) => attack.IsChainPropagation ? (attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex) : -1;
    internal LuYuShengDamageReceipt? LuYuShengDamageState(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        CardUseFrame f => f.LuYuShengDamage,
        ProgramSkillFrame f => f.LuYuShengDamage,
        JudgmentFrame f => f.LuYuShengDamage,
        _ => null
    };
    internal void SetLuYuShengDamageState(long id, LuYuShengDamageReceipt? r)
    {
        var f = _resolutionStackOperations._resolutionStack.Single(f => f.Id == id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f switch
        {
            CardUseFrame c => c with { LuYuShengDamage = r },
            ProgramSkillFrame p => p with { LuYuShengDamage = r },
            JudgmentFrame j => j with { LuYuShengDamage = r },
            _ => throw new InvalidOperationException("Cangxin requires its exact owning native damage producer.")});
    }

    internal void SetLuYuShengDamageAmount(GameEngine.IDamageAttempt attack, int amount)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || amount < 0)
            throw new InvalidOperationException("Cangxin cannot edit another native damage instance.");
        if (attack is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, s => s!with { DamageAmount = amount });
    }

    internal void ApplyLuYuShengReduction(ProgramSkillFrame f)
    {
        var r = f.LuYuSheng!;
        var w = _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single(p => p.Id == r.WindowId);
        var apply = _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[r.Payer].IsAlive;
        if (_coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.ResolutionId != r.AttackId || attack.TargetSeat != r.Payer || attack.DamageAmount != w.Amount || w.TargetSeat != r.Payer || w.Prevented || w.RedirectedTargetSeat is not null || LuYuShengChainCursor(attack) != r.ChainCursor || attack.IsChainPropagation != r.ChainPropagation || (r.SourceSeat is { } source ? attack.IsSourceLess || attack.SourceSeat != source : !attack.IsSourceLess))
            throw new InvalidOperationException("Cangxin lost its original native before-damage instance after its paid bottom discard.");
        var actualHearts = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && r.Selected.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.DiscardPile && m.Reason.Value == LuYuShengBottomDiscardReason && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Suit == Suit.Heart);
        if (actualHearts != r.Count)
            throw new InvalidOperationException("Cangxin must count only genuinely discarded original Heart entities.");
        var after = apply ? Math.Max(0, w.Amount - actualHearts) : w.Amount;
        var inherited = LuYuShengDamageState(attack.ResolutionId)is { } old && old.ChainCursor == r.ChainCursor && (old.TransferredTargetSeat ?? old.TargetSeat) == r.Payer ? old.Before : w.Amount;
        var receipt = new LuYuShengDamageReceipt(w.Id, attack.ResolutionId, r.Payer, r.ChainCursor, inherited, after);
        if (apply)
        {
            SetLuYuShengDamageAmount(attack, after);
            _coreRuntimeOperations.ReplaceRuntimeFrame(w.Id, w with { Amount = after, Prevented = after == 0 });
            SetLuYuShengDamageState(attack.ResolutionId, receipt);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengDamageAdjustedEvent(f.Id, receipt, actualHearts, apply));
    }

    internal void PrepareLuYuShengChainAdvance(GameEngine.IDamageAttempt attack)
    {
        if (LuYuShengDamageState(attack.ResolutionId)is not { } r)
            return;
        var remaining = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats;
        var cursor = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex;
        if (remaining is null || !remaining.Skip(cursor).Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive))
            return;
        if (attack.IsChainPropagation && r.ChainCursor == LuYuShengChainCursor(attack) && (r.TransferredTargetSeat ?? r.TargetSeat) == attack.TargetSeat)
        {
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<LuYuShengDamageAdjustedEvent>().Any(e => e.Applied && e.Receipt.WindowId == r.WindowId && e.Receipt.AttackId == r.AttackId && e.Receipt.ChainCursor == r.ChainCursor && e.Receipt.Before == r.Before && e.Receipt.After == r.After))
                throw new InvalidOperationException("Cangxin chain restoration lost its exact paid native reduction.");
            SetLuYuShengDamageAmount(attack, r.Before);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengDamageRebasedEvent(r.WindowId, r.AttackId, attack.TargetSeat, r.ChainCursor, r.Before));
        }

        SetLuYuShengDamageState(attack.ResolutionId, null);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26901)]
    internal void ObserveLuYuShengDamageTransfer(IGameEvent payload)
    {
        if (payload is not ProgramDamageTransferredEvent transfer || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || LuYuShengDamageState(attack.ResolutionId)is not { TransferredTargetSeat: null } r || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^1] is not ProgramSkillFrame p || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame w || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(p)?.Op != SkillProgramEffectOp.RedirectCurrentDamage || p.WindowContext?.ParentFrameId != w.Id || (w.ContinuationAttackResolutionId ?? w.ParentFrameId) != attack.ResolutionId || w.TargetSeat != r.TargetSeat || transfer.ResolutionId != attack.ResolutionId || transfer.SourceSeat != attack.SourceSeat || transfer.TargetSeat != attack.TargetSeat || w.RedirectedTargetSeat != attack.TargetSeat || transfer.DamageAmount != w.Amount || transfer.OwnerSeat != p.OwnerSeat || transfer.SkillId != p.SkillId || !attack.DamageRedirected || r.ChainCursor != LuYuShengChainCursor(attack))
            return;
        SetLuYuShengDamageState(attack.ResolutionId, r with { TransferredTargetSeat = attack.TargetSeat, TransferWindowId = w.Id });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LuYuShengDamageTransferredEvent(r.WindowId, r.AttackId, attack.TargetSeat, r.TargetSeat, r.ChainCursor, w.Id));
    }
}
