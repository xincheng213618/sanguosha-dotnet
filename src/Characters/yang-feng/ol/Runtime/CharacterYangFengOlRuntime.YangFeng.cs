#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangFengOlRuntime
{
    internal const string JiaweiDiscard = "skill-program.yang-feng.jiawei.discard", JiaweiOwnerGain = "skill-program.yang-feng.jiawei.owner-gain", JiaweiTargetGain = "skill-program.yang-feng.jiawei.target-gain", JiaweiCleanup = "skill-program.yang-feng.jiawei.cleanup", QujiaGain = "skill-program.yang-feng.qujia.gain", JiaweiRevealBind = "jiawei-public-top";
    internal static CardConversionSource YangFengSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_yang_feng_ol_YangFeng.YangFengSource(f);
    internal int YangFengCompletedTurns(int owner, int cutoff) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Take(cutoff).OfType<TurnEndedEvent>().Count(e => e.ActorSeat == owner);
    SkillProgramStepOutcome IYangFengHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginYangFeng(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27150)]
    internal bool QualifyYangFeng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveQujiaLimitedTurnEnd))
            return true;
        return context.Window == SkillProgramTriggerWindow.AfterTurnEnded && candidate.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && context.TargetSeat == candidate.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Finished && !_coreProgramEventHistoryOperations.ProgramEventHistory<YangFengStartedEvent>().Any(e => e.Op == SkillProgramEffectOp.ResolveQujiaLimitedTurnEnd && e.Source.OwnerSeat == candidate.OwnerSeat && e.Source.SkillId == candidate.SkillId);
    }

    internal SkillProgramStepOutcome BeginYangFeng(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var jiawei = op == SkillProgramEffectOp.ResolveJiaweiDiscardReveal;
        if (f.YangFeng is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0)
            throw new InvalidOperationException("Yang Feng requires an exact unissued first instruction.");
        var cutoff = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count();
        var completed = YangFengCompletedTurns(f.OwnerSeat, cutoff);
        var x = jiawei ? Math.Min(completed, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp) : 0;
        int target;
        long parent = 0;
        IReadOnlyList<YangFengMaterial> candidates = [];
        if (jiawei)
        {
            var a = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation;
            if (a is null || f.TriggerId is not null || f.WindowContext is not null || f.SelectedTargetSeats is not [var selected] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(selected) || !_coreMatchParticipantsPlayersAccess._players[selected].IsAlive || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, a.UsageGroup)) != 1)
                throw new InvalidOperationException("Jiawei requires its real once-per-Play any-living target activation.");
            YangFengComposition.Activation(f.SkillId, a);
            target = selected;
            candidates = JiaweiDiscardCandidates(f, target);
        }
        else
        {
            if (f.TriggerId is null || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Finished || context.TargetSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().LastOrDefault()is not { } ended || !_coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(ended, f, context) || context.Facts is not { OwnerKilledThisTurn: false } || _coreProgramEventHistoryOperations.ProgramEventHistory<YangFengStartedEvent>().Any(e => e.Op == op && e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId))
                throw new InvalidOperationException("Qujia requires its genuine no-kill completed own actual turn.");
            YangFengComposition.Trigger(f.SkillId, _coreProgramLifecycleOperations.GetProgramTrigger(f));
            target = f.OwnerSeat;
            parent = context.ParentFrameId;
        }

        var required = Math.Min(x, candidates.Count);
        var r = new YangFengReceipt(YangFengSource(f), f.GameplayHash, op, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, parent, target, cutoff, completed, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp, x, required, jiawei ? YangFengStage.DiscardChoice : YangFengStage.QujiaChoice, candidates, [], [], []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YangFeng = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YangFengStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ActualTurn, r.TurnOwner, r.Phase, parent, target, cutoff, completed, r.MaximumHp, x, required));
        if (jiawei && required == 0)
        {
            PayJiaweiDiscard(f);
            ContinueYangFeng(f.Id);
        }
        else
            PublishYangFeng(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> YangFengChoices(ProgramSkillFrame f)
    {
        var r = f.YangFeng!;
        if (r.Stage == YangFengStage.QujiaChoice)
            return Array.AsReadOnly(new[] { "extra", "quota" }.Select(branch => new PromptChoice(new($"yang-feng.{(f.Id)}.{(branch)}"), branch == "extra" ? "执行一个额外回合" : "本局使用【杀】的次数+1并获得一张【杀】", [], [], new Dictionary<string, string> { { "program-action", "yang-feng" }, { "branch", branch } })).ToArray());
        if (r.Stage != YangFengStage.DiscardChoice)
            return[];
        return Array.AsReadOnly(r.Candidates.Where(m => !r.Selected.Any(s => s.CardId == m.CardId)).Select(m =>
        {
            var hidden = m.From.Zone == CardZoneKind.Hand && m.From.OwnerSeat != f.OwnerSeat;
            return new PromptChoice(new($"yang-feng.{(f.Id)}.discard.{(m.From.Zone)}.{(m.Slot)}"), hidden ? $"弃置目标第{(m.Slot + 1)}张手牌" : $"弃置【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).DisplayName)}】", hidden ? [] : [m.CardId], [], new Dictionary<string, string> { { "program-action", "yang-feng" }, { "branch", "discard" }, { "source-zone", m.From.Zone.ToString() }, { "slot-index", m.Slot.ToString() } });
        }).ToArray());
    }

    internal void PublishYangFeng(ProgramSkillFrame f)
    {
        var r = f.YangFeng!;
        var choices = YangFengChoices(f);
        if (choices.Count == 0)
            throw new InvalidOperationException("Yang Feng cannot publish an empty unpaid selection.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage == YangFengStage.QujiaChoice ? "驱驾：选择本局一次的收益。" : $"假威：选择弃置目标的{(r.RequiredDiscard)}张牌（{(r.Selected.Count)}/{(r.RequiredDiscard)}）。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.TargetSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("yang-feng")]
    internal void AnswerYangFeng(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YangFeng: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !YangFengChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Yang Feng requires his exact published private selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == YangFengStage.QujiaChoice)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QujiaChoiceIssuedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ParentId, branch));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YangFeng = r with { QujiaChoice = branch } });
            if (branch == "extra")
            {
                _coreSkillProgramsOperations.PendProgramExtraTurn(f, null);
                FinishYangFeng(f);
                return;
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QujiaSlashBonusGrantedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YangFeng = f.YangFeng!with { Stage = YangFengStage.QujiaGainChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault(c => CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind));
            var material = card is null ? null : new YangFengMaterial(card.Id, card.Kind, CardLocation.DrawPile, 0);
            if (material is not null && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                MoveYangFengEntities(f, [material], CardLocation.Hand(f.OwnerSeat), QujiaGain, YangFengStage.QujiaGainChildren, before);
            else
                RecordYangFengMovement(f.Id, YangFengStage.QujiaGainChildren, QujiaGain, before, 0);
            ContinueYangFeng(f.Id);
            return;
        }

        if (branch != "discard" || r.Stage != YangFengStage.DiscardChoice)
            throw new InvalidOperationException("Unknown Yang Feng selection.");
        var materialSelected = r.Candidates.Single(m => m.From.Zone.ToString() == choice.Parameters["source-zone"] && m.Slot.ToString() == choice.Parameters["slot-index"]);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YangFeng = r with { Selected = r.Selected.Append(materialSelected).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiaweiMaterialSelectedEvent(f.Id, r.Selected.Count, materialSelected));
        if (f.YangFeng!.Selected.Count < r.RequiredDiscard)
            PublishYangFeng(f);
        else
        {
            PayJiaweiDiscard(f);
            ContinueYangFeng(f.Id);
        }
    }

    internal void MoveYangFengEntities(ProgramSkillFrame f, IReadOnlyList<YangFengMaterial> materials, CardLocation to, string reason, YangFengStage stage, long before)
    {
        if (materials.Count > 0)
        {
            if (stage == YangFengStage.DiscardChildren)
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, materials.Select(m => m.CardId).ToArray(), new(reason));
            else
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(materials.Select(m => m.CardId).ToArray(), to, new(reason));
        }

        for (var i = 0; i < materials.Count; i++)
        {
            var m = materials[i];
            var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(v => v.Sequence > before && v.CardId == m.CardId && v.Reason.Value == reason);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YangFengMaterialMovedEvent(f.Id, stage, i, m, move.To, move.Sequence));
        }

        RecordYangFengMovement(f.Id, stage, reason, before, materials.Count);
    }

    internal void RecordYangFengMovement(long id, YangFengStage stage, string reason, long before, int count)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.YangFeng!;
        var move = new YangFengMovement(r.Movements.Count, stage, reason, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { YangFeng = r with { Movements = r.Movements.Append(move).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YangFengMovementIssuedEvent(id, move));
    }

    internal bool DrainYangFengChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueYangFeng(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { YangFeng: { } r } f && f.Id == id)
        {
            if (r.Stage is YangFengStage.DiscardChoice or YangFengStage.QujiaChoice)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishYangFeng(f);
                return;
            }

            if (DrainYangFengChildren(id))
                return;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
            if (r.Stage == YangFengStage.DiscardChildren)
                RevealJiawei(f);
            else if (r.Stage == YangFengStage.RevealChildren)
                SettleJiawei(f, YangFengStage.OwnerGainChildren);
            else if (r.Stage == YangFengStage.OwnerGainChildren)
                SettleJiawei(f, YangFengStage.TargetGainChildren);
            else if (r.Stage == YangFengStage.TargetGainChildren)
                SettleJiawei(f, YangFengStage.CleanupChildren);
            else
            {
                if (r.Stage == YangFengStage.CleanupChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                {
                    _coreCardUseModulesOperations.GrantProgramTurnRuleModifier(f, SkillRuleQuery.CardUseDistanceLimit, SkillRuleOperation.Unlimited, 0, []);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JiaweiDistanceGrantedEvent(f.Id, f.OwnerSeat, r.ActualTurn));
                }

                FinishYangFeng(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            }
        }
    }

    internal void FinishYangFeng(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YangFengCompletedEvent(f.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YangFeng = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27150)]
    internal bool ResumeYangFeng(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YangFeng: not null } f || f.Id != id)
            return false;
        ContinueYangFeng(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27150)]
    internal bool ReturnYangFengMovement(ProgramSkillFrame f)
    {
        if (f.YangFeng is null)
            return false;
        ContinueYangFeng(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveJiaweiDiscardReveal, SkillProgramEffectOp.ResolveQujiaLimitedTurnEnd)]
    internal PromptChoice ChooseYangFengAi(PendingDecision p, ProgramSkillFrame f) => f.YangFeng!.Stage == YangFengStage.QujiaChoice ? p.Choices.First(c => c.Parameters["branch"] == "extra") : p.Choices.OrderBy(c => c.Cards.Count == 0 ? 2 : CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.YangFeng.TargetSeat]) * (f.YangFeng.TargetSeat == f.OwnerSeat ? 1 : -1)).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
