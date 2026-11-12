#nullable enable

namespace CardGame.Core.EngineOwners;

// Executes a declared pass on its original use. Source rules issue episodes;
// this native owner never selects a character or refreshes source eligibility.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeCardEffectEpisodeExecutionRuntime(INativeCardActionInvalidations _invalidations,
    IProgramRuntimeFrames _frames, INativeCardRepresentations _representations,
    IMatchParticipantStateAccess _participants,
    RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _cardUses,
    RuntimeEntries.CoreGameEngineCardAttackStateEntries _attacks,
    RuntimeEntries.CoreGameEngineSequentialTrickTargetsEntries _sequential,
    RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries _adjusted,
    RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries _targetEffects,
    RuntimeEntries.CoreActiveNativeAttackQueriesEntries _activeAttacks,
    RuntimeEntries.CoreNativeCardContinuationCursorsEntries _cursors,
    RuntimeEntries.CoreNativeNullificationEntries _nullification,
    RuntimeEntries.CoreNativePromptBoundaryEntries _prompts,
    RuntimeEntries.CoreNativeSlashUseEntries _slash,
    RuntimeEntries.CoreNativeRecoveryCardsEntries _recovery,
    RuntimeEntries.DeferredRuntime<CoreDyingCursorRuntimeRuntime> _dying,
    IKernelCardAttackAndFangtianHandlesLookup _handles,
    EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _outcome,
    RuntimeEntries.CoreDirectedTurnCardPoliciesEntries _coreDirectedTurnCardPoliciesOperations,
    RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries _coreDeckEquipmentProgramsOperations,
    RuntimeEntries.CoreGameEngineGuanduActivitiesEntries _coreGuanduActivitiesOperations,
    RuntimeEntries.CoreGameEngineProgramCardUseRolesEntries _coreProgramCardUseRolesOperations,
    RuntimeEntries.CoreGameEngineShenwuEquipmentEntries _coreShenwuEquipmentOperations,
    RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations,
    RuntimeEntries.CoreNativeBorrowedSwordEntries _coreNativeBorrowedSwordOperations,
    RuntimeEntries.CoreNativeDamagePoliciesEntries _coreNativeDamagePoliciesOperations,
    RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations,
    RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations,
    EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess)
    : INativeCardEffectEpisodeExecution
{
    public bool IsMatchCompleted => _outcome._winner != Winner.None;
    public bool IsUseIneffective(CardUseFrame use) => _invalidations.IsUseIneffective(use);
    public bool HasPendingTargets(CardUseFrame use) =>
        _sequential.HasRemainingSequentialTrickTargets(use.Id) || _adjusted.HasRemainingAdjustedSimpleTargets(use.Id) ||
        _adjusted.HasRemainingAdjustedBorrowedSwordTargets(use.Id) || use.NativeDraw is not null ||
        use.NativeCardEffectDraw is not null || use.WumiaoFlood is not null || use.GuanduCardEffect is not null ||
        use.DragonBoatCardEffect is not null;

    public bool Begin(CardUseFrame supplied, NativeCardEffectOriginalState original, NativeCardEffectEpisode episode,
        NativeCardEffectEpisodeCursor cursor, bool afterAttack)
    {
        var use = _frames.Find(supplied.Id) as CardUseFrame ?? throw new InvalidOperationException("A repeated native effect lost its owning use.");
        if (episode.Issue.Spec.Replay == NativeCardEffectEpisodeReplay.ConsumePhysicalPlacement) return false;
        var local = episode.Issue.Spec.Scope != NativeCardEffectEpisodeScope.WholeUse;
        if (afterAttack)
        {
            _activeAttacks.ActiveCardAttack = null;
            _cursors.ActiveDuel = null;
            _prompts.ClearPendingDecision();
        }
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
        {
            var first = original.Attacks.OrderBy(seed => seed.TargetIndex).FirstOrDefault(seed =>
                (!local || seed.TargetIndex == cursor.TargetIndex) && IsAvailableSlashTarget(use, cursor, seed, local));
            if (first is null) return false;
            var prepared = use.PreparedTargetAttacks?.Select((state, index) =>
                original.Attacks.SingleOrDefault(seed => seed.TargetIndex == index)?.Attack ?? state).ToArray();
            _cardUses.UpdateLifecycleCardUse(use.Id, current => current with { TargetIndex = first.TargetIndex,
                PreparedTargetAttacks = prepared is null ? null : Array.AsReadOnly(prepared) });
            if (!local && original.Fangtian is { } fangtian && first.TargetIndex < fangtian.TargetSeats.Count)
            {
                _attacks.UpdateCardContinuations(use.Id, state => state with { FangtianHalberd = fangtian with { Active = true, CurrentAttackOwnerId = use.Id } });
                _cursors.ActiveFangtianHalberd = _handles.CreateFangtianHalberdHandle(use.Id);
            }
            else _cursors.ActiveFangtianHalberd = null;
            BeginSlashTarget(use.Id, first);
            return true;
        }
        if (original.Trick is { } trick)
        {
            _cursors.ActiveGroupCard = null;
            _cursors.ActiveBorrowedSword = null;
            var card = _representations.Get(use.Id, use.CardId, requireProcessing: true);
            _nullification.BeginNullificationWindow(use.Id, card, use.SourceSeat, cursor.TargetSeats,
                trick.ActionKind, null, trick.RequiredCardKind, use.CardKind);
            return true;
        }
        if (original.Simple is not { } simple)
            throw new InvalidOperationException("An issued effect episode requires its original native continuation.");
        if (episode.Issue.Spec.Replay == NativeCardEffectEpisodeReplay.BoundSelfRescueRecovery)
            return BeginBoundSelfRecovery(use, simple);
        if (!local && use.TargetSeats.Count > 1 && simple.Effect is SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol or SimpleCardUseEffect.GuanduIntoxication)
            _cardUses.UpdateLifecycleCardUse(use.Id, current => current with { AdjustedSimpleContinuation = simple });
        _cardUses.ContinueSimpleCardUse(use.Id, simple);
        return true;
    }

    public bool ContinueSlashTargets(CardUseFrame use, NativeCardEffectOriginalState original,
        NativeCardEffectEpisodeCursor cursor)
    {
        if (IsMatchCompleted || IsUseIneffective(use)) return false;
        if (_frames.Top?.Id != use.Id)
            throw new InvalidOperationException("A repeated Slash must finish its native child before advancing its target occurrence.");
        var next = original.Attacks.OrderBy(seed => seed.TargetIndex).FirstOrDefault(seed =>
            seed.TargetIndex > use.TargetIndex && IsAvailableSlashTarget(use, cursor, seed, local: false));
        if (next is null) return false;
        _cardUses.UpdateLifecycleCardUse(use.Id, current => current with
            { TargetIndex = next.TargetIndex, Step = ResolutionFrameStep.ResolvingEffect });
        _cursors.ActiveDuel = null;
        _prompts.ClearPendingDecision();
        BeginSlashTarget(use.Id, next);
        return true;
    }

    private bool IsAvailableSlashTarget(CardUseFrame use, NativeCardEffectEpisodeCursor cursor,
        NativeCardEffectAttackSeed seed, bool local)
    {
        var slot = local ? seed.TargetIndex - cursor.TargetIndex : seed.TargetIndex;
        return slot >= 0 && slot < cursor.TargetSeats.Count && cursor.TargetSeats[slot] == seed.Attack.TargetSeat &&
            seed.TargetIndex >= 0 && seed.TargetIndex < use.TargetSeats.Count && use.TargetSeats[seed.TargetIndex] == seed.Attack.TargetSeat &&
            _participants.At(seed.Attack.TargetSeat).IsAlive && !_targetEffects.IsCardEffectIneffective(use.Id, seed.Attack.TargetSeat);
    }

    private void BeginSlashTarget(long useId, NativeCardEffectAttackSeed seed)
    {
        _attacks.UpdateCardAttackState(useId, _ => CoreGameEngineExtraCardEffectRoundsRuntime.ResetExtraCardEffectAttack(seed.Attack));
        var attack = _handles.CreateCardAttackHandle(useId);
        _activeAttacks.ActiveCardAttack = attack;
        _slash.ContinueSlashDefense(attack);
    }

    private bool BeginBoundSelfRecovery(CardUseFrame use, ProgramSimpleCardContinuation simple)
    {
        // The typed producer already paid and recorded this rescue. An episode
        // repeats its recovery only, never another Alcohol use or rescue record.
        if (use is not { CardId: 0, CardKind: CardKind.Alcohol, PhysicalCardIds.Count: 0, LegacyDyingAlcoholReturn: { } returned } ||
            simple is not { CardId: 0, Effect: SimpleCardUseEffect.Recovery } || !use.TargetSeats.SequenceEqual([use.SourceSeat]) ||
            _frames.ParentOf(use.Id) is not ProgramSkillFrame parent || parent.Id != returned.ProgramFrameId ||
            parent.InstructionIndex != returned.InstructionIndex || parent.OwnerSeat != use.SourceSeat ||
            returned.ProducerSource != new CardConversionSource(parent.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId) ||
            parent.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context ||
            _dying.Value.ActiveDying is not { } dying || dying.FrameId != context.ParentFrameId || dying.VictimSeat != use.SourceSeat)
            throw new InvalidOperationException("A bound recovery episode changed its original self-rescue producer or typed return.");
        _recovery.CompleteRecoveryCardUse(_participants.At(use.SourceSeat), _participants.At(use.SourceSeat),
            new Card(0, CardKind.Alcohol, Suit.None, 0), use.Id, CardKind.Alcohol, simple.RecoveryAmount, simple.RecoveryPolicySources ?? []);
        return true;
    }

    public bool TargetsRemainLegal(CardUseFrame use)
    {
        if (use.Action is not { } action || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive || use.TargetSeats.Count == 0)
            return false;
        bool IsTrickTargetAllowed(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(use.SourceSeat, seat, use.CardKind, use.Id) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[seat], use.CardKind, action.EffectiveSuit ?? Suit.None, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(use.SourceSeat, seat, action.EffectiveSuit) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(use.SourceSeat, seat, use.CardKind);
        // Borrowed Sword designates weapon owners. Each following seat is that
        // owner's Slash victim, not another target of the original trick.
        if (use.CardKind == CardKind.BorrowedSword)
        {
            if (use.TargetSeats.Count % 2 != 0)
                return false;
            for (var i = 0; i < use.TargetSeats.Count; i += 2)
            {
                var owner = use.TargetSeats[i];
                var victim = use.TargetSeats[i + 1];
                if (!IsTrickTargetAllowed(owner) || owner == use.SourceSeat || _coreNativeBorrowedSwordOperations.GetWeapon(_coreMatchParticipantsPlayersAccess._players[owner])is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) || !_coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[victim]))
                    return false;
            }

            return true;
        }

        foreach (var seat in use.TargetSeats)
        {
            if (!IsTrickTargetAllowed(seat))
                return false;
            var target = _coreMatchParticipantsPlayersAccess._players[seat];
            var legal = use.CardKind switch
            {
                CardKind.Peach or CardKind.Zong => _coreTreatedWoundedOperations.IsRuleWounded(target),
                CardKind.Reinforcements => seat != use.SourceSeat && _coreTreatedWoundedOperations.IsRuleWounded(target),
                CardKind.Alcohol or CardKind.RealgarWine or CardKind.DrawTwo or CardKind.IronChain => true,
                CardKind.Intoxication => !_coreGuanduActivitiesOperations.GuanduAllies(use.SourceSeat, seat),
                CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
                CardKind.StoreGrain or CardKind.UnitedEffort or CardKind.PeachGarden or CardKind.FiveGrains or CardKind.BarbarianAssault or CardKind.ArrowBarrage => true,
                _ => _coreProgramCardUseRolesOperations.CanBeProgramCardUseRoleTarget(use, target)};
            if (!legal)
                return false;
        }

        return true;
    }
}
