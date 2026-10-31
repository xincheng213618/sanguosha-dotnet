#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiMouRuntime
{
    internal MouSimaYiRuntime MouSimaYi => _mouSimaYiOperations.Get();

    int IMouSimaYiFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    int IMouSimaYiFacts.TurnOwner => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    int IMouSimaYiFacts.PhaseId => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;

    long IMouSimaYiFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouSimaYiFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouSimaYiFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool IMouSimaYiFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    string IMouSimaYiFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    ContentSkillDefinition IMouSimaYiFacts.Skill(string s) => _contentRegistryOperations._contentRegistry.GetSkill(s);
    IEnumerable<T> IMouSimaYiFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    bool IMouSimaYiFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouSimaYiComposition.Quanmou && f.ActivationId == "in-range-give" && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && ((IMouSimaYiFacts)this).History<ProgramSkillStartedEvent>().Count(p => p.FrameId == f.Id && p.SkillId == f.SkillId && p.OwnerSeat == f.OwnerSeat && p.ActivationId == f.ActivationId) == 1;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (n < 1 || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack[n - 1].Id != w.ParentFrameId)
            return false;
        return _resolutionStackOperations._resolutionStack[n - 1] switch
        {
            ProgramCardTriggerWindowFrame p => p.MouSimaYiPrivate is not null && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && p.Candidates[p.CandidateIndex] is { } c && c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash && ValidMouSmPrivateWindow(p),
            BeforeDamageProgramWindowFrame p => p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex].Candidate),
            DamageTriggerWindowFrame p => _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(f, p),
            ProgramLifecycleTriggerWindowFrame p => p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]),
            _ => false
        };
    }

    bool IMouSimaYiFacts.ActualPlay(int s) => s == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == s && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p);
    bool IMouSimaYiFacts.Yin(int s, string instance) => ((IMouSimaYiFacts)this).History<MouSmPolarityEvent>().LastOrDefault(x => x.Source.OwnerSeat == s && x.Source.SkillId == MouSimaYiComposition.Quanmou && x.Source.SkillInstanceId == instance)?.Yin ?? false;
    IReadOnlyList<int> IMouSimaYiFacts.InRange(int s) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != s && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(s, p.Seat)).Select(p => p.Seat).Order().ToArray());
    IReadOnlyList<int> IMouSimaYiFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    IReadOnlyList<MouSmEntity> IMouSimaYiFacts.Gifts(ProgramSkillFrame f, int s) => !((IMouSimaYiFacts)this).Alive(s) ? [] : Array.AsReadOnly(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }.SelectMany(z => _cardZonesOperations._cardZones.CardsAt(new(z, s)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(s, c, new(z, s), OwnedCardMoveIntent.Transfer)).Select(c => new MouSmEntity(c.Id, c.Kind, new(z, s), c.DisplayName))).ToArray());
    IReadOnlyList<MouSmPlayedPlan> IMouSimaYiFacts.Plays(ProgramSkillFrame f) => MouSmPlayedPlans(f);
    bool IMouSimaYiFacts.Declaration(MouSmPlayedPlan plan) => plan.Source is { } source && _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is not null;
    MouSmPrivateUse? IMouSimaYiFacts.PrivateUse(int owner, ProgramSkillWindowContext? w)
    {
        if (w?.Window != SkillProgramTriggerWindow.CardUseCommitted || w.OwnerSeat != owner || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame { MouSimaYiPrivate: { } back } p || !ValidMouSmPrivateWindow(p) || p.Action.ActorSeat != owner || p.CandidateIndex < 0 || p.CandidateIndex >= p.Candidates.Count || p.Candidates[p.CandidateIndex].SkillId != MouSimaYiComposition.Pingliao)
            return null;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(back.UseId)!;
        return new(use.Id, p.Action.ActionId, use.TargetSeats);
    }

    MouSmDebtIssuedEvent? IMouSimaYiFacts.Debt(int owner, ProgramSkillWindowContext? w, bool yin)
    {
        if (w is null || w.SourceSeat != owner || w.TargetSeat is not { } target || w.Amount <= 0 || w.OwnerSeat != owner)
            return null;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId);
        if (yin ? (w.Window != SkillProgramTriggerWindow.AfterDamageApplied || parent is not DamageTriggerWindowFrame d || d.SourceSeat != owner || d.TargetSeat != target) : (w.Window != SkillProgramTriggerWindow.BeforeDamageApplied || parent is not BeforeDamageProgramWindowFrame b || b.Prevented || b.SourceSeat != owner || b.TargetSeat != target))
            return null;
        return ((IMouSimaYiFacts)this).History<MouSmDebtIssuedEvent>().FirstOrDefault(d => d.Source.OwnerSeat == owner && d.Target == target && d.Yin == yin && d.Turn == ((IMouSimaYiFacts)this).Turn && d.TurnOwner == ((IMouSimaYiFacts)this).TurnOwner && d.PhaseId == ((IMouSimaYiFacts)this).PhaseId && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && ((IMouSimaYiFacts)this).Skill(d.Source.SkillId).Program?.GameplayHash == d.GameplayHash && ((IMouSimaYiFacts)this).History<MouSmStartedEvent>().Count(s => s.FrameId == d.FrameId && s.Source == d.Source && s.GameplayHash == d.GameplayHash && s.Operation == SkillProgramEffectOp.ResolveMouSimaYiQuanmou) == 1 && !((IMouSimaYiFacts)this).History<MouSmDebtConsumedEvent>().Any(c => c.DebtId == d.FrameId));
    }

    int IMouSimaYiFacts.Keep(int s, int id) => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[s]);
    double IMouSimaYiFacts.DamageValue(int s, int t) => _coreAiBrainsAiBrainsAccess._aiBrains[s].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(s), t, new(0, 0, 1, 0, 0, 0, false, false));
    void IMouSimaYiNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouSimaYiNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IMouSimaYiNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouSimaYiNative.Give(ProgramSkillFrame f)
    {
        if (!MouSimaYi.Valid(f) || f.MouSimaYi is not { Stage: MouSmStage.GiveChildren, Materials: [var c] } r || !_characterSimaYiMouOperations.CreateMouSmFacts().Gifts(f, r.Target).Contains(c))
            throw new InvalidOperationException("Quanmou requires the giver's current real entity.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([c.Id], CardLocation.Hand(f.OwnerSeat), new(MouSimaYiRuntime.GiveReason), (_, moves) =>
        {
            if (moves is not [var m] || m.CardId != c.Id || m.CardKind != c.Kind || m.From != c.From || m.Reason.Value != MouSimaYiRuntime.GiveReason || m.Sequence <= r.Before || m.To != CardLocation.Hand(f.OwnerSeat) && !IsMouSmShopEquipmentGivePayment(f, r, c, m) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouSmGivePaidEvent>().Any(p => p.FrameId == f.Id))
                throw new InvalidOperationException("Quanmou lost the exact native Give movement.");
            var own = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, own with { MouSimaYi = own.MouSimaYi!with { After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSmGivePaidEvent(f.Id, r.Target, c.Id, m.Sequence));
            _characterHuangQuanMouOperations.RecordNativeGivenCards(f.Id, r.Target, f.OwnerSeat, [c.Id], r.Before);
        });
    }

    void IMouSimaYiNative.Prevent(ProgramSkillFrame f) => _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
    void IMouSimaYiNative.Damage(ProgramSkillFrame f, int target) => _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, 1);
    void IMouSimaYiNative.Disclose(ProgramSkillFrame f) => DiscloseMouSmTargets(f);
    void IMouSimaYiNative.ProhibitHand(ProgramSkillFrame f, int target)
    {
        if (!MouSimaYi.Valid(f) || f.MouSimaYi is not { Operation: SkillProgramEffectOp.ResolveMouSimaYiPingliao } r || !r.Targets.Contains(target) || r.PlayedSeats.Contains(target) || target == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)?.MouSimaYiSecret is not { Released: true })
            throw new InvalidOperationException("Pingliao prohibits only the exact original living target which did not Play a red Basic.");
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(f);
        foreach (var red in new[]
        {
            false,
            true
        }

        )
        {
            var grant = _turnCardUseEffectsOperations._turnCardUseEffects.GrantHandColorRestriction(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, f.Id, -2 * target - (red ? 2 : 1), CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f), target, red);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HandCardColorRestrictionGrantedEvent(grant));
        }
    }

    void IMouSimaYiNative.Declare(ProgramSkillFrame f) => IssueMouSmPlayedDeclaration(f);
    void IMouSimaYiNative.Play(ProgramSkillFrame f) => IssueMouSmPlayed(f);
    void IMouSimaYiNative.CompletePlayed(ProgramSkillFrame f) => BeginMouSmPlayedCompleted(f);
    internal bool IsMouSmShopEquipmentGivePayment(ProgramSkillFrame f, MouSmReceipt receipt, MouSmEntity material, CardMovementRecord movement) => MouSimaYi.Valid(f) && receipt.Source == MouSimaYiRuntime.Source(f) && material.From == CardLocation.Equipment(receipt.Target) && movement.CardId == material.Id && movement.CardKind == material.Kind && movement.From == material.From && movement.Reason.Value == MouSimaYiRuntime.GiveReason && movement.Sequence > receipt.Before && _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(movement) && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentCreatedEvent>().Where(p => p.CardId == material.Id).ToArray()is [var created] && created.Seat == receipt.Target && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Where(p => p.CardId == material.Id).ToArray()is [var destroyed] && destroyed.MovementSequence == movement.Sequence && destroyed.ParentFrameId == f.Id;
    internal IEnumerable<RuleQueryContribution> MouSmSlashContributions(CharacterState player, SkillRuleQuery query) => query != SkillRuleQuery.SlashLimit ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<MouSmSlashAllowanceEvent>().Where(x => x.Source.OwnerSeat == player.Seat && x.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && x.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && x.PhaseId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreProgramEventHistoryOperations.ProgramEventHistory<MouSmStartedEvent>().Count(s => s.FrameId == x.FrameId && s.Source == x.Source && s.Operation == SkillProgramEffectOp.ResolveMouSimaYiPingliao) == 1).Select(x => (RuleQueryContribution)new FiniteRuleQueryContribution($"mou-sima-yi:{(x.FrameId)}", SkillRuleOperation.Add, 1));
}
