#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouEnAnniversaryRuntime
{
    internal const string XiahouEnGainReason = "skill-program.anniversary-shiwu.damage-card";
    internal static CardConversionSource XiahouEnSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IAnniversaryXiahouEnHost.Begin(ProgramSkillFrame f, SkillProgramEffectOp operation) => BeginAnniversaryXiahouEn(f, operation);
    [ProgramPipelineHook(ProgramPipelinePhase.VirtualEquipment, 45000)]
    internal CardKind? XiahouEnVirtualQinggang(int seat, EquipmentSlot slot) => slot == EquipmentSlot.Weapon && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], AnniversaryXiahouEnComposition.Chijian) && !_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Any(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == EquipmentSlot.Weapon) ? CardKind.QinggangSword : null;
    [ProgramDistanceRule(ProgramDistanceStage.AttackRangeBaseTerms, 45000)]
    internal void XiahouEnVirtualSwordRange(ProgramAttackRangeQuery query, List<RuleQueryBaseTerm> terms)
    {
        if (query.ExcludedEquipmentSlot != EquipmentSlot.Weapon && XiahouEnVirtualQinggang(query.Player.Seat, EquipmentSlot.Weapon)is { } kind && _coreEquipmentContentOperations.EquipmentRules.Get(kind).WeaponAttackRange is { } range && terms[0].Value < range)
            terms[0] = new RuleQueryBaseTerm($"skill:{(query.Player.Seat)}:anniversary-chijian:attack-range", range);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 45000, SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwu, SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwuDamage, SkillProgramEffectOp.BequeathAnniversaryXiahouEnChijian)]
    internal bool CanOfferAnniversaryXiahouEn(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => trigger.Id switch
    {
        AnniversaryXiahouEnComposition.Duel => context.SourceSeat is { } other && other == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(other, candidate.OwnerSeat),
        AnniversaryXiahouEnComposition.Damage => XiahouEnDamageOrigin(candidate.OwnerSeat, context)is not null,
        AnniversaryXiahouEnComposition.Death => context.SourceSeat is { } killer && killer != candidate.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(killer) && _coreMatchParticipantsPlayersAccess._players[killer].IsAlive,
        _ => false
    };
    internal XiahouEnShiwuOrigin? XiahouEnDamageOrigin(int owner, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.Amount is not > 0 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, DamageWasApplied: true, DamageAmount: > 0 } attack || attack.EffectiveCardKind != CardKind.Duel || context.SourceSeat != attack.SourceSeat || context.TargetSeat != attack.TargetSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { SelectedActorDuelOrigin: { XiahouEnShiwu: { } origin } } use || origin.Source.OwnerSeat != owner || !ValidXiahouEnShiwuUse(use, use.SelectedActorDuelOrigin) || !(attack.SourceSeat == owner && attack.TargetSeat == origin.TurnOwner || attack.TargetSeat == owner && attack.SourceSeat == origin.TurnOwner))
            return null;
        return origin;
    }

    internal SkillProgramStepOutcome BeginAnniversaryXiahouEn(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.AnniversaryXiahouEn is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context)
            throw new InvalidOperationException("Shiwu requires its original unpaid trigger instruction.");
        if (operation == SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwu)
        {
            if (!_coreForeignTurnContestsOperations.Value.IsForeignActualTurnPindian(f) || context.SourceSeat is not { } other || !_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(other, f.OwnerSeat))
                throw new InvalidOperationException("Shiwu requires the exact other actual turn start and legal owner-to-turn-owner Duel.");
            var origin = new XiahouEnShiwuOrigin(f.Id, f.InstructionIndex, XiahouEnSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, other, context.ParentFrameId, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, 0);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouEn = new(XiahouEnSource(f), f.GameplayHash, operation, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, other, AnniversaryXiahouEnStage.NativeUse, origin) });
            var native = new ProgramSelectedActorDuelOrigin(f.Id, f.InstructionIndex, f.OwnerSeat, f.OwnerSeat, f.SkillId, f.ActivationId!, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
            {
                InitialTargetSeat = other,
                XiahouEnShiwu = origin
            };
            var card = new Card(0, CardKind.Duel, Suit.None, 0);
            var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [other], CardKind.Duel, physicalCardIds: [], selectedActorDuelOrigin: native);
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
            if (id != origin.UseId || use.Action is not { Type: CardActionType.Use } action)
                throw new InvalidOperationException("Shiwu lost its exact declared native Duel.");
            origin = origin with
            {
                ActionId = action.ActionId
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { SelectedActorDuelOrigin = native with { XiahouEnShiwu = origin } });
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { AnniversaryXiahouEn = f.AnniversaryXiahouEn!with { Origin = origin } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouEnShiwuIssuedEvent(origin));
            _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, f.OwnerSeat, [other], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (operation == SkillProgramEffectOp.BequeathAnniversaryXiahouEnChijian)
        {
            if (context.Window != SkillProgramTriggerWindow.OwnerDied || context.SourceSeat is not { } killer || killer == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[killer].IsAlive || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().LastOrDefault()is not { } death || death.VictimSeat != f.OwnerSeat || death.KillerSeat != killer)
                throw new InvalidOperationException("Shiwu bequest requires the real death and its actual surviving other killer.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouEn = new(XiahouEnSource(f), f.GameplayHash, operation, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, AnniversaryXiahouEnStage.SkillChildren, Before: before, After: before) });
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[killer], $"anniversary-shiwu-bequest:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(f.Id)}", [AnniversaryXiahouEnComposition.Chijian]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouEnChijianGrantedEvent(f.Id, f.OwnerSeat, killer, true));
            ContinueAnniversaryXiahouEn(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var damageOrigin = XiahouEnDamageOrigin(f.OwnerSeat, context) ?? throw new InvalidOperationException("Shiwu settlement lost its original actual Duel damage.");
        if (context.DamageFrameId is not { } damageId || context.SourceSeat is not { } source || context.TargetSeat is not { } target)
            throw new InvalidOperationException("Shiwu damage requires an exact original native damage frame.");
        var receipt = new AnniversaryXiahouEnReceipt(XiahouEnSource(f), f.GameplayHash, operation, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, damageOrigin.TurnOwner, source == f.OwnerSeat ? AnniversaryXiahouEnStage.GainChildren : AnniversaryXiahouEnStage.SkillChildren, damageOrigin, damageId, source, target, context.Amount, before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouEn = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouEnShiwuDamageBoundEvent(f.Id, damageOrigin, damageId, source, target, receipt.DamageAmount));
        if (source == f.OwnerSeat)
        {
            var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => CoreGameEngineMouLuMengRuntime.IsDamageCardKind(c.Kind) || CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(c.Kind) || c.Kind == CardKind.Lightning).ToArray();
            if (pool.Length > 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                var card = pool[_randomOperations._random.Next(pool.Length)];
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), AnniversaryXiahouEn = receipt with { GainedCardId = card.Id } });
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new(XiahouEnGainReason));
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouEn = f.AnniversaryXiahouEn!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouEnShiwuGainPaidEvent(f.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == XiahouEnGainReason)));
        }
        else
        {
            // The period follows only the owner's loss clause. The opponent's new independent source is permanent.
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
                _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[source], $"anniversary-shiwu-duel:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(damageOrigin.FrameId)}", [AnniversaryXiahouEnComposition.Chijian]);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouEnChijianGrantedEvent(f.Id, f.OwnerSeat, source, false));
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            foreach (var grant in owner.SkillGrants.Grants.Where(g => g.SkillId == AnniversaryXiahouEnComposition.Chijian && !g.IsModeRule).ToArray())
            {
                owner.SkillGrants.RemoveGrant(grant.GrantId);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouEnChijianLostGrantEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, owner.General.Id, owner.SecondaryGeneral?.Id, grant));
            }
        }

        ContinueAnniversaryXiahouEn(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 45000)]
    internal bool ResumeAnniversaryXiahouEn(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AnniversaryXiahouEn: not null } f || f.Id != id)
            return false;
        ContinueAnniversaryXiahouEn(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 45000)]
    internal bool ReturnAnniversaryXiahouEnMovement(ProgramSkillFrame f) => f.AnniversaryXiahouEn is not null && ResumeAnniversaryXiahouEn(f.Id);
    internal void ContinueAnniversaryXiahouEn(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.AnniversaryXiahouEn!;
        if (r.Stage == AnniversaryXiahouEnStage.NativeUse)
        {
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.Origin!.UseId)is not null)
                return;
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
                throw new InvalidOperationException("Shiwu must wait for its native attack cleanup.");
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.Origin.UseId && e.CardId == 0 && e.CardKind == CardKind.Duel) != 1)
                throw new InvalidOperationException("Shiwu cannot finish an unreturned or different Duel.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AnniversaryXiahouEn = r with { Stage = AnniversaryXiahouEnStage.NativeReturned } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouEnShiwuReturnedEvent(f.Id, r.Origin.UseId, r.Origin.ActionId));
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 45000)]
    internal void RestoreXiahouEnSwordAtActualTurnEnd(TurnEndedEvent ended)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryXiahouEnShiwuDamage))
            return;
        var restored = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouEnChijianRestoredEvent>().Select(e => (e.FrameId, e.GrantId)).ToHashSet();
        foreach (var loss in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouEnChijianLostGrantEvent>().Where(e => e.ActualTurn == ended.TurnNumber && e.TurnOwner == ended.ActorSeat && !restored.Contains((e.FrameId, e.Grant.GrantId))).ToArray())
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[loss.OwnerSeat];
            var grant = loss.Grant;
            var outcome = _coreAlcoholSlashSkillReplacementsOperations.WasGrantPermanentlyRevokedAfterLoss(owner.Seat, grant, loss) ? "permanently-lost" : !owner.IsAlive ? "owner-dead" : grant.SourceId == CharacterState.PrimarySkillSource && owner.General.Id != loss.GeneralId || grant.SourceId == CharacterState.SecondarySkillSource && owner.SecondaryGeneral?.Id != loss.SecondaryGeneralId ? "template-changed" : _coreRandomDamageTargetSkillsOperations.RandomDamageTargetSkillGrantExpired(grant) || grant.PhaseExpiry is not null ? "grant-expired" : "restored";
            if (outcome == "restored" && owner.SkillGrants.Grants.SingleOrDefault(g => g.GrantId == grant.GrantId)is { } existing)
            {
                if (existing != grant)
                    throw new InvalidOperationException("Shiwu cannot replace a newly changed grant identity.");
                outcome = "already-present";
            }

            if (outcome == "restored")
                owner.SkillGrants.Grant(grant);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiahouEnChijianRestoredEvent(loss.FrameId, loss.OwnerSeat, grant.GrantId, ended.TurnNumber, ended.ActorSeat, outcome));
        }
    }
}
