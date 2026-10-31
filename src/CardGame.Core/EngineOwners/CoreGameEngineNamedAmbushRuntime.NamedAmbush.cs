#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNamedAmbushRuntime
{
    internal const string NamedAmbushRecordReason = "skill-program.named-ambush.record";
    internal const string NamedAmbushRemoveReason = "skill-program.named-ambush.remove";
    internal bool HasNamedAmbushCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.NullifyMatchingHandUse);

    internal static CardKind AmbushCardName(CardKind kind) => RuntimeSkills.RuntimeRules_Core_NamedAmbush.AmbushCardName(kind);
    internal static CardConversionSource AmbushSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_NamedAmbush.AmbushSource(f);
    internal static CardLocation AmbushLocation(CardConversionSource source) => RuntimeSkills.RuntimeRules_Core_NamedAmbush.AmbushLocation(source);
    internal NamedAmbushDeposit[] LiveAmbushDeposits(int owner, string? skill = null, string? instance = null)
    {
        if (!HasNamedAmbushCapability || !_cardZonesOperations._cardZones.HasPrivatePersistentCards(owner))
            return[];
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var removed = facts.OfType<NamedAmbushRemovedEvent>().Select(e => (e.Deposit.DepositId, e.Deposit.Source, e.Deposit.CardId)).ToHashSet();
        return facts.OfType<NamedAmbushDepositedEvent>().Select(e => e.Deposit).Where(d => d.Source.OwnerSeat == owner && (skill is null || d.Source.SkillId == skill) && (instance is null || d.Source.SkillInstanceId == instance) && !removed.Contains((d.DepositId, d.Source, d.CardId)) && _cardZonesOperations._cardZones.GetLocation(d.CardId) == d.Location).ToArray();
    }

    internal bool TryGetAmbushUse(ProgramSkillWindowContext context, out NamedAmbushUseIdentity identity, out CardActionContext action)
    {
        identity = null !;
        action = null !;
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseCommitted || window.Action.PhysicalCards.Count == 0)
            return false;
        action = window.Action;
        var response = action.Type == CardActionType.Response;
        var actorSeat = action.ActorSeat;
        if (action.ActorSeat != action.ProviderSeat || action.PhysicalCards.Any(c => c.From != CardLocation.Hand(actorSeat)) || action.PhysicalCards.Select(c => c.CardId).Distinct().Count() != action.PhysicalCards.Count || CardCatalog.Get(action.EffectiveKind).CategoryName == "装备牌" || response && (window.CompletedResponseReturn is not { IsCommitted: true } typed || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(action, typed.Kind)) || !response && (_coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { } actual } use || actual.ActionId != action.ActionId || actual.ActorSeat != action.ActorSeat || actual.ProviderSeat != action.ProviderSeat || use.CardKind != action.EffectiveKind))
            return false;
        identity = new(window.Id, window.ParentFrameId, action.ActionId, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, response);
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 260)]
    internal bool CanOfferNamedAmbush(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.RecordNamedPrivateAmbush))
            return _coreActualTurnProgressCurrentSeatAccess._currentSeat == candidate.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0;
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.NullifyMatchingHandUse))
            return true;
        return _coreActualTurnProgressCurrentSeatAccess._currentSeat != candidate.OwnerSeat && TryGetAmbushUse(context, out var identity, out _) && identity.ActorSeat != candidate.OwnerSeat && LiveAmbushDeposits(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId).Any(d => d.RecordedKind == AmbushCardName(identity.EffectiveKind));
    }

    internal SkillProgramStepOutcome BeginNamedAmbush(SkillProgramEffect effect, ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.NamedAmbush is not null || f.WindowContext is not { } context || f.TriggerId is null)
            throw new InvalidOperationException("Named ambush requires one unpaid owning trigger.");
        var removing = effect.Op == SkillProgramEffectOp.NullifyMatchingHandUse;
        NamedAmbushUseIdentity? use = null;
        CardActionContext? originalAction = null;
        if (removing)
        {
            if (_coreActualTurnProgressCurrentSeatAccess._currentSeat == f.OwnerSeat || !TryGetAmbushUse(context, out var identity, out var action) || identity.ActorSeat == f.OwnerSeat)
                throw new InvalidOperationException("Named ambush lost its off-turn actual hand use.");
            use = identity;
            originalAction = action;
        }

        var receipt = new NamedAmbushReceipt
        {
            Source = AmbushSource(f),
            GameplayHash = f.GameplayHash,
            InstructionIndex = f.InstructionIndex,
            WindowFrameId = context.ParentFrameId,
            Removing = removing,
            Use = use,
            OriginalAction = originalAction,
            Stage = removing ? NamedAmbushStage.SelectDeposit : NamedAmbushStage.SelectCard
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedAmbush = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NamedAmbushOfferedEvent(f.Id, receipt));
        PublishNamedAmbushChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> NamedAmbushChoices(ProgramSkillFrame f)
    {
        var r = f.NamedAmbush!;
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null, CardKind? name = null) => new(new($"named-ambush.{(f.Id)}.{(r.Stage)}.{(branch)}.{(string.Join('.', cards ?? []))}.{(name)}"), label, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "named-ambush", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["recorded-kind"] = name?.ToString() ?? "" });
        var choices = new List<PromptChoice>();
        if (r.Stage == NamedAmbushStage.SelectCard)
            choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => Choice("card", $"暗置【{(c.DisplayName)}】", [c.Id])));
        else if (r.Stage == NamedAmbushStage.SelectName && r.SelectedCardId is { } selected && _cardZonesOperations._cardZones.GetLocation(selected) == CardLocation.Hand(f.OwnerSeat))
            choices.AddRange(Enum.GetValues<CardKind>().Select(AmbushCardName).Distinct().Where(k => CardCatalog.Get(k).CategoryName != "装备牌" && _modeRulesOperations._modeRules.Cards.IsCardAllowed(k)).Select(k => Choice("name", $"记录【{(CardCatalog.Get(k).DisplayName)}】", name: k)));
        else if (r.Stage == NamedAmbushStage.SelectDeposit && r.Use is { } use)
            choices.AddRange(LiveAmbushDeposits(f.OwnerSeat, f.SkillId, f.SkillInstanceId).Where(d => d.RecordedKind == AmbushCardName(use.EffectiveKind)).Select(d => Choice("remove", $"移去伏兵【{(_coreCardAttackStateOperations.GetAttackCard(d.CardId).DisplayName)}】，令【{(CardCatalog.Get(use.EffectiveKind).DisplayName)}】无效", [d.CardId])));
        choices.Add(Choice("decline", "取消"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishNamedAmbushChoice(ProgramSkillFrame f)
    {
        var choices = NamedAmbushChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedAmbush = f.NamedAmbush!with { Choices = choices } });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("named-ambush")]
    internal void ResolveNamedAmbushChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Ambush lost owning frame.");
        RequireNamedAmbushReceipt(f);
        var r = f.NamedAmbush!;
        if (!CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], r.Choices.Where(c => c.Id == choice.Id).ToArray()) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], NamedAmbushChoices(f).Where(c => c.Id == choice.Id).ToArray()) || r.Stage == NamedAmbushStage.MovementChildren)
            throw new InvalidOperationException("Ambush choice changed after publication.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["branch"] == "decline")
        {
            CompleteNamedAmbush(f, false);
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            CompleteNamedAmbush(f, false);
            return;
        }

        if (r.Stage == NamedAmbushStage.SelectCard)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedAmbush = r with { SelectedCardId = choice.Cards.Single(), Stage = NamedAmbushStage.SelectName, Choices = [] } });
            PublishNamedAmbushChoice(f);
            return;
        }

        NamedAmbushDeposit deposit;
        CardLocation from, to;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (r.Removing)
        {
            if (r.Use is not { } identity || !TryGetAmbushUse(f.WindowContext!, out var live, out _) || live != identity || _coreActualTurnProgressCurrentSeatAccess._currentSeat == f.OwnerSeat)
                throw new InvalidOperationException("Ambush payment lost actual committed hand use.");
            deposit = LiveAmbushDeposits(f.OwnerSeat, f.SkillId, f.SkillInstanceId).Single(d => d.CardId == choice.Cards.Single() && d.RecordedKind == AmbushCardName(identity.EffectiveKind));
            from = deposit.Location;
            to = CardLocation.DiscardPile;
        }
        else
        {
            if (r.SelectedCardId is not { } cardId || _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(f.OwnerSeat) || !Enum.TryParse<CardKind>(choice.Parameters["recorded-kind"], out var name))
                throw new InvalidOperationException("Ambush deposit lost exact hand card or name.");
            from = CardLocation.Hand(f.OwnerSeat);
            to = AmbushLocation(r.Source);
            _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(to);
            deposit = new(f.Id, r.Source, f.GameplayHash, cardId, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == cardId).Kind, name, to, before, before);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedAmbush = r with { Deposit = deposit, MovementBefore = before, Stage = NamedAmbushStage.MovementChildren, Choices = [] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(deposit.CardId), from, to, new(r.Removing ? NamedAmbushRemoveReason : NamedAmbushRecordReason));
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (!r.Removing)
            deposit = deposit with
            {
                SequenceAfter = after
            };
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedAmbush = f.NamedAmbush!with { Deposit = deposit, MovementAfter = after } });
        if (r.Removing)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NamedAmbushRemovedEvent(f.Id, deposit, r.Use, before, after, "used"));
        else
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NamedAmbushDepositedEvent(deposit));
        if (!DrainNamedHandChoiceChildren(f))
            ReturnNamedAmbushMovement(f);
    }

    internal bool DrainNamedHandChoiceChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Movement, 20700)]
    internal bool ReturnNamedAmbushMovement(ProgramSkillFrame f)
    {
        if (f.NamedAmbush is null)
            return false;
        RequireNamedAmbushReceipt(f);
        if (f.NamedAmbush.Stage != NamedAmbushStage.MovementChildren || f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Ambush movement lost typed return.");
        if (DrainNamedHandChoiceChildren(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (f.NamedAmbush.Removing && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            _coreNamedAmbushInvalidationOperations.InvalidateNamedAmbushUse(f);
        CompleteNamedAmbush(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 20700)]
    internal bool ResumeNamedAmbush(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { NamedAmbush: not null } f || f.Id != id)
            return false;
        RequireNamedAmbushReceipt(f);
        if (f.PendingMovementContinuation is not null)
            ReturnNamedAmbushMovement(f);
        return true;
    }

    internal void CompleteNamedAmbush(ProgramSkillFrame f, bool completed)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { NamedAmbush = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    internal void RequireNamedAmbushReceipt(ProgramSkillFrame f)
    {
        if (f.NamedAmbush is not { } r || f.TriggerId is null || r.Source != AmbushSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.WindowContext?.ParentFrameId != r.WindowFrameId || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != (r.Removing ? SkillProgramEffectOp.NullifyMatchingHandUse : SkillProgramEffectOp.RecordNamedPrivateAmbush) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NamedAmbushOfferedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt.Source == r.Source && e.Receipt.InstructionIndex == r.InstructionIndex && e.Receipt.WindowFrameId == r.WindowFrameId && e.Receipt.Use == r.Use && (r.OriginalAction is null ? e.Receipt.OriginalAction is null : e.Receipt.OriginalAction is { } issued && CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(issued, r.OriginalAction))) != 1 || r.Removing && (r.Use is null || r.OriginalAction is not { } original || !TryGetAmbushUse(f.WindowContext!, out var use, out var actual) || use != r.Use || !CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(original, actual)))
            throw new InvalidOperationException("Ambush lost exact instruction, source, window or current hand-use identity.");
        if (r.Stage != NamedAmbushStage.MovementChildren)
            return;
        if (r.Deposit is not { } d || r.MovementAfter <= r.MovementBefore || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == d.CardId && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.From == (r.Removing ? d.Location : CardLocation.Hand(f.OwnerSeat)) && m.To == (r.Removing ? CardLocation.DiscardPile : d.Location) && m.Reason.Value == (r.Removing ? NamedAmbushRemoveReason : NamedAmbushRecordReason)) != 1)
            throw new InvalidOperationException("Ambush lost its one real paid movement.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 420)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 420)]
    internal bool CanContinueNamedAmbush(ProgramSkillFrame f)
    {
        if (f.NamedAmbush is not { Stage: NamedAmbushStage.MovementChildren })
            return false;
        RequireNamedAmbushReceipt(f);
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 70)]
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 80)]
    internal void CleanupLostNamedAmbushPiles()
    {
        if (!HasNamedAmbushCapability || !_cardZonesOperations._cardZones.HasPrivatePersistentCards())
            return;
        foreach (var d in _coreMatchParticipantsPlayersAccess._players.SelectMany(p => LiveAmbushDeposits(p.Seat)).Where(d => !_coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[d.Source.OwnerSeat].SkillGrants.Grants.Any(g => g.SkillId == d.Source.SkillId && g.SkillInstanceId == d.Source.SkillInstanceId)).ToArray())
        {
            // Moving an earlier entity can recursively settle another deposit.
            // Its consumed identity and actual zone remain authoritative.
            if (_cardZonesOperations._cardZones.GetLocation(d.CardId) != d.Location || !LiveAmbushDeposits(d.Source.OwnerSeat, d.Source.SkillId, d.Source.SkillInstanceId).Any(live => live.DepositId == d.DepositId))
                continue;
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _corePhysicalMovementFacadesOperations.MoveCard(_coreCardAttackStateOperations.GetAttackCard(d.CardId), d.Location, CardLocation.DiscardPile, new("skill-program.named-ambush.source-lost"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NamedAmbushRemovedEvent(0, d, null, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, "source-lost"));
        }
    }

    internal IReadOnlyList<NamedAmbushPileSnapshot>? ProjectNamedAmbushPiles(int owner, int viewer, bool revealAll)
    {
        var deposits = LiveAmbushDeposits(owner);
        if (deposits.Length == 0)
            return null;
        return Array.AsReadOnly(deposits.GroupBy(d => (d.Source.SkillId, d.Source.SkillInstanceId)).Select(g => new NamedAmbushPileSnapshot(g.Key.SkillId, "伏兵", g.Count(), revealAll || owner == viewer ? Array.AsReadOnly(g.Select(d => new NamedAmbushPrivateEntrySnapshot(_coreSnapshotValueProjectionOperations.ToSnapshot(_coreCardAttackStateOperations.GetAttackCard(d.CardId)), d.RecordedKind)).ToArray()) : null)).ToArray());
    }

    [ProgramAiChoice(SkillProgramEffectOp.RecordNamedPrivateAmbush, SkillProgramEffectOp.NullifyMatchingHandUse)]
    internal PromptChoice SelectAiNamedAmbush(PendingDecision decision, ProgramSkillFrame f)
    {
        if (f.NamedAmbush!.Stage == NamedAmbushStage.SelectName)
            return decision.Choices.FirstOrDefault(c => c.Parameters["recorded-kind"] == CardKind.Slash.ToString()) ?? decision.Choices[0];
        return decision.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).FirstOrDefault() ?? decision.Choices[0];
    }

    SkillProgramStepOutcome INamedAmbushHost.ExecuteNamedAmbush(SkillProgramEffect e, ProgramSkillFrame f) => BeginNamedAmbush(e, f);
}
