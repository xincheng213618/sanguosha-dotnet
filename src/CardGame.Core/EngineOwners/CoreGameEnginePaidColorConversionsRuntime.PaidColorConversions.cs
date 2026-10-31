#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePaidColorConversionsRuntime
{
    internal const string PaidColorDiscardReason = "program.paid-color-conversion.discard";
    internal static bool HasActualRedOrBlackSuit(Suit suit) => RuntimeSkills.RuntimeRules_Core_PaidColorConversions.HasActualRedOrBlackSuit(suit);
    internal bool MatchesPaidColorDrawOwner(ProgramSkillFrame root)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame parent && parent.Window == SkillProgramTriggerWindow.DrawPhaseEnded && parent.OwnerSeat == root.OwnerSeat && root.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 && root.WindowContext is { Window: SkillProgramTriggerWindow.DrawPhaseEnded } c && c.ParentFrameId == parent.Id && c.OwnerSeat == root.OwnerSeat && c.SourceSeat == root.OwnerSeat && c.TargetSeat == root.OwnerSeat && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, parent.Candidates[parent.CandidateIndex]) && c.OccurrenceIndex == parent.Candidates[parent.CandidateIndex].OccurrenceIndex;
    }

    internal SkillProgramStepOutcome BeginPaidColorConversion(ProgramSkillFrame frame, string bind)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.PaidColorConversion is not null || !MatchesPaidColorDrawOwner(frame))
            throw new InvalidOperationException("A paid color conversion requires its original completed-draw candidate exactly once.");
        var cards = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId) || cards.CardIds is not [var cardId] || cards.SourceLocations is not [var from] || from.OwnerSeat != owner.Seat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _cardZonesOperations._cardZones.GetLocation(cardId) != from)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "技能来源或原弃牌付款失效，未支付且不授予转换。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == cardId);
        var sequence = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 1 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence + 1;
        var origin = new ProgramPaidColorConversionOrigin(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frame.GameplayHash, cardId, card.Kind, from, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card), sequence, sequence, card.IsGeneralWeapon);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PaidColorConversion = new(frame.InstructionIndex, bind, origin), PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, frame.OwnerSeat, card, from, new(PaidColorDiscardReason));
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        origin = origin with
        {
            SequenceAfter = _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PaidColorConversion = frame.PaidColorConversion!with { Origin = origin } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPaidColorConversionPaidEvent(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), owner.Seat, frame.SkillInstanceId), frame.InstructionIndex - 1, origin));
        if (AwaitPaidColorDamageClaimMovements(frame.Id) != SkillProgramStepOutcome.Continue)
            return SkillProgramStepOutcome.AwaitChild;
        return ResumePaidColorDamageClaims(frame.Id) ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 760)]
    internal bool ResumePaidColorDamageClaims(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.PaidColorConversion is null && f.ActualTurnDamageClaim is null)
            return false;
        if (AwaitPaidColorDamageClaimMovements(id) != SkillProgramStepOutcome.Continue)
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.PaidColorConversion is { Granted: false } paid)
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            var p = paid.Origin;
            if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && owner.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId) && p.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && HasActualRedOrBlackSuit(p.EffectiveSuit))
            {
                var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantConversion(p.ActualTurnNumber, p.TurnOwnerSeat, f.Id, paid.InstructionIndex - 1, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), paid.SourceBind, SkillProgramCardColorRelation.OppositeBoundCard, CoreNativeCardClassificationsRuntime.IsRedSuit(p.EffectiveSuit), CardKind.Duel, p);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardConversionGrantedEvent(grant));
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PaidColorConversion = paid with { Granted = true } });
        }

        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), "仅结清实际付款或领取的子链，取消死亡或胜负已定的未发收益。");
            return true;
        }

        return false;
    }

    internal SkillProgramStepOutcome AwaitPaidColorDamageClaimMovements(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (frame.PendingMovementContinuation is null)
            return SkillProgramStepOutcome.Continue;
        // Equipment removal can queue Jiuyuan before the ordinary HP/movement windows.
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return SkillProgramStepOutcome.AwaitChild;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        return SkillProgramStepOutcome.Continue;
    }

    internal IReadOnlyList<CardConversionSource> GetPaidColorTurnDuelConversions(CharacterState owner, Card card, CardKind kind, bool forResponse, CardZoneKind zone)
    {
        if (!owner.IsAlive || forResponse || kind != CardKind.Duel || card.IsGeneralWeapon || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !HasActualRedOrBlackSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)))
            return[];
        var color = CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card));
        return _turnCardUseEffectsOperations._turnCardUseEffects.Conversions.Where(g => g.PaidColorOrigin is not null && g.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && g.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && g.Source.OwnerSeat == owner.Seat && g.OutputKind == CardKind.Duel && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, g.Source.SkillId, g.Source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program?.GameplayHash == g.PaidColorOrigin.GameplayHash).GroupBy(g => (g.Source.SkillId, g.Source.SkillInstanceId)).Select(group => group.OrderByDescending(g => g.GrantSequence).First()).Where(g => g.BoundCardIsRed != color).Select(g => new CardConversionSource(g.Source.SkillId, $"{(g.Source.BindingId)}.turn-{(g.EffectIndex)}", g.Source.OwnerSeat, g.Source.SkillInstanceId)).Distinct().ToArray();
    }

    [ProgramCardAction(ProgramCardActionPhase.AfterProgram, 40)]
    internal void AddPaidColorEquipmentDuelActions(List<LegalAction> actions, CharacterState owner, int? selectedPhysicalCardId = null)
    {
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => c.Kind == CardKind.Duel)).Where(c => selectedPhysicalCardId is null || c.Id == selectedPhysicalCardId).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, c) && !_coreCardConversionsOperations.HasProgramCardIdentity(owner, c)))
            foreach (var source in GetPaidColorTurnDuelConversions(owner, card, CardKind.Duel, false, _cardZonesOperations._cardZones.GetLocation(card.Id).Zone))
                foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner.Seat && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, CardKind.Duel, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card), CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)))))
                    actions.Add(new LegalAction(LegalActionKind.Duel, card.Id, target.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"将【{(card.DisplayName)}】当作【决斗】对 {(target.Name)} 使用"), CardKind.Duel) { ConversionSource = source });
    }

    internal bool IsPaidColorEquipmentDuelAction(CharacterState owner, LegalAction action, Card card) => action.Kind == LegalActionKind.Duel && action.PlayedCardKind == CardKind.Duel && action.ConversionSource is { } source && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Equipment(owner.Seat) && GetPaidColorTurnDuelConversions(owner, card, CardKind.Duel, false, CardZoneKind.Equipment).Contains(source);
    internal bool TracksPaidColorUseAppearance(int actorSeat, CardKind kind, IReadOnlyList<CardConversionSource> chain) => kind == CardKind.Duel && _turnCardUseEffectsOperations._turnCardUseEffects.Conversions.Any(g => g.PaidColorOrigin is not null && g.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && g.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && g.Source.OwnerSeat == actorSeat && chain.Any(source => source.OwnerSeat == actorSeat && source.SkillId == g.Source.SkillId && source.SkillInstanceId == g.Source.SkillInstanceId && source.BindingId == $"{(g.Source.BindingId)}.turn-{(g.EffectIndex)}"));

    internal bool IsActualPaidColorMovement(int owner, ProgramPaidColorConversionOrigin p) => p.From.OwnerSeat == owner && p.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence == p.MovementSequence && m.CardId == p.CardId && m.From == p.From && m.CardKind == p.CardKind && m.Reason.Value == PaidColorDiscardReason && (m.To == CardLocation.DiscardPile && !p.GeneratedMaterial || p.GeneratedMaterial && p.From.Zone == CardZoneKind.Equipment && m.To == CardLocation.OutsideGame)) == 1 && p.SequenceAfter >= p.MovementSequence && _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > p.MovementSequence && m.Sequence <= p.SequenceAfter).All(m => p.CardKind == CardKind.WoodenOx && p.From == CardLocation.Equipment(owner) && m.From == CardLocation.WoodenOxGrain(owner) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard);
    SkillProgramStepOutcome IPaidColorDamageClaimProgramHost.DiscardBoundCardForOppositeTurnDuel(ProgramSkillFrame f, string bind) => BeginPaidColorConversion(f, bind);
    SkillProgramStepOutcome IPaidColorDamageClaimProgramHost.ClaimActualTurnDamageEntities(ProgramSkillFrame f) => _coreActualTurnDamageClaimsOperations.BeginActualTurnDamageClaim(f);
}
