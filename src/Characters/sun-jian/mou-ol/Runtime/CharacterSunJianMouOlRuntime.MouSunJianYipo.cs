#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunJianMouOlRuntime
{
    internal const string MouSunJianOlYipoDrawReason = "skill-program.mou-ol:yipo.draw", MouSunJianOlYipoDiscardReason = "skill-program.mou-ol:yipo.discard";
    internal static CardConversionSource MouSunJianOlSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IMouSunJianOlHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveMouSunJianYipo ? BeginMouSunJianOlYipo(f) : BeginMouSunJianOlHulie(f, op);
    internal bool HasMouSunJianOlHpSubject(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramTriggerWindow.AfterHealthChanged).Any(c => c.SkillId == MouSunJianOlComposition.Yipo && _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.ResolveMouSunJianYipo }]);
    internal void CaptureMouSunJianOlHp(HpChangeContext change)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouSunJianYipo) || change.Amount <= 0 || change.HpAfter <= 0 || change.HpAfter == change.HpBefore || !_coreMatchParticipantsPlayersAccess._players[change.TargetSeat].IsAlive || _coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlHpFirstReachedEvent>().Any(e => e.OwnerSeat == change.TargetSeat && e.Hp == change.HpAfter))
            return;
        var qualified = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[change.TargetSeat], SkillProgramTriggerWindow.AfterHealthChanged).Where(c => c.SkillId == MouSunJianOlComposition.Yipo && _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is [{ Op: SkillProgramEffectOp.ResolveMouSunJianYipo }]).ToArray();
        if (qualified.Length == 0)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlHpFirstReachedEvent(change.TargetSeat, change.HpAfter, change.Id));
        foreach (var c in qualified)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlYipoQualifiedEvent(new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, change.Id, change.ParentFrameId, change.HpBefore, change.HpAfter));
    }

    internal bool QualifiedMouSunJianOlYipo(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == MouSunJianOlComposition.Yipo && c.BindingId == "first-positive-hp" && w.Window == SkillProgramTriggerWindow.AfterHealthChanged && w.OwnerSeat == c.OwnerSeat && c.OccurrenceIndex == 0 && w.HpChange is { Amount: > 0, HpAfter: > 0 } h && h.TargetSeat == c.OwnerSeat && h.HpBefore != h.HpAfter && w.ParentFrameId == h.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlHpFirstReachedEvent>().SingleOrDefault(e => e.OwnerSeat == c.OwnerSeat && e.Hp == h.HpAfter)?.ChangeId == h.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlYipoQualifiedEvent>().Count(e => e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && e.GameplayHash == c.GameplayHash && e.ChangeId == h.Id && e.ParentId == h.ParentFrameId && e.HpBefore == h.HpBefore && e.HpAfter == h.HpAfter) == 1;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32500)]
    internal bool CanOfferMouSunJianOlYipo(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => !t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMouSunJianYipo) || QualifiedMouSunJianOlYipo(c, w);
    internal SkillProgramStepOutcome BeginMouSunJianOlYipo(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouSunJianOlYipo is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || !QualifiedMouSunJianOlYipo(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), w))
            throw new InvalidOperationException("Yipo requires its genuine first reached positive HP occurrence.");
        var r = new MouSunJianOlYipoReceipt(MouSunJianOlSource(f), f.GameplayHash, w.ParentFrameId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlYipo = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlYipoStartedEvent(f.Id, r.Source, r.GameplayHash, r.ChangeId));
        PublishMouSunJianOlYipo(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Card[] MouSunJianOlYipoDiscardable(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(seat)).Where(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal IReadOnlyList<PromptChoice> MouSunJianOlYipoChoices(ProgramSkillFrame f)
    {
        var r = f.MouSunJianOlYipo!;
        var choices = new List<PromptChoice>();
        PromptChoice Choice(string branch, string label, int? target = null, int? card = null) => new(new($"mou-yipo.{(f.Id)}.{(r.Stage)}.{(branch)}.{(target)}.{(card)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mou-sun-jian-yipo", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
        if (r.Stage == MouSunJianOlYipoStage.Target)
        {
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => Choice("target", p.Name, p.Seat)));
            choices.Add(Choice("decline", "不发动毅魄"));
        }
        else if (r.Stage == MouSunJianOlYipoStage.Option)
        {
            var x = Math.Max(1, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
            choices.Add(Choice("draw-many", $"令其摸{(x)}张牌，然后其弃置一张牌"));
            choices.Add(Choice("discard-many", $"令其摸一张牌，然后其弃置{(x)}张牌"));
            choices.Add(Choice("decline", "不发动毅魄"));
        }
        else if (r.Stage == MouSunJianOlYipoStage.Discard && r.TargetSeat is { } payer && _coreMatchParticipantsPlayersAccess._players[payer].IsAlive)
        {
            var cards = MouSunJianOlYipoDiscardable(payer);
            var count = Math.Min(r.DiscardCount, cards.Length);
            if (r.Selected.Count < count)
                choices.AddRange(cards.Where(card => !r.Selected.Contains(card.Id)).Select(card => Choice("card", $"弃置【{(card.DisplayName)}】", card: card.Id)));
            choices.AddRange(r.Selected.Select(id => Choice("unpick", $"撤回【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", card: id)));
            if (r.Selected.Count == count)
                choices.Add(Choice("confirm", $"弃置{(count)}张牌"));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouSunJianOlYipo(ProgramSkillFrame f)
    {
        var r = f.MouSunJianOlYipo!;
        var chooser = r.Stage == MouSunJianOlYipoStage.Discard ? r.TargetSeat!.Value : f.OwnerSeat;
        var choices = MouSunJianOlYipoChoices(f);
        if (choices.Count == 0)
        {
            FinishMouSunJianOl(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MouSunJianOlYipoStage.Discard ? "毅魄：选择自己要弃置的牌。" : "毅魄：选择一名角色及摸弃方式。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-sun-jian-yipo")]
    internal void AnswerMouSunJianOlYipo(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouSunJianOlYipo: { } r } f || !ValidMouSunJianOlYipo(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouSunJianOlYipoStage.Discard ? r.TargetSeat : f.OwnerSeat) || !MouSunJianOlYipoChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Yipo changed its actual chooser, original HP event or published payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "decline")
        {
            FinishMouSunJianOl(f);
            return;
        }

        if (r.Stage == MouSunJianOlYipoStage.Target)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlYipo = r with { TargetSeat = choice.Targets.Single(), Stage = MouSunJianOlYipoStage.Option } });
            PublishMouSunJianOlYipo(f);
            return;
        }

        if (r.Stage == MouSunJianOlYipoStage.Option)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat!.Value].IsAlive)
            {
                FinishMouSunJianOl(f);
                return;
            }

            var x = Math.Max(1, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp);
            var draw = branch == "draw-many" ? x : 1;
            var discard = branch == "draw-many" ? 1 : x;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlYipo = r with { Branch = branch, X = x, DrawCount = draw, DiscardCount = discard, Stage = MouSunJianOlYipoStage.Drawing } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlYipoChosenEvent(f.Id, r.TargetSeat.Value, branch, x, draw, discard));
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.TargetSeat.Value, draw, new(MouSunJianOlYipoDrawReason));
            return;
        }

        if (r.Stage != MouSunJianOlYipoStage.Discard)
            throw new InvalidOperationException("Yipo cannot pay before its native draw returns.");
        if (branch is "card" or "unpick")
        {
            var selected = branch == "card" ? r.Selected.Append(choice.Cards.Single()).ToArray() : r.Selected.Where(id => id != choice.Cards.Single()).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlYipo = r with { Selected = selected } });
            PublishMouSunJianOlYipo(f);
            return;
        }

        var eligible = MouSunJianOlYipoDiscardable(r.TargetSeat!.Value);
        if (branch != "confirm" || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat.Value].IsAlive || r.Selected.Count != Math.Min(r.DiscardCount, eligible.Length) || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Any(id => eligible.All(card => card.Id != id)))
            throw new InvalidOperationException("Yipo requires the recipient's current discardable HE entities and exact frozen requested count before paying.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlYipo = r with { Stage = MouSunJianOlYipoStage.DiscardChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (r.Selected.Count > 0)
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TargetSeat!.Value, r.Selected, new(MouSunJianOlYipoDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        r = f.MouSunJianOlYipo!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlYipo = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlYipoDiscardEvent(f.Id, r.TargetSeat!.Value, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, r.Selected.Count));
        ContinueMouSunJianOlYipo(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouSunJianYipo)]
    internal void ReturnMouSunJianOlYipoDraw(ProgramSkillFrame f, NativeDrawOutcome draw)
    {
        if (!ValidMouSunJianOlYipo(f) || f.MouSunJianOlYipo is not { Stage: MouSunJianOlYipoStage.Drawing } r || draw.RecipientSeat != r.TargetSeat || draw.RequestedCount != r.DrawCount)
            throw new InvalidOperationException("Yipo lost its once-issued native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlYipo = r with { Draw = draw, Stage = MouSunJianOlYipoStage.DrawChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlYipoDrawEvent(f.Id, draw.RecipientSeat, draw.RequestedCount, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count));
        ContinueMouSunJianOlYipo(f.Id);
    }

    internal void ContinueMouSunJianOlYipo(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouSunJianOlYipo: { } r } f || f.Id != id)
            return;
        if (!ValidMouSunJianOlYipo(f))
            throw new InvalidOperationException("Yipo lost its accepted native effect cursor.");
        if (r.Stage is MouSunJianOlYipoStage.Target or MouSunJianOlYipoStage.Option or MouSunJianOlYipoStage.Discard)
        {
            PublishMouSunJianOlYipo(f);
            return;
        }

        if (r.Stage == MouSunJianOlYipoStage.Drawing)
            return;
        if (DrainMouSunJianOlChildren(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouSunJianOlYipo!;
        if (r.Stage == MouSunJianOlYipoStage.DrawChildren && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat!.Value].IsAlive && MouSunJianOlYipoDiscardable(r.TargetSeat.Value).Length > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, MouSunJianOlYipo = r with { Stage = MouSunJianOlYipoStage.Discard } });
            PublishMouSunJianOlYipo(f);
            return;
        }

        FinishMouSunJianOl(f);
    }

    internal bool DrainMouSunJianOlChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void FinishMouSunJianOl(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlCompletedEvent(f.Id, f.SkillId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlYipo = null, MouSunJianOlHulie = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 32500)]
    internal bool ResumeMouSunJianOl(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id)
            return false;
        if (f.MouSunJianOlYipo is not null)
        {
            ContinueMouSunJianOlYipo(id);
            return true;
        }

        if (f.MouSunJianOlHulie is { Stage: MouSunJianOlHulieStage.Children })
        {
            if (!DrainMouSunJianOlChildren(id))
                FinishMouSunJianOl(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return true;
        }

        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 32500)]
    internal bool ReturnMouSunJianOlMovement(ProgramSkillFrame f) => (f.MouSunJianOlYipo is not null || f.MouSunJianOlHulie is not null) && ResumeMouSunJianOl(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouSunJianYipo)]
    internal PromptChoice AiMouSunJianOlYipo(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.MouSunJianOlYipo!.Stage == MouSunJianOlYipoStage.Target)
            return p.Choices.First(c => c.Targets.Contains(f.OwnerSeat));
        if (f.MouSunJianOlYipo.Stage == MouSunJianOlYipoStage.Option)
            return p.Choices.Single(c => c.Parameters["branch"] == (CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[f.MouSunJianOlYipo.TargetSeat!.Value]) ? "draw-many" : "discard-many"));
        return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "confirm") ?? p.Choices.Where(c => c.Parameters["branch"] == "card").OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[p.PlayerSeat])).First();
    }
}
