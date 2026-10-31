#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingOlMouRuntime
{
    internal ActualDiscardRecoveryPhaseKey? MouLixianPhase(int owner, SkillProgramTriggerWindow window)
    {
        var phase = _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();
        var kind = window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow ? ActualDiscardRecoveryPhaseKind.Preparation : ActualDiscardRecoveryPhaseKind.Ending;
        return phase is { } p && p.Kind == kind && p.ActorSeat == owner && p.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && p.ActualTurnOwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) ? p : null;
    }

    internal bool MouLixianWindowAvailable(int owner, ProgramSkillWindowContext context) => context.Window switch
    {
        SkillProgramTriggerWindow.AfterDamageApplied => context.TargetSeat == owner && context.Amount > 0,
        SkillProgramTriggerWindow.TurnStartBeforeNormalFlow => MouLixianPreparationUnlocked(owner) && MouLixianPhase(owner, context.Window)is not null,
        SkillProgramTriggerWindow.TurnEnding => MouLixianEndingUnlocked(owner) && MouLixianPhase(owner, context.Window)is not null,
        _ => false
    };
    internal IReadOnlyList<Card> MouLixianMaterials(int owner) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner])).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.WoodenOxGrain(owner))).DistinctBy(c => c.Id).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(_coreMatchParticipantsPlayersAccess._players[owner], c) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(owner, [c.Id])).ToArray());
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> MouLixianOptions(ProgramSkillFrame f, Card card, CardKind kind) => _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card)), hasActualColor: true, physicalCardIds: [card.Id], includeNextActualUseAdjustment: true, conversionSource: MouHuangYueyingSource(f)).Where(o => o.ActionKind != LegalActionKind.Recast).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28960)]
    internal bool CanOfferMouLixian(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouHuangYueyingLixian))
            return true;
        if (candidate.SkillId != MouLixianSkill || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !MouLixianWindowAvailable(candidate.OwnerSeat, context) || _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).ProgramInstances.Where(i => i.SkillId == MouLixianSkill).OrderBy(i => i.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault()?.SkillInstanceId != candidate.SkillInstanceId || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLixianStartedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && e.Window == context.Window && e.OriginFrameId == context.ParentFrameId))
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        return MouLixianMaterials(owner.Seat).Any(card => MouLixianNames(owner.Seat).Any(kind => _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(owner, kind, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card), enforceUsePermission: true, beneficiaryShieldSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card), actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, card)), hasActualColor: true, physicalCardIds: [card.Id], includeNextActualUseAdjustment: true, conversionSource: new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId)).Any(o => o.ActionKind != LegalActionKind.Recast)));
    }

    internal SkillProgramStepOutcome BeginMouLixian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouLixian is not null)
        {
            ContinueMouLixian(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != MouLixianSkill || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !MouLixianWindowAvailable(f.OwnerSeat, context) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Lixian requires its original damage/actually unlocked Preparation or Ending binding.");
        var phase = context.Window == SkillProgramTriggerWindow.AfterDamageApplied ? null : MouLixianPhase(f.OwnerSeat, context.Window);
        var r = new MouLixianReceipt(MouHuangYueyingSource(f), f.GameplayHash, context.ParentFrameId, context.ParentFrameId)
        {
            Stage = MouLixianStage.NameChoice,
            Phase = phase,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLixian = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianStartedEvent(f.Id, r.Source, r.GameplayHash, context.Window, r.OriginFrameId, phase));
        PublishMouLixian(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouLixianChoices(ProgramSkillFrame f)
    {
        var r = f.MouLixian!;
        var choices = new List<PromptChoice>();
        PromptChoice C(string branch, string suffix, string label, int? card = null, CardKind? kind = null, IReadOnlyList<int>? targets = null) => new(new($"mou-lixian.{(f.Id)}.{(branch)}.{(suffix)}"), label, card is { } id ? [id] : [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "mou-lixian", ["branch"] = branch, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["output-kind"] = kind?.ToString() ?? "" });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return[];
        var materials = MouLixianMaterials(f.OwnerSeat);
        if (r.Stage == MouLixianStage.NameChoice)
            choices.AddRange(MouLixianNames(f.OwnerSeat).Where(kind => materials.Any(c => MouLixianOptions(f, c, kind).Count > 0)).Select(kind => C("name", kind.ToString(), $"使用理贤：转化为【{(CardCatalog.Get(kind).DisplayName)}】", kind: kind)));
        else if (r.Stage == MouLixianStage.MaterialChoice && r.OutputKind is { } name)
            choices.AddRange(materials.Where(c => MouLixianOptions(f, c, name).Count > 0).Select(c => C("material", c.Id.ToString(CultureInfo.InvariantCulture), $"将【{(c.DisplayName)}】当【{(CardCatalog.Get(name).DisplayName)}】使用", c.Id, name)));
        else if (r.Stage == MouLixianStage.TargetChoice && r.OutputKind is { } output && r.MaterialId is { } cardId && materials.SingleOrDefault(c => c.Id == cardId)is { } card)
            choices.AddRange(MouLixianOptions(f, card, output).Select(o => C("target", o.Id.Value, o.Description, cardId, output, o.TargetSeats)));
        choices.Add(C("cancel", "none", "取消理贤"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouLixian(ProgramSkillFrame f)
    {
        var choices = MouLixianChoices(f);
        if (choices.Count == 0)
        {
            FinishMouLixian(f);
            return;
        }

        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, choices, f.MouLixian!.Stage switch
        {
            MouLixianStage.NameChoice => "理贤：选择已解锁的锦囊牌名",
            MouLixianStage.MaterialChoice => "理贤：选择一张真实手牌、装备或木牛流马粮",
            _ => "理贤：选择原生合法目标"
        }, f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-lixian")]
    internal void AnswerMouLixian(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLixian: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], MouLixianChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Lixian cannot answer an unpublished source, material or target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (choice.Parameters["branch"])
        {
            case "cancel":
                FinishMouLixian(f);
                return;
            case "name":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLixian = r with { OutputKind = Enum.Parse<CardKind>(choice.Parameters["output-kind"]), Stage = MouLixianStage.MaterialChoice } });
                PublishMouLixian(f);
                return;
            case "material":
                var card = MouLixianMaterials(f.OwnerSeat).Single(c => c.Id == choice.Cards.Single());
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLixian = r with { Stage = MouLixianStage.TargetChoice, MaterialId = card.Id, PrintedKind = card.Kind, From = _cardZonesOperations._cardZones.GetLocation(card.Id), EffectiveSuit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), EffectiveRank = _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card) } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianMaterialSelectedEvent(f.Id, r.OutputKind!.Value, card.Id, card.Kind, _cardZonesOperations._cardZones.GetLocation(card.Id), _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card), _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card)));
                PublishMouLixian(f);
                return;
            case "target":
                DispatchMouLixian(f, choice);
                return;
            default:
                throw new InvalidOperationException("Lixian cannot invent a use branch.");
        }
    }

    internal void DispatchMouLixian(ProgramSkillFrame f, PromptChoice choice)
    {
        var r = f.MouLixian!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var card = MouLixianMaterials(f.OwnerSeat).Single(c => c.Id == r.MaterialId);
        var option = MouLixianOptions(f, card, r.OutputKind!.Value).Single(o => choice.Id.Value == $"mou-lixian.{(f.Id)}.target.{(o.Id.Value)}" && o.TargetSeats.SequenceEqual(choice.Targets));
        if (_cardZonesOperations._cardZones.GetLocation(card.Id) != r.From || card.Kind != r.PrintedKind || _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card) != r.EffectiveSuit || _coreAlcoholIdentityPoliciesOperations.EffectiveOwnedCardRank(actor, card) != r.EffectiveRank)
            throw new InvalidOperationException("Lixian's selected real entity changed its original zone or effective appearance before native acceptance.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLixian = r with { Stage = MouLixianStage.NativeIssuing, Paid = true, Targets = option.TargetSeats, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        r = f.MouLixian!;
        if (option.NextActualUseAdjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = option.TargetSeats;
        var appearance = card with
        {
            Suit = r.EffectiveSuit,
            Rank = r.EffectiveRank ?? card.Rank
        };
        var id = _coreNativeCardUseOperations.BeginCardUse(appearance, actor.Seat, option.TargetSeats, r.OutputKind.Value, physicalCardIds: [card.Id], conversionSource: r.Source);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouLixianOrigin is null)
            throw new InvalidOperationException("Lixian's true material use must bind its owning native return before payment.");
        _corePhysicalMovementFacadesOperations.MoveCard(card, r.From!.Value, CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        var current = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == f.Id);
        if (current.MouLixian!.MovementSequence <= r.Before || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouLixianMaterialPaidEvent>().Count(p => p.UseId == id && p.CardId == card.Id) != 1)
            throw new InvalidOperationException("Lixian's original material must sign its native movement before removal children.");
        var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(id, r.Source.SkillId, r.Source.BindingId, actor.Seat, [card.Id], r.OutputKind.Value, true, targets));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, appearance, actor.Seat, targets, option.ActionKind, option.TargetCardId, option.RequiredCardKind, r.OutputKind.Value);
    }

    internal void ContinueMouLixian(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLixian: { } r } f || f.Id != id)
            return;
        if (!r.Paid)
        {
            PublishMouLixian(f);
            return;
        }

        if (!IsReturnedMouLixianUse(f))
            throw new InvalidOperationException("Lixian cannot advance before its whole accepted native Use returns.");
        if (DrainMouHuangYueying(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        FinishMouLixian(f);
    }

    internal void FinishMouLixian(ProgramSkillFrame f)
    {
        var r = f.MouLixian!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLixianCompletedEvent(f.Id, r.UseId, r.Paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLixian = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, r.Paid);
    }
}
