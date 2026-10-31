#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanOlMagicRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.SoleTargetRedirectSource, 10)]
    internal bool MoDiaoChanSoleTargetRedirectSource(CardConversionSource source) => HasMoDiaoChan && source.SkillId == MoDiaoChanComposition.Qingshi;
    internal bool HasMoDcQingshiCard(int owner, int card) => card > 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanCardGrantedEvent>().Any(e => e.Source.OwnerSeat == owner && e.CardId == card);
    internal bool HasMoDcQingshiCard(int card) => HasMoDiaoChan && card > 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanCardGrantedEvent>().Any(e => e.CardId == card);
    internal bool MoDcRefreshReady(int owner)
    {
        var cohort = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanCohortStartedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner);
        return cohort is null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanCardGrantedEvent>().Where(e => e.CohortId == cohort.CohortId).All(e => _cardZonesOperations._cardZones.GetLocation(e.CardId) != CardLocation.Hand(e.RecipientSeat));
    }

    internal static bool IsMoDcSingleDamageKind(CardKind kind) => RuntimeSkills.RuntimeRules_diao_chan_ol_magic_MoDiaoChanQingshi.IsMoDcSingleDamageKind(kind);
    internal CardKind[] MoDcModeDamageKinds() => _characterCaoZhiLeOperations.LeCaoZhiModeNamePool().Where(IsMoDcSingleDamageKind).ToArray();
    internal void StartMoDcCohort(ProgramSkillFrame f)
    {
        var r = f.MoDiaoChan!;
        var recipients = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray();
        var cohort = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanCohortStartedEvent>().Select(e => e.CohortId).DefaultIfEmpty().Max() + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r with { Paid = true, Stage = MoDiaoChanStage.Obtaining, CohortId = cohort, Targets = recipients, Cursor = 0 } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanCohortStartedEvent(cohort, f.Id, r.Source, r.GameplayHash, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, recipients.Length));
        ContinueMoDc(f.Id);
    }

    internal void ObtainMoDcCohortCard(ProgramSkillFrame f)
    {
        var r = f.MoDiaoChan!;
        var target = r.Targets[r.Cursor];
        var allowed = MoDcModeDamageKinds();
        var pool = _coreMatchParticipantsPlayersAccess._players[target].IsAlive ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => allowed.Contains(c.Kind)).ToArray() : [];
        var randomBefore = _randomOperations._random.State;
        var index = pool.Length == 0 ? -1 : _randomOperations._random.Next(pool.Length);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r = r with { Stage = MoDiaoChanStage.GainChildren, Before = before, After = before, TargetSeat = target }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanObtainChoiceEvent(f.Id, target, pool.Length, index, randomBefore, _randomOperations._random.State, index < 0 ? null : pool[index].Id));
        if (index >= 0)
            _corePhysicalMovementFacadesOperations.MoveCard(pool[index], CardLocation.DrawPile, CardLocation.Hand(target), new("skill-program.ol:mo-qingshi.obtain"), m => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanCardGrantedEvent(r.CohortId, f.Id, r.Source, r.GameplayHash, target, m.CardId, m.CardKind, m.Sequence)), disclosure: CardMovementDisclosure.FaceDown);
        SealMoDcMovement(f.Id, before);
    }

    internal CardUseFrame? MoDcCurrentMarkedUse(int owner, ProgramSkillWindowContext c)
    {
        if (c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == c.ParentFrameId)is not ProgramCardTriggerWindowFrame w || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } use || a.ActionId != identity.CardActionId || use.SourceSeat != a.ActorSeat || a.ActorSeat == owner || use.TargetSeats.Distinct().Count() != 1 || !a.PhysicalCards.Any(m => HasMoDcQingshiCard(owner, m.CardId)))
            return null;
        return use;
    }

    internal int[] MoDcRedirectTargets(CardUseFrame use) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && !use.TargetSeats.Contains(p.Seat) && (!_coreCardUseModulesOperations.HasTurnCardTargetRestriction(use.SourceSeat, SkillProgramCardTargetRestriction.SelfOnly) || p.Seat == use.SourceSeat) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(use.SourceSeat, p.Seat, use.CardKind, use.Id) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(use.SourceSeat, p.Seat, use.Action!.EffectiveSuit) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, use.CardKind, use.Action.EffectiveSuit ?? Suit.None, CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(use.Action)) && (CoreNativeCardClassificationsRuntime.IsDelayedCard(use.CardKind) ? !p.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(p, use.CardKind) && !_coreProgramCardPoliciesOperations.HasCardPolicy(p, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) : _coreDesignatedExtraTargetsOperations.CanBeDistanceFreeDesignatedTarget(use, p))).Select(p => p.Seat).ToArray();
    internal void ReplaceMoDcUniqueTarget(ProgramSkillFrame f, int target)
    {
        var r = f.MoDiaoChan!;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId);
        if (use?.Action is not { } a || a.ActionId != r.ActionId || a.ActorSeat != r.ActorSeat || a.ProviderSeat != r.ProviderSeat || use.TargetSeats is not [var previous] || previous != r.PreviousTarget || !MoDcRedirectTargets(use).Contains(target))
        {
            FinishMoDc(f);
            return;
        }

        var targets = new[]
        {
            target
        };
        var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(a, targets);
        var attack = use.CardAttack;
        if (attack?.TargetSeat == previous)
            attack = attack with
            {
                TargetSeat = target,
                IgnoresArmor = _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[target], use.CardKind)
            };
        var prepared = use.PreparedTargetAttacks is null ? null : Array.AsReadOnly(use.PreparedTargetAttacks.Select(x => x.TargetSeat != previous ? x : x with { TargetSeat = target, IgnoresArmor = _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[target], use.CardKind) }).ToArray());
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanRetargetedEvent(f.Id, r.Source, r.GameplayHash, use.Id, a.ActionId, a.ActorSeat, a.ProviderSeat, previous, target));
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = Array.AsReadOnly(targets), Action = action, CardAttack = attack, PreparedTargetAttacks = prepared, TargetsAdjusted = true }, action);
        // This is the native sole-target mutation fact, with a distinct exact Qingshi producer proof.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleTargetReplacedEvent(use.Id, a.ActionId, r.Source, previous, target, Array.AsReadOnly(new[] { previous }), Array.AsReadOnly(targets)));
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
        {
            var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == r.WindowId);
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { SoleTargetRedirectPending = new(use.Id, a.ActionId, r.Source, target, 0) });
        }
        else
        {
            _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!, targets);
            _coreSoleTargetQuarantineOperations.RefreshSoleProtectionObservers(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!);
        }

        FinishMoDc(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    [ProgramTargetWindow(ProgramTargetWindowStage.SoleTargetReplacementFact, 10)]
    internal bool IsMoDcSoleTargetFact(CardUseFrame use, SoleTargetReplacedEvent fact, IReadOnlyList<int> before) => HasMoDiaoChan && fact.Source.SkillId == MoDiaoChanComposition.Qingshi && fact.Source.BindingId == "cohort-unique-target" && before is [var old] && fact.BeforeTargets.SequenceEqual(before) && fact.AfterTargets is [var target] && old == fact.PreviousTargetSeat && target == fact.NewTargetSeat && target != old && fact.CardUseFrameId == use.Id && fact.ActionId == use.Action?.ActionId && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanRetargetedEvent>().Count(e => e.Source == fact.Source && e.UseId == use.Id && e.ActionId == fact.ActionId && e.PreviousTarget == old && e.NewTarget == target && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanCostEvent>().Any(p => p.FrameId == e.FrameId && p.From.OwnerSeat == e.Source.OwnerSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == p.Sequence && m.CardId == p.CardId && m.From == p.From && m.To == p.To))) == 1;
    internal bool MoDcDamageEligible(int owner, ProgramSkillWindowContext c) => c.Window == SkillProgramTriggerWindow.AfterDamageApplied && c.Amount > 0 && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } attack && attack.PhysicalCards.Any(card => HasMoDcQingshiCard(owner, card.Id)) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanStartedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Op == SkillProgramEffectOp.ResolveMoDiaoChanQingshiDamage && e.WindowId == c.ParentFrameId);
    internal bool MoDcUsePaymentMovement(CardMovementRecord movement)
    {
        foreach (var p in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanUseMaterialEvent>().Where(p => p.CardId == movement.CardId))
        {
            var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.CardId == p.CardId && m.Sequence > p.Before && m.From == p.From);
            if (paid is null)
                continue;
            if (paid.Sequence == movement.Sequence)
                return true;
            if (paid.To == CardLocation.Processing && movement.From == CardLocation.Processing && movement.To == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.CardId == p.CardId && m.Sequence > paid.Sequence && m.From == CardLocation.Processing)?.Sequence == movement.Sequence)
                return true;
        }

        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 31401)]
    internal void CaptureMoDcMaterialPayments(IGameEvent fact)
    {
        if (!HasMoDiaoChan)
            return;
        var use = fact is CardUseDeclaredEvent d ? _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId) : null;
        var action = use?.Action ?? (fact is CardActionAcceptedEvent accepted ? accepted.Action : null);
        if (action is null || !_characterLuBuOlMagicOperations.IsMoLuBuTrueUse(action))
            return;
        foreach (var cost in action.PhysicalCards)
        {
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanUseMaterialEvent>().Any(e => e.ActionId == action.ActionId && e.CardId == cost.CardId))
                continue;
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            if (action.Type == CardActionType.Response || use?.AcceptedDeclarationPayment?.Cost.CardId == cost.CardId)
            {
                var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId && m.From == cost.From && m.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber);
                if (paid is not null)
                    before = paid.Sequence - 1;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanUseMaterialEvent(use?.Id ?? action.ParentActionId ?? 0, action.ActionId, cost.CardId, cost.From, before));
        }
    }

    internal int[] MoDcRecaptureIndexes(CardMovementBatchContext batch, int owner) => batch.Movements.Select((m, i) => (m, i)).Where(x => x.m.To == CardLocation.DiscardPile && HasMoDcQingshiCard(owner, x.m.CardId) && _cardZonesOperations._cardZones.GetLocation(x.m.CardId) == CardLocation.DiscardPile && !MoDcUsePaymentMovement(x.m) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanDiscardClaimedEvent>().Any(e => e.Source.OwnerSeat == owner && e.CardId == x.m.CardId && e.Sequence == x.m.Sequence)).Select(x => x.i).ToArray();
    internal CardMovementRecord[] MoDcRecaptureCards(int owner, ProgramSkillWindowContext c) => c.Window == SkillProgramTriggerWindow.DiscardPileReceived && c.MovementBatch is { } batch && c.MovementIndex is { } index && MoDcRecaptureIndexes(batch, owner).Contains(index) ? [batch.Movements[index]] : [];
    internal void RecaptureMoDcCards(ProgramSkillFrame f)
    {
        var r = f.MoDiaoChan!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoDiaoChan = r = r with { Paid = true, Stage = MoDiaoChanStage.GainChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            foreach (var m in MoDcRecaptureCards(f.OwnerSeat, f.WindowContext!))
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoDiaoChanDiscardClaimedEvent(f.Id, r.Source, m.CardId, m.Sequence));
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new("skill-program.ol:mo-qingshi.recapture"));
            }

        SealMoDcMovement(f.Id, before);
    }

    internal bool IsMoDcQingshiRumoOrigin(OlMoRumoEnteredEvent e) => e.Origin.SkillId == MoDiaoChanComposition.Qingshi && e.Origin.BindingId == "prepare-enter-or-refresh" && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanQingshiEnteredEvent>().Count(p => p.FrameId == e.EntryId && p.Source == e.Origin && p.GameplayHash == e.OriginGameplayHash && p.ActualTurn == e.ActualTurn && p.Round == e.EntryRound) == 1;
    internal bool IsPaidMoDcQingshiOrigin(long id, CardConversionSource source, string hash) => source.SkillId == MoDiaoChanComposition.Qingshi && source.BindingId == "prepare-enter-or-refresh" && _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash == hash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanQingshiEnteredEvent>().Count(e => e.FrameId == id && e.Source == source && e.GameplayHash == hash) == 1;
    internal bool IsPaidMoDcQingshiEntry(ProgramSkillFrame f) => f.MoDiaoChan is { Op: SkillProgramEffectOp.ResolveMoDiaoChanQingshiPrep, Paid: true } r && ValidMoDc(f) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MoDiaoChanQingshiEnteredEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == f.GameplayHash) == 1;
}
