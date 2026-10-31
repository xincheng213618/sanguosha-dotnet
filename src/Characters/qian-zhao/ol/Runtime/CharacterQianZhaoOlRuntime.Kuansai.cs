#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQianZhaoOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 120)]
    internal bool KuansaiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasKuansaiCapability;
    internal const string KuansaiGiftReason = "skill-program.qian-zhao.kuansai.gift";
    internal bool HasKuansaiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveKuansaiTargetChoice);

    internal CardUseFrame? KuansaiUse(ProgramSkillWindowContext context) => context is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } c } && _coreCardUseLifecycleOperations.LifecycleCardUse(c.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } action } use && action.ActionId == c.CardActionId && action.EffectiveKind == use.CardKind && !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.CardKind) && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Id == context.ParentFrameId && w.ParentFrameId == use.Id && w.Action.ActionId == action.ActionId) ? use : null;
    internal static IReadOnlyList<int> KuansaiActualTargets(CardUseFrame use) => global::CardGame.Core.RuntimeSkills.RuntimeRules_qian_zhao_ol_Kuansai.KuansaiActualTargets(use);
    internal bool CanKuansaiRecover(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].HasHpValue && _coreMatchParticipantsPlayersAccess._players[owner].Hp < _coreMatchParticipantsPlayersAccess._players[owner].MaxHp;
    internal IReadOnlyList<Card> KuansaiGiftCards(int chooser, int owner) => chooser == owner || !_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive ? [] : Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[chooser]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(chooser)).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Transfer)).ToArray());
    internal bool KuansaiHasChoice(int target, int owner) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && (CanKuansaiRecover(owner) || KuansaiGiftCards(target, owner).Count > 0);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26620)]
    internal bool QualifyQianZhao(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferWeifuCategoryTargets))
            return HasIssuedWeifuCandidate(c, context) && _coreCardUseLifecycleOperations.LifecycleCardUse(context.CardUse!.ParentCardUseFrameId)is { WeifuUse.Offered: false } use && WeifuCanAddTargets(use);
        if (!t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveKuansaiTargetChoice))
            return true;
        return HasKuansaiCapability && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].HasHpValue && KuansaiUse(context)is { } native && KuansaiActualTargets(native).Count >= _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Hp && KuansaiActualTargets(native).Any(s => KuansaiHasChoice(s, c.OwnerSeat));
    }

    internal SkillProgramStepOutcome BeginKuansai(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != "ol:kuansai" || f.Kuansai is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } context || KuansaiUse(context)is not { } use || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].HasHpValue)
            throw new InvalidOperationException("Kuansai requires its actual finalized-use observer activation.");
        var targets = KuansaiActualTargets(use);
        if (targets.Count < _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp)
            throw new InvalidOperationException("Kuansai cannot replace the actual target-count threshold.");
        var r = new KuansaiReceipt(QianZhaoSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, use.Id, use.Action!.ActionId, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp)
        {
            Targets = targets
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuansai = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuansaiStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwnerSeat, use.Id, r.ActionId, r.ThresholdHp, targets.Count));
        PublishQianZhaoChoice(f.OwnerSeat, f, KuansaiChoices(f), "款塞：选择此牌的一个实际目标。", false);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> KuansaiChoices(ProgramSkillFrame f)
    {
        var r = f.Kuansai!;
        var result = new List<PromptChoice>();
        Dictionary<string, string> P(string branch) => new()
        {
            ["program-action"] = "qian-zhao",
            ["branch"] = branch
        };
        if (r.Stage == KuansaiStage.Target)
        {
            foreach (var seat in r.Targets.Where(s => KuansaiHasChoice(s, f.OwnerSeat)))
                result.Add(new(new($"kuansai.{(f.Id)}.target.{(seat)}"), "令" + _coreMatchParticipantsPlayersAccess._players[seat].Name + "选择款塞的效果", [], [seat], P("kuansai-target")));
        }
        else if (r.Stage == KuansaiStage.Benefit && r.ChooserSeat is { } chooser)
        {
            foreach (var card in KuansaiGiftCards(chooser, f.OwnerSeat))
                result.Add(new(new($"kuansai.{(f.Id)}.gift.{(card.Id)}"), "交给" + _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name + "【" + card.DisplayName + "】", [card.Id], [], P("kuansai-gift")));
            if (CanKuansaiRecover(f.OwnerSeat))
                result.Add(new(new($"kuansai.{(f.Id)}.recover"), "令" + _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name + "回复1点体力", [], [], P("kuansai-recover")));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void PayKuansai(ProgramSkillFrame f, PromptChoice selected)
    {
        var r = f.Kuansai!;
        var chooser = r.ChooserSeat!.Value;
        var gift = selected.Parameters["branch"] == "kuansai-gift";
        var card = gift ? _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Cards.Single()) : null;
        var from = card is null ? (CardLocation? )null : _cardZonesOperations._cardZones.GetLocation(card.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuansai = r with { Stage = KuansaiStage.Children, Gift = gift, CardId = card?.Id, From = from, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (card is not null)
            _corePhysicalMovementFacadesOperations.MoveCard(card, from!.Value, CardLocation.Hand(f.OwnerSeat), new(KuansaiGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, from!.Value.OwnerSeat!.Value, f.OwnerSeat, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
        else if (CanKuansaiRecover(f.OwnerSeat))
        {
            if (!_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(f.Id, f.OwnerSeat, f.OwnerSeat, 1, new(RecoveryAttemptProducer.Program, f.InstructionIndex)))
            {
                var recovery = _coreNativeRecoveryFramesOperations.BeginRecovery(f.Id, f.OwnerSeat, f.OwnerSeat, 1);
                var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
                owner.Hp = Math.Min(owner.MaxHp, owner.Hp + 1);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(f.OwnerSeat, f.OwnerSeat, 1, owner.Hp));
                _coreRuntimeOperations.PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
            }
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuansai = f.Kuansai!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuansaiPaidEvent(f.Id, chooser, gift, card?.Id, from, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 26621)]
    internal bool ResumeKuansai(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Kuansai: { } r } f || f.Id != id)
            return false;
        AssertQianZhao(f);
        if (r.Stage == KuansaiStage.Children)
        {
            if (DrainQianZhaoChildren(id))
                return true;
            FinishQianZhao(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return true;
        }

        var choices = KuansaiChoices(f);
        if (choices.Count == 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishQianZhao(f);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishQianZhaoChoice(r.ChooserSeat ?? f.OwnerSeat, f, choices, r.Stage == KuansaiStage.Target ? "款塞：选择此牌的一个实际目标。" : "款塞：交给牵招一张牌，或令其回复1点体力。", r.Stage == KuansaiStage.Benefit);
        return true;
    }

    [ProgramAnswer("qian-zhao")]
    internal void AnswerQianZhao(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Qian Zhao lost its owning choice frame.");
        AssertQianZhao(f);
        var choices = f.WeifuTargets is not null ? WeifuTargetChoices(f) : KuansaiChoices(f);
        var chooser = f.Kuansai is { Stage: KuansaiStage.Benefit, ChooserSeat: { } seat } ? seat : f.OwnerSeat;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != chooser || !choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Qian Zhao requires the exact actual participant's published choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (f.WeifuTargets is { } targets)
        {
            if (selected.Targets.Count == 0)
            {
                CompleteWeifuTargets(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WeifuTargets = targets with { Selected = targets.Selected.Concat(selected.Targets).ToArray() } });
            var next = WeifuTargetChoices(f);
            if (next.Count == 1)
                CompleteWeifuTargets(f);
            else
                PublishQianZhaoChoice(f.OwnerSeat, f, next, "威抚：继续选择额外目标，或完成选择。", false);
            return;
        }

        var r = f.Kuansai!;
        if (r.Stage == KuansaiStage.Target)
        {
            var target = selected.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kuansai = r with { Stage = KuansaiStage.Benefit, ChooserSeat = target } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuansaiTargetChosenEvent(f.Id, target));
            _ = ResumeKuansai(f.Id);
            return;
        }

        PayKuansai(f, selected);
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferWeifuCategoryTargets, SkillProgramEffectOp.ResolveKuansaiTargetChoice)]
    internal PromptChoice ChooseQianZhaoAi(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.WeifuTargets is not null)
        {
            var use = RequireWeifuTargetUse(f);
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
            var draw = use.CardKind is CardKind.DrawTwo or CardKind.Peach or CardKind.Alcohol;
            var hint = draw ? new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false) : new(0, 0, 0, 0, 0, 1, false, false);
            return p.Choices.Select(c => (Choice: c, Score: c.Targets.Count == 0 ? 0 : _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, c.Targets[0], hint))).OrderByDescending(c => c.Score).ThenBy(c => c.Choice.Id.Value, StringComparer.Ordinal).First().Choice;
        }

        if (f.Kuansai is { Stage: KuansaiStage.Target })
            return p.Choices.OrderByDescending(c => KuansaiGiftCards(c.Targets.Single(), f.OwnerSeat).Count).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
        return p.Choices.OrderByDescending(c => c.Parameters.GetValueOrDefault("branch") == "kuansai-recover").ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }
}
