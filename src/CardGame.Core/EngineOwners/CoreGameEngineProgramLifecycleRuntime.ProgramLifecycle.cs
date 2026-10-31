#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramLifecycleRuntime
{
    internal ProgramInteractionRouter ProgramInteractions => _programInteractionsOperations._programInteractions ??= new(ProgramInteractionCatalog.Default, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler, ResolveProgramActivationChoice, _modeRuntimeOperations._modeRuntime.PromptAnswers);

    [ProgramAnswerBefore(1)]
    internal bool TryResolveRoundOnceProgramAnswer(PromptChoice selected) => _coreRoundOnceCardlessProgramRequestsOperations.TryResolveRoundOnceCardlessProgramSlash(selected, false);
    [ProgramAiChoice(SkillProgramEffectOp.ChooseHpBoundOtherAndDealRandomDamage)]
    internal PromptChoice SelectAiLangxiChoice(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => _characterLiJueOlOperations.LangxiChoices(frame).First();
    [ProgramAiChoice(SkillProgramEffectOp.AddTargetForQirangPhysicalTrick)]
    internal PromptChoice SelectAiQirangExtraChoice(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => _characterZhugeGuoOlOperations.Value.QirangExtraChoices(frame).Last();
    [ProgramAiChoice(SkillProgramEffectOp.SelectTargets)]
    internal PromptChoice SelectAiConfiguredProgramTargets(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => effect.TargetAiOrder is { } order ? SelectAiProgramTargets(decision, order) : throw ProgramAiChoiceSelector.UnsupportedInstruction(effect);
    [ProgramAiChoice(SkillProgramEffectOp.SelectAndMoveOwnedCard)]
    internal PromptChoice SelectAiCoverageAwareOwnedCard(PendingDecision decision, ProgramSkillFrame frame, SkillProgramEffect effect) => ProgramAiChoiceSelector.SelectByCoverage(decision, effect, cardId => _coreProgramCardPaymentsOperations.WouldEquipmentRemovalReduceCoverage(_coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.CardOwnerRef!), cardId));
    internal SkillProgramStepOutcome StartProgramPindian(ProgramSkillFrame frame, ProgramParticipantReference opponentReference, string resultBind, SkillProgramCardSetVisibility visibility) => _programPindianOperations.Get().Start(frame, opponentReference, resultBind, visibility);
    internal void BeginNormalTurnStartAfterProgramBindings(CharacterState current)
    {
        _coreActualTurnsOperations.BeginTurnStartAfterProgramLifecycle(current);
    }

    internal void CompleteCurrentPlayPhase()
    {
        if (TryBeginPlayEndingProgramWindow(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat]))
            return;
        CompletePlayPhaseAfterProgramWindow();
    }

    internal void CompletePlayPhaseAfterProgramWindow()
    {
        if (_coreActualPhaseEndsOperations.TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind.Play, ActualPhaseEndContinuation.Play))
            return;
        _corePlayPhaseSkillGrantsOperations.CloseCurrentPlayPhaseSkillGrants("play-phase-ended");
        _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess._programPhaseColorRestrictions.Clear();
        _coreStrategicProgramsOperations.ResetPlayPhaseProgramStates(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        if (_characterLuBuOlMagicOperations.TryFinishMoLuBuOpeningPlay())
            return;
        if (ResumeScheduledProgramPhase())
            return;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat].IsAlive)
        {
            // Paid Play-ending children have returned. A dead actor has no
            // actual Discard or Ending phase; keep turn expiry and cursor
            // progression on the existing finalizer after all phase cleanup.
            _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
            if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
                _coreActualTurnsOperations.FinalizeEndTurn(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat]);
            else
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        _coreActualDiscardPhaseOperations.BeginDiscardPhase();
    }

    internal ProgramSkillFrame GetActiveProgramFrame(long frameId) => _resolutionSchedulerOperations._resolutionScheduler.ActiveProgram(frameId);
    internal SkillProgramTrigger GetProgramTrigger(ProgramSkillFrame frame)
    {
        var triggerId = frame.TriggerId ?? throw new InvalidOperationException("The active program frame is not a trigger binding.");
        var program = _coreActivityOrdinaryTricksOperations.ActivityRuntimeSkillDefinition(frame.SkillId).Program ?? throw new InvalidOperationException("The active program binding has no definition.");
        if (program.GameplayHash != frame.GameplayHash)
            throw new InvalidOperationException("The active program binding definition changed.");
        return ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Trigger, triggerId).Trigger!;
    }

    internal static string GetProgramBindingId(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame);
    internal void SetProgramCardSet(long frameId, string bind, IReadOnlyList<int> cardIds, SkillProgramCardSetVisibility visibility, IReadOnlyList<CardLocation>? sourceLocations = null, Suit? frozenRevealedSuit = null, int? selectionActorSeat = null) => _programBoundCardOperations.Get().SetProgramCardSet(frameId, bind, cardIds, visibility, sourceLocations, frozenRevealedSuit, selectionActorSeat);
    internal ProgramSkillCardSetBinding GetProgramCardSet(ProgramSkillFrame frame, string bind) => ProgramBoundCardRuntime.GetProgramCardSet(frame, bind);
    internal SkillProgramStepOutcome ScheduleProgramPhase(ProgramSkillFrame frame, TurnPhase phase, SkillProgramPhaseContinuation continuation, int? beneficiarySeat = null) => _programPhaseSchedulingOperations.Get().ScheduleProgramPhase(frame, phase, continuation, beneficiarySeat);
    internal bool IsEndOwnedExtraPlayPhase() => _programPhaseSchedulingOperations.Get().IsEndOwnedExtraPlayPhase();
    internal void ExpireEndOwnedPlayEffects(ProgramSkillFrame frame) => _programPhaseSchedulingOperations.Get().ExpireEndOwnedPlayEffects(frame);
    internal bool TryBeginZhangYiDianjunExtraPlay(ProgramSkillFrame frame) => _programPhaseSchedulingOperations.Get().TryBeginZhangYiDianjunExtraPlay(frame);
    internal bool ResumeScheduledProgramPhase() => _programPhaseSchedulingOperations.Get().ResumeScheduledProgramPhase();
    internal void CancelScheduledProgramPhase() => _programPhaseSchedulingOperations.Get().CancelScheduledProgramPhase();
    internal void RecoverProgramTargetTo(long frameId, int ownerSeat, int targetSeat, SkillProgramNumberExpression expression, int minimumValue, bool clampToMaxHp) => _programHealthStateOperations.Get().RecoverProgramTargetTo(frameId, ownerSeat, targetSeat, expression, minimumValue, clampToMaxHp);
    internal void DiscardProgramOwnedZoneCards(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _programOwnedZoneDiscardOperations.Get().Discard(frame, zones, reason);
    internal void SetProgramChainedState(ProgramSkillFrame frame, bool chained, int targetSeat) => _programHealthStateOperations.Get().SetProgramChainedState(frame, chained, targetSeat);
    internal void ChangeProgramMaximumHp(ProgramSkillFrame frame, int amount, string? sourceSkillId = null) => _programHealthStateOperations.Get().ChangeProgramMaximumHp(frame, amount, sourceSkillId);
    internal void ChangeProgramParticipantMaximumHp(ProgramSkillFrame frame, int recipientSeat, int amount, string? sourceSkillId = null) => _programHealthStateOperations.Get().ChangeProgramParticipantMaximumHp(frame, recipientSeat, amount, sourceSkillId);
    internal void TurnOverProgramTarget(long frameId, int ownerSeat, int targetSeat) => _programHealthStateOperations.Get().TurnOverProgramTarget(frameId, ownerSeat, targetSeat);
    internal void SetProgramTargetFaceState(long frameId, int ownerSeat, int targetSeat, bool faceDown) => _programHealthStateOperations.Get().SetProgramTargetFaceState(frameId, ownerSeat, targetSeat, faceDown);
    internal SkillProgramStepOutcome StartProgramJudgment(ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, SkillProgramCardSetVisibility visibility, int sourceSeat) => _programJudgmentOperations.Get().StartProgramJudgment(frame, targetSeat, reason, resultBind, visibility, sourceSeat);
    internal bool IsValidProgramJudgmentContinuation(JudgmentFrame pending, ProgramSkillFrame? frame) => _programJudgmentOperations.Get().IsValidProgramJudgmentContinuation(pending, frame);
    internal bool MatchesProgramJudgmentInstruction(ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, SkillProgramCardSetVisibility visibility) => _programJudgmentOperations.Get().MatchesProgramJudgmentInstruction(frame, targetSeat, reason, resultBind, visibility);
    internal void RevealProgramTopCards(long frameId, int ownerSeat, int amount, SkillProgramNumberExpression? numberExpression, string resultBind, SkillProgramCardSetVisibility visibility) => _programBoundCardOperations.Get().RevealProgramTopCards(frameId, ownerSeat, amount, numberExpression, resultBind, visibility);
    internal void FilterProgramBoundCards(long frameId, string sourceBind, string resultBind, IReadOnlyList<Suit> suits, ProgramParticipantReference? effectiveSuitFor, IReadOnlyList<SkillProgramCardCategory> categories, IReadOnlyList<EquipmentSlot> equipmentSlots, IReadOnlyList<CardKind> cardKinds, string? matchSuitOfBind, bool availableAtSourceOnly = false) => _programBoundCardOperations.Get().FilterProgramBoundCards(frameId, sourceBind, resultBind, suits, effectiveSuitFor, categories, equipmentSlots, cardKinds, matchSuitOfBind, availableAtSourceOnly);
    internal void DrawProgramCards(long frameId, int targetSeat, int amount, SkillProgramNumberExpression? numberExpression, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason) => _programDrawOperations.Get().DrawProgramCards(frameId, targetSeat, amount, numberExpression, resultBind, visibility, reason);
    internal int? ResolveProgramDrawCount(ProgramSkillFrame frame, int targetSeat, int amount, SkillProgramNumberExpression? numberExpression) => _programDrawOperations.Get().ResolveProgramDrawCount(frame, targetSeat, amount, numberExpression);
    internal void CompleteProgramDrawCards(long frameId, int targetSeat, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason, long fixedRecipientSequenceBefore, IReadOnlyList<int> drawn) => _programDrawOperations.Get().CompleteProgramDrawCards(frameId, targetSeat, resultBind, visibility, reason, fixedRecipientSequenceBefore, drawn);
    internal void DrawProgramSelectedTargets(long frameId, int amount, CardMoveReason reason) => _programDrawOperations.Get().DrawProgramSelectedTargets(frameId, amount, reason);
    internal IReadOnlyList<int> GetProgramTargetSeats(int ownerSeat, SkillProgramTargetKind targetKind, ProgramSkillWindowContext? windowContext = null, PlayerMarkerKind? marker = null, ProgramParticipantReference? actorReference = null) => _programTargetSelectionOperations.Get().GetProgramTargetSeats(ownerSeat, targetKind, windowContext, marker, actorReference);
    internal bool IsProgramTargetEligible(int ownerSeat, SkillProgramTargetKind targetKind, IReadOnlyList<CardZoneKind> zones, int targetSeat, PlayerMarkerKind? marker = null, ProgramSkillWindowContext? windowContext = null, ProgramParticipantReference? actorReference = null) => _programTargetSelectionOperations.Get().IsProgramTargetEligible(ownerSeat, targetKind, zones, targetSeat, marker, windowContext, actorReference);
    internal SkillProgramStepOutcome SelectProgramTargets(long frameId, int ownerSeat, SkillProgramTargetKind targetKind, int minimumTargets, int maximumTargets, SkillProgramNumberExpression? numberExpression, SkillProgramTargetAiOrder aiOrder) => _programTargetSelectionOperations.Get().SelectProgramTargets(frameId, ownerSeat, targetKind, minimumTargets, maximumTargets, numberExpression, aiOrder);
    internal int GetProgramPlannedNormalDrawCount(ProgramSkillFrame frame, bool allowReplacedNoOp = false) => _programDrawOperations.Get().GetProgramPlannedNormalDrawCount(frame, allowReplacedNoOp);
    internal void TakeProgramRandomHandCards(long frameId, int ownerSeat, int amountPerTarget, CardMoveReason reason) => _programRandomCardAcquisitionOperations.Get().TakeProgramRandomHandCards(frameId, ownerSeat, amountPerTarget, reason);
    internal void TakeProgramRandomCardFromEveryOtherCharacter(long frameId, int ownerSeat, IReadOnlyList<CardZoneKind> zones, CardMoveReason reason) => _programRandomCardAcquisitionOperations.Get().TakeProgramRandomCardFromEveryOtherCharacter(frameId, ownerSeat, zones, reason);
    internal SkillProgramStepOutcome SelectProgramTarget(long frameId, int ownerSeat, SkillProgramTargetKind targetKind, IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind? marker, ProgramParticipantReference? actorReference = null, bool skipIfNoTarget = false) => _programTargetSelectionOperations.Get().SelectProgramTarget(frameId, ownerSeat, targetKind, zones, marker, actorReference, skipIfNoTarget);
    internal SkillProgramStepOutcome SelectProgramSourceCard(long frameId, int ownerSeat, SkillProgramCardSource cardSource, IReadOnlyList<CardZoneKind> zones, string resultBind, IReadOnlyList<EquipmentSlot> equipmentSlots, bool skipIfNoCards, bool allowSameSource) => _programBoundCardOperations.Get().SelectProgramSourceCard(frameId, ownerSeat, cardSource, zones, resultBind, equipmentSlots, skipIfNoCards, allowSameSource);
    internal SkillProgramStepOutcome GiveProgramBoundCard(long frameId, int ownerSeat, string sourceBind, SkillProgramTargetKind targetKind, CardMoveReason reason) => _programBoundCardOperations.Get().GiveProgramBoundCard(frameId, ownerSeat, sourceBind, targetKind, reason);
    internal void ClaimProgramDamageCards(long frameId, int ownerSeat, CardMoveReason reason) => _programDamageCardClaimOperations.Get().Claim(frameId, ownerSeat, reason);
    internal Card[] GetClaimableProgramDamageCards(ProgramSkillFrame frame) => _programDamageCardClaimOperations.Get().GetClaimable(frame);
    internal SkillProgramStepOutcome SelectProgramCardSubset(long frameId, int ownerSeat, string sourceBind, string resultBind, int minimumCards, int maximumCards, int maximumRankSum, SkillProgramSubsetAiOrder aiOrder, bool allowFewerWhenInsufficient, bool onePerSuit, bool availableAtSourceOnly = false) => _programBoundCardOperations.Get().SelectProgramCardSubset(frameId, ownerSeat, sourceBind, resultBind, minimumCards, maximumCards, maximumRankSum, aiOrder, allowFewerWhenInsufficient, onePerSuit, availableAtSourceOnly);
    internal void CancelProgramBindingAndCleanup(ProgramSkillFrame frame, string reason)
    {
        _coreMatchLogOperations.AddLog("SkillCancelled", reason, frame.OwnerSeat);
        _coreSkillProgramsOperations.FinishProgramSkill(frame, completed: false);
    }

    internal void CleanupProgramBoundCards(ProgramSkillFrame frame, bool completed) => _programBoundCardOperations.Get().CleanupProgramBoundCards(frame, completed);
    internal SkillProgramStepOutcome MoveProgramBoundCards(long frameId, int ownerSeat, string sourceBind, string? exceptBind, SkillProgramCardDestination destination, CardZoneKind? destinationZone, CardMoveReason reason, IReadOnlyList<int>? bottomOrder = null) => _programBoundCardOperations.Get().MoveProgramBoundCards(frameId, ownerSeat, sourceBind, exceptBind, destination, destinationZone, reason, bottomOrder);
    internal void GrantProgramTurnSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds)
    {
        var active = GetActiveProgramFrame(frame.Id);
        if (active.OwnerSeat != frame.OwnerSeat || active.SkillId != frame.SkillId || active.TriggerId != frame.TriggerId)
            throw new InvalidOperationException("Turn-scoped skill grants require the active program binding.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var granted = skillIds.Distinct(StringComparer.Ordinal).ToArray();
        foreach (var skillId in granted)
            _ = _contentRegistryOperations._contentRegistry!.GetSkill(skillId);
        var sourceId = $"turn:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)}:{(frame.SkillId)}:{(GetProgramBindingId(frame))}";
        foreach (var skillId in granted)
        {
            var grantId = $"{(sourceId)}:{(skillId)}";
            if (!owner.SkillGrants.Grants.Any(grant => grant.GrantId == grantId))
                owner.SkillGrants.Grant(new SkillGrant(grantId, skillId, grantId, sourceId));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnSkillsGrantedEvent(frame.Id, frame.SkillId, GetProgramBindingId(frame), owner.Seat, Array.AsReadOnly(granted)));
    }

    internal bool HasPendingProgramBoundCards => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SelectMany(frame => frame.CardSetBindings).SelectMany(binding => binding.CardIds).Distinct().Any(cardId => _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.Processing);

    internal IReadOnlyList<ProgramTriggerCandidate> CollectProgramTriggerCandidates(CharacterState owner, SkillProgramTriggerWindow window, int occurrenceIndex = 0) => _programTriggerOperations.Get().CollectProgramTriggerCandidates(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), owner, window, occurrenceIndex);
    internal SkillProgramTrigger GetProgramTrigger(ProgramTriggerCandidate candidate) => _programTriggerOperations.Get().GetProgramTrigger(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), candidate);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(CharacterState owner, SkillProgramTriggerWindow window, SkillProgramTriggerFacts facts) => _programTriggerOperations.Get().CollectEligibleProgramTriggerCandidates(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), owner, window, facts);
    internal bool CanRunProgramTrigger(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _programTriggerOperations.Get().CanRunProgramTrigger(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), candidate, context);
    internal void ConsumeProgramTriggerUsage(ProgramTriggerCandidate candidate) => _programTriggerOperations.Get().ConsumeProgramTriggerUsage(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), candidate);
    internal string ProgramTriggerUsageId(ProgramTriggerCandidate candidate) => _programTriggerOperations.Get().ProgramTriggerUsageId(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), candidate);
    internal (int Value, bool Undefined, int? FixedMinimum, IReadOnlyList<SkillProgramUnselectedDualHpFacts>? Unselected) CaptureProgramLivingHpFacts() => _programTriggerFactProjectorOperations.Get().CaptureLivingHp(_kernelCoreProgramTriggerFactProjectorQueriesOperations.CreateProgramTriggerFactQueries());
    internal SkillProgramTriggerFacts CaptureProgramTriggerFacts(CharacterState owner) => _programTriggerFactProjectorOperations.Get().Capture(_kernelCoreProgramTriggerFactProjectorQueriesOperations.CreateProgramTriggerFactQueries(), owner);
    internal SkillProgramTriggerFacts CaptureProgramTriggerFacts(CharacterState owner, CardActionContext action) => _programTriggerFactProjectorOperations.Get().Capture(_kernelCoreProgramTriggerFactProjectorQueriesOperations.CreateProgramTriggerFactQueries(), _kernelCoreProgramTriggerFactProjectorQueriesOperations.CreateProgramTriggerActionFactQueries(), owner, action);
    internal void BeginProgramBinding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _programBindingOperations.Get().Begin(candidate, context);
    internal void ExposeProgramTriggerDecision(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _programBindingOperations.Get().Expose(candidate, context);
    internal bool HasLivingProgramTriggerBindings(SkillProgramTriggerWindow window) => _programTurnWindowEntryOperations.Get().HasLivingProgramTriggerBindings(window);
    internal bool TryBeginTurnStartProgramWindow(CharacterState owner) => _programTurnWindowEntryOperations.Get().TryBeginTurnStartProgramWindow(owner);
    internal bool TryBeginRoundStartingProgramWindow(CharacterState turnOwner) => _programTurnWindowEntryOperations.Get().TryBeginRoundStartingProgramWindow(turnOwner);
    internal bool TracksForeignTurnStartPrograms => _programTurnWindowEntryOperations.Get().TracksForeignTurnStartPrograms;

    internal IReadOnlyList<ProgramTriggerCandidate> CollectForeignTurnStartCandidates(CharacterState turnOwner, SkillProgramTriggerFacts turnOwnerFacts) => _programTurnWindowEntryOperations.Get().CollectForeignTurnStartCandidates(turnOwner, turnOwnerFacts);
    internal bool TryBeginPlayEndingProgramWindow(CharacterState owner) => _programTurnWindowEntryOperations.Get().TryBeginPlayEndingProgramWindow(owner);
    internal static bool MatchesPlayEndingTurnOwnerScope(SkillProgramTurnOwnerScope scope, bool ownSeat) => ProgramTurnWindowEntryRuntime.MatchesPlayEndingTurnOwnerScope(scope, ownSeat);
    internal static bool IsGuanduForeignDrawTrigger(SkillProgramTrigger trigger) => ProgramDrawDiscardWindowEntryRuntime.IsGuanduForeignDrawTrigger(trigger);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeGuanduForeignDrawObservers(CharacterState subject, IReadOnlyList<ProgramTriggerCandidate> existing, out IReadOnlyDictionary<int, SkillProgramTriggerFacts>? participants) => _programDrawDiscardWindowEntryOperations.Get().IncludeGuanduForeignDrawObservers(subject, existing, out participants);
    internal bool TryBeginDrawPhaseProgramWindow(CharacterState owner, bool skipPlayPhaseAfterDraw) => _programDrawDiscardWindowEntryOperations.Get().TryBeginDrawPhase(owner, skipPlayPhaseAfterDraw);
    internal bool TryBeginAfterNormalDrawProgramWindow(CharacterState owner) => _programDrawDiscardWindowEntryOperations.Get().TryBeginAfterNormalDraw(owner);
    internal bool TryBeginDrawPhaseEndedProgramWindow(CharacterState owner, GameEngine.DelayedTurnEffects delayedEffects) => _programDrawDiscardWindowEntryOperations.Get().TryBeginDrawPhaseEnded(owner, (int)delayedEffects);
    internal bool TryBeginDiscardPhaseProgramWindow(CharacterState owner) => _programDrawDiscardWindowEntryOperations.Get().TryBeginDiscardPhase(owner);
    internal bool TryBeginDiscardPhaseEndedProgramWindow(CharacterState phaseOwner) => _programDrawDiscardWindowEntryOperations.Get().TryBeginDiscardPhaseEnded(phaseOwner);
    internal bool TryBeginTurnEndingBoundary(CharacterState owner) => _programPhaseBoundaryOperations.Get().TryBeginTurnEndingBoundary(owner);
    internal ProgramSkillWindowContext CreateTurnEndingProgramContext(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? candidateFacts = null) => _programPhaseBoundaryOperations.Get().CreateTurnEndingProgramContext(frame, candidate, candidateFacts);
    internal void AdvanceTurnEndingBoundaryCandidate(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate candidate, bool activated, bool completed) => _programPhaseBoundaryOperations.Get().AdvanceTurnEndingBoundaryCandidate(frame, candidate, activated, completed);
    internal void AdvanceTurnEndingBoundaryCursor(TurnEndingBoundaryFrame frame) => _programWindowCursorOperations.Get().AdvanceTurnEndingBoundaryCursor(frame);
    internal bool TryBeginPlayPhaseStartingBoundary(CharacterState owner) => _programPhaseBoundaryOperations.Get().TryBeginPlayPhaseStartingBoundary(owner);
    internal void AdvanceProgramLifecycleCursor(ProgramLifecycleTriggerWindowFrame frame) => _programWindowCursorOperations.Get().AdvanceProgramLifecycleCursor(frame);
    internal IReadOnlyList<ProgramTriggerCandidate> GetTriggerChoiceGroupMembers(IEnumerable<ProgramTriggerCandidate> candidatesFromCursor, ProgramTriggerCandidate first, string choiceGroup) => _programBindingOperations.Get().ChoiceGroup(candidatesFromCursor, first, choiceGroup);
    internal void ExposeProgramTriggerGroupDecision(IReadOnlyList<ProgramTriggerCandidate> members, ProgramTriggerCandidate first, ProgramSkillWindowContext context, string choiceGroup) => _programBindingOperations.Get().ExposeGroup(members, first, context, choiceGroup);
    internal void AdjustProgramNormalDraw(ProgramSkillFrame frame, int amount)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != frame.Id || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting, ParentFrameId: var parentFrameId } || frame.TriggerId is null || amount == 0 || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != parentFrameId || parent.Continuation != ProgramLifecycleContinuation.CompleteDrawPhase)
        {
            throw new InvalidOperationException("A normal-draw adjustment requires the active draw-phase program and its parent window.");
        }

        var total = checked(parent.NormalDrawAdjustment + amount);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[^2].Id, parent with { NormalDrawAdjustment = total });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramNormalDrawAdjustedEvent(frame.Id, frame.SkillId, frame.TriggerId, frame.OwnerSeat, amount, total));
    }

    internal CommandResult SubmitProgramTriggerAnswer(int actorSeat, PromptId promptId, ChoiceId choiceId)
    {
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(actorSeat, DecisionKind.ProgramTrigger, promptId, CommandErrorCode.IllegalAction);
        if (error is not null)
            return _kernelCoreMainOperations.Reject(error.Code, error.Message);
        var selected = _coreProgramParticipantPromptsOperations.GetPendingDecisionForActor(actorSeat)?.Choices.SingleOrDefault(choice => choice.Id == choiceId);
        if (selected?.Parameters.GetValueOrDefault("program-action") == "diamond-delayed" && !_coreDiamondDelayedProgramsOperations.Value.IsDiamondDelayedChoiceLegal(selected))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The diamond delayed payment or target is no longer legal.");
        if (selected is null)
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The program choice is unavailable.");
        if (selected.Parameters.GetValueOrDefault("program-action") == "phase-hand-debt-return" && !_corePhaseHandSeizureReceiptsOperations.IsPhaseHandDebtChoiceLegal(selected))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The issued phase-hand return is no longer legal.");
        if (selected.Parameters.GetValueOrDefault("program-action")is "alternative-phase-cost-card" or "alternative-phase-cost-marker" && !_coreAlternativePhaseCostProgramsOperations.Value.IsAlternativePhaseCostChoiceLegal(selected))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The alternative phase cost is no longer payable.");
        if (selected.Parameters.GetValueOrDefault("program-action") == "slash-suit-discard" && !_coreSlashSuitDiscardOperations.Value.IsProgramSlashSuitDiscardChoiceLegal(selected))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The actual HE suit discard is no longer legal.");
        if (selected.Parameters.GetValueOrDefault("program-action") == "choose-option" && !_coreProgramChoicesOperations.IsClaimableProgramOptionStillAvailable(selected))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The damage cards are no longer available for this choice.");
        return _kernelCoreMainOperations.Accept(() =>
        {
            ProgramInteractions.Resolve(selected);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    [ProgramAnswer("current-turn-hand-exchange")]
    internal void ResolveCurrentTurnHandExchangeAnswer(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The current-turn hand exchange choice lost its owning program.");
        _coreCurrentTurnHandExchangeOperations.Value.ResolveCurrentTurnHandExchangeChoice(frame, selected);
    }

    [ProgramAnswer("prep-discard-ending")]
    internal void ResolvePrepDiscardEndingAnswer(PromptChoice selected)
    {
        _corePrepDiscardPaymentsOperations.ResolvePrepDiscardChoice(_resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The preparation choice lost its owning program."), selected);
    }

    [ProgramAnswer("order-bound-cards")]
    internal void ResolveProgramBoundCardOrderAnswer(PromptChoice selected) => _programBoundCardOperations.Get().ResolveProgramBoundCardOrderAnswer(selected);
    [ProgramAnswer("select-subset")]
    internal void ResolveProgramCardSubsetAnswer(PromptChoice selected) => _programBoundCardOperations.Get().ResolveProgramCardSubsetAnswer(selected);
    internal void ResolveProgramActivationChoice(PromptChoice selected) => _programBindingOperations.Get().ResolveActivation(selected);
    [ProgramAnswer("sole-target-protection", "pair-obtain", "shown-pair-gift", "equipment-donation", "actual-ended-equipment", "limited-owned-gift", "finish-limited-owned-gift", "select-target", "select-targets", "select-source-card", "select-and-move-owned-card", "choose-other-owned-card-discard", "choose-other-owned-card-decline", "restore-phase-hand-discard", "restore-phase-hand-decline", "choose-own-card-discard", "select-owned-cards", "finish-owned-cards", "give-bound-card", "keep-bound-cards", "distribute-owned-card", "decline-owned-card-distribution", "reveal-target-hand-card", "reveal-target-hand-card-decline", "request-slash", "request-slash-decline", "request-slash-nearest", "request-slash-nearest-decline", "nearest-legal-slash", "unlimited-virtual-slash", "damage-judgment-suit-payment", "attack-range-aid-discard-weapon", "attack-range-aid-draw")]
    internal void ResolveProgramInstructionChoice(PromptChoice selected, string action)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The program instruction choice lost its frame.");
        if (selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || frame.InstructionIndex == 0)
            throw new InvalidOperationException("The program instruction choice has a stale frame identity.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        switch (action)
        {
            case "sole-target-protection":
                _coreSoleTargetProtectionOperations.Value.ResolveSoleProtectionChoice(selected);
                return;
            case "pair-obtain":
                _corePairObtainShownGiftsOperations.ResolvePairObtainChoice(selected);
                return;
            case "shown-pair-gift":
                _corePairObtainShownGiftsOperations.ResolveShownPairGiftChoice(selected);
                return;
            case "equipment-donation":
                _coreEquipmentDonationProgramsOperations.ResolveEquipmentDonationChoice(frame, selected);
                return;
            case "actual-ended-equipment":
                _coreActualEndedTurnEquipmentOperations.ResolveActualEndedEquipmentChoice(frame, selected);
                return;
            case "limited-owned-gift":
            case "finish-limited-owned-gift":
                _coreLostHpOwnedGiftsOperations.Value.ResolveLostHpOwnedGift(frame, effect, selected);
                return;
            case "select-target":
                _programTargetSelectionOperations.Get().ResolveTargetChoice(frame, effect, selected);
                return;
            case "select-targets":
                _programTargetSelectionOperations.Get().ResolveTargetsChoice(frame, effect, selected);
                return;
            case "reveal-target-hand-card":
            {
                _coreProgramRevealsOperations.ResolveProgramRevealCardSelection(selected);
                return;
            }

            case "reveal-target-hand-card-decline":
            {
                _coreProgramRevealsOperations.ResolveProgramRevealCardSelection(selected);
                return;
            }

            case "select-source-card":
                _programBoundCardOperations.Get().ResolveSourceCardChoice(frame, effect, selected);
                return;
            case "select-owned-cards":
            case "finish-owned-cards":
                if (effect.Op == SkillProgramEffectOp.HoldTargetCards)
                {
                    if (selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || frame.InstructionIndex == 0)
                        throw new InvalidOperationException("The card-hold choice has a stale frame identity.");
                    _coreProgramHoldCardsOperations.Value.ResolveProgramHoldCardSelection(frame, selected);
                    return;
                }

                _coreProgramOwnedCardsOperations.ResolveProgramOwnedCardSelection(frame, effect, selected);
                return;
            case "distribute-owned-card":
            case "decline-owned-card-distribution":
                _coreProgramOwnedCardDistributionsOperations.Value.ResolveProgramOwnedCardDistribution(frame, effect, selected);
                return;
            case "attack-range-aid-discard-weapon":
            case "attack-range-aid-draw":
                _coreProgramAttackRangeAidOperations.ResolveProgramAttackRangeAid(frame, effect, selected);
                return;
            case "select-and-move-owned-card":
            {
                if (effect.Op != SkillProgramEffectOp.SelectAndMoveOwnedCard)
                    throw new InvalidOperationException("The payment choice does not match the suspended instruction.");
                _coreProgramCardPaymentsOperations.ResolveSelectAndMoveOwnedCardChoice(frame, effect, selected);
                return;
            }

            case "choose-other-owned-card-discard":
            case "choose-other-owned-card-decline":
                _coreProgramOtherOwnedCardDiscardsOperations.ResolveProgramOtherOwnedCardDiscardChoice(selected);
                return;
            case "restore-phase-hand-discard":
            case "restore-phase-hand-decline":
                _coreProgramPhaseHandDiscardsOperations.Value.ResolveProgramPhaseHandDiscardChoice(selected);
                return;
            case "request-slash":
            case "request-slash-decline":
                _coreProgramSlashRequestOperations.ResolveProgramRequestSlashChoice(selected);
                return;
            case "damage-judgment-suit-payment":
                _coreDamageJudgmentSuitPaymentsOperations.ResolveDamageJudgmentSuitPaymentChoice(selected);
                return;
            case "nearest-legal-slash":
                _coreNearestLegalSlashRequestsOperations.ResolveNearestLegalSlashChoice(selected);
                return;
            case "unlimited-virtual-slash":
                _coreNearestLegalSlashRequestsOperations.ResolveUnlimitedVirtualSlashChoice(selected);
                return;
            case "request-slash-nearest":
            case "request-slash-nearest-decline":
                _coreProgramSlashRequestOperations.ResolveProgramRequestSlashByNearestChoice(selected);
                return;
            case "choose-own-card-discard":
                _coreProgramOwnCardDiscardsOperations.Value.ResolveProgramOwnCardDiscardChoice(selected);
                return;
            case "give-bound-card":
            case "keep-bound-cards":
                _programBoundCardOperations.Get().ResolveBoundCardGiftChoice(frame, effect, selected, action);
                return;
            default:
                throw new InvalidOperationException("Unsupported program instruction choice.");
        }
    }

    internal (ProgramTriggerCandidate Candidate, ProgramSkillWindowContext Context) GetPendingProgramTriggerCandidate() => _programActivationWindowOperations.Get().PendingCandidate();
    internal void CompleteSkippedProgramCandidate(ProgramTriggerCandidate candidate) => _programActivationWindowOperations.Get().CompleteSkipped(candidate);
    internal bool IsAiProgramTriggerPending() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, PlayerSeat: var seat } && !_coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    internal void ResolvePendingAiProgramTrigger()
    {
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } current ? current : throw new InvalidOperationException("The AI program prompt is unavailable.");
        PromptChoice selected;
        if (_coreColdDamageOperations.Value.TryChooseColdDamageAi(decision, out var coldChoice))
            selected = coldChoice;
        else if (_characterChengJiChengCuiOlOperations.TryChooseTousuiAi(decision, out var tousuiChoice))
            selected = tousuiChoice;
        else if (_characterMaXiuMaTieOlOperations.TryChooseKenshangAi(decision, out var kenshangChoice))
            selected = kenshangChoice;
        else if (_characterCaoPiAnniversaryGodOperations.TryChooseKemingEchoAi(decision, out var kemingChoice))
            selected = kemingChoice;
        else if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame frame)
        {
            var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
            selected = ProgramInteractions.SelectAi(decision, frame, paused);
        }
        else
        {
            selected = SelectAiProgramActivation(decision);
        }

        ProgramInteractions.Resolve(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal PromptChoice SelectAiProgramActivation(PendingDecision decision)
    {
        var activateChoices = decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate").ToArray();
        var skip = decision.Choices.SingleOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
        if (activateChoices.Length > 1)
            return SelectAiProgramChoiceGroup(decision, activateChoices, skip);
        var pendingContext = GetPendingProgramTriggerCandidate().Context;
        if (GetProgramTrigger(GetPendingProgramTriggerCandidate().Candidate).Effects is [{ Op: SkillProgramEffectOp.OfferRiceAfterDamagePoint }])
            return activateChoices.Single();
        if (pendingContext.OptionalChooserSeat is not null)
        {
            var(choice, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseOptionalBeneficiaryDraw(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, pendingContext.OwnerSeat, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++);
            _coreNativeAiDecisionsOperations.AddThought(thought);
            return decision.Choices.Single(c => c.Id == choice);
        }

        var activate = activateChoices.Single();
        if (skip is null)
            return activate;
        var skillId = activate.Parameters["skill-id"];
        var bindingId = activate.Parameters["binding-id"];
        var skillInstanceId = activate.Parameters["skill-instance-id"];
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(skillId);
        var trigger = ProgramInstructionResolver.Default.Resolve(skill.Program!, ProgramInstructionSourceKind.Trigger, bindingId).Trigger!;
        var owner = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var publicContext = CoreGameEnginePhaseHandExtraTurnAiRuntime.WithPhaseHandExtraTurnTargetPricing(WithProgramConditionFacts(CreateProgramAiPublicContext(trigger, owner, decision.PlayerSeat), owner, skillId, skillInstanceId), skill.Program!, trigger);
        var estimate = EstimateCompositionForAi(owner, trigger.Effects, publicContext, windowContext: pendingContext).Estimate;
        estimate = _corePhaseHandExtraTurnAiOperations.IncludePhaseHandExtraTurnDeclarationForAi(estimate, owner, skillId, skillInstanceId, skill.Program!, trigger, publicContext, pendingContext);
        var shouldActivate = !estimate.IsSelfLethal && estimate.Score > 0d;
        var activateAction = new LegalAction(LegalActionKind.UseProgramSkill, null, null, $"发动【{(skill.Name)}】");
        var skipAction = new LegalAction(LegalActionKind.UseProgramSkill, null, null, $"跳过【{(skill.Name)}】");
        var candidates = new[]
        {
            new AiCandidateScore(activateAction, shouldActivate ? estimate.Score : Math.Min(estimate.Score, -1000d), estimate.IsSelfLethal ? "公开效果将令自身失去全部体力。" : estimate.Approximation),
            new AiCandidateScore(skipAction, 0d, "保留当前状态。")
        };
        _coreNativeAiDecisionsOperations.AddThought(new AiThoughtRecord(_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, decision.PlayerSeat, shouldActivate ? activateAction.Description : skipAction.Description, candidates.OrderByDescending(candidate => candidate.Score).ToArray(), $"组合估值 {(estimate.Score):0.##}；{(estimate.Approximation)}"));
        return shouldActivate ? activate : skip;
    }

    internal PromptChoice SelectAiProgramChoiceGroup(PendingDecision decision, IReadOnlyList<PromptChoice> activateChoices, PromptChoice? skip)
    {
        var parentOwnerSeat = _resolutionStackOperations._resolutionStack.LastOrDefault() switch
        {
            TurnEndingBoundaryFrame ending => ending.Items[ending.ItemIndex].Candidate?.OwnerSeat ?? -1,
            ProgramLifecycleTriggerWindowFrame lifecycle => lifecycle.Candidates[lifecycle.CandidateIndex].OwnerSeat,
            PlayPhaseStartingBoundaryFrame starting when starting.Items.Skip(starting.ItemIndex).Any(item => item.Candidate?.OwnerSeat == decision.PlayerSeat) => decision.PlayerSeat,
            _ => -1
        };
        if (parentOwnerSeat != decision.PlayerSeat)
            throw new InvalidOperationException("A program choice group lost its lifecycle parent.");
        var owner = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var estimated = activateChoices.Select(choice =>
        {
            var skill = _contentRegistryOperations._contentRegistry!.GetSkill(choice.Parameters["skill-id"]);
            var trigger = ProgramInstructionResolver.Default.Resolve(skill.Program!, ProgramInstructionSourceKind.Trigger, choice.Parameters["binding-id"]).Trigger!;
            var candidateContext = GetPendingProgramTriggerCandidate().Context;
            var publicContext = CoreGameEnginePhaseHandExtraTurnAiRuntime.WithPhaseHandExtraTurnTargetPricing(WithProgramConditionFacts(CreateProgramAiPublicContext(trigger, owner, decision.PlayerSeat), owner, choice.Parameters["skill-id"], choice.Parameters["skill-instance-id"]), skill.Program!, trigger);
            var estimate = EstimateCompositionForAi(owner, trigger.Effects, publicContext, windowContext: candidateContext).Estimate;
            estimate = _corePhaseHandExtraTurnAiOperations.IncludePhaseHandExtraTurnDeclarationForAi(estimate, owner, choice.Parameters["skill-id"], choice.Parameters["skill-instance-id"], skill.Program!, trigger, publicContext, candidateContext);
            var action = new LegalAction(LegalActionKind.UseProgramSkill, null, null, $"发动【{(skill.Name)}】");
            return (Choice: choice, Action: action, Estimate: estimate);
        }).OrderByDescending(item => item.Estimate.IsSelfLethal ? double.NegativeInfinity : item.Estimate.Score).ThenBy(item => item.Choice.Id.Value, StringComparer.Ordinal).ToArray();
        var best = estimated[0];
        var activate = skip is null || !best.Estimate.IsSelfLethal && best.Estimate.Score > 0d;
        var ordering = activateChoices.Any(choice => choice.Parameters.ContainsKey("order-group"));
        var skipAction = new LegalAction(LegalActionKind.UseProgramSkill, null, null, ordering ? "跳过本次可选技能" : "跳过组合技能分支");
        var candidates = estimated.Select(item => new AiCandidateScore(item.Action, item.Estimate.IsSelfLethal ? -1000d : item.Estimate.Score, item.Estimate.IsSelfLethal ? "公开效果将令自身失去全部体力。" : item.Estimate.Approximation));
        if (skip is not null)
            candidates = candidates.Append(new AiCandidateScore(skipAction, 0d, "保留当前状态。"));
        _coreNativeAiDecisionsOperations.AddThought(new AiThoughtRecord(_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, decision.PlayerSeat, activate ? best.Action.Description : skipAction.Description, candidates.OrderByDescending(candidate => candidate.Score).ToArray(), ordering ? "同一时机的技能按公开上下文估值，未选技能保留后续机会。" : "互斥组使用同一操作目录与公开上下文估值。"));
        return activate ? best.Choice : skip!;
    }

    internal ProgramAiPublicContext CreateProgramAiPublicContext(SkillProgramTrigger trigger, CharacterState owner, int ownerSeat)
    {
        if (trigger.Effects is [{ Op: SkillProgramEffectOp.ChooseFirstSlashDuelDiamondBenefit }] && _resolutionStackOperations._resolutionStack.LastOrDefault()is FirstSlashDuelLegacyWindowFrame legacyBenefit && _coreCardUseLifecycleOperations.LifecycleCardUse(legacyBenefit.ParentFrameId)is { } legacyUse)
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
            {
                Actor = _corePublicDamagePredictionOperations.CreateAiSkillContext(owner, ownerSeat),
                CardActionActorIsOwner = true,
                CardUseEffectiveKind = legacyUse.CardKind
            };
        if (trigger.Window == SkillProgramTriggerWindow.TurnEnding && trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawEndingPairThenBlockRoundIfUnequal))
        {
            var turnContext = _corePublicDamagePredictionOperations.CreateAiSkillContext(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat], ownerSeat);
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
            {
                Actor = turnContext,
                SelectedTarget = turnContext,
                CardActionActorIsOwner = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == ownerSeat
            };
        }

        if (trigger.Window == SkillProgramTriggerWindow.OtherActualUseTargeted && _resolutionStackOperations._resolutionStack.OfType<ActualUseTargetWindowFrame>().LastOrDefault()is { } actualTargets && actualTargets.Contexts[actualTargets.CandidateIndex].ActualUseTarget is { } actualUse)
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
            {
                CardUseEffectiveKind = actualUse.EffectiveKind
            };
        if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.PayHpToGrantOneUseDamageShield))
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
            {
                HpDamageShieldTargetValue = _coreHpDamageShieldsOperations.GetHpDamageShieldAiTargetValue(owner)
            };
        if (trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.PeekTurnQuotaTop))
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
            {
                TurnCriterionQuota = _coreQuotaTopProgramsOperations.ComputeTurnCriterionQuota(owner.Seat)
            };
        if (trigger.Window == SkillProgramTriggerWindow.AfterDamageApplied && ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.ReflectAppliedDamage) && _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is { } reflectionWindow)
        {
            var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(reflectionWindow);
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
            {
                AfterDamageAmount = attack.DamageWasApplied ? attack.DamageAmount : 0,
                SelectedTarget = !attack.IsSourceLess && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) && _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive ? _corePublicDamagePredictionOperations.CreateAiSkillContext(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], ownerSeat) : null
            };
        }

        if (trigger.Window == SkillProgramTriggerWindow.AfterDamageApplied && trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.Recover && effect.NumberExpression == SkillProgramNumberExpression.AfterDamageAmount) && _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger is { } afterDamage)
        {
            var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(afterDamage);
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
            {
                AfterDamageAmount = attack.DamageAmount,
                SelectedTarget = _corePublicDamagePredictionOperations.CreateAiSkillContext(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], ownerSeat)
            };
        }

        if (trigger.Window is SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseBeforeTargetEffects or SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardResponseAccepted or SkillProgramTriggerWindow.CardResponseCompleted or SkillProgramTriggerWindow.CardSupplyCompleted or SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.SlashTargetRedirecting or SkillProgramTriggerWindow.SlashBeforeResponse or SkillProgramTriggerWindow.SlashFullyDodged)
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault() ?? throw new InvalidOperationException("A card-action estimate lost its frozen parent action.");
            var suits = parent.Action.PhysicalCards.Select(cost =>
            {
                var location = _cardZonesOperations._cardZones.GetLocation(cost.CardId);
                return _cardZonesOperations._cardZones.CardsAt(location).Single(card => card.Id == cost.CardId).Suit;
            }).ToArray();
            var actionSnapshot = parent.Action.ActorSeat == ownerSeat ? null : _corePlayerViewProjectionOperations.CreateSnapshot(ownerSeat);
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
            {
                CardUseIsRed = suits.Length == 0 ? null : suits.All(suit => suit is Suit.Heart or Suit.Diamond),
                Actor = _corePublicDamagePredictionOperations.CreateAiSkillContext(_coreMatchParticipantsPlayersAccess._players[parent.Action.ActorSeat], ownerSeat, actionSnapshot),
                CardActionActorIsOwner = parent.Action.ActorSeat == ownerSeat,
                CardUseEffectiveKind = parent.Action.EffectiveKind,
                WholeCardUseNullificationScore = ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.NullifyCurrentCardUse) || ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.RepeatIncomingSoleCardEffectAndDrawByRange) ? _coreProgramCardUseNullificationOperations.Value.GetWholeCardUseNullificationAiScore(ownerSeat, parent) : null,
                CardEffectInterventionScore = parent.Action.Type == CardActionType.Use && (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.NullifySelectedCardEffects) || ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.RemoveSelectedCurrentArrowBarrageTarget)) ? parent.Action.EffectiveDesignatedTargetSeats.Distinct().Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == parent.ParentFrameId).IneffectiveTargetSeats?.Contains(seat) != true).Select(seat => Math.Max(0d, _coreAiBrainsAiBrainsAccess._aiBrains[ownerSeat].ScoreCardEffectIntervention(actionSnapshot ??= _corePlayerViewProjectionOperations.CreateSnapshot(ownerSeat), parent.Action.EffectiveKind, seat))).Aggregate(0d, (total, score) => ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.RemoveSelectedCurrentArrowBarrageTarget) ? Math.Max(total, score) : total + score) : 0d,
                PairedColorDispositionScore = _coreProgramPairedColorResponseAiOperations.Value.GetPairedColorDispositionAiScore(trigger, ownerSeat, parent),
                CardUseDebitActive = _coreCardUseDebitsOperations.IsCardUseDebitActive(parent.Action.ActionId)
            };
        }

        if (trigger.Window != SkillProgramTriggerWindow.DrawPhaseStarting)
            return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramLifecycleTriggerWindowFrame drawPhase || drawPhase.Window != SkillProgramTriggerWindow.DrawPhaseStarting || drawPhase.OwnerSeat != ownerSeat && !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.AddMarkerSubjectNormalDraw) && !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.GiveCardSkipDrawForEnding) && !IsGuanduForeignDrawTrigger(trigger))
            throw new InvalidOperationException("A draw-phase estimate lost its lifecycle parent.");
        return _coreAttributedMarkerEventsOperations.CreateAttributedPaymentAiContext(owner, trigger)with
        {
            NormalDrawCount = drawPhase.NormalDrawReplaced ? 0 : Math.Max(0, checked((drawPhase.FrozenBaseDrawCount ?? _coreActualTurnsOperations.GetTurnDrawCount(_coreMatchParticipantsPlayersAccess._players[drawPhase.OwnerSeat])) + drawPhase.NormalDrawAdjustment)),
            EligibleTargetCount = ProgramInstructionResolver.Default.Features(trigger).First(SkillProgramEffectOp.SelectTargets)?.TargetKind is { } targetKind ? GetProgramTargetSeats(ownerSeat, targetKind).Count : 0,
            ReplacesNormalDraw = trigger.DrawPhaseMode == SkillProgramDrawPhaseMode.Replacement
        };
    }

    internal ProgramAiPublicContext CreateProgramAiPublicContext(CharacterState owner) => new(_coreRuleQueriesOperations.GetLivingFactionCount(), CanUseSlashOnOther: !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(owner.Seat, SkillProgramCardTargetRestriction.SelfOnly) && new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash }.Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, kind, CardActionType.Use)), AttackRange: _coreNativeRuleQueriesOperations.GetAttackRange(owner.Seat), AttackRangeCoverageDecreased: _ => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat).Any(player => _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Any(card => _coreProgramCardPaymentsOperations.WouldEquipmentRemovalReduceCoverage(player.Seat, card.Id))), LivingPlayersMinHp: CaptureProgramLivingHpFacts()is { Undefined: false } livingHp ? livingHp.Value : null, TurnOwnerDiscardPhaseHandDiscardCount: _coreActualDiscardPhaseLedgerOperations.TurnOwnerDiscardPhaseHandDiscardCount, CurrentTurnUsedCardCategoryCount: _coreCurrentTurnUseKindsOperations.TracksCurrentTurnUseKinds ? _coreCurrentTurnUseKindsOperations.CurrentTurnUseKinds(owner.Seat).Categories : null, PublicLivingFactionCount: _coreTurnDrawDebtsOperations.TracksTurnDrawDebt ? _corePublicFactionCountsOperations.GetPublicLivingFactionCount() : null, EquipmentCardCount: _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count, LivingPlayerCount: _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive), SourceOccurrenceDamageBonus: _corePublicDamagePredictionOperations.CreatePublicSourceOccurrenceDamageBonus(owner.Seat), VirtualSlashDamagePrediction: target => _corePublicDamagePredictionOperations.PredictPublicActionDamage(owner.Seat, new LegalAction(LegalActionKind.Slash, null, target, "", CardKind.Slash), target));
    internal ProgramAiPublicContext WithProgramConditionFacts(ProgramAiPublicContext context, CharacterState owner, string skillId, string skillInstanceId)
    {
        var cardFrame = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault();
        var cardFacts = cardFrame is not null && cardFrame.CandidateIndex < cardFrame.Candidates.Count ? cardFrame.Candidates[cardFrame.CandidateIndex].FrozenContext?.Facts : null;
        var ending = _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().LastOrDefault();
        var endingFacts = ending is not null && ending.ItemIndex < ending.Items.Count ? ending.Items[ending.ItemIndex].Facts ?? ending.Facts : null;
        var movementFrame = _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault();
        var movementContext = movementFrame is not null && movementFrame.Contexts is { } contexts && movementFrame.CandidateIndex < contexts.Count ? contexts[movementFrame.CandidateIndex] : null;
        var movementFacts = movementContext?.OwnerSeat == owner.Seat && movementContext.Window == SkillProgramTriggerWindow.CardsMoved ? movementContext.Facts : null;
        var effectFrame = _resolutionStackOperations._resolutionStack.OfType<CardEffectBeforeApplyFrame>().LastOrDefault();
        var effectFacts = effectFrame is not null && effectFrame.CandidateIndex < effectFrame.Candidates.Count ? effectFrame.Candidates[effectFrame.CandidateIndex].FrozenContext?.Facts : null;
        var facts = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault()?.Facts ?? endingFacts ?? movementFacts ?? effectFacts ?? cardFacts;
        return context with
        {
            EventMovedCardCount = movementFacts?.MovedCardCount,
            BooleanState = stateId => facts is null ? _coreProgramPersistentStateOperations.GetProgramBooleanState2(owner.Seat, skillId, skillInstanceId, stateId) : facts.GetBooleanState(skillId, skillInstanceId, stateId)
        };
    }

    internal (ProgramAiEstimate Estimate, int? TargetSeat) EstimateCompositionForAi(CharacterState owner, IEnumerable<SkillProgramEffect> sourceEffects, ProgramAiPublicContext publicContext, IReadOnlyList<int>? publishedTargets = null, ProgramSkillWindowContext? windowContext = null)
    {
        var effects = sourceEffects.ToArray();
        if (effects is [{ Op: SkillProgramEffectOp.ResolveWushuangZengou }])
            return _characterQingheGongzhuOlOperations.EstimateActualDodgeZengouForAi(owner, windowContext);
        if (effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouYuJinXiayuan))
            return _characterYuJinMouMobileOperations.Value.EstimateMouXiayuanForAi(owner, publicContext, windowContext);
        if (effects is [{ Op: SkillProgramEffectOp.ResolveMouHuangGaiKurouGift }])
            return _characterHuangGaiMouMobileOperations.EstimateMouKurouGiftForAi(owner, publicContext);
        if (effects.FirstOrDefault(e => e.Op is SkillProgramEffectOp.SelectAllPublicTeamTargets or SkillProgramEffectOp.SelectRandomPublicTeamTarget)is { TargetKind: { } teamKind } teamSelection)
        {
            publishedTargets = GetProgramTargetSeats(owner.Seat, teamKind, windowContext);
            publicContext = publicContext with
            {
                EligibleTargetCount = Math.Min(publishedTargets.Count, teamSelection.MaximumTargets)
            };
            // Estimate a representative public recipient; execution still uses the complete frozen set.
            effects = effects.Select(e => e.Op == SkillProgramEffectOp.DamageParticipants ? new SkillProgramEffect(SkillProgramEffectOp.Damage, SkillProgramEffectTarget.SelectedTarget, e.Amount, e.Condition, damageNature: e.DamageNature) : e).ToArray();
        }

        if (effects.Any(e => e.Op is SkillProgramEffectOp.ResolveUnequalHandHpEnding or SkillProgramEffectOp.AdjustTargetedOrdinaryTrick))
            publicContext = publicContext with
            {
                TargetedTrickHandHpScore = _coreTargetedTrickHandHpAiOperations.GetTargetedTrickHandHpPublicScore(owner.Seat, effects, windowContext)
            };
        if (effects.Any(e => e.Op == SkillProgramEffectOp.RefillAndScheduleNextTurnSelfOnly))
            publicContext = publicContext with
            {
                EndRefillNextTurnScore = _coreEndRefillNextTurnOperations.EndRefillNextTurnPublicScore(owner.Seat)
            };
        if (effects.Any(e => e.Op == SkillProgramEffectOp.UsePhaseOwnerVirtualAlcohol) && windowContext is { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } wineActor })
            publishedTargets = [wineActor];
        if (effects.Any(e => e.NumberExpression == SkillProgramNumberExpression.ActualTurnDeathBenefits) && windowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded, Facts: { ActualTurnDeathCount: { } deaths, OwnerActualTurnKillCount: { } kills } })
            publicContext = publicContext with
            {
                ActualTurnDeathBenefits = checked(effects.Single(e => e.NumberExpression == SkillProgramNumberExpression.ActualTurnDeathBenefits).ActualTurnDeathOwnerKillsOnly == true ? 3 * kills : deaths + 2 * kills)
            };
        publishedTargets ??= _coreFixedRecipientBenefitsOperations.PublishedFixedRecipientForAi(owner, effects, windowContext);
        publishedTargets ??= _coreEndingPairDrawsOperations.Value.PublishedEndingPairActorForAi(owner, effects, windowContext);
        if (publishedTargets is null && effects.Any(effect => effect.Op == SkillProgramEffectOp.YuanziResolve) && windowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, SourceSeat: { } yuanziTurnSeat } && yuanziTurnSeat != owner.Seat)
            publishedTargets = [yuanziTurnSeat];
        if (publishedTargets is null && effects.Any(effect => effect.Op == SkillProgramEffectOp.GrantTurnSkills && effect.TargetReference is { Kind: ProgramParticipantRef.Actor }) && windowContext?.CardUse is { } grantedUse)
            publishedTargets = [grantedUse.ActorSeat];
        if (effects.Any(effect => effect.Op is SkillProgramEffectOp.RecoverOtherDyingVictimTo or SkillProgramEffectOp.SelectDyingOwnedCard) && windowContext is { Window: SkillProgramTriggerWindow.DyingEntering, TargetSeat: { } dyingVictim })
            publishedTargets = [dyingVictim];
        if (effects.Any(e => e.Op == SkillProgramEffectOp.ObtainDamageTargetCardAndResolveCategory) && windowContext?.TargetSeat is { } obtainTarget)
            publishedTargets = [obtainTarget];
        if (effects.Any(e => e.Op == SkillProgramEffectOp.DiscardDamageTargetAndClaimMount) && windowContext?.TargetSeat is { } mountTarget)
            publishedTargets = [mountTarget];
        GameSnapshot? targetSnapshot = null;
        if (CoreGameEngineFinalTargetSlashRuntime.HasFinalTargetSlashEffects(effects) && windowContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, Facts: { } finalFacts, TargetSeat: { } finalSeat })
        {
            targetSnapshot = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
            publicContext = publicContext with
            {
                FinalTargetFacts = finalFacts,
                SelectedTarget = _corePublicDamagePredictionOperations.CreateAiSkillContext(_coreMatchParticipantsPlayersAccess._players[finalSeat], owner.Seat, targetSnapshot)
            };
            publishedTargets = [finalSeat];
        }

        var selection = effects.FirstOrDefault(effect => effect.Op == SkillProgramEffectOp.SelectTarget);
        var targets = publishedTargets ?? (selection?.TargetKind is { } kind ? GetProgramTargetSeats(owner.Seat, kind, windowContext, selection.Marker) : Array.Empty<int>());
        var context = _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner);
        if (targets.Count == 0)
            return (ProgramCompositionAi.Estimate(effects, context, owner.IsFaceDown, publicContext), null);
        var snapshot = targetSnapshot ?? _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
        return targets.Select(targetSeat =>
        {
            var estimate = ProgramCompositionAi.Estimate(effects, context, owner.IsFaceDown, publicContext with { SelectedTarget = _corePublicDamagePredictionOperations.CreateAiSkillContext(_coreMatchParticipantsPlayersAccess._players[targetSeat], owner.Seat, snapshot), HasOwnedCardCategory = (zones, categories, kinds) => zones.Contains(CardZoneKind.Equipment) && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[targetSeat]).Any(card => CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardFilter(card.Kind, categories, kinds)), AttackRangeCoverageDecreased = _ => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[targetSeat]).Any(card => _coreProgramCardPaymentsOperations.WouldEquipmentRemovalReduceCoverage(targetSeat, card.Id)) });
            var targetValue = _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ScoreProgramTarget(snapshot, targetSeat, estimate.Hint);
            return (Estimate: estimate with { Score = estimate.Score + targetValue }, TargetSeat: (int? )targetSeat);
        }).OrderByDescending(candidate => candidate.Estimate.Score).ThenBy(candidate => candidate.TargetSeat).First();
    }

    [ProgramAiChoice(SkillProgramEffectOp.SelectTarget)]
    internal PromptChoice SelectAiCompositionTarget(PendingDecision decision, ProgramSkillFrame frame)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var program = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!;
        var effects = ProgramInstructionResolver.Default.Resolve(frame, program).Instructions;
        if (_coreMatchDefinitionOptionsAccess._options.AiPolicyVersion >= AiDecisionPolicy.CooperativeCategoryGiftVersion && frame.InstructionIndex < effects.Count && effects[frame.InstructionIndex].Op == SkillProgramEffectOp.ChooseDifferentActionCategoryGift && frame.WindowContext?.CardUse is { } completedUse)
        {
            var(choice, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ChooseActionCategoryGiftProvider(_corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat), completedUse.EffectiveKind, decision.Choices, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++);
            _coreNativeAiDecisionsOperations.AddThought(thought);
            return decision.Choices.Single(item => item.Id == choice);
        }

        var targets = decision.Choices.SelectMany(choice => choice.Targets).Distinct().ToArray();
        var publicContext = CreateProgramAiPublicContext(owner)with
        {
            BooleanState = stateId => _coreProgramPersistentStateOperations.GetProgramBooleanState2(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, stateId),
            PindianWon = bind => frame.PindianResultBindings.SingleOrDefault(item => item.Name == bind)?.SourceWon ?? false,
            PindianOutcome = bind => frame.PindianResultBindings.SingleOrDefault(item => item.Name == bind)?.SourceWon,
            ChoiceResult = bind => frame.ChoiceBindings.SingleOrDefault(item => item.Name == bind)?.OptionId
        };
        if (frame.TriggerId is { } triggerId)
            publicContext = CoreGameEnginePhaseHandExtraTurnAiRuntime.WithPhaseHandExtraTurnTargetPricing(publicContext, program, ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Trigger, triggerId).Trigger!);
        var selected = EstimateCompositionForAi(owner, effects, publicContext, targets);
        return decision.Choices.First(choice => choice.Targets.Contains(selected.TargetSeat ?? throw new InvalidOperationException("A configured target selection has no published candidate.")));
    }

    internal PromptChoice SelectAiProgramTargets(PendingDecision decision, SkillProgramTargetAiOrder aiOrder)
    {
        if (aiOrder == SkillProgramTargetAiOrder.Stable)
        {
            return decision.Choices.OrderByDescending(choice => choice.Targets.Count).ThenBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
        }

        var(choiceId, thought) = aiOrder switch
        {
            SkillProgramTargetAiOrder.HostileThenHandCount => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseHostileHandTargets(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++),
            SkillProgramTargetAiOrder.SupportFirstThenOpposeSecond => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseSupportFirstTransferTargets(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++),
            SkillProgramTargetAiOrder.SupportDraw => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseSupportDrawTargets(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++),
            SkillProgramTargetAiOrder.SupportRecovery => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseSupportRecoveryTargets(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), decision.Choices, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++),
            SkillProgramTargetAiOrder.CardEffectIntervention => _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseCardEffectInterventionTargets(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last().Action.EffectiveKind, decision.Choices, _coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence++),
            _ => throw new InvalidOperationException("Unsupported configured target AI order.")};
        _coreNativeAiDecisionsOperations.AddThought(thought);
        return decision.Choices.Single(choice => choice.Id == choiceId);
    }

    [ProgramAiChoice(SkillProgramEffectOp.SelectOwnedCards, SkillProgramEffectOp.ResolveDeferredHandAlignment, SkillProgramEffectOp.SelectTurnDamageUseDebtPayment, SkillProgramEffectOp.SelectFrozenHandExchangeDebtPayment)]
    internal PromptChoice SelectAiProgramOwnedCards(PendingDecision decision, ProgramSkillFrame frame)
    {
        var draft = frame.OwnedCardSelection ?? throw new InvalidOperationException("The configured owned-card choice lost its draft.");
        if (frame.MouYuJinXiayuan is not null && draft.ResultBind == "xiayuan-cost" && _characterYuJinMouMobileOperations.Value.ValidMouXiayuan(frame))
            return _characterYuJinMouMobileOperations.Value.SelectAiMouXiayuanCost(decision, frame);
        if (draft.MinimumCount > 0 && draft.SelectedCardIds.Count >= draft.MinimumCount)
            return decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards");
        return decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-owned-cards").OrderBy(choice => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(choice.Cards.Single())).Single(card => card.Id == choice.Cards[0]).Kind is CardKind.Peach ? 1 : 0).ThenBy(choice => choice.Cards[0]).First();
    }

    internal void CompleteDetachedProgramBinding(ProgramSkillFrame frame, bool completed)
    {
        var context = frame.WindowContext ?? throw new InvalidOperationException("A detached program frame lost its window context.");
        CleanupProgramBoundCards(frame, completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramBindingResolvedEvent(frame.Id, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.OwnerSeat, context.Window, Activated: true, Completed: completed));
    }

    internal void CompleteDyingProgramBinding(ProgramSkillFrame frame, bool completed)
    {
        var dying = _coreDyingCursorRuntimeOperations.Value.ActiveDying ?? throw new InvalidOperationException("The dying program lost its dying resolution.");
        if (dying.FrameId != frame.WindowContext!.ParentFrameId || dying.VictimSeat != frame.WindowContext.TargetSeat || dying.ResponderSeat != frame.OwnerSeat)
            throw new InvalidOperationException("The dying program returned to the wrong responder or victim.");
        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        if (frame.WindowContext.Window == SkillProgramTriggerWindow.SelfDyingResponse)
        {
            dying = _coreNativeDyingFramesOperations.UpdateDyingFrame(dying.Id, current => current with { AttemptedSelfDyingBindings = [..current.AttemptedSelfDyingBindings, SelfDyingBindingKey(frame.SkillId, frame.TriggerId!, frame.SkillInstanceId)] });
            if (victim.Hp > 0)
                _coreNativeDyingOperations.CompleteDying(dying, survived: true);
            else if (!TryBeginMandatorySelfDyingProgram(dying) && !_coreDyingEntryProgramsOperations.TryBeginDyingEntryProgramWindow(dying))
            {
                _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.FrameId, ResolutionFrameStep.AwaitingResponse);
                _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
                _coreNativeDyingOperations.ExposeHumanDyingPrompt();
            }

            return;
        }

        if (victim.Hp > 0)
        {
            _coreNativeDyingOperations.CompleteDying(dying, survived: true);
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreDyingRescueContinuationsOperations.Value.ProgramPaidDyingRescue(frame, dying))
            dying = _coreNativeDyingFramesOperations.UpdateDyingFrame(dying.Id, current => current with { ResponderIndex = current.ResponderIndex + 1 });
        if (dying.ResponderIndex >= dying.ResponderSeats.Count)
            _coreNativeDyingOperations.CompleteDying(dying, survived: false);
        else
        {
            _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.FrameId, ResolutionFrameStep.AwaitingResponse);
            _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
            _coreNativeDyingOperations.ExposeHumanDyingPrompt();
        }
    }

    internal ProgramSkillWindowContext CreateDyingProgramContext(DyingFrame dying, ProgramTriggerCandidate candidate) => new(GetProgramTrigger(candidate).Window, dying.FrameId, candidate.OwnerSeat, SourceSeat: _coreNativeDamageHandlesOperations.Value.GetDyingAttack(dying)is { IsSourceLess: false } attack ? attack.SourceSeat : null, TargetSeat: dying.VictimSeat, DamageFrameId: dying.DamageFrameId, OccurrenceIndex: candidate.OccurrenceIndex);
    internal IReadOnlyList<ProgramTriggerCandidate> GetDyingProgramCandidates(CharacterState responder, DyingFrame dying) => new[]
    {
        SkillProgramTriggerWindow.SelfDyingResponse,
        SkillProgramTriggerWindow.DyingResponse
    }.Where(window => window != SkillProgramTriggerWindow.SelfDyingResponse || responder.Seat == dying.VictimSeat).SelectMany(window => CollectProgramTriggerCandidates(responder, window)).Where(candidate => GetProgramTrigger(candidate).Window != SkillProgramTriggerWindow.SelfDyingResponse || !dying.AttemptedSelfDyingBindings.Contains(SelfDyingBindingKey(candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId))).Where(candidate => CanRunProgramTrigger(candidate, CreateDyingProgramContext(dying, candidate))).ToArray();
    internal static string SelfDyingBindingKey(string skillId, string bindingId, string skillInstanceId) => RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.SelfDyingBindingKey(skillId, bindingId, skillInstanceId);
    internal bool TryBeginMandatorySelfDyingProgram(DyingFrame dying)
    {
        if (dying.ResponderIndex >= dying.ResponderSeats.Count || dying.ResponderSeat != dying.VictimSeat)
            return false;
        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        var candidate = GetDyingProgramCandidates(victim, dying).FirstOrDefault(item => GetProgramTrigger(item)is { Window: SkillProgramTriggerWindow.SelfDyingResponse, Optional: false });
        if (candidate is null)
            return false;
        BeginDyingProgramBinding(candidate, dying);
        return true;
    }

    internal void BeginDyingProgramBinding(ProgramTriggerCandidate candidate, DyingFrame dying) => BeginProgramBinding(candidate, CreateDyingProgramContext(dying, candidate));
    internal ProgramSkillWindowContext CreateAfterDamageProgramContext(DamageTriggerWindowFrame damage, ProgramTriggerCandidate candidate)
    {
        if (CharacterTaoQianOlRuntime.IsZhaohuoTrigger(GetProgramTrigger(candidate)))
            return _characterTaoQianOlOperations.CreateZhaohuoDamageContext(damage, candidate);
        if (_characterXiZhiCaiOlOperations.IsXianfuEchoTrigger(GetProgramTrigger(candidate)))
            return _characterXiZhiCaiOlOperations.CreateXianfuDamageContext(damage, candidate);
        if (_characterHuangQuanOlOperations.IsDianhuDrawTrigger(GetProgramTrigger(candidate)))
            return _characterHuangQuanOlOperations.CreateDianhuDamageContext(damage, candidate);
        var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(damage);
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        var appearanceSourceLess = attack.IsDelayedJudgmentDamage && ProgramInstructionResolver.Default.Features(GetProgramTrigger(candidate)).HasOperation(SkillProgramEffectOp.DrawByDamageCardColor);
        var facts = CaptureProgramTriggerFacts(owner)with
        {
            DamageNature = ProgramInstructionResolver.Default.Features(GetProgramTrigger(candidate)).UsesCondition(SkillProgramTriggerConditionKind.DamageNatureIs) ? _coreNativeCardClassificationsOperations.GetDamageNature(attack) : null,
            CardActionActorIsCurrentTurn = !attack.IsSourceLess && attack.SourceSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat,
            CardActionPhaseIsPlay = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play,
            OtherDamageParticipantAlive = !attack.IsSourceLess && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) && attack.SourceSeat != attack.TargetSeat && _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive,
            DirectCardUseDamage = !attack.IsSourceLess && !attack.IsChainPropagation && attack.Card is not null && attack.CardUserSeat == attack.SourceSeat,
            DamageCardIsRed = !attack.IsSourceLess && attack.Card?.Suit is Suit.Heart or Suit.Diamond,
            DamageCardSuit = !attack.IsSourceLess ? attack.Card?.Suit : null,
            DamageCardIsSlash = !attack.IsSourceLess && attack.EffectiveCardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash,
            SourceToTargetDistanceAtDamage = attack.IsSourceLess ? null : attack.SourceToTargetDistanceAtDamage,
            EventTargetHp = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Hp,
            EventTargetHpUndefined = !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].HasHpValue,
            EventTargetRuleWounded = _coreTreatedWoundedOperations.UsesTreatedWounded ? _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]) : null,
            EventTargetMaxHp = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].MaxHp,
            EventTargetDamageInstancesTakenThisTurn = damage.EventTargetDamageInstancesTakenThisTurn,
            DamageTargetIsOther = candidate.OwnerSeat != attack.TargetSeat,
            AppliedDamageAmount = _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerValue(SkillProgramTriggerValueKind.AppliedDamageAmount) ? attack.DamageAmount : null,
            DamageTargetIsPublicTeamEnemy = _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(SkillProgramTriggerConditionKind.DamageTargetIsPublicTeamEnemy) ? candidate.OwnerSeat != attack.TargetSeat && owner.TeamRevealed && _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].TeamRevealed && _coreTeamActivityProgramsOperations.PublicTeamEnemy(candidate.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]) : null,
            DamageFromOwnerTrickUse = _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(SkillProgramTriggerConditionKind.DamageFromOwnerTrickUse) ? !attack.IsSourceLess && !attack.IsChainPropagation && attack.SourceSeat == candidate.OwnerSeat && attack.CardUserSeat == candidate.OwnerSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action is { Type: CardActionType.Use } trickUse && trickUse.ActorSeat == candidate.OwnerSeat && trickUse.EffectiveKind == attack.EffectiveCardKind && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(trickUse.EffectiveKind) == SkillProgramCardCategory.Trick : null,
            DamageSourceIsOwner = !attack.IsSourceLess && candidate.OwnerSeat == attack.SourceSeat,
            EventSourceMarkerCounts = _coreAttributedMarkerEventsOperations.HasAttributedEventOperations() && !attack.IsSourceLess && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) ? new Dictionary<PlayerMarkerKind, int>(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].Markers) : null,
            DamageSourceFactionId = attack.IsSourceLess ? null : _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat]),
            DamageInstancesTakenThisTurn = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Any(instance => ProgramInstructionResolver.Default.ProgramUsesTriggerValue(instance.Program, SkillProgramTriggerValueKind.DamageInstancesTakenThisTurn)) ? _corePlayCardHistoryOperations.DamageInstancesTakenThisTurn(owner.Seat) : null
        };
        return _characterBaoXinOlOperations.CreateBaoXinYimouContext(damage, candidate, new(damage.TriggerWindow, damage.Id, candidate.OwnerSeat, SourceSeat: attack.IsSourceLess || appearanceSourceLess ? null : attack.SourceSeat, TargetSeat: attack.TargetSeat, DamageFrameId: damage.ParentFrameId, Amount: attack.DamageAmount, OccurrenceIndex: candidate.OccurrenceIndex, Facts: facts) { OptionalChooserSeat = GetProgramTrigger(candidate).Effects is [{ Op: SkillProgramEffectOp.OfferRiceAfterDamagePoint }] ? attack.TargetSeat : null });
    }
}
