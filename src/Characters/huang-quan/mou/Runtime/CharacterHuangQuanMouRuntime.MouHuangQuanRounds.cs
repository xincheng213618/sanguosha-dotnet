#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanMouRuntime
{
    internal void IssueMouHqOriginalEffect(ProgramSkillFrame f)
    {
        if (!ValidMouHq(f) || f.MouHuangQuan is not { Paid: true, Stage: MouHqStage.GiveChildren } r || !TryMouHqOriginalUse(f.OwnerSeat, f.WindowContext, out var use, out var window) || use.Id != r.UseId || use.Action!.ActionId != r.ActionId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHqEffectIssuedEvent>().Any(e => e.Grant.FrameId == f.Id))
            throw new InvalidOperationException("Qiaodui requires the exact once-paid original Use after all Give children return.");
        var grant = new MouHqEffectGrant(f.Id, r.Source, r.GameplayHash, r.WindowId, r.UseId, r.ActionId, r.Recipient, r.Count);
        if (!r.Outgoing)
        {
            if (use.SourceSeat == f.OwnerSeat || !use.Action.EffectiveDesignatedTargetSeats.Contains(f.OwnerSeat))
                throw new InvalidOperationException("Incoming Qiaodui lost its original other-user target identity.");
            if (use.MouHqIneffective is null)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouHqIneffective = grant });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHqEffectIssuedEvent(grant, false));
            foreach (var target in use.TargetSeats.Distinct())
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardEffectSkippedEvent(use.Id, use.SourceSeat, target, use.CardKind, CardEffectSkipReason.SkillNullified));
            return;
        }

        if (use.SourceSeat != f.OwnerSeat || use.MouHqRounds is { Started: > 0 })
            throw new InvalidOperationException("Outgoing Qiaodui cannot attach another card or restart an effect already in progress.");
        var old = use.MouHqRounds;
        var native = old?.Native ?? new ExtraCardEffectState
        {
            Source = r.Source,
            ProgramFrameId = f.Id,
            ActionId = r.ActionId,
            Simple = window.SimpleContinuation,
            Trick = window.TrickContinuation
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouHqRounds = new(r.ActionId, native) { Grants = (old?.Grants ?? []).Append(grant).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHqEffectIssuedEvent(grant, true));
    }

    internal bool ValidMouHqEffectGrant(CardUseFrame use, MouHqEffectGrant g, bool repeats) => use.Action is { Type: CardActionType.Use } a && g.UseId == use.Id && g.ActionId == a.ActionId && g.Source.SkillId == MouHqQiaodui && g.Source.BindingId == "give-for-original-effect" && g.WindowId > use.Id && g.FrameId > g.WindowId && g.GivenCount is >= 0 and <= 2 && g.Recipient != g.Source.OwnerSeat && _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program?.GameplayHash == g.GameplayHash && (repeats ? a.ActorSeat == g.Source.OwnerSeat && a.EffectiveDesignatedTargetSeats.Any(t => t != g.Source.OwnerSeat) : a.ActorSeat != g.Source.OwnerSeat && a.EffectiveDesignatedTargetSeats.Contains(g.Source.OwnerSeat)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHqEffectIssuedEvent>().Count(e => e.Grant == g && e.Repeats == repeats) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouHqCommittedEvent>().Count(e => e.FrameId == g.FrameId && e.OwnerSeat == g.Source.OwnerSeat && e.Recipient == g.Recipient && e.Count == g.GivenCount) == 1;
    internal bool IsMouHqUseIneffective(CardUseFrame use)
    {
        if (use.MouHqIneffective is not { } g)
            return false;
        if (!ValidMouHqEffectGrant(use, g, false))
            throw new InvalidOperationException("Qiaodui whole-use ineffectiveness lost its original paid target-owner proof.");
        return true;
    }

    internal bool HasUnstartedMouHqRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouHqRounds: { } r, Action: { } a } u && a.ActionId == r.ActionId && r.Started < r.Grants.Count && _coreMatchOutcomeWinnerAccess._winner == Winner.None && !IsMouHqUseIneffective(u) && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(u) && !_coreProgramCardUseNullificationOperations.Value.IsProgramUseIneffective(u);
    internal bool IsMouHqRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouHqRounds is { Active: true };
    internal void CaptureMouHqAttack(NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouHqRounds: { Started: 0 } r, CardAttack: { } original } u || !CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind) || r.Native.OriginalAttacks.Any(s => s.TargetIndex == u.TargetIndex))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(u.Id, current => current with { MouHqRounds = r with { Native = r.Native with { OriginalAttacks = r.Native.OriginalAttacks.Append(new(u.TargetIndex, original)).ToArray(), Fangtian = current.Continuations.FangtianHalberd ?? r.Native.Fangtian } } });
    }

    internal void CompleteMouHqRound(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouHqRounds: { Active: true, Started: > 0 } r } u)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || u.NativeDraw is not null || u.NativeCardEffectDraw is not null || u.WumiaoFlood is not null || u.GuanduCardEffect is not null || u.DragonBoatCardEffect is not null)
            throw new InvalidOperationException("Qiaodui cannot finish an original effect with native children still pending.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MouHqRounds = r with { Active = false } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHqEffectRoundCompletedEvent(id, r.ActionId, r.Grants[r.Started - 1].FrameId, r.Started));
    }

    internal bool TryBeginMouHqRound(long id, bool afterAttack)
    {
        if (!HasUnstartedMouHqRound(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { MouHqRounds: { } r, Action: { } a } u || u.ExtraCardEffect is { WholeGroupRepeated: false } || _characterFeiYiOlOperations.HasUnstartedFeiYiExtraEffect(id) || _characterDaQiaoLeOperations.HasUnstartedZixiExtraEffect(id) || _characterSunQuanOlMagicOperations.HasUnstartedMoQuanyuExtraRound(id) || _characterHuangYueyingOlMouOperations.HasUnstartedMouBingcaiExtraEffect(id) || _coreSoleIncomingEffectRepeatOperations.Value.HasUnstartedSoleIncomingEffect(id) || _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id) || _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(id))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || r.Active)
            throw new InvalidOperationException("Qiaodui must await the original native effect and its actual children.");
        var grant = r.Grants[r.Started];
        if (!ValidMouHqEffectGrant(u, grant, true))
            throw new InvalidOperationException("Qiaodui lost the original-use extra-effect grant.");
        // Delayed placement has one actual entity. The extra placement cannot duplicate it after it entered Judgment.
        if (CoreNativeCardClassificationsRuntime.IsDelayedCard(u.CardKind))
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { MouHqRounds = r with { Started = r.Started + 1 } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHqEffectRoundStartedEvent(id, a.ActionId, grant.FrameId, r.Started + 1));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHqEffectRoundCompletedEvent(id, a.ActionId, grant.FrameId, r.Started + 1));
            return false;
        }

        var previous = _characterFeiYiOlOperations.NativeExtraEffectState(u);
        var native = r.Native with
        {
            WholeGroupRepeated = true,
            MaterialClaims = previous?.MaterialClaims ?? r.Native.MaterialClaims,
            FrozenGroupDamageSourceSeat = previous?.FrozenGroupDamageSourceSeat ?? r.Native.FrozenGroupDamageSourceSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, u with { MouHqRounds = r with { Started = r.Started + 1, Active = true, Native = native }, TargetIndex = 0, Step = ResolutionFrameStep.ResolvingEffect, FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHqEffectRoundStartedEvent(id, a.ActionId, grant.FrameId, r.Started + 1));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraCardEffectStartedEvent(id, a.ActionId, 0, u.TargetSeats.FirstOrDefault(u.SourceSeat), u.CardKind));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        if (CoreNativeCardClassificationsRuntime.IsSlashCard(u.CardKind))
        {
            var first = native.OriginalAttacks.OrderBy(s => s.TargetIndex).FirstOrDefault(s => _coreMatchParticipantsPlayersAccess._players[s.Attack.TargetSeat].IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, s.Attack.TargetSeat));
            if (first is null)
            {
                CompleteMouHqRound(id);
                return false;
            }

            var prepared = u.PreparedTargetAttacks?.Select((state, index) => native.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == index)?.Attack ?? state).ToArray();
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { TargetIndex = first.TargetIndex, PreparedTargetAttacks = prepared is null ? null : Array.AsReadOnly(prepared) });
            if (native.Fangtian is { } fang)
            {
                _coreCardAttackStateOperations.UpdateCardContinuations(id, c => c with { FangtianHalberd = fang with { Active = true, CurrentAttackOwnerId = id } });
                _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id);
            }

            _coreCardAttackStateOperations.UpdateCardAttackState(id, _ => CoreGameEngineExtraCardEffectRoundsRuntime.ResetExtraCardEffectAttack(first.Attack));
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
            return true;
        }

        if (native.Trick is { } trick)
        {
            _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = null;
            _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
            var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, u.CardId, requireProcessing: true);
            _coreNativeNullificationOperations.BeginNullificationWindow(id, card, u.SourceSeat, u.TargetSeats, trick.ActionKind, null, trick.RequiredCardKind, u.CardKind);
            return true;
        }

        if (native.Simple is { } simple)
        {
            if (u.TargetSeats.Count > 1 && simple.Effect is SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol or SimpleCardUseEffect.GuanduIntoxication)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { AdjustedSimpleContinuation = simple });
            _coreCardUseLifecycleOperations.ContinueSimpleCardUse(id, simple);
            return true;
        }

        throw new InvalidOperationException("Qiaodui lost its original native card-effect continuation.");
    }

    internal bool ContinueMouHqFangtian(NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouHqRounds: { Active: true } r } u)
            return false;
        var seed = r.Native.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == u.TargetIndex);
        if (seed is null)
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return true;
        }

        _coreCardAttackStateOperations.UpdateCardAttackState(u.Id, _ => CoreGameEngineExtraCardEffectRoundsRuntime.ResetExtraCardEffectAttack(seed.Attack));
        _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
        return true;
    }
}
