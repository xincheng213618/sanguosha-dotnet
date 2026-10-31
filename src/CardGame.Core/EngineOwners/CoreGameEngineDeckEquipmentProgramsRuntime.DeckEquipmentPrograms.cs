#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeckEquipmentProgramsRuntime
{
    SkillProgramStepOutcome IDeckEquipmentProgramHost.UseRandomDeckEquipment(ProgramSkillFrame f, int seat, string bind) => UseProgramRandomDeckEquipment(f, seat, bind);
    void IDeckEquipmentProgramHost.GrantRandomSkillAndSuitShield(ProgramSkillFrame f, int seat, IReadOnlyList<string> skills, Suit suit) => GrantProgramRandomSkillAndSuitShield(f, seat, skills, suit);
    internal bool IsShieldedRescueSelection(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && HasBeneficiarySuitShield(owner.Seat, dying.VictimSeat, _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) ? Suit.None : selection.Cards.Select(card => _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)).Distinct().ToArray()is [var suit] ? suit : null);
    internal bool HasBeneficiarySuitShield(int actor, int target, Suit? suit) => actor != target && suit is { } actual && (_coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShields.Any(s => s.BeneficiarySeat == target && s.Suit == actual) || _characterDuXiOlOperations.IsDuXiSuitTargetProhibited(actor, target, actual));
    internal IReadOnlyList<LegalAction> FilterBeneficiarySuitShieldActions(CharacterState actor, IReadOnlyList<LegalAction> actions)
    {
        if (_coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShields.Count == 0 && !_characterDuXiOlOperations.HasAnyDuXiSuitProhibition())
            return actions;
        return actions.Where(action =>
        {
            if (action.Kind is LegalActionKind.Recast || action.CardId is not { } id)
                return true;
            var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id);
            var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(actor, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, card, action.ConversionSource));
            var targets = action.Kind == LegalActionKind.BorrowedSword ? action.TargetSeats.Where((_, index) => index % 2 == 0) : _coreDirectedCardPolicyQueriesOperations.GetDeclaredCardTargets(actor, action.Kind, action.TargetSeats);
            return action.Kind is LegalActionKind.BarbarianAssault or LegalActionKind.ArrowBarrage or LegalActionKind.PeachGarden or LegalActionKind.FiveGrains ? targets.Any(t => !HasBeneficiarySuitShield(actor.Seat, t, suit)) : targets.All(t => !HasBeneficiarySuitShield(actor.Seat, t, suit));
        }).ToArray();
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 200)]
    internal void ObserveBeneficiarySuitShield(IGameEvent payload)
    {
        if (payload is TurnStartedEvent turn)
            _coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShields.RemoveAll(s => s.BeneficiarySeat == turn.ActorSeat);
    }

    internal SkillProgramStepOutcome UseProgramRandomDeckEquipment(ProgramSkillFrame frame, int targetSeat, string bind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        var instruction = ProgramInstructionResolver.Default.Resolve(active, _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!).GetPausedInstruction(active.InstructionIndex).Effect;
        var ownRecipient = instruction.TargetReference?.Kind == ProgramParticipantRef.Owner && targetSeat == active.OwnerSeat;
        if (!ownRecipient && (active.SelectedTargetSeats is not [var selected] || selected != targetSeat))
            throw new InvalidOperationException("Random equipment lost its exact selected participant.");
        var candidates = target.IsAlive ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && target.EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(targetSeat, c.Kind, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(target, c.Kind, [targetSeat]) && !_coreResponseEntityExchangeOperations.IsResponseEntityRestricted(targetSeat, c.Id) && !_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(target, c)).OrderBy(c => c.Id).ToArray() : [];
        if (candidates.Length == 0)
        {
            _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, bind, [], SkillProgramCardSetVisibility.Public, []);
            return SkillProgramStepOutcome.Continue;
        }

        var card = candidates[_randomOperations._random.Next(candidates.Length)];
        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, bind, [card.Id], SkillProgramCardSetVisibility.Public, [CardLocation.DrawPile], _coreNativeCardAppearanceOperations.EffectiveSuit(target, card));
        active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PendingMovementContinuation = new ProgramMovementContinuation(targetSeat, 0, null) });
        var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, targetSeat, [], conversionSource: source);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.EquipmentUse);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(target, card, id))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(target, card, id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [NativeUseReturn(130)]
    internal void ContinueProgramAfterRandomEquipmentUse()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.PendingMovementContinuation is null)
            return;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (frame.InstructionIndex == 0 || plan.Instructions[frame.InstructionIndex - 1].Op != SkillProgramEffectOp.UseRandomDeckEquipment)
            return;
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
    }

    internal bool RandomEquipmentFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath)
    {
        if (ride is CardsMovedTriggerWindowFrame returned && beneath is ProgramSkillFrame returnedProducer && ReturnedRandomEquipmentMovementMatches(returnedProducer, returned))
            return true;
        if (_characterMaChaoAnniversaryGodOperations.IsShouliStartingEquipmentRide(ride, beneath))
            return true;
        if (_characterXiangChongOlOperations.IsXiangChongGuyingEquipmentRide(ride, beneath) || _characterYangFuOlOperations.IsSuyiEquipmentRide(ride, beneath))
            return true;
        if (_characterHuoJunOlOperations.IsHuoJunFenruiNativeEquipment(ride, beneath))
            return true;
        if (_characterGuoZhaoAnniversaryOperations.IsAnniversaryGuoZhaoNativeEquipment(ride, beneath))
            return true;
        if (_coreFriendTrioOperations.FriendXuanjianFrameRidesOn(ride, beneath))
            return true;
        if (ride is ProgramSkillFrame { WindowContext: { } observer } && observer.ParentFrameId == beneath.Id && beneath is HpChangedTriggerWindowFrame or CardsMovedTriggerWindowFrame)
        {
            var resume = beneath switch
            {
                HpChangedTriggerWindowFrame hp => hp.ResumeFrameId,
                CardsMovedTriggerWindowFrame movement => movement.Batch.AwaitingProgramFrameId ?? movement.Batch.ParentFrameId,
                _ => null
            };
            var producer = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == resume);
            if (producer?.TriggerId is { } binding && ProgramInstructionResolver.Default.Resolve(_contentRegistryOperations._contentRegistry.GetSkill(producer.SkillId).Program!, ProgramInstructionSourceKind.Trigger, binding).Trigger is { NoDyingAtActivation: true } trigger && ProgramInstructionResolver.Default.Features(trigger).HasEquipmentObserverInterruptOptIn)
                return observer.Window == (beneath is HpChangedTriggerWindowFrame ? SkillProgramTriggerWindow.AfterHpRecovered : SkillProgramTriggerWindow.CardsMoved);
        }

        if (ride is ProgramSkillFrame { WindowContext: { } context } && beneath is ProgramCardTriggerWindowFrame window)
            return context.ParentFrameId == window.Id && context.CardUse?.ParentCardUseFrameId == window.ParentFrameId && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(window.Action.EffectiveKind);
        if (ride is ProgramCardTriggerWindowFrame cardWindow && beneath is CardUseFrame use)
            return cardWindow.ParentFrameId == use.Id && cardWindow.Action.ActionId == use.Action?.ActionId && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.CardKind);
        if (ride is not CardUseFrame equipment || beneath is not ProgramSkillFrame parent || parent.PendingMovementContinuation is not { } pending || equipment.Action is not { } action)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(parent, _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!);
        if (parent.InstructionIndex == 0 || plan.Instructions[parent.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.UseRandomDeckEquipment, ResultBind: { } bind } effect)
            return false;
        var target = effect.TargetReference?.Kind == ProgramParticipantRef.Owner ? parent.OwnerSeat : parent.SelectedTargetSeats.Count == 1 ? parent.SelectedTargetSeats[0] : -1;
        if (target < 0 || pending.SubjectSeat != target || action.ActorSeat != target || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(action.EffectiveKind) || parent.CardSetBindings.SingleOrDefault(b => b.Name == bind)is not { CardIds: [var cardId] } || cardId != equipment.CardId)
            return false;
        return action.ConversionChain.Any(source => source.OwnerSeat == parent.OwnerSeat && source.SkillId == parent.SkillId && source.BindingId == CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent) && source.SkillInstanceId == parent.SkillInstanceId);
    }

    internal bool ReturnedRandomEquipmentMovementMatches(ProgramSkillFrame parent, CardsMovedTriggerWindowFrame moved)
    {
        var batch = moved.Batch;
        if (parent.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || parent.InstructionIndex < 1 || moved.Id != batch.Id || batch.ParentFrameId is not { } useId || useId <= parent.Id || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not null || batch.AwaitingProgramFrameId != parent.Id || moved.ResumeProgramFrameId is { } resume && resume != parent.Id || RuntimeSkills.RuntimeRules_Core_CommonRules.HasAlternateProgramMovementReturn(moved) || batch.OriginOwnerSeat != parent.OwnerSeat || batch.OriginSkillId != parent.SkillId || batch.OriginSkillInstanceId != parent.SkillInstanceId || batch.TurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || batch.Movements.Count == 0 || batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m)))
            return false;
        var definition = _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program!;
        if (definition.GameplayHash != parent.GameplayHash)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(parent, definition);
        if (plan.Instructions[parent.InstructionIndex - 1] is not { Op: SkillProgramEffectOp.UseRandomDeckEquipment, ResultBind: { } bind } effect || !_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().EvaluateCondition(parent, effect.Condition, _coreNativeSkillContextQueriesOperations.CreateSkillContext(_coreMatchParticipantsPlayersAccess._players[parent.OwnerSeat])))
            return false;
        var target = effect.TargetReference?.Kind == ProgramParticipantRef.Owner ? parent.OwnerSeat : parent.SelectedTargetSeats is [var selected] ? selected : -1;
        if (target < 0 || pending.SubjectSeat != target || parent.CardSetBindings.SingleOrDefault(b => b.Name == bind)is not { CardIds: [var cardId], SourceLocations: [var source] } || source != CardLocation.DrawPile)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var declarations = history.OfType<CardUseDeclaredEvent>().Where(e => e.ResolutionId == useId).ToArray();
        var completions = history.OfType<CardUseFinishedEvent>().Where(e => e.ResolutionId == useId).ToArray();
        var equipmentChanges = history.OfType<EquipmentChangedEvent>().Where(e => e.ResolutionId == useId).ToArray();
        if (declarations is not [var declared] || declared.CardId != cardId || declared.SourceSeat != target || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(declared.CardKind) || completions is not [var completed] || completed.CardId != cardId || completed.CardKind != declared.CardKind || equipmentChanges is not [var changed] || changed.PlayerSeat != target || changed.CardId != cardId || changed.CardKind != declared.CardKind)
            return false;
        var declarationIndex = Array.IndexOf(history, declared);
        var completionIndex = Array.IndexOf(history, completed);
        if (completionIndex <= declarationIndex || history.Skip(declarationIndex + 1).Take(completionIndex - declarationIndex - 1).OfType<CardMovedEvent>().Count(e => e.CardId == cardId && e.From == CardLocation.DrawPile && e.To == CardLocation.Processing && e.Reason == CardMoveReasons.EquipmentUse) != 1)
            return false;
        // The native use has left the stack; only its exact paid entry/replacement batch may return to this instruction.
        return batch.Movements.All(m => m.CardId == cardId && m.From == CardLocation.Processing && m.To == CardLocation.Equipment(target) && m.Reason == CardMoveReasons.EquipmentEnter || m.CardId == changed.ReplacedCardId && m.From == CardLocation.Equipment(target) && m.Reason == CardMoveReasons.EquipmentReplace && (m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), m.From, _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile) || _coreProgramAtomicCardMovesOperations.IsRedirectedPaidDiscardMovement(m, m.From, CardMoveReasons.EquipmentReplace)) || changed.ReplacedCardId is { } replaced && _coreAdvancedLifecycleOperations.GetAdvancedCard(replaced).Kind == CardKind.WoodenOx && m.From == CardLocation.WoodenOxGrain(target) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard);
    }

    internal void GrantProgramRandomSkillAndSuitShield(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<string> skills, Suit suit)
    {
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.OwnerDied || frame.SelectedTargetSeats is not [var selected] || selected != targetSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            throw new InvalidOperationException("Random death bequest lost its living beneficiary.");
        var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var chosen = skills[_randomOperations._random.Next(skills.Count)];
        _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[targetSeat], $"bequest:{(frame.OwnerSeat)}:{(frame.SkillInstanceId)}:{(frame.Id)}", [chosen]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RandomSkillGrantedEvent(targetSeat, chosen, source));
        var shield = new BeneficiarySuitShield(++_coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShieldSequence, targetSeat, suit, source);
        _coreGameEngineDeckEquipmentProgramsBeneficiarySuitShieldSequenceBeneficiarySuitShieldsAccess._beneficiarySuitShields.Add(shield);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BeneficiarySuitShieldGrantedEvent(shield));
    }
}
