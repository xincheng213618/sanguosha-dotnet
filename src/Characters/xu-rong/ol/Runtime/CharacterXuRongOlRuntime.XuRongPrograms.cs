#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuRongOlRuntime
{
    internal const string XuRongTakeReason = "program.xionghuo.random-take", XuRongClaimReason = "program.shajue2023.claim";
    internal CardConversionSource XuRongSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int XuRongPool(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.Baoli, owner));
    internal IEnumerable<XionghuoGrant> XuRongSubjectGrants(int owner, int subject) => _coreMatchParticipantsPlayersAccess._players[subject].XionghuoGrants.Where(g => g.TargetSeat == subject && g.Source.OwnerSeat == owner && LiveXionghuoSource(g));
    internal int XuRongHandLimitPenalty(CharacterState owner) => owner.XuRongRestrictions.Count(r => r.Branch == 1 && r.SubjectSeat == owner.Seat && r.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.ActualTurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
    internal bool IsXuRongSlashTargetForbidden(int actor, int target, CardKind kind) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreMatchParticipantsPlayersAccess._players[actor].XuRongRestrictions.Any(r => r.Branch == 0 && r.SubjectSeat == actor && r.Source.OwnerSeat == target && r.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && r.ActualTurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 470)]
    internal void ObserveXuRongTurnExpiry(IGameEvent payload)
    {
        if (payload is TurnEndedEvent t)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                p.ExpireXuRongRestrictions(t.TurnNumber);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 430)]
    internal bool CanOfferXuRongLifecycle(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        var t = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        if (t.Effects.Count != 1)
            return true;
        return t.Effects[0].Op switch
        {
            SkillProgramEffectOp.InitializeXionghuoPool => context.Window == SkillProgramTriggerWindow.GameStarting && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XionghuoPoolChangedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Kind == "initialize"),
            SkillProgramEffectOp.SettleXionghuoMarkedPlay => context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && context.SourceSeat is { } subject && subject != candidate.OwnerSeat && XuRongSubjectGrants(candidate.OwnerSeat, subject).Any(),
            SkillProgramEffectOp.ReplenishShaJueAndClaimDyingCause => context.Window == SkillProgramTriggerWindow.DyingEntering && context.TargetSeat is { } victim && victim != candidate.OwnerSeat && context.XuRongDying is { } proof && proof.VictimSeat == victim,
            _ => true
        };
    }

    internal bool CanTargetXuRong(int owner, int target, SkillProgramActivation activation) => !activation.Effects.Any(e => e.Op == SkillProgramEffectOp.TransferXionghuoMark) || target != owner && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].Markers.GetValueOrDefault(PlayerMarkerKind.Baoli) < 3;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 60)]
    internal bool CanActivateXuRong(int owner, SkillProgramActivation activation) => !activation.Effects.Any(e => e.Op == SkillProgramEffectOp.TransferXionghuoMark) || XuRongPool(owner) > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.Seat != owner && p.IsAlive && p.Markers.GetValueOrDefault(PlayerMarkerKind.Baoli) < 3);
    internal SkillProgramStepOutcome ExecuteXuRong(SkillProgramEffectOp op, ProgramSkillFrame f) => op switch
    {
        SkillProgramEffectOp.InitializeXionghuoPool => InitializeXuRong(f),
        SkillProgramEffectOp.TransferXionghuoMark => TransferXuRong(f),
        SkillProgramEffectOp.SettleXionghuoMarkedPlay => SettleXuRong(f),
        SkillProgramEffectOp.ReplenishShaJueAndClaimDyingCause => ClaimXuRong(f),
        _ => throw new InvalidOperationException("Unknown Xu Rong operation.")};
    internal SkillProgramStepOutcome InitializeXuRong(ProgramSkillFrame f)
    {
        var before = XuRongPool(f.OwnerSeat);
        if (f.WindowContext?.Window != SkillProgramTriggerWindow.GameStarting || before != 0)
            throw new InvalidOperationException("Xu Rong initial pool requires one actual unpaid GameStart.");
        _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.Baoli, 3);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XionghuoPoolChangedEvent(f.Id, XuRongSource(f), before, 3, "initialize"));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome TransferXuRong(ProgramSkillFrame f)
    {
        if (f.TriggerId is not null || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || XuRongPool(f.OwnerSeat) < 1 || _coreMatchParticipantsPlayersAccess._players[target].Markers.GetValueOrDefault(PlayerMarkerKind.Baoli) >= 3 || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Xionghuo needs one actual owned pool marker and a real other target.");
        var before = XuRongPool(f.OwnerSeat);
        var targetBefore = _coreMatchParticipantsPlayersAccess._players[target].Markers.GetValueOrDefault(PlayerMarkerKind.Baoli);
        RemoveXuRongTargetMarkers(f, f.OwnerSeat, f.OwnerSeat, 1);
        _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, target, PlayerMarkerKind.Baoli, 1);
        var grant = new XionghuoGrant(XuRongSource(f), f.GameplayHash, f.Id, target, 1);
        _coreMatchParticipantsPlayersAccess._players[target].AddXionghuoGrant(grant);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XionghuoMarkTransferredEvent(f.Id, grant, before, before - 1, targetBefore, targetBefore + 1));
        return SkillProgramStepOutcome.Continue;
    }

    internal SkillProgramStepOutcome SettleXuRong(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } subject } w || subject == f.OwnerSeat || subject != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.InstructionIndex != 1)
            throw new InvalidOperationException("Xionghuo requires the actual marked foreign Play boundary.");
        var grants = XuRongSubjectGrants(f.OwnerSeat, subject).ToArray();
        if (grants.Length == 0 || f.XuRong is not null)
            throw new InvalidOperationException("Xionghuo phase debt has no unpaid source-attributed mark.");
        var before = _randomOperations._random.State;
        var branch = _randomOperations._random.Next(3);
        var after = _randomOperations._random.State;
        foreach (var g in grants)
        {
            _coreMatchParticipantsPlayersAccess._players[subject].RemoveXionghuoGrant(g);
            RemoveXuRongTargetMarkers(f, subject, g.Source.OwnerSeat, g.Count);
        }

        var source = XuRongSource(f);
        var r = new XuRongPending(source, f.GameplayHash, "phase-paid", f.InstructionIndex, w.ParentFrameId, subject, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, Array.AsReadOnly(grants), before, after, branch, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Array.AsReadOnly(Array.Empty<int>()));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XuRong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XionghuoPlayDebtPaidEvent(f.Id, source, f.GameplayHash, w.ParentFrameId, subject, r.ActualTurnNumber, r.PhaseInstanceId, grants.Sum(g => g.Count), before, after, branch));
        if (branch is 0 or 1)
            _coreMatchParticipantsPlayersAccess._players[subject].AddXuRongRestriction(new(source, f.Id, subject, r.ActualTurnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, r.PhaseInstanceId, branch));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XuRong = r with { Stage = branch == 0 ? "fire-native" : branch == 1 ? "hp-native" : "take-native" } });
        if (branch == 0)
        {
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, subject, 1, nature: DamageNature.Fire);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (branch == 1)
        {
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, subject, 1);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        TakeXuRongRandomCards(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void RemoveXuRongTargetMarkers(ProgramSkillFrame f, int subject, int source, int count)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[subject];
        var key = (PlayerMarkerKind.Baoli, source);
        var actual = target.MarkerSourceCounts.GetValueOrDefault(key);
        if (count < 1 || actual < count)
            throw new InvalidOperationException("Xionghuo removed a marker not owned by this actual source.");
        if (actual == count)
            target.MarkerSourceCounts.Remove(key);
        else
            target.MarkerSourceCounts[key] = actual - count;
        var total = target.Markers[PlayerMarkerKind.Baoli] - count;
        if (total == 0)
            target.Markers.Remove(PlayerMarkerKind.Baoli);
        else
            target.Markers[PlayerMarkerKind.Baoli] = total;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, subject, PlayerMarkerKind.Baoli, -count, total, source, "program.xionghuo.consume"));
    }

    internal void TakeXuRongRandomCards(ProgramSkillFrame f)
    {
        var r = f.XuRong!;
        var subject = _coreMatchParticipantsPlayersAccess._players[r.SubjectSeat];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(subject).OrderBy(c => c.Id).ToArray();
        var equip = _corePhysicalOwnedCardQueriesOperations.GetEquipment(subject).OrderBy(c => c.Id).ToArray();
        var ids = new List<int>();
        if (hand.Length > 0)
            ids.Add(hand[_randomOperations._random.Next(hand.Length)].Id);
        if (equip.Length > 0)
            ids.Add(equip[_randomOperations._random.Next(equip.Length)].Id);
        IssueXuRongMove(f, ids, XuRongTakeReason);
    }

    internal SkillProgramStepOutcome ClaimXuRong(ProgramSkillFrame f)
    {
        if (f.XuRong is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering, XuRongDying: { } proof } w || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.Id != proof.DyingFrameId || dying.XuRongProof != proof || proof.VictimSeat == f.OwnerSeat || f.InstructionIndex != 1)
            throw new InvalidOperationException("ShaJue2023 requires the actual other native dying entry and its original frozen rescue requirement.");
        var before = XuRongPool(f.OwnerSeat);
        var gained = Math.Max(0, Math.Min(1, 3 - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.Baoli)));
        if (gained > 0)
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.Baoli, gained);
        var source = XuRongSource(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XionghuoPoolChangedEvent(f.Id, source, before, before + gained, "dying"));
        var r = new XuRongPending(source, f.GameplayHash, "claim-native", f.InstructionIndex, w.ParentFrameId, proof.VictimSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, Array.AsReadOnly(Array.Empty<XionghuoGrant>()), 0, 0, -1, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Array.AsReadOnly(Array.Empty<int>()), proof.DyingFrameId, proof.NativeActionId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XuRong = r });
        var ids = proof.RescueCardsRequired > 1 ? proof.Entities.Where(e => _cardZonesOperations._cardZones.GetLocation(e.CardId) == e.OriginalLocation).Select(e => e.CardId).ToArray() : [];
        if (ids.Length > 0)
            ArmXuRongCausalClaim(f, proof, ids);
        IssueXuRongMove(f, ids, XuRongClaimReason);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ArmXuRongCausalClaim(ProgramSkillFrame f, XuRongDyingProof proof, IReadOnlyList<int> ids)
    {
        if (proof.RescueCardsRequired <= 1 || proof.DamageAttemptResolutionId is not { } nativeId || ids.Any(id => !proof.Entities.Any(e => e.CardId == id && _cardZonesOperations._cardZones.GetLocation(id) == e.OriginalLocation)))
            throw new InvalidOperationException("ShaJue cannot claim unrelated entities or a later dying requirement.");
        var receipt = new XuRongCausalClaimReceipt(f.Id, XuRongSource(f), f.GameplayHash, proof.DyingFrameId, nativeId, proof.NativeActionId, proof.HpAtEntry, proof.RescueCardsRequired, Array.AsReadOnly(ids.ToArray()));
        var native = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == nativeId) ?? throw new InvalidOperationException("ShaJue causal native owner disappeared before payment.");
        if (native is CardUseFrame { Action: { } action } use && action.ActionId == proof.NativeActionId && ids.All(id => action.PhysicalCards.Any(c => c.CardId == id)))
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { XuRongCausalClaims = Array.AsReadOnly((current.XuRongCausalClaims ?? []).Append(receipt).ToArray()) });
        else if (native is JudgmentFrame { Continuation: JudgmentContinuationKind.Lightning, DelayedCardId: { } held } judgment && ids.SequenceEqual(new[] { held }))
            _coreRuntimeOperations.ReplaceRuntimeFrame(judgment.Id, judgment with { XuRongCausalClaims = Array.AsReadOnly((judgment.XuRongCausalClaims ?? []).Append(receipt).ToArray()) });
        else
            throw new InvalidOperationException("ShaJue causal entities do not belong to the exact paid native card or held Lightning.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XuRongCausalClaimIssuedEvent(receipt));
    }

    internal bool IsXuRongCausalEntityClaim(long nativeOwnerId, int cardId)
    {
        var native = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == nativeOwnerId);
        var claims = native switch
        {
            CardUseFrame c => c.XuRongCausalClaims,
            JudgmentFrame j => j.XuRongCausalClaims,
            _ => null
        };
        if (claims is null)
            return false;
        return claims.Any(r => r.NativeOwnerId == nativeOwnerId && r.HpAtEntry < 0 && r.RescueCardsRequired == 1 - r.HpAtEntry && r.RescueCardsRequired > 1 && _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash == r.GameplayHash && r.CardIds.Contains(cardId) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XuRongCausalClaimIssuedEvent>().Count(e => (e.Receipt with { CardIds = r.CardIds }) == r && e.Receipt.CardIds.SequenceEqual(r.CardIds)) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XuRongCardsPaidEvent>().Count(e => e.FrameId == r.ProducerFrameId && e.Source == r.Source && e.Kind == XuRongClaimReason && e.CardIds.SequenceEqual(r.CardIds)) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XuRongCardsPaidEvent>().Single(e => e.FrameId == r.ProducerFrameId && e.Source == r.Source && e.Kind == XuRongClaimReason && e.CardIds.SequenceEqual(r.CardIds))is { } paid && r.CardIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.CardId == id && m.Reason.Value == XuRongClaimReason && m.To == CardLocation.Hand(r.Source.OwnerSeat)) == 1));
    }

    internal void IssueXuRongMove(ProgramSkillFrame f, IReadOnlyList<int> ids, string reason)
    {
        var r = f.XuRong!;
        var paid = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? ids.Where(id => _cardZonesOperations._cardZones.GetLocation(id).Zone is CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Processing or CardZoneKind.Judgment or CardZoneKind.DiscardPile).ToArray() : [];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XuRong = r with { MovementBefore = before, MovementAfter = before, PaidCardIds = Array.AsReadOnly(paid) }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (paid.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(paid, CardLocation.Hand(f.OwnerSeat), new(reason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XuRong = f.XuRong!with { MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XuRongCardsPaidEvent(f.Id, r.Source, reason, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Array.AsReadOnly(paid)));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    SkillProgramStepOutcome IXuRongProgramHost.ExecuteXuRong(SkillProgramEffectOp op, ProgramSkillFrame frame) => ExecuteXuRong(op, frame);
}
