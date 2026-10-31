#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaXiongMouOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33301)]
    internal void ObserveMouHxYangwei(IGameEvent fact)
    {
        int owner, turn, total;
        long invocation;
        bool outgoing;
        if (fact is NativeDrawInvocationRecordedEvent draw)
        {
            if (draw.ActualCount <= 0 || draw.Phase?.Kind == ActualDiscardRecoveryPhaseKind.Draw)
                return;
            owner = draw.OwnerSeat;
            turn = draw.TurnNumber;
            invocation = draw.InvocationId;
            outgoing = true;
            total = draw.ActualCount + _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat == owner && e.TurnNumber == turn && e.InvocationId != invocation && e.Phase?.Kind != ActualDiscardRecoveryPhaseKind.Draw).Sum(e => e.ActualCount);
        }
        else if (fact is NativeLogicalDiscardInvocationRecordedEvent discard)
        {
            if (discard.ActualCount <= 0 || discard.Phase?.Kind == ActualDiscardRecoveryPhaseKind.Discard)
                return;
            owner = discard.ExecutorSeat;
            turn = discard.TurnNumber;
            invocation = discard.InvocationId;
            outgoing = false;
            total = discard.ActualCount + _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ExecutorSeat == owner && e.TurnNumber == turn && e.InvocationId != invocation && e.Phase?.Kind != ActualDiscardRecoveryPhaseKind.Discard).Sum(e => e.ActualCount);
        }
        else
            return;
        if (turn < 1 || total < 2 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[owner], MouHuaXiongOlComposition.Yangwei) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouHxYangweiTicketEvent>().Any(e => e.Source.OwnerSeat == owner && e.Turn == turn && e.Outgoing == outgoing))
            return;
        var source = new CardConversionSource(MouHuaXiongOlComposition.Yangwei, outgoing ? "outside-draw" : "outside-discard", owner, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[owner], MouHuaXiongOlComposition.Yangwei));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHxYangweiTicketEvent(source, turn, outgoing, invocation, total));
    }

    internal IReadOnlyList<MouHxYangweiTicketEvent> ActiveMouHxYangweiTickets(int owner, bool outgoing) => Array.AsReadOnly(_coreProgramEventHistoryOperations.ProgramEventHistory<MouHxYangweiTicketEvent>().Where(e => e.Source.OwnerSeat == owner && e.Outgoing == outgoing && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHxYangweiConsumedEvent>().Any(c => c.Source == e.Source && c.IssuedTurn == e.Turn && c.Outgoing == e.Outgoing && c.InvocationId == e.InvocationId)).ToArray());
    internal string? MouHxYangweiPublicDescription(int owner)
    {
        var outgoing = ActiveMouHxYangweiTickets(owner, true).Count;
        var incoming = ActiveMouHxYangweiTickets(owner, false).Count;
        if (outgoing == 0 && incoming == 0)
            return null;
        return $"下次造成的伤害+{(outgoing)}；下次受到的伤害+{(incoming)}。分别生效，未用次数跨回合保留。";
    }

    internal int MouHxYangweiChainCursor(global::CardGame.Core.GameEngine.IDamageAttempt a) => a is global::CardGame.Core.NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(a.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(a.ResolutionId).ChainedTargetIndex;
    internal void SetMouHxYangweiChainAmount(global::CardGame.Core.GameEngine.IDamageAttempt a, int amount)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(a, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !a.IsChainPropagation)
            throw new InvalidOperationException("Yangwei can rebase only its exact current native chain instance.");
        if (a is global::CardGame.Core.NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(a.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(a.ResolutionId, s => s!with { DamageAmount = amount });
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 33300)]
    internal int ApplyMouHxYangwei(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack.DamageAmount <= 0)
            return 0;
        var outgoing = attack.IsSourceLess ? [] : ActiveMouHxYangweiTickets(attack.SourceSeat, true);
        var incoming = ActiveMouHxYangweiTickets(attack.TargetSeat, false);
        var tickets = outgoing.Concat(incoming).ToArray();
        foreach (var t in tickets)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHxYangweiConsumedEvent(t.Source, t.Turn, t.Outgoing, t.InvocationId, attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, attack.IsChainPropagation, attack.IsChainPropagation ? MouHxYangweiChainCursor(attack) : 0));
        return tickets.Length;
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 33300)]
    internal void BeforeMouHxYangweiChain(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!attack.IsChainPropagation)
            return;
        var cursor = MouHxYangweiChainCursor(attack);
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MouHxYangweiChainIssuedEvent>().SingleOrDefault(e => e.AttackId == attack.ResolutionId && e.Cursor == cursor)is not { } issued || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouHxYangweiConsumedEvent>().Any(e => e.AttackId == attack.ResolutionId && e.Chain && e.Cursor == cursor) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouHxYangweiChainReturnedEvent>().Any(e => e.AttackId == attack.ResolutionId && e.Cursor == cursor))
            return;
        // Earlier native rebasers may already have removed their own temporary increment.
        // Never reintroduce it when removing this chain target's newly consumed tickets.
        SetMouHxYangweiChainAmount(attack, Math.Min(attack.DamageAmount, issued.Inherited));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHxYangweiChainReturnedEvent(attack.ResolutionId, cursor));
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 33300)]
    internal void AfterMouHxYangweiChain(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (!attack.IsChainPropagation || (attack.IsSourceLess || ActiveMouHxYangweiTickets(attack.SourceSeat, true).Count == 0) && ActiveMouHxYangweiTickets(attack.TargetSeat, false).Count == 0)
            return;
        var cursor = MouHxYangweiChainCursor(attack);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHxYangweiChainIssuedEvent(attack.ResolutionId, cursor, attack.DamageAmount));
        if (attack.DamageAmountFinalized)
            SetMouHxYangweiChainAmount(attack, checked(attack.DamageAmount + ApplyMouHxYangwei(attack)));
    }
}
