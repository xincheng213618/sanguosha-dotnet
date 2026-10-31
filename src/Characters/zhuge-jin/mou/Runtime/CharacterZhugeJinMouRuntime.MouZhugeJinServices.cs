#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeJinMouRuntime
{
    internal MouZhugeJinRuntime MouZhugeJin => _mouZhugeJinOperations.Get();

    internal bool MouZhugeJinCooling(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhugeJinCooldownIssuedEvent>().Any(e => e.Source.OwnerSeat == owner && e.DueRound > _coreActualRoundProgressRoundNumberAccess._roundNumber);
    internal bool HasMouZhugeJinZijin(int seat) => _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[seat]).Any(p => p.Id == MouZhugeJinComposition.Zijin);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 35600, SkillProgramEffectOp.ResolveMouZhugeJinTaozhou)]
    internal bool MouZhugeJinCanActivate(ProgramActivationLegalityContext c) => c.SkillId == MouZhugeJinComposition.Taozhou && c.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == c.OwnerSeat && !MouZhugeJinCooling(c.OwnerSeat) && _characterZhugeJinMouOperations.CreateMouZhugeJinFacts().Targets(c.OwnerSeat).Count > 0;
    int IMouZhugeJinFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    int IMouZhugeJinFacts.TurnOwner => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    int IMouZhugeJinFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    long IMouZhugeJinFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouZhugeJinFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouZhugeJinFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool IMouZhugeJinFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    bool IMouZhugeJinFacts.Friendly(int owner, int other) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    int IMouZhugeJinFacts.Hp(int s) => _coreMatchParticipantsPlayersAccess._players[s].Hp;
    string IMouZhugeJinFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    SkillProgram? IMouZhugeJinFacts.Program(string s) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(s)?.Program;
    IEnumerable<T> IMouZhugeJinFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<int> IMouZhugeJinFacts.Targets(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0 && !HasMouZhugeJinZijin(p.Seat)).Select(p => p.Seat).Order().ToArray();
    IReadOnlyList<MouZhugeJinCard> IMouZhugeJinFacts.Hand(int s) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Select(c => new MouZhugeJinCard(c.Id, CardLocation.Hand(s), c.DisplayName)).ToArray());
    IReadOnlyList<MouZhugeJinCard> IMouZhugeJinFacts.Costs(int chooser, int holder) => !((IMouZhugeJinFacts)this).Alive(chooser) || !((IMouZhugeJinFacts)this).Alive(holder) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, holder)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, c, new(zone, holder), OwnedCardMoveIntent.Discard) && (chooser != holder || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(chooser, c, new(zone, holder), OwnedCardMoveIntent.Discard))).Select(c => new MouZhugeJinCard(c.Id, new(zone, holder), c.DisplayName))).ToArray());
    MouZhugeJinUse? IMouZhugeJinFacts.Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w, SkillProgramEffectOp op)
    {
        var result = MouZhugeJinFacts_NativeUse(c, w);
        if (result is null)
            return null;
        var action = result.Action;
        if (op == SkillProgramEffectOp.ResolveMouZhugeJinZijin)
            return w.Window == SkillProgramTriggerWindow.CardUseCompleted && action.ActorSeat == c.OwnerSeat && w.Facts?.CardUseCausedDamage == false && ((IMouZhugeJinFacts)this).History<CurrentTurnCardUseKindsRecordedEvent>().Any(p => p.CardActionId == action.ActionId && p.ActorSeat == c.OwnerSeat && p.WholeActualTurnUseSequence is> 0) ? result : null;
        if (op != SkillProgramEffectOp.ResolveMouZhugeJinHoude || w.Window != SkillProgramTriggerWindow.CardUseBeforeTargetEffects || result.UseId == 0 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == c.OwnerSeat || action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || !action.TargetSeats.Contains(c.OwnerSeat) || !NativeOwners.MouZhugeJinFacts.HoudeKind(action, result.Red))
            return null;
        var history = _corePlayCardHistoryOperations.EventsSinceLastBoundary(p => p is PhaseChangedEvent or TurnStartedEvent or TurnEndedEvent);
        var first = history.Select(p => p switch
        {
            CardUseAppearanceCapturedEvent appearance => appearance.Action,
            CardActionAcceptedEvent accepted => accepted.Action,
            _ => null
        }).OfType<CardActionContext>().Append(action).Where(a => a.ActorSeat == action.ActorSeat && a.TargetSeats.Contains(c.OwnerSeat) && NativeOwners.MouZhugeJinFacts.HoudeKind(a, result.Red)).DistinctBy(a => a.ActionId).MinBy(a => a.ActionId);
        return first?.ActionId == action.ActionId ? result : null;
    }

    bool IMouZhugeJinFacts.Parent(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (op == SkillProgramEffectOp.ResolveMouZhugeJinTaozhou)
            return f.SkillId == MouZhugeJinComposition.Taozhou && f.ActivationId == "secret-number-and-give" && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && ((IMouZhugeJinFacts)this).History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.SkillId == f.SkillId && p.OwnerSeat == f.OwnerSeat && p.ActivationId == f.ActivationId) == 1;
        if (f.WindowContext is not { } w || f.TriggerId != f.ActivationId || f.SkillId != (op == SkillProgramEffectOp.ResolveMouZhugeJinHoude ? MouZhugeJinComposition.Houde : MouZhugeJinComposition.Zijin) || w.Window != (op == SkillProgramEffectOp.ResolveMouZhugeJinHoude ? SkillProgramTriggerWindow.CardUseBeforeTargetEffects : SkillProgramTriggerWindow.CardUseCompleted))
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return n > 0 && _resolutionStackOperations._resolutionStack[n - 1] is ProgramCardTriggerWindowFrame window && window.Id == w.ParentFrameId && MouZhugeJinFacts_NativeUse(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), w)is not null;
    }

    internal MouZhugeJinUse? MouZhugeJinFacts_NativeUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } card || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != card.CardActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } current || current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash)
            return null;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == window.Action.ActionId);
        return new(window.Id, use?.Id ?? 0, window.Action, window.Action.EffectiveIsRed == true);
    }

    internal static bool MouZhugeJinFacts_HoudeKind(CardActionContext a, bool red) => a.EffectiveIsRed == red && (red ? CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) : CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(a.EffectiveKind));
    void IMouZhugeJinNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouZhugeJinNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IMouZhugeJinNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouZhugeJinNative.Give(ProgramSkillFrame f)
    {
        if (!MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Stage: MouZhugeJinStage.GiveChildren, Committed: true } r || r.Cards.Count == 0 || r.Cards.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Taozhou requires the giver's committed current Hand entities.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.Hand(f.OwnerSeat), new(MouZhugeJinRuntime.GiveReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, r.Target, f.OwnerSeat, r.Cards, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = current.MouZhugeJin!;
            if (!MouZhugeJin.Valid(current) || moves.Count != r.Cards.Count || moves.Any(m => !r.Cards.Contains(m.CardId) || m.From != CardLocation.Hand(r.Target) || m.Sequence <= paid.Before || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Taozhou cannot substitute a primary real gift receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, current with { MouZhugeJin = paid with { Given = moves.Count(m => m.To == CardLocation.Hand(f.OwnerSeat)), After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves.Where(m => m.To == CardLocation.Hand(f.OwnerSeat)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhugeJinGivenEvent(f.Id, r.Target, f.OwnerSeat, m.CardId, m.Sequence));
        }));
    }

    void IMouZhugeJinNative.Discard(ProgramSkillFrame f)
    {
        if (!MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Stage: MouZhugeJinStage.CostChildren, Paying: { } card } r || !_characterZhugeJinMouOperations.CreateMouZhugeJinFacts().Costs(f.OwnerSeat, card.From.OwnerSeat!.Value).Contains(card))
            throw new InvalidOperationException("Houde/Zijin requires its currently payable actual HE cost.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [card.Id], new(MouZhugeJinRuntime.DiscardReason), (_, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = current.MouZhugeJin!;
            if (!MouZhugeJin.Valid(current) || paid.Paying != card || moves is not [var m] || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.CardId != card.Id || m.From != card.From || m.Sequence <= paid.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Houde/Zijin lost its original primary logical discard receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, current with { MouZhugeJin = paid with { CostPaid = true, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhugeJinCostPaidEvent(f.Id, f.OwnerSeat, card.From.OwnerSeat!.Value, card.Id, m.Sequence));
        });
    }

    void IMouZhugeJinNative.Lose(ProgramSkillFrame f)
    {
        if (!MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Operation: SkillProgramEffectOp.ResolveMouZhugeJinZijin, Stage: MouZhugeJinStage.LossChildren })
            throw new InvalidOperationException("Zijin requires the accepted native undamaging-use loss branch.");
        _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, 1);
    }

    void IMouZhugeJinNative.Nullify(ProgramSkillFrame f)
    {
        if (!MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Operation: SkillProgramEffectOp.ResolveMouZhugeJinHoude, CostPaid: true })
            throw new InvalidOperationException("Houde cannot nullify before paying its true discard.");
        _coreProgramCardTargetEffectsOperations.NullifyCurrentProgramCardEffect(f);
    }

    void IMouZhugeJinNative.Grant(ProgramSkillFrame f)
    {
        if (!MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Stage: MouZhugeJinStage.GrantChildren } r || r.SecretNumber - r.Given < 2 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhugeJinZijinGrantedEvent>().Any(p => p.FrameId == f.Id))
            throw new InvalidOperationException("Taozhou grants derived Zijin only after its exact committed actual-gift shortfall.");
        _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[r.Target], $"taozhou:{(f.OwnerSeat)}:{(f.SkillInstanceId)}:{(f.Id)}", [MouZhugeJinComposition.Zijin]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhugeJinZijinGrantedEvent(f.Id, r.Target));
    }

    MouZhugeJinCard IMouZhugeJinNative.RandomHand(ProgramSkillFrame f, IReadOnlyList<MouZhugeJinCard> cards)
    {
        if (!MouZhugeJin.Valid(f) || f.MouZhugeJin is not { Operation: SkillProgramEffectOp.ResolveMouZhugeJinHoude, Red: false } r || cards.Count == 0 || cards.Any(c => c.From != CardLocation.Hand(r.Target)))
            throw new InvalidOperationException("Houde's hidden Hand choice requires the original other actor's actual Hand.");
        var before = _randomOperations._random.State;
        var card = cards[_randomOperations._random.Next(cards.Count)];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhugeJinHandChosenEvent(f.Id, card.Id, before, _randomOperations._random.State, cards.Count));
        return card;
    }

    internal IEnumerable<MouZhugeJinBudgetIssuedEvent> MouZhugeJinBudgets(int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhugeJinBudgetIssuedEvent>().Where(e => e.Target == target && e.Count > 0 && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhugeJinBudgetHitEvent>().Count(hit => hit.FrameId == e.FrameId) < e.Count);
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 35600)]
    internal int MouZhugeJinDamageBonus(GameEngine.IDamageAttempt a) => MouZhugeJinBudgets(a.TargetSeat).Count();
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 35600)]
    internal bool MouZhugeJinDamageApplied(GameEngine.IDamageAttempt a, long id)
    {
        if (!a.DamageWasApplied || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == id)is not DamageFrame { Amount: > 0 } damage || damage.TargetSeat != a.TargetSeat)
            return false;
        foreach (var grant in MouZhugeJinBudgets(a.TargetSeat).ToArray())
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhugeJinBudgetHitEvent>().Any(hit => hit.FrameId == grant.FrameId && hit.DamageId == id))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhugeJinBudgetHitEvent(grant.FrameId, id, a.TargetSeat));
        return false;
    }
}
