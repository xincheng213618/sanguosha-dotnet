#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhuoMouOlRuntime
{
    internal MouDongZhuoOlRuntime MouDongZhuoOl => _mouDongZhuoOlOperations.Get();

    int IMouDongZhuoOlFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int IMouDongZhuoOlFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    int IMouDongZhuoOlFacts.PlayerCount => _coreMatchDefinitionPlayerCountAccess._playerCount;

    int IMouDongZhuoOlFacts.PhaseId => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;

    long IMouDongZhuoOlFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouDongZhuoOlFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouDongZhuoOlFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouDongZhuoOlFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string IMouDongZhuoOlFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    int IMouDongZhuoOlFacts.Hp(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Hp;
    bool IMouDongZhuoOlFacts.Owns(int seat, string skill) => _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[seat], skill);
    SkillProgram IMouDongZhuoOlFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program!;
    CardLocation IMouDongZhuoOlFacts.Location(int id) => _cardZonesOperations._cardZones.GetLocation(id);
    IReadOnlyList<MouDongZhuoOlCard> IMouDongZhuoOlFacts.Discardable(int seat) => Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, seat)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, new(z, seat), OwnedCardMoveIntent.Discard)).Select(c => new MouDongZhuoOlCard(c.Id, new(z, seat), _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], c)))).ToArray());
    IReadOnlyList<MouDongZhuoOlCard> IMouDongZhuoOlFacts.CurrentDiscard() => Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == c.Id && m.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && m.To == CardLocation.DiscardPile)).Select(c => new MouDongZhuoOlCard(c.Id, CardLocation.DiscardPile, c.Suit)).ToArray());
    IEnumerable<T> IMouDongZhuoOlFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    bool IMouDongZhuoOlFacts.ExactStart(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != op || ((IMouDongZhuoOlFacts)this).Program(f.SkillId).GameplayHash != f.GameplayHash || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            return false;
        if (f.TriggerId is null)
            return op == SkillProgramEffectOp.ResolveMouDongZhuoFengshang && f.SkillId == MouDongZhuoOlRuntime.Fengshang && f.ActivationId == "distribute-pair" && f.WindowContext is null && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && MouDongZhuoOl.CanFengshang(f.OwnerSeat, false);
        return MouDongZhuoOlParent(f) && (op switch
        {
            SkillProgramEffectOp.InitializeMouDongZhuoGuanbian => f.SkillId == MouDongZhuoOlRuntime.Guanbian && f.WindowContext!.Window == SkillProgramTriggerWindow.GameStarting,
            SkillProgramEffectOp.RemoveMouDongZhuoGuanbian => f.SkillId == MouDongZhuoOlRuntime.Guanbian && f.WindowContext!.Window == SkillProgramTriggerWindow.RoundEnding && _coreActualRoundProgressRoundNumberAccess._roundNumber == 1,
            SkillProgramEffectOp.ResolveMouDongZhuoXiongni => f.SkillId == MouDongZhuoOlRuntime.Xiongni && f.WindowContext!.Window == SkillProgramTriggerWindow.PlayPhaseStarting && f.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
            SkillProgramEffectOp.ResolveMouDongZhuoFengshang => f.SkillId == MouDongZhuoOlRuntime.Fengshang && f.WindowContext!.Window == SkillProgramTriggerWindow.DyingEntering && MouDongZhuoOl.CanFengshang(f.OwnerSeat, true),
            _ => f.SkillId == MouDongZhuoOlRuntime.Zhibing && f.WindowContext!.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && f.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat
        });
    }

    bool IMouDongZhuoOlFacts.ParentMatches(ProgramSkillFrame f) => MouDongZhuoOlParent(f);
    int IMouDongZhuoOlFacts.KeepValue(int seat, int id) => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[seat]);
    bool IMouDongZhuoOlFacts.Friendly(int owner, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    internal bool MouDongZhuoOlParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.WindowContext is null && f.ActivationId == "distribute-pair" && f.SkillId == MouDongZhuoOlRuntime.Fengshang;
        if (f.ActivationId != f.TriggerId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1].Id != w.ParentFrameId)
            return false;
        var c = _resolutionStackOperations._resolutionStack[i - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p when p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            PlayPhaseStartingBoundaryFrame p when p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            _ => null
        };
        return c is not null && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }

    void IMouDongZhuoOlNative.SetGuanbian(ProgramSkillFrame f, int amount) => _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.MouDongZhuoGuanbian, f.OwnerSeat, amount, f);
    void IMouDongZhuoOlNative.LoseGuanbian(ProgramSkillFrame f)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var enabled = owner.SkillGrants.Grants.Where(g => g.IsEnabled && g.SkillId == MouDongZhuoOlRuntime.Guanbian).ToArray();
        if (enabled.Length == 0)
            return;
        _coreStrategicProgramsOperations.SetStrategicMarker(f.OwnerSeat, PlayerMarkerKind.MouDongZhuoGuanbian, f.OwnerSeat, 0, f);
        foreach (var grant in enabled)
            owner.SkillGrants.RemoveGrant(grant.GrantId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhuoOlGuanbianEvent(f.Id, f.OwnerSeat, 0, true));
    }

    bool IMouDongZhuoOlNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouDongZhuoOlNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouDongZhuoOlNative.Discard(ProgramSkillFrame f, int payer, MouDongZhuoOlCard card)
    {
        if (!MouDongZhuoOl.Valid(f) || f.MouDongZhuoOl is not { Paid: true } r || r.Payer != payer || !r.Materials.SequenceEqual([card]) || !_characterDongZhuoMouOlOperations.CreateMouDongZhuoOlFacts().Discardable(payer).Contains(card))
            throw new InvalidOperationException("Xiongni requires its actual payable HE entity.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, payer, [card.Id], new(MouDongZhuoOlRuntime.CostReason), (_, records) =>
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
            var paid = parent.MouDongZhuoOl!;
            if (!MouDongZhuoOl.Valid(parent) || records is not [var m] || m.CardId != card.Id || m.From != card.From || m.Sequence <= paid.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Xiongni's real discard return changed its paid material.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouDongZhuoOl = paid with { After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhuoOlPaidEvent(f.Id, payer, card.Id, card.From, card.Suit, m.Sequence));
        });
    }

    void IMouDongZhuoOlNative.Give(ProgramSkillFrame f, int id, int recipient)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.Hand(recipient), new(MouDongZhuoOlRuntime.GiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, [id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: CardMovementDisclosure.FaceUp);
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
        var paid = parent.MouDongZhuoOl!;
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouDongZhuoOl = paid with { Before = before, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.CardId == id && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(recipient)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhuoOlGiftEvent(f.Id, id, recipient, m.Sequence));
    }

    void IMouDongZhuoOlNative.Damage(ProgramSkillFrame f, int target) => _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1);
    bool IMouDongZhuoOlNative.BeginWine(ProgramSkillFrame f) => BeginMouDongZhuoOlWine(f);
    void IMouDongZhuoOlNative.MaximumHp(ProgramSkillFrame f) => _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 1);
    void IMouDongZhuoOlNative.Recover(ProgramSkillFrame f) => _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 1, null, null);
    void IMouDongZhuoOlNative.Grant(ProgramSkillFrame f, string skill) => _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, [skill]);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 34700)]
    internal void ObserveMouDongZhuoOlUse(IGameEvent payload)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouDongZhuoZhibing))
            return;
        CardActionContext? action = null;
        long frameId = 0;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { Type: CardActionType.Use } a } use && use.SourceSeat == declared.SourceSeat && use.CardId == declared.CardId && use.CardKind == declared.CardKind && a.ActorSeat == use.SourceSeat && a.EffectiveKind == use.CardKind)
        {
            action = a;
            frameId = use.Id;
        }
        else if (payload is CardActionAcceptedEvent { Action.Type: CardActionType.Response } accepted)
        {
            // The earlier shared observer has already published the exact
            // Slash-defense Dodge / Nullification true-Use fact. Pure played
            // responses never get that fact and cannot add to this total.
            var proof = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().LastOrDefault(e => e.CardActionId == accepted.Action.ActionId && e.ActorSeat == accepted.Action.ActorSeat && e.RoundActionType == CardActionType.Response);
            if (proof is not null)
            {
                action = accepted.Action;
                frameId = proof.OriginFrameId;
            }
        }

        if (action is null || action.ActionId <= 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouDongZhuoOlBlackUseEvent>().Any(e => e.ActionId == action.ActionId))
            return;
        var suit = action.EffectiveSuit ?? (action.PhysicalCards is [var card] ? _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], _coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId)) : Suit.None);
        var black = suit != Suit.None && (action.EffectiveIsRed is { } red ? !red : suit is Suit.Spade or Suit.Club);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhuoOlBlackUseEvent(action.ActorSeat, action.FactionOrigin?.ActorFactionId ?? _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]), frameId, action.ActionId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, black));
    }
}
