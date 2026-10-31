#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoShuangOlRuntime
{
    internal const string CaoShuangPlacementReason = "skill-program.caoshuang.shanzhuan.place";
    internal bool CaoShuangPlaceCardEligible(int chooser, int target, Card card, CardZoneKind zone) => !card.IsGeneralWeapon && (CoreNativeCardClassificationsRuntime.IsDelayedCard(card.Kind) || _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[target], card)is Suit.Heart or Suit.Diamond or Suit.Club or Suit.Spade) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, card, new(zone, target), OwnedCardMoveIntent.Transfer);
    internal bool CaoShuangCanPlace(int owner, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && owner != target && _coreMatchParticipantsPlayersAccess._players[target] is { IsAlive: true, JudgmentAreaAbolished: false } && _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0 && (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Any(c => CaoShuangPlaceCardEligible(owner, target, c, CardZoneKind.Hand)) || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Any(c => CaoShuangPlaceCardEligible(owner, target, c, CardZoneKind.Equipment)));
    internal SkillProgramStepOutcome BeginCaoShuangShanzhuan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.CaoShuangShanzhuan is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } target, Amount: > 0 } context || context.SourceSeat != f.OwnerSeat || target == f.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } damage || !_coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(f, damage))
            throw new InvalidOperationException("Shanzhuan requires the exact real other-target after-damage source window.");
        if (!CaoShuangCanPlace(f.OwnerSeat, target))
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoShuangShanzhuan = new(CaoShuangSource(f), f.GameplayHash, f.InstructionIndex, context.ParentFrameId, target), Step = ResolutionFrameStep.AwaitingResponse });
        PublishCaoShuangShanzhuan(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CaoShuangShanzhuanChoices(ProgramSkillFrame f)
    {
        var r = f.CaoShuangShanzhuan!;
        if (r.Placed || !CaoShuangCanPlace(f.OwnerSeat, r.TargetSeat))
            return[];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.TargetSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, canSelect: (zone, card) => CaoShuangPlaceCardEligible(f.OwnerSeat, r.TargetSeat, card, zone)).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "caoshuang-shanzhuan" } }).ToArray());
    }

    internal void PublishCaoShuangShanzhuan(ProgramSkillFrame f)
    {
        var choices = CaoShuangShanzhuanChoices(f);
        if (choices.Count == 0)
        {
            CompleteCaoShuangProgram(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "擅专：将受伤角色的一张手牌或装备牌置于其判定区", choices.SelectMany(c => c.Cards).ToArray(), [], f.CaoShuangShanzhuan!.TargetSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("caoshuang-shanzhuan")]
    internal void AnswerCaoShuangShanzhuan(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoShuangShanzhuan: { Placed: false } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], CaoShuangShanzhuanChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Shanzhuan lost the actual empty-judgment target or anonymous owned-card slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            CompleteCaoShuangProgram(f, false);
            return;
        }

        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
        var card = (zone == CardZoneKind.Hand ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]) : _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat]))[slot];
        var from = new CardLocation(zone, r.TargetSeat);
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], card);
        var effective = CoreNativeCardClassificationsRuntime.IsDelayedCard(card.Kind) ? card.Kind : CoreNativeCardClassificationsRuntime.IsRedSuit(suit) ? CardKind.Indulgence : CardKind.SupplyShortage;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoShuangShanzhuan = r with { Placed = true, CardId = card.Id, From = from, EffectiveKind = effective, SequenceBefore = before, SequenceAfter = before }, Step = ResolutionFrameStep.ResolvingEffect });
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Judgment(r.TargetSeat), new(CaoShuangPlacementReason));
        if (card.Kind != effective)
            _coreJudgmentEffectiveCardsJudgmentEffectiveCardKindsAccess._judgmentEffectiveCardKinds[card.Id] = effective;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.CaoShuangShanzhuan!with
        {
            SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoShuangShanzhuan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoShuangShanzhuanPlacedEvent(f.Id, r.Source, r.TargetSeat, card.Id, from, card.Kind, effective, suit, before, r.SequenceAfter));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DelayedCardPlacedEvent(f.Id, card.Id, effective, f.OwnerSeat, r.TargetSeat));
        ContinueCaoShuangProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.PlaceDamagedCardAsDelayedTrick)]
    internal PromptChoice ChooseAiCaoShuangPlacement(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => c.Cards.Count > 0 ? CardCatalog.Get(_coreResponseEntityExchangeOperations.EntityAtCurrentLocation(c.Cards.Single()).Kind).HandKeepValue : 35).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    internal bool DrainCaoShuangChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueCaoShuangProgram(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.CaoShuangTuogu is null && f.CaoShuangShanzhuan is null)
            return;
        if (DrainCaoShuangChildren(id))
            return;
        if (f.CaoShuangTuogu is { Granted: false })
        {
            PublishCaoShuangTuogu(f);
            return;
        }

        if (f.CaoShuangShanzhuan is { Placed: false })
        {
            PublishCaoShuangShanzhuan(f);
            return;
        }

        CompleteCaoShuangProgram(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
    }

    internal void CompleteCaoShuangProgram(ProgramSkillFrame f, bool completed)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoShuangTuogu = null, CaoShuangShanzhuan = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26060)]
    internal bool ResumeCaoShuang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.CaoShuangTuogu is null && f.CaoShuangShanzhuan is null)
            return false;
        ContinueCaoShuangProgram(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26060)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26060)]
    internal bool CanContinuePaidCaoShuang(ProgramSkillFrame f) => f.CaoShuangTuogu is { Granted: true } && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoShuangTuoguInheritedEvent>().Any(e => e.FrameId == f.Id && e.Source == f.CaoShuangTuogu.Source) || f.CaoShuangShanzhuan is { Placed: true } && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoShuangShanzhuanPlacedEvent>().Any(e => e.FrameId == f.Id && e.Source == f.CaoShuangShanzhuan.Source);
}
