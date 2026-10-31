#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed partial class ProgramSkillHost(Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineAssistedPhysicalCardsEntries> getCoreGameEngineAssistedPhysicalCardsRuntime, Func<RuntimeEntries.CoreGameEngineEquipmentPlacementAndHpPairEntries> getCoreGameEngineEquipmentPlacementAndHpPairRuntime, Func<RuntimeEntries.CoreGameEnginePaidHpLossProgramsEntries> getCoreGameEnginePaidHpLossProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramContinuationPermissionsEntries> getCoreGameEngineProgramContinuationPermissionsRuntime, Func<RuntimeEntries.CoreGameEngineProgramNativeDrawEntries> getCoreGameEngineProgramNativeDrawRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineProgramVirtualSlashOffersEntries> getCoreGameEngineProgramVirtualSlashOffersRuntime, Func<ProgramCapabilityRegistry> getProgramCapabilityRegistry, Func<RuntimeEntries.CoreGameEngineProgramEquipmentExchangesEntries> getCoreGameEngineProgramEquipmentExchangesRuntime, Func<RuntimeEntries.CoreGameEngineStrategicProgramsEntries> getCoreGameEngineStrategicProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, Func<FrameStore> getFrameStore, Func<ContentRegistry> getContentRegistry, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreNativeSkillContextQueriesEntries> getCoreNativeSkillContextQueriesRuntime, EngineStates.CoreMatchOutcomeState matchOutcomeState, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<CardZoneStore> getCardZoneStore, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CoreGameEngineProgramCategoryChallengesEntries> getCoreGameEngineProgramCategoryChallengesRuntime, Func<RuntimeEntries.CoreGameEngineProgramChoicesEntries> getCoreGameEngineProgramChoicesRuntime, Func<RuntimeEntries.CoreGameEngineProgramRandomTransfersEntries> getCoreGameEngineProgramRandomTransfersRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, ProgramExpressionWorldFactory createProgramExpressionWorld, Func<RuntimeEntries.CharacterXiaoQiaoMouOlEntries> getCharacterXiaoQiaoMouOlRuntime, Func<RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleChoicesEntries> getCoreGameEngineAdvancedLifecycleChoicesRuntime, Func<RuntimeEntries.CoreGameEngineHealthQueriesEntries> getCoreGameEngineHealthQueriesRuntime, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, Func<RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramExpressionsEntries> getCoreGameEngineProgramExpressionsRuntime, Func<RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries> getCoreGameEngineProgramCategoryTargetLedgerRuntime, Func<RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries> getCharacterDaQiaoXiaoQiaoHappyGodRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEnginePairBenefitObserversEntries> getCoreGameEnginePairBenefitObserversRuntime, Func<RuntimeEntries.CoreGameEngineRecoveryAmountBonusesEntries> getCoreGameEngineRecoveryAmountBonusesRuntime, Func<RuntimeEntries.CoreNativeRecoveryFramesEntries> getCoreNativeRecoveryFramesRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries> getCoreGameEnginePrepPoisonOutcomesRuntime, Func<RuntimeEntries.CoreGameEngineJudgmentReplacementProgramsEntries> getCoreGameEngineJudgmentReplacementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramJudgmentClaimsEntries> getCoreGameEngineProgramJudgmentClaimsRuntime, Func<RuntimeEntries.CoreGameEngineProgramTopReorderEntries> getCoreGameEngineProgramTopReorderRuntime, Func<RuntimeEntries.CoreGameEngineProgramRepeatedJudgmentEntries> getCoreGameEngineProgramRepeatedJudgmentRuntime, Func<RuntimeEntries.CoreGameEngineProgramTurnPhaseEntries> getCoreGameEngineProgramTurnPhaseRuntime, Func<RuntimeEntries.CoreGameEngineProgramSlashRequestEntries> getCoreGameEngineProgramSlashRequestRuntime, Func<RuntimeEntries.CoreGameEngineDeathProgramsEntries> getCoreGameEngineDeathProgramsRuntime, Func<RuntimeEntries.CoreActualDiscardPhaseLedgerEntries> getCoreActualDiscardPhaseLedgerRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineOrderedSelectedActorDuelsEntries> getCoreGameEngineOrderedSelectedActorDuelsRuntime, Func<RuntimeEntries.CoreHellMengpoProgramsEntries> getCoreHellMengpoProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAtomicCardMovesEntries> getCoreGameEngineProgramAtomicCardMovesRuntime, Func<RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries> getCoreHappyWeiYanGainDestinationsRuntime, Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<RuntimeEntries.CharacterHuangQuanMouEntries> getCharacterHuangQuanMouRuntime, Func<RuntimeEntries.CoreGameEnginePhaseGiftBasicEntries> getCoreGameEnginePhaseGiftBasicRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramOwnedCardsEntries> getCoreGameEngineProgramOwnedCardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramHoldCardsEntries> getCoreGameEngineProgramHoldCardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramRevealsEntries> getCoreGameEngineProgramRevealsRuntime, Func<RuntimeEntries.CoreGameEngineDyingProgramsEntries> getCoreGameEngineDyingProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries> getCoreGameEngineCurrentTurnUseKindsRuntime, Func<RuntimeEntries.CharacterLiuYanOlEntries> getCharacterLiuYanOlRuntime, Func<RuntimeEntries.CoreGameEngineProgramDamageTransferEntries> getCoreGameEngineProgramDamageTransferRuntime, Func<RuntimeEntries.CoreProgramPopulationOperationsEntries> getCoreProgramPopulationOperationsRuntime, Func<RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime, Func<RuntimeEntries.CoreGameEngineActualTurnSkillGrantsEntries> getCoreGameEngineActualTurnSkillGrantsRuntime, Func<RuntimeEntries.CoreGameEngineProgramOrdinaryTrickUsesEntries> getCoreGameEngineProgramOrdinaryTrickUsesRuntime, Func<RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries> getCoreGameEngineProgramVirtualDuelRuntime, Func<RuntimeEntries.CoreNativeFactionSlashEntries> getCoreNativeFactionSlashRuntime, Func<RuntimeEntries.CoreGameEngineProgramHandExchangesEntries> getCoreGameEngineProgramHandExchangesRuntime, Func<RuntimeEntries.CoreGameEngineProgramActivationLedgerEntries> getCoreGameEngineProgramActivationLedgerRuntime, Func<RuntimeEntries.CoreGameEngineProgramOwnCardDiscardsEntries> getCoreGameEngineProgramOwnCardDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramOwnedCardDistributionsEntries> getCoreGameEngineProgramOwnedCardDistributionsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAttackRangeAidEntries> getCoreGameEngineProgramAttackRangeAidRuntime, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.CoreGameEnginePlannedPhaseQuantitiesEntries> getCoreGameEnginePlannedPhaseQuantitiesRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, Func<RuntimeEntries.CoreProgramAbolishAreasOperationEntries> getCoreProgramAbolishAreasOperationRuntime, Func<RuntimeEntries.CoreProgramLoseDeathSourceSkillsOperationEntries> getCoreProgramLoseDeathSourceSkillsOperationRuntime, Func<RuntimeEntries.CoreGameEngineDamagePreventionProgramsEntries> getCoreGameEngineDamagePreventionProgramsRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardTargetEffectsEntries> getCoreGameEngineProgramCardTargetEffectsRuntime, Func<RuntimeEntries.CoreGameEngineProgramAttackEffectsEntries> getCoreGameEngineProgramAttackEffectsRuntime, Func<RuntimeEntries.CoreGameEngineProgramOtherOwnedCardDiscardsEntries> getCoreGameEngineProgramOtherOwnedCardDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineProgramPhaseHandDiscardsEntries> getCoreGameEngineProgramPhaseHandDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineCardUseDebitsEntries> getCoreGameEngineCardUseDebitsRuntime, Func<RuntimeEntries.CoreDirectedTurnCardPoliciesEntries> getCoreDirectedTurnCardPoliciesRuntime) : ISkillProgramExecutionHost, ISkillProgramEffectHost
{
    public T GetCapability<T>()
        where T : class => this is T shared ? shared : getProgramCapabilityRegistry().Get<T>();
    public bool TryGetCapability<T>([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? capability)
        where T : class
    {
        if (this is T shared)
        {
            capability = shared;
            return true;
        }

        return getProgramCapabilityRegistry().TryGet(out capability);
    }

    public SkillProgramStepOutcome ExchangeSelectedTargetEquipment(ProgramSkillFrame frame) => getCoreGameEngineProgramEquipmentExchangesRuntime().ExchangeProgramSelectedTargetEquipment(frame);
    public SkillProgramStepOutcome ExecuteStrategicEffect(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineStrategicProgramsRuntime().ExecuteStrategicProgramEffect(effect, frame, targetSeat);
    public int ResolveParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => getCoreGameEngineProgramCardPaymentsRuntime().ResolveProgramParticipant(frame, reference);
    public ProgramSkillFrame? GetActiveFrame(long frameId) => getFrameStore().LastOrDefault()is ProgramSkillFrame frame && frame.Id == frameId ? frame : null;
    public SkillProgram GetProgram(string skillId) => getContentRegistry().Skills.GetValueOrDefault(skillId)?.Program ?? throw new InvalidOperationException("A running program is missing its compiled definition.");
    public SkillProgramActorState GetActor(int seat)
    {
        var actor = matchParticipantsState.ReferenceToPlayers[seat];
        return new(getCoreNativeSkillContextQueriesRuntime().CreateSkillContext(actor), actor.IsAlive);
    }

    public bool IsGameOver => matchOutcomeState.ReferenceToWinner != Winner.None;

    public bool OwnsSkillInstance(int ownerSeat, string skillId, string skillInstanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId, skillInstanceId);
    public bool OwnsCards(int ownerSeat, IReadOnlyList<int> cardIds, IReadOnlyList<CardZoneKind> sourceZones) => cardIds.All(id => sourceZones.Any(zone => getCardZoneStore().Contains(new CardLocation(zone, ownerSeat), id)));
    public bool EvaluateCondition(ProgramSkillFrame frame, SkillProgramCondition condition, PlayerSkillContext context)
    {
        // Most instructions only read ordinary actor state. Usability builds
        // native card actions, so compute it only for a condition that reads
        // it, against the current state after the preceding instruction.
        if (condition.RequiresHandLimitContext)
            context = getCoreNativeSkillContextQueriesRuntime().CreateSkillContext(matchParticipantsState.ReferenceToPlayers[context.Seat], includeHandLimit: true);
        var selectedTarget = frame.SelectedTargetSeats.Count == 1 ? getCoreNativeSkillContextQueriesRuntime().CreateSkillContext(matchParticipantsState.ReferenceToPlayers[frame.SelectedTargetSeats[0]]) : null;
        return condition.Evaluate(context, selectedTarget, bind => frame.PindianResultBindings.Single(item => item.Name == bind).SourceWon, stateId => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState(frame, stateId), frame.WindowContext?.CardUse?.IsPublicRed, bind => frame.ChoiceBindings.SingleOrDefault(item => item.Name == bind)?.OptionId, bind => getCoreGameEngineProgramCategoryChallengesRuntime().AreProgramBoundCardsSameColor(frame, bind), (bind, categories) => getCoreGameEngineProgramCategoryChallengesRuntime().DoProgramBoundCardsMatchCategories(frame, bind, categories), (bind, kinds) => getCoreGameEngineProgramCategoryChallengesRuntime().DoProgramBoundCardsMatchKinds(frame, bind, kinds), bind => EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.IsProgramAttackRangeCoverageDecreased(frame, bind), bind => getCoreGameEngineProgramChoicesRuntime().CountChooserProgramBoundCards(frame, bind, context.Seat), frame.SelectedCardIds.Count, (cardBind, choiceBind) => getCoreGameEngineProgramRandomTransfersRuntime().DoesProgramFrozenSuitMatchChoice(frame, cardBind, choiceBind), (bind, suits) => getCoreGameEngineProgramCategoryChallengesRuntime().DoProgramBoundCardsMatchSuits(frame, bind, suits), (bind, _) => getCoreGameEngineProgramCategoryChallengesRuntime().DoProgramBoundCardCategoryMatchAction(frame, bind), requestedSlashDamagedOwner: frame.RequestedSlashDamagedOwner == true, eventSourceExists: frame.WindowContext?.SourceSeat is { } sourceSeat && getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(sourceSeat), expression: expression => ProgramExpressions.EvaluateBool(expression, createProgramExpressionWorld(frame)));
    }

    public bool TryStartPostInstructionWindow(long frameId) => getCharacterXiaoQiaoMouOlRuntime().TryBeginPublicHandRevealWindow(frameId, PublicHandRevealReturnKind.Program) || getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(frameId, PostEventContinuation.Program) || getCoreGameEngineAdvancedLifecycleChoicesRuntime().TryContinueAdvancedDamage(frameId);
    public bool TryStartInstructionHealthQuery(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineHealthQueriesRuntime().TryBeginHappyQiaoInstructionQuery(frame, effect);
    public void UpdateFrame(ProgramSkillFrame frame)
    {
        if (GetActiveFrame(frame.Id)is null)
            throw new InvalidOperationException("A program can only advance its active frame.");
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(frame);
    }

    public void Complete(ProgramSkillFrame frame, bool completed, string? reason = null)
    {
        if (reason is not null)
            getCoreMatchLogRuntime().AddLog("ActiveSkill", reason, frame.OwnerSeat);
        getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(frame, completed);
    }

    public int? EvaluateAmountExpression(ProgramSkillFrame frame, ProgramExpression expression) => getCoreGameEngineProgramExpressionsRuntime().EvaluateProgramAmountExpression(frame.Id, expression);
    public int? EvaluateAmountExpression(ProgramSkillFrame frame, ProgramExpression expression, SkillProgramUnknownAmountBehavior onUnknown) => getCoreGameEngineProgramExpressionsRuntime().EvaluateProgramAmountExpression(frame.Id, expression, onUnknown);
    public void Draw(long frameId, int ownerSeat, int targetSeat, int amount, SkillProgramNumberExpression? numberExpression, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().DrawProgramCards(frameId, targetSeat, amount, numberExpression, resultBind, visibility, reason);
    public void DrawSelectedTargets(long frameId, int amount, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().DrawProgramSelectedTargets(frameId, amount, reason);
    public void DrawBoundCardCount(long frameId, int ownerSeat, int targetSeat, string sourceBind, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason)
    {
        var count = getCoreGameEngineProgramLifecycleRuntime().GetProgramCardSet(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId), sourceBind).CardIds.Count;
        getCoreGameEngineProgramLifecycleRuntime().DrawProgramCards(frameId, targetSeat, count, null, resultBind, visibility, reason);
    }

    public void DrawPhaseSkillUsage(long frameId, int ownerSeat, int targetSeat, string usageId, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().DrawProgramCards(frameId, targetSeat, getCoreGameEngineProgramCategoryTargetLedgerRuntime().GetProgramPhaseSkillUsage(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId), usageId), null, resultBind, visibility, reason);
    public void Recover(long frameId, int ownerSeat, int targetSeat, int amount, SkillProgramNumberExpression? numberExpression, string? sourceBind)
    {
        var target = matchParticipantsState.ReferenceToPlayers[targetSeat];
        if (!target.HasHpValue)
            return;
        var requested = numberExpression switch
        {
            null => amount,
            SkillProgramNumberExpression.BoundCardCount when sourceBind is not null => getCoreGameEngineProgramLifecycleRuntime().GetProgramCardSet(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId), sourceBind).CardIds.Count,
            SkillProgramNumberExpression.AfterDamageAmount when sourceBind is null && getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId).WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, DamageFrameId: not null, Amount: > 0 } damage && damage.TargetSeat == targetSeat => damage.Amount,
            _ => throw new InvalidOperationException($"Unsupported recovery number expression '{numberExpression}'.")};
        if (target.HappyQiaoHealth is not null)
        {
            if (getCharacterDaQiaoXiaoQiaoHappyGodRuntime().HappyQiaoAnyWounded(targetSeat))
                _ = getCoreRecoveryReplacementRuntime().TryQueueRecoveryReplacement(frameId, ownerSeat, targetSeat, requested, new(RecoveryAttemptProducer.Program, getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId).InstructionIndex));
            return;
        }

        var recovered = Math.Min(requested, target.MaxHp - target.Hp);
        if (recovered <= 0)
            return;
        if (getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId).FixedRecipient is not null)
            getCoreGameEnginePairBenefitObserversRuntime().CaptureFixedRecipientRecovery(frameId, ownerSeat, targetSeat, recovered);
        if (getCoreRecoveryReplacementRuntime().TryQueueRecoveryReplacement(frameId, ownerSeat, targetSeat, getCoreGameEngineRecoveryAmountBonusesRuntime().TracksRecoveryAmountBonuses ? requested : recovered, new(RecoveryAttemptProducer.Program, getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId).InstructionIndex)))
            return;
        var recovery = getCoreNativeRecoveryFramesRuntime().BeginRecovery(frameId, ownerSeat, targetSeat, recovered);
        target.Hp += recovered;
        advanceEventRulesAndQueueFact(new RecoveryAppliedEvent(ownerSeat, targetSeat, recovered, target.Hp));
        getCoreGameEngineRuntimeRuntime().PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
        if (getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId).WindowContext?.JudgmentReplacement is { } replacement)
        {
            var active = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(active with { WindowContext = active.WindowContext!with { JudgmentReplacement = replacement with { RecoveredHp = replacement.RecoveredHp + recovered } } });
        }
    }

    public void RecoverSelectedTargets(long frameId, int ownerSeat, int amount)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId);
        foreach (var seat in frame.SelectedTargetSeats)
            if (matchParticipantsState.ReferenceToPlayers[seat].IsAlive)
                Recover(frameId, ownerSeat, seat, amount, null, null);
    }

    public SkillProgramStepOutcome LoseHp(long frameId, string skillId, int targetSeat, int amount)
    {
        if (!getCharacterDaQiaoXiaoQiaoHappyGodRuntime().HasHappyQiaoHealth(targetSeat))
            return getCoreGameEnginePrepPoisonOutcomesRuntime().ApplyProgramHpLoss(frameId, skillId, targetSeat, amount);
        if (getCharacterDaQiaoXiaoQiaoHappyGodRuntime().TryBeginHappyQiaoProgramHealthChoice(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frameId), targetSeat, HpChangeKind.Loss, amount))
            return SkillProgramStepOutcome.AwaitChild;
        return getCoreGameEnginePrepPoisonOutcomesRuntime().ApplyProgramHpLoss(frameId, skillId, targetSeat, amount, (before, after, _) => getCharacterDaQiaoXiaoQiaoHappyGodRuntime().CompleteHappyQiaoHealthPayment(frameId, targetSeat, HpChangeKind.Loss, before, after));
    }

    public SkillProgramStepOutcome Damage(ProgramSkillFrame frame, int targetSeat, int amount, ProgramParticipantReference? sourceReference = null, DamageNature? nature = null) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillDamage(frame, targetSeat, amount, sourceReference, nature);
    public void ReplaceJudgment(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineJudgmentReplacementProgramsRuntime().ReplaceProgramJudgment(frame, effect);
    public SkillProgramStepOutcome ClaimJudgmentCard(ProgramSkillFrame frame) => getCoreGameEngineProgramJudgmentClaimsRuntime().ClaimProgramJudgmentCard(frame);
    public SkillProgramStepOutcome ReorderTopCards(ProgramSkillFrame frame, int maximumCards, SkillProgramNumberExpression? numberExpression) => getCoreGameEngineProgramTopReorderRuntime().BeginProgramTopReorder(frame, maximumCards, numberExpression);
    public SkillProgramStepOutcome RepeatJudgment(ProgramSkillFrame frame, string reason, string resultBind, IReadOnlyList<Suit> successSuits) => getCoreGameEngineProgramRepeatedJudgmentRuntime().BeginProgramRepeatedJudgment(frame, reason, resultBind, successSuits);
    public SkillProgramStepOutcome SkipTurnPhases(ProgramSkillFrame frame, IReadOnlyList<SkillProgramTurnPhase> phases) => getCoreGameEngineProgramTurnPhaseRuntime().SkipProgramTurnPhases(frame, phases);
    public SkillProgramStepOutcome UseVirtualCard(ProgramSkillFrame frame, int targetSeat, CardKind cardKind, bool ignoreDistance) => getCoreGameEngineProgramTurnPhaseRuntime().BeginProgramVirtualCardUse(frame, targetSeat, cardKind, ignoreDistance);
    public SkillProgramStepOutcome UseBoundCardByTarget(ProgramSkillFrame frame, int targetSeat, string sourceBind) => getCoreGameEngineSkillProgramsRuntime().UseProgramBoundCardByTarget(frame, targetSeat, sourceBind);
    public SkillProgramStepOutcome RequestSlashByTarget(ProgramSkillFrame frame, int targetSeat, string resultBind) => getCoreGameEngineProgramSlashRequestRuntime().RequestProgramSlashByTarget(frame, targetSeat, resultBind);
    public void PendExtraTurn(ProgramSkillFrame frame, int? targetSeat = null) => getCoreGameEngineSkillProgramsRuntime().PendProgramExtraTurn(frame, targetSeat);
    public void ClaimDeathCleanupCards(ProgramSkillFrame frame) => getCoreGameEngineDeathProgramsRuntime().ClaimProgramDeathCleanupCards(frame);
    public SkillProgramStepOutcome BindDiscardPhaseDiscards(ProgramSkillFrame frame, string resultBind) => getCoreActualDiscardPhaseLedgerRuntime().BindProgramDiscardPhaseDiscards(frame, resultBind);
    public SkillProgramStepOutcome Pindian(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineSkillProgramsRuntime().BeginProgramSkillPindian(frame, targetSeat);
    public SkillProgramStepOutcome MoveSelected(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<int> cardIds, bool toDiscard, CardMoveReason reason)
    {
        var program = GetProgram(frame.SkillId);
        var activation = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Activation, frame.ActivationId).Activation!;
        var selected = cardIds.Select(id =>
        {
            var locations = activation.SourceZones.Select(zone => new CardLocation(zone, frame.OwnerSeat)).Where(location => getCardZoneStore().Contains(location, id)).ToArray();
            if (locations.Length != 1)
                throw new InvalidOperationException("A selected program cost no longer has one valid owner location.");
            return (Card: getCardZoneStore().CardsAt(locations[0]).Single(card => card.Id == id), Location: locations[0]);
        }).ToArray();
        var active = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frame.Id);
        if (toDiscard && selected.Any(item => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, item.Card, item.Location, OwnedCardMoveIntent.Discard)))
        {
            getCoreGameEngineProgramLifecycleRuntime().CancelProgramBindingAndCleanup(active, "所选手牌不能自行弃置，费用未支付。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var operation = toDiscard ? SkillProgramEffectOp.DiscardSelected : SkillProgramEffectOp.GiveSelected;
        if (active.PendingMovementContinuation is not null || active.SelectedCardPayment is not null && (active.SelectedCardPaymentResult is null || active.SelectedCardPayment.InstructionIndex >= active.InstructionIndex) || active.InstructionIndex < 1 || activation.Effects[active.InstructionIndex - 1].Op != operation)
            throw new InvalidOperationException("A selected-card payment has no current unpaid instruction.");
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(active with { SelectedCardPayment = new ProgramSelectedCardPayment(active.InstructionIndex, operation, Array.AsReadOnly(cardIds.ToArray()), targetSeat), SelectedCardPaymentResult = null });
        var destination = toDiscard ? CardLocation.DiscardPile : CardLocation.Hand(targetSeat);
        // Removing a persistent-pile cost still pays the selected entities,
        // but only owned Hand/Equipment/Judgment materials form a discard invoice.
        var discardInvocation = toDiscard ? getCoreGameEngineNativeLogicalDiscardsRuntime().CaptureNativeLogicalDiscardInvocation(frame.OwnerSeat, frame.OwnerSeat, selected.Where(item => EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(item.Location)).Select(item => item.Card.Id).ToArray(), reason, frame.Id) : null;
        var nativeGiveBefore = cardMovementJournalState.ReferenceToMovementSequence;
        if (toDiscard && getCoreGameEngineOrderedSelectedActorDuelsRuntime().TryPayOrderedSelectedActorDuel(getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frame.Id), cardIds, reason))
        {
        }
        else if (toDiscard && getCoreHellMengpoProgramsRuntime().HasHellShiyouCapability)
            getCoreGameEngineProgramAtomicCardMovesRuntime().MoveProgramCardsFromMultipleSources(cardIds, destination, reason, disclosure: CardMovementDisclosure.FaceUp);
        else if (!toDiscard && selected.Any(item => getCoreHappyWeiYanGainDestinationsRuntime().PlanHappyWeiYanGainDestination(item.Card, item.Location, destination, reason)is not null))
            getCoreHappyWeiYanGainDestinationsRuntime().MoveNativeGivenCards(frame.Id, frame.OwnerSeat, targetSeat, cardIds, reason, settlement =>
            {
                var paid = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frame.Id);
                getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(paid with { SelectedCardPayment = paid.SelectedCardPayment!with { GainRequestSettlement = settlement } });
            }, CardMovementDisclosure.FaceDown);
        else
            foreach (var group in selected.GroupBy(item => item.Location))
                getCorePhysicalMovementFacadesRuntime().MoveCards(group.Select(item => item.Card).ToArray(), group.Key, destination, reason, toDiscard ? null : getCharacterHuangQuanMouRuntime().NativeGiveCompletion(frame.Id, frame.OwnerSeat, targetSeat, cardIds, nativeGiveBefore), disclosure: toDiscard ? CardMovementDisclosure.FaceUp : CardMovementDisclosure.FaceDown);
        getCoreGameEngineNativeLogicalDiscardsRuntime().CompleteNativeLogicalDiscardInvocation(discardInvocation);
        active = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(frame.Id);
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeTop(active with { SelectedCardPayment = active.SelectedCardPayment!with { MovementCommitted = true } });
        getCoreGameEnginePhaseGiftBasicRuntime().CapturePaidPhaseGift(frame.Id, activation, toDiscard);
        if (!getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow())
            getCoreGameEngineRuntimeRuntime().ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    public SkillProgramStepOutcome InsertPhase(ProgramSkillFrame frame, TurnPhase phase, SkillProgramPhaseContinuation continuation, int? beneficiarySeat = null) => getCoreGameEngineProgramLifecycleRuntime().ScheduleProgramPhase(frame, phase, continuation, beneficiarySeat);
    public void RecoverTo(long frameId, int ownerSeat, int targetSeat, SkillProgramNumberExpression expression, int minimumValue, bool clampToMaxHp) => getCoreGameEngineProgramLifecycleRuntime().RecoverProgramTargetTo(frameId, ownerSeat, targetSeat, expression, minimumValue, clampToMaxHp);
    public void DiscardOwnedZoneCards(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().DiscardProgramOwnedZoneCards(frame, zones, reason);
    public void SetSelectedTargetsChainedState(ProgramSkillFrame frame, bool chained)
    {
        foreach (var seat in frame.SelectedTargetSeats)
            if (matchParticipantsState.ReferenceToPlayers[seat].IsAlive)
                getCoreGameEngineProgramLifecycleRuntime().SetProgramChainedState(frame, chained, seat);
    }

    public void SetChainedState(ProgramSkillFrame frame, bool chained, int? targetSeat = null) => getCoreGameEngineProgramLifecycleRuntime().SetProgramChainedState(frame, chained, targetSeat ?? frame.OwnerSeat);
    public SkillProgramStepOutcome ChooseOption(ProgramSkillFrame frame, int chooserSeat, string resultBind, IReadOnlyList<SkillProgramChoiceOption> options) => getCoreGameEngineProgramChoicesRuntime().ChooseProgramOption(frame, chooserSeat, resultBind, options);
    public SkillProgramStepOutcome SelectOwnedCards(ProgramSkillFrame frame, int cardOwnerSeat, int amount, SkillProgramNumberExpression? expression, IReadOnlyList<CardZoneKind> zones, string resultBind, int minimumCards, int maximumCards, IReadOnlyList<CardKind> cardKinds, IReadOnlyList<Suit> suits) => getCoreGameEngineProgramOwnedCardsRuntime().SelectProgramOwnedCards(frame, cardOwnerSeat, amount, expression, zones, resultBind, minimumCards, maximumCards, cardKinds, suits);
    public SkillProgramStepOutcome HoldTargetCards(ProgramSkillFrame frame, int chooserSeat, int holderSeat, IReadOnlyList<CardZoneKind> zones, string resultBind, int minimumCards) => getCoreGameEngineProgramHoldCardsRuntime().HoldProgramTargetCards(frame, chooserSeat, holderSeat, zones, resultBind, minimumCards);
    public SkillProgramStepOutcome RevealTargetHandCard(ProgramSkillFrame frame, ProgramParticipantReference chooser, ProgramParticipantReference cardOwner, string resultBind, SkillProgramRevealMode mode, IReadOnlyList<Suit> eligibleSuits, bool allowDecline) => getCoreGameEngineProgramRevealsRuntime().RevealProgramTargetHandCard(frame, chooser, cardOwner, resultBind, mode, eligibleSuits, allowDecline);
    public void CaptureSelectedCards(ProgramSkillFrame frame, string resultBind) => getCoreGameEngineProgramCategoryChallengesRuntime().CaptureProgramSelectedCards(frame, resultBind);
    public void RevealBoundCards(ProgramSkillFrame frame, string sourceBind) => getCoreGameEngineProgramCategoryChallengesRuntime().RevealProgramBoundCards(frame, sourceBind);
    public void UseBoundCardAsDyingAlcohol(ProgramSkillFrame frame, string sourceBind, CardMoveReason reason) => getCoreGameEngineDyingProgramsRuntime().UseProgramBoundCardAsDyingAlcohol(frame, sourceBind, reason);
    public SkillProgramStepOutcome UseBoundCardAsDyingAlcoholWithCompletion(ProgramSkillFrame frame, string sourceBind, CardMoveReason reason)
    {
        getCoreGameEngineDyingProgramsRuntime().UseProgramBoundCardAsDyingAlcohol(frame, sourceBind, reason);
        return SkillProgramStepOutcome.AwaitChild;
    }

    public void UseVirtualDyingAlcohol(ProgramSkillFrame frame) => getCoreGameEngineDyingProgramsRuntime().UseProgramVirtualDyingAlcohol(frame);
    public SkillProgramStepOutcome UseVirtualDyingAlcoholWithCompletion(ProgramSkillFrame frame)
    {
        getCoreGameEngineDyingProgramsRuntime().UseProgramVirtualDyingAlcohol(frame);
        return getCoreGameEngineCurrentTurnUseKindsRuntime().TracksActualTurnUseOrdinal || getCharacterLiuYanOlRuntime().HasLiuYanCapability ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }

    public void ClaimMovedCards(ProgramSkillFrame frame) => getCoreGameEngineCardMovementProgramsRuntime().ClaimProgramMovedCards(frame);
    public void RevealUniqueRankForDying(ProgramSkillFrame frame, CardZoneKind zone, int rescueHp) => getCoreGameEngineDyingProgramsRuntime().RevealProgramUniqueRankForDying(frame, zone, rescueHp);
    public void RedirectCurrentDamage(ProgramSkillFrame frame, string sourceBind, bool drawLostHpAfterDamage) => getCoreGameEngineProgramDamageTransferRuntime().RedirectProgramCurrentDamage(frame, sourceBind, drawLostHpAfterDamage);
    public SkillProgramStepOutcome ChooseDifferentCategoryDiscard(ProgramSkillFrame frame, ProgramParticipantReference chooser, ProgramParticipantReference cardOwner, IReadOnlyList<CardZoneKind> zones, string sourceBind, string resultBind, CardMoveReason reason) => getCoreGameEngineProgramCategoryChallengesRuntime().ChooseProgramDifferentCategoryDiscard(frame, chooser, cardOwner, zones, sourceBind, resultBind, reason);
    public void GrowMaximumHpAndHp(ProgramSkillFrame frame, SkillProgramNumberExpression expression) => getCoreProgramPopulationOperationsRuntime().GrowProgramMaximumHpAndHp(frame, expression);
    public void ChangeMaximumHp(ProgramSkillFrame frame, int amount) => getCoreGameEngineProgramLifecycleRuntime().ChangeProgramMaximumHp(frame, amount);
    public void GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => getCoreGameEngineProgramSkillChangesRuntime().GrantProgramSkills(frame, skillIds);
    public void GrantTurnSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => getCoreGameEngineProgramLifecycleRuntime().GrantProgramTurnSkills(frame, skillIds);
    public void GrantTurnSkills(ProgramSkillFrame frame, int targetSeat, IReadOnlyList<string> skillIds, SkillProgramTurnSkillExpiry? expiry) => getCoreGameEngineActualTurnSkillGrantsRuntime().GrantProgramTurnSkills(frame, targetSeat, skillIds, expiry);
    public SkillProgramStepOutcome UseSelectedCardsAs(ProgramSkillFrame frame, int targetSeat, string viewAsId, CardKind outputKind) => getCoreGameEngineSkillProgramsRuntime().UseProgramSelectedCardsAs(frame, targetSeat, viewAsId, outputKind);
    public SkillProgramStepOutcome UseAllHandCardsAsOrdinaryTrick(ProgramSkillFrame frame, string viewAsId) => getCoreGameEngineProgramOrdinaryTrickUsesRuntime().UseProgramAllHandCardsAsOrdinaryTrick(frame, viewAsId);
    public SkillProgramStepOutcome StartVirtualDuel(ProgramSkillFrame frame) => getCoreGameEngineProgramVirtualDuelRuntime().BeginProgramVirtualDuel(frame);
    public SkillProgramStepOutcome RequestFactionCard(ProgramSkillFrame frame, int targetSeat, string providerFactionId, CardKind requiredKind)
    {
        getCoreNativeFactionSlashRuntime().BeginProgramFactionCardRequest(frame.Id, frame.OwnerSeat, targetSeat, providerFactionId, requiredKind);
        return SkillProgramStepOutcome.AwaitChild;
    }

    public SkillProgramStepOutcome TransferRandomOwnedCard(ProgramSkillFrame frame, int targetSeat, string resultBind) => getCoreGameEngineProgramRandomTransfersRuntime().TransferProgramRandomOwnedCard(frame, targetSeat, resultBind);
    public SkillProgramStepOutcome ExchangeSelectedTargetHands(ProgramSkillFrame frame) => getCoreGameEngineProgramHandExchangesRuntime().ExchangeProgramSelectedTargetHands(frame);
    public void AccumulateSelectedCardCount(ProgramSkillFrame frame, string usageId, int threshold, string resultBind) => getCoreGameEngineProgramActivationLedgerRuntime().AccumulateProgramSelectedCardCount(frame, usageId, threshold, resultBind);
    public void TurnOver(long frameId, int ownerSeat, int targetSeat) => getCoreGameEngineProgramLifecycleRuntime().TurnOverProgramTarget(frameId, ownerSeat, targetSeat);
    public void SetFaceState(long frameId, int ownerSeat, int targetSeat, bool faceDown) => getCoreGameEngineProgramLifecycleRuntime().SetProgramTargetFaceState(frameId, ownerSeat, targetSeat, faceDown);
    public SkillProgramStepOutcome StartJudgment(ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, SkillProgramCardSetVisibility visibility, int sourceSeat) => getCoreGameEngineProgramLifecycleRuntime().StartProgramJudgment(frame, targetSeat, reason, resultBind, visibility, sourceSeat);
    public SkillProgramStepOutcome ChooseOwnCardDiscard(ProgramSkillFrame frame, ProgramParticipantReference? chooser, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => getCoreGameEngineProgramOwnCardDiscardsRuntime().ChooseProgramOwnCardDiscard(frame, chooser, zones, reason);
    public void RevealTopCards(long frameId, int ownerSeat, int amount, SkillProgramNumberExpression? numberExpression, string resultBind, SkillProgramCardSetVisibility visibility) => getCoreGameEngineProgramLifecycleRuntime().RevealProgramTopCards(frameId, ownerSeat, amount, numberExpression, resultBind, visibility);
    public void FilterBoundCards(long frameId, string sourceBind, string resultBind, IReadOnlyList<Suit> suits, ProgramParticipantReference? effectiveSuitFor = null, IReadOnlyList<SkillProgramCardCategory>? categories = null, IReadOnlyList<EquipmentSlot>? equipmentSlots = null, IReadOnlyList<CardKind>? cardKinds = null, string? matchSuitOfBind = null, bool availableAtSourceOnly = false) => getCoreGameEngineProgramLifecycleRuntime().FilterProgramBoundCards(frameId, sourceBind, resultBind, suits, effectiveSuitFor, categories ?? [], equipmentSlots ?? [], cardKinds ?? [], matchSuitOfBind, availableAtSourceOnly);
    public SkillProgramStepOutcome SelectCardSubset(long frameId, int ownerSeat, string sourceBind, string resultBind, int minimumCards, int maximumCards, int maximumRankSum, SkillProgramSubsetAiOrder aiOrder, bool allowFewerWhenInsufficient, bool onePerSuit) => getCoreGameEngineProgramLifecycleRuntime().SelectProgramCardSubset(frameId, ownerSeat, sourceBind, resultBind, minimumCards, maximumCards, maximumRankSum, aiOrder, allowFewerWhenInsufficient, onePerSuit);
    public SkillProgramStepOutcome MoveBoundCards(long frameId, int ownerSeat, string sourceBind, string? exceptBind, SkillProgramCardDestination destination, CardZoneKind? destinationZone, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().MoveProgramBoundCards(frameId, ownerSeat, sourceBind, exceptBind, destination, destinationZone, reason);
    public SkillProgramStepOutcome SelectTarget(long frameId, int ownerSeat, SkillProgramTargetKind targetKind, IReadOnlyList<CardZoneKind> zones) => getCoreGameEngineProgramLifecycleRuntime().SelectProgramTarget(frameId, ownerSeat, targetKind, zones, marker: null);
    public SkillProgramStepOutcome SelectTarget(long frameId, int ownerSeat, SkillProgramTargetKind targetKind, IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind? marker, ProgramParticipantReference? actorReference = null, bool skipIfNoTarget = false) => getCoreGameEngineProgramLifecycleRuntime().SelectProgramTarget(frameId, ownerSeat, targetKind, zones, marker, actorReference, skipIfNoTarget);
    public void ChangeAttributedMarker(ProgramSkillFrame frame, ProgramParticipantReference target, PlayerMarkerKind marker, int amount) => getCoreGameEngineDeathProgramsRuntime().ChangeProgramAttributedMarker(frame, target, marker, amount);
    public SkillProgramStepOutcome CauseDeathUnlessBoundCardKind(ProgramSkillFrame frame, string sourceBind, IReadOnlyList<CardKind> excludedCardKinds) => getCoreGameEngineDeathProgramsRuntime().CauseProgramDeathUnlessBoundCardKind(frame, sourceBind, excludedCardKinds);
    public SkillProgramStepOutcome SelectTargets(long frameId, int ownerSeat, SkillProgramTargetKind targetKind, int minimumTargets, int maximumTargets, SkillProgramNumberExpression? numberExpression, SkillProgramTargetAiOrder aiOrder) => getCoreGameEngineProgramLifecycleRuntime().SelectProgramTargets(frameId, ownerSeat, targetKind, minimumTargets, maximumTargets, numberExpression, aiOrder);
    public SkillProgramStepOutcome SelectSourceCard(long frameId, int ownerSeat, SkillProgramCardSource cardSource, IReadOnlyList<CardZoneKind> zones, string resultBind, IReadOnlyList<EquipmentSlot> equipmentSlots, bool skipIfNoCards, bool allowSameSource) => getCoreGameEngineProgramLifecycleRuntime().SelectProgramSourceCard(frameId, ownerSeat, cardSource, zones, resultBind, equipmentSlots, skipIfNoCards, allowSameSource);
    public SkillProgramStepOutcome GiveBoundCard(long frameId, int ownerSeat, string sourceBind, SkillProgramTargetKind targetKind, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().GiveProgramBoundCard(frameId, ownerSeat, sourceBind, targetKind, reason);
    public SkillProgramStepOutcome DistributeOwnedCards(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, string sourceBind, SkillProgramTargetKind targetKind, bool allowDeclineBeforeFirst, CardMoveReason reason) => getCoreGameEngineProgramOwnedCardDistributionsRuntime().DistributeProgramOwnedCards(frame, zones, sourceBind, targetKind, allowDeclineBeforeFirst, reason);
    public SkillProgramStepOutcome RequestAttackRangeAid(ProgramSkillFrame frame, CardMoveReason reason) => getCoreGameEngineProgramAttackRangeAidRuntime().RequestProgramAttackRangeAid(frame, reason);
    public void ClaimDamageCards(long frameId, int ownerSeat, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().ClaimProgramDamageCards(frameId, ownerSeat, reason);
    public void TakeRandomHandCardFromSelectedTargets(long frameId, int ownerSeat, int amountPerTarget, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().TakeProgramRandomHandCards(frameId, ownerSeat, amountPerTarget, reason);
    public void TakeRandomCardFromEveryOtherCharacter(long frameId, int ownerSeat, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().TakeProgramRandomCardFromEveryOtherCharacter(frameId, ownerSeat, zones, reason);
    public void AdjustNormalDrawByLivingFactionCount(ProgramSkillFrame frame) => getCoreGameEngineProgramLifecycleRuntime().AdjustProgramNormalDraw(frame, getCoreGameEngineRuleQueriesRuntime().GetLivingFactionCount());
    public void AdjustNormalDrawByPlannedCount(ProgramSkillFrame frame) => getCoreGameEnginePlannedPhaseQuantitiesRuntime().AdjustProgramNormalDrawByPlannedCount(frame);
    public void AdjustNormalDraw(ProgramSkillFrame frame, int amount) => getCoreGameEngineProgramLifecycleRuntime().AdjustProgramNormalDraw(frame, amount);
    public void GrantTurnCardDamageModifier(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, int amount, SkillProgramDamageModifierExpiration expiration, SkillProgramDamageModifierSourceScope sourceScope) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnCardDamageModifier(frame, cardKinds, amount, expiration, sourceScope);
    public void GrantTurnCardActionProhibition(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, IReadOnlyList<CardActionType> actionTypes) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnCardActionProhibition(frame, cardKinds, actionTypes);
    public void GrantTurnHandColorRestriction(ProgramSkillFrame frame, string sourceBind, int targetSeat, bool useFrozenSuit = false) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnHandColorRestriction(frame, sourceBind, targetSeat, useFrozenSuit);
    public void GrantTurnHandCardProhibition(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnHandCardProhibition(frame, targetSeat);
    public void AbolishOwnerAreas(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones) => getCoreProgramAbolishAreasOperationRuntime().AbolishProgramOwnerAreas(frame, zones);
    public void LoseDeathSourceSkills(ProgramSkillFrame frame) => getCoreProgramLoseDeathSourceSkillsOperationRuntime().LoseProgramDeathSourceSkills(frame);
    public void PreventCurrentDamage(ProgramSkillFrame frame) => getCoreGameEngineDamagePreventionProgramsRuntime().PreventProgramCurrentDamage(frame);
    public void NullifyCurrentCardEffect(ProgramSkillFrame frame) => getCoreGameEngineProgramCardTargetEffectsRuntime().NullifyCurrentProgramCardEffect(frame);
    public void NullifySelectedCardEffects(ProgramSkillFrame frame) => getCoreGameEngineProgramCardTargetEffectsRuntime().NullifySelectedProgramCardEffects(frame);
    public void ProhibitCurrentResponse(ProgramSkillFrame frame) => getCoreGameEngineProgramAttackEffectsRuntime().ProhibitCurrentProgramResponse(frame);
    public void RedirectCurrentAttack(ProgramSkillFrame frame, int targetSeat) => getCoreGameEngineProgramAttackEffectsRuntime().RedirectCurrentProgramAttack(frame, targetSeat);
    public void GrantTurnRuleModifier(ProgramSkillFrame frame, SkillRuleQuery query, SkillRuleOperation operation, int amount, IReadOnlyList<CardKind> cardKinds, PlayerMarkerKind? amountFromMarker = null) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnRuleModifier(frame, query, operation, amount, cardKinds, amountFromMarker);
    public void GrantTurnCardTargetRestriction(ProgramSkillFrame frame, SkillProgramCardTargetRestriction restriction) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnCardTargetRestriction(frame, restriction, frame.OwnerSeat);
    public void GrantTurnCardTargetRestriction(ProgramSkillFrame frame, SkillProgramCardTargetRestriction restriction, int targetSeat) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnCardTargetRestriction(frame, restriction, targetSeat);
    public void GrantTurnCardConversion(ProgramSkillFrame frame, string sourceBind, SkillProgramCardColorRelation colorRelation, CardKind outputKind) => getCoreGameEngineCardUseModulesRuntime().GrantProgramTurnCardConversion(frame, sourceBind, colorRelation, outputKind);
    public SkillProgramStepOutcome SelectAndMoveOwnedCard(ProgramSkillFrame frame, ProgramParticipantReference chooser, ProgramParticipantReference cardOwner, IReadOnlyList<CardZoneKind> zones, SkillProgramCardDestination destination, ProgramParticipantReference? destinationRef, string? resultBind, CardMoveReason reason, IReadOnlyList<SkillProgramCardCategory>? cardCategories = null, bool skipIfNoCards = false, bool allowSameOwnerHandReturn = false, string? coverageResultBind = null, bool awaitMovementTriggers = false, bool revealBeforeMove = false, IReadOnlyList<CardKind>? cardKinds = null, bool prohibitReplacingEquipment = false, IReadOnlyList<Suit>? suits = null) => getCoreGameEngineProgramCardPaymentsRuntime().SelectAndMoveProgramOwnedCard(frame, chooser, cardOwner, zones, destination, destinationRef, resultBind, reason, cardCategories, skipIfNoCards, allowSameOwnerHandReturn, coverageResultBind, awaitMovementTriggers, revealBeforeMove, cardKinds, prohibitReplacingEquipment, suits);
    public SkillProgramStepOutcome ChooseOtherOwnedCardDiscard(ProgramSkillFrame frame, ProgramParticipantReference chooser, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => getCoreGameEngineProgramOtherOwnedCardDiscardsRuntime().ChooseProgramOtherOwnedCardDiscard(frame, chooser, zones, reason);
    public SkillProgramStepOutcome RestorePhaseHandDiscards(ProgramSkillFrame frame, ProgramParticipantReference chooser, ProgramParticipantReference phaseOwner) => getCoreGameEngineProgramPhaseHandDiscardsRuntime().RestoreProgramPhaseHandDiscards(frame, chooser, phaseOwner);
    public void RefundCardUseDebit(ProgramSkillFrame frame) => getCoreGameEngineCardUseDebitsRuntime().RefundProgramCardUseDebit(frame);
    public SkillProgramStepOutcome StartPindian(ProgramSkillFrame frame, ProgramParticipantReference opponentReference, string resultBind, SkillProgramCardSetVisibility visibility) => getCoreGameEngineProgramLifecycleRuntime().StartProgramPindian(frame, opponentReference, resultBind, visibility);
    public void SetBooleanState(ProgramSkillFrame frame, string stateId, bool value) => getCoreGameEngineProgramSkillChangesRuntime().SetProgramBooleanState(frame, stateId, value);
    public void ToggleBooleanState(ProgramSkillFrame frame, string stateId) => getCoreGameEngineProgramSkillChangesRuntime().SetProgramBooleanState(frame, stateId, !getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState(frame, stateId));
    public void GrantDirectedTurnCardPolicy(ProgramSkillFrame frame, ProgramParticipantReference actorReference, ProgramParticipantReference targetReference, IReadOnlyList<CardKind> cardKinds, DirectedTurnCardPolicyEffect effects) => getCoreDirectedTurnCardPoliciesRuntime().GrantProgramDirectedTurnCardPolicy(frame, actorReference, targetReference, cardKinds, effects);
}
