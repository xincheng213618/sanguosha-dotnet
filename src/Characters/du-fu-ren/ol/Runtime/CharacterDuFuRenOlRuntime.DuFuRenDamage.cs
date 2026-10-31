#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    internal DuFuRenDamageReceipt? DuFuRenDamageState(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        CardUseFrame f => f.DuFuRenDamage,
        ProgramSkillFrame f => f.DuFuRenDamage,
        JudgmentFrame f => f.DuFuRenDamage,
        _ => null
    };
    internal void SetDuFuRenDamageState(long id, DuFuRenDamageReceipt? receipt)
    {
        var frame = _resolutionStackOperations._resolutionStack.Single(f => f.Id == id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, frame switch
        {
            CardUseFrame f => f with { DuFuRenDamage = receipt },
            ProgramSkillFrame f => f with { DuFuRenDamage = receipt },
            JudgmentFrame f => f with { DuFuRenDamage = receipt },
            _ => throw new InvalidOperationException("Yise requires its owning native card attack.")});
    }

    internal int DuFuRenDamageCursor(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack.IsChainPropagation ? _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex : -1;
    internal IReadOnlyList<DuFuRenBlackIssuedEvent> AvailableDuFuRenBlack(int target) => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenBlackIssuedEvent>().Where(g => g.RecipientSeat == target && !_coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenBlackConsumedEvent>().Any(c => c.GrantId == g.FrameId) && !_resolutionStackOperations._resolutionStack.Any(f => (f switch
    {
        CardUseFrame u => u.DuFuRenDamage,
        ProgramSkillFrame p => p.DuFuRenDamage,
        JudgmentFrame j => j.DuFuRenDamage,
        _ => null
    })is { } r && r.GrantIds.Contains(g.FrameId))).ToArray());
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 120)]
    internal int DuFuRenYiseDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!HasDuFuRenYise || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return 0;
        var cursor = DuFuRenDamageCursor(attack);
        if (DuFuRenDamageState(attack.ResolutionId)is { } previous && previous.TargetSeat == attack.TargetSeat && previous.ChainCursor == cursor)
        {
            if (!previous.GrantIds.Any(id => _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenBlackConsumedEvent>().Any(e => e.GrantId == id)))
                return previous.GrantIds.Count;
            SetDuFuRenDamageState(attack.ResolutionId, null);
        }

        var grants = AvailableDuFuRenBlack(attack.TargetSeat);
        if (grants.Count == 0)
            return 0;
        var r = new DuFuRenDamageReceipt(attack.ResolutionId, cursor, attack.TargetSeat, attack.DamageAmount, grants.Select(g => g.FrameId).ToArray());
        SetDuFuRenDamageState(attack.ResolutionId, r);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenDamageIssuedEvent(attack.ResolutionId, attack.TargetSeat, cursor, r.InheritedAmount, grants.Count));
        return grants.Count;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26522)]
    internal void ConsumeDuFuRenActualDamage(IGameEvent payload)
    {
        if (HasDuFuRenYise && payload is ExtraCardEffectStartedEvent extra && _coreCardUseLifecycleOperations.LifecycleCardUse(extra.CardUseFrameId)is { Action: { } action, DuFuRenDamage: not null } restarted && _coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(restarted.Id) && action.ActionId == extra.ActionId && restarted.Id == extra.CardUseFrameId && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == restarted.Id)
            SetDuFuRenDamageState(restarted.Id, null);
        if (HasDuFuRenYise && payload is ProgramDamageTransferredEvent transfer && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } moved && DuFuRenDamageState(moved.ResolutionId)is { TransferredTargetSeat: null } original && _resolutionStackOperations._resolutionStack.Count >= 2 && _resolutionStackOperations._resolutionStack[^1] is ProgramSkillFrame program && _resolutionStackOperations._resolutionStack[^2] is BeforeDamageProgramWindowFrame window && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(program)is { Op: SkillProgramEffectOp.RedirectCurrentDamage } && program.WindowContext?.ParentFrameId == window.Id && transfer.ResolutionId == moved.ResolutionId && original.AttackId == moved.ResolutionId && (window.ContinuationAttackResolutionId ?? window.ParentFrameId) == moved.ResolutionId && original.TargetSeat == window.TargetSeat && original.ChainCursor == DuFuRenDamageCursor(moved) && window.RedirectedTargetSeat == moved.TargetSeat && transfer.TargetSeat == moved.TargetSeat && transfer.SourceSeat == moved.SourceSeat && transfer.DamageAmount == window.Amount && transfer.SkillId == program.SkillId && transfer.OwnerSeat == program.OwnerSeat && moved.DamageRedirected)
        {
            SetDuFuRenDamageState(moved.ResolutionId, original with { TransferredTargetSeat = moved.TargetSeat, TransferWindowId = window.Id });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenDamageTransferredEvent(moved.ResolutionId, original.TargetSeat, moved.TargetSeat, original.ChainCursor, window.Id, transfer.DamageAmount));
        }

        if (!HasDuFuRenYise || payload is not DamageAppliedEvent { Amount: > 0 } e || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || damage.ParentFrameId != attack.ResolutionId || damage.TargetSeat != e.TargetSeat || damage.Amount != e.Amount || attack.TargetSeat != e.TargetSeat || DuFuRenDamageState(attack.ResolutionId)is not { } r || r.AttackId != attack.ResolutionId || (r.TransferredTargetSeat ?? r.TargetSeat) != e.TargetSeat || r.ChainCursor != DuFuRenDamageCursor(attack))
            return;
        if (r.TransferredTargetSeat is { } recipient && (r.TransferWindowId is not { } windowId || !attack.DamageRedirected || _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenDamageTransferredEvent>().Count(x => x.AttackId == attack.ResolutionId && x.OriginalTargetSeat == r.TargetSeat && x.TargetSeat == recipient && x.ChainCursor == r.ChainCursor && x.WindowId == windowId && x.Amount == e.Amount) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramDamageTransferredEvent>().Any(x => x.ResolutionId == attack.ResolutionId && x.TargetSeat == recipient && x.SourceSeat == attack.SourceSeat && x.DamageAmount == e.Amount)))
            throw new InvalidOperationException("Yise's transferred finalized amount lost its exact native original recipient invoice.");
        foreach (var grant in r.GrantIds)
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenBlackConsumedEvent>().Any(c => c.GrantId == grant))
                throw new InvalidOperationException("One Yise mark cannot fund two actual damage instances.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenBlackConsumedEvent(grant, attack.ResolutionId, damage.Id, e.TargetSeat, r.ChainCursor));
        }
    }

    internal void SetDuFuRenChainDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !attack.IsChainPropagation || amount < 0)
            throw new InvalidOperationException("Yise can only rebase its exact native chained recipient.");
        _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, s => s!with { DamageAmount = amount });
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 40)]
    internal void PrepareDuFuRenChainAdvance(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!HasDuFuRenYise || DuFuRenDamageState(attack.ResolutionId)is not { } r)
            return;
        var state = _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId);
        var cursor = state.ChainedTargetIndex;
        if (state.ChainedTargetSeats is null || !state.ChainedTargetSeats.Skip(cursor).Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive))
            return;
        // The first recipient's finalized damage remains the native propagation
        // base. A bonus added to a later chained recipient is local to it.
        if (attack.IsChainPropagation && r.ChainCursor == DuFuRenDamageCursor(attack) && (r.TransferredTargetSeat ?? r.TargetSeat) == attack.TargetSeat)
        {
            SetDuFuRenChainDamage(attack, r.InheritedAmount);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuFuRenDamageRebasedEvent(attack.ResolutionId, r.TargetSeat, r.ChainCursor, r.InheritedAmount));
        }

        SetDuFuRenDamageState(attack.ResolutionId, null);
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 20)]
    internal void ApplyDuFuRenChainedDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!HasDuFuRenYise || !attack.IsChainPropagation || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return;
        // Formal armor will run the ordinary finalizer for this recipient.
        // Legacy propagation is already finalized and needs just its new mark.
        if (attack.DamageAmountFinalized)
            SetDuFuRenChainDamage(attack, checked(attack.DamageAmount + DuFuRenYiseDamageBonus(attack)));
    }
}
