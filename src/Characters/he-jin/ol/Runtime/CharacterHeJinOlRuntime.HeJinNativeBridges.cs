#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeJinOlRuntime
{
    internal bool CanIssueHeJinMouzhuCard(int actor, int target, CardKind kind)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || actor == target || !_coreMatchParticipantsPlayersAccess._players[actor].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return false;
        if (kind == CardKind.Duel)
            return _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(target, actor);
        return kind == CardKind.Slash && !_coreCardUseModulesOperations.IsCardUseForbidden(actor, kind, CardActionType.Use) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], kind, Suit.None) && (_coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], kind) || _coreCardConversionsOperations.IgnoresSlashUseDistance(_coreMatchParticipantsPlayersAccess._players[actor]) || _coreFinalTargetSlashOperations.IsWithinSpecificSlashRange(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], kind, 0)) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[target]);
    }

    internal void BeginHeJinMouzhuCard(ProgramSkillFrame frame, HeJinMouzhuUseOrigin origin)
    {
        if (!MatchesHeJinMouzhu(frame, origin) || origin.CardUseFrameId != _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1 || !CanIssueHeJinMouzhuCard(origin.ActorSeat, origin.TargetSeat, origin.CardKind) || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("Mouzhu requires its exact paid gift, choosing donor and native legal assault.");
        var card = new Card(0, origin.CardKind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, origin.ActorSeat, [origin.TargetSeat], origin.CardKind, physicalCardIds: [], heJinMouzhu: origin);
        if (id != origin.CardUseFrameId)
            throw new InvalidOperationException("Mouzhu native use changed its reserved frame.");
        if (origin.CardKind == CardKind.Duel)
        {
            _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, origin.ActorSeat, [origin.TargetSeat], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
            return;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[origin.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[origin.TargetSeat];
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, use => use with { HeJinMouzhu = origin with { AttackStarted = true } });
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, CardKind.Slash), programSkillCardUseFrameId: frame.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, target.Seat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        var action = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!;
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool IsHeJinMouzhuDuelUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { HeJinMouzhu.CardKind: CardKind.Duel, CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0 };
    internal bool IsHeJinMouzhuAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing) => attack.Card is null && processing.Count == 0 && attack.PhysicalCards.Count == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.HeJinMouzhu is { AttackStarted: true } origin && attack.EffectiveCardKind == origin.CardKind && attack.ProgramSkillCardUseFrameId == origin.ProgramFrameId && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == origin.ProgramFrameId && MatchesHeJinMouzhu(f, origin));
    [NativeUseReturn(26710)]
    internal void ReturnHeJinMouzhuUse(CardUseFrame use)
    {
        if (use.HeJinMouzhu is not { AttackStarted: false } origin)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || use.Id != origin.CardUseFrameId || !MatchesHeJinMouzhu(frame, origin))
            throw new InvalidOperationException("Mouzhu lost its canceled native assault return.");
        FinishHeJinMouzhu(frame, true);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 26710)]
    internal bool ReturnHeJinMouzhuAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.HeJinMouzhu is not { } origin)
            return false;
        if (!origin.AttackStarted || completion.ProgramFrameId != origin.ProgramFrameId || completion.ResolutionId != origin.CardUseFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || !MatchesHeJinMouzhu(frame, origin))
            throw new InvalidOperationException("Mouzhu lost its native response, damage or dying completion.");
        FinishHeJinMouzhu(frame, true);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 26710)]
    internal void AssertHeJinMouzhuNativeUses(ProgramSkillFrame frame)
    {
        if (frame.HeJinMouzhu is null)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.HeJinMouzhu is not null))
        {
            var origin = use.HeJinMouzhu!;
            var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || !MatchesHeJinMouzhu(parent, origin) || use.Id != origin.CardUseFrameId || use.CardId != 0 || use.CardKind != origin.CardKind || use.PhysicalCardIds?.Count != 0 || use.SourceSeat != origin.ActorSeat || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: false } action || action.EffectiveKind != origin.CardKind || action.ActorSeat != origin.ActorSeat || action.ProviderSeat != origin.ActorSeat)
                throw new InvalidOperationException("Mouzhu requires its exact issued zero-entity native use and original paid owner.");
        }
    }
}
