#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoXiaoQiaoHappyGodRuntime
{
    internal const string HappyQiaoAction = "happy-qiao-skills";
    internal static CardConversionSource HappyQiaoSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IHappyQiaoHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginHappyQiaoSkill(frame, operation);
    [ProgramPipelineHook(ProgramPipelinePhase.HealthChangeSubject, 42400)]
    internal bool HappyQiaoHealthSubject(int target) => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.HappyQiaoHealth is not null && _coreNationalSkillsOperations.HasRuntimeSkill(p, HappyQiaoComposition.Tongxin));
    internal CardUseFrame? HappyQiaoCausingUse(HpChangeContext change)
    {
        if (change.ParentFrameId is not { } id || change.HpBefore == change.HpAfter)
            return null;
        if (change.Kind == HpChangeKind.Damage)
        {
            var damageOwner = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id)is DamageFrame damage ? damage.ParentFrameId : id;
            var attack = _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(damageOwner);
            return attack is global::CardGame.Core.NativeOwners.CardAttackHandle ? _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId) : null;
        }

        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id)is RecoveryReplacementFrame recovery)
            return recovery.Attempt.Completion.Producer is RecoveryAttemptProducer.CardUse or RecoveryAttemptProducer.GroupCard ? _coreCardUseLifecycleOperations.LifecycleCardUse(recovery.ParentFrameId) : null;
        return _coreCardUseLifecycleOperations.LifecycleCardUse(id);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureHealthChange, 42400)]
    internal void CaptureHappyQiaoRedHealth(HpChangeContext change)
    {
        if (!HappyQiaoHealthSubject(change.TargetSeat) || HappyQiaoCausingUse(change)is not { Action: { } action } use || !(action.Type == CardActionType.Use || action.Type == CardActionType.Response && action.EffectiveKind is CardKind.Dodge or CardKind.Nullification) || !(action.EffectiveIsRed ?? (action.EffectiveSuit is Suit.Heart or Suit.Diamond)) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive || !HasHappyQiaoHealth(action.ActorSeat))
            return;
        foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], SkillProgramTriggerWindow.AfterHealthChanged).Where(c => c.SkillId == HappyQiaoComposition.Tongxin))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoRedHealthQualifiedEvent(change.Id, c.OwnerSeat, c.SkillInstanceId, use.Id, action.ActionId, change.TargetSeat, change.Kind));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IncludeHealthChangeCandidates, 42400)]
    internal IReadOnlyList<ProgramTriggerCandidate> HappyQiaoHealthCandidates(HpChangeContext change, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        var peers = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && HasHappyQiaoHealth(p.Seat)).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.AfterHealthChanged)).Where(c => c.SkillId == HappyQiaoComposition.Tongxin && HappyQiaoRedQualification(c.OwnerSeat, c.SkillInstanceId, change));
        return Array.AsReadOnly(ordinary.Where(c => c.SkillId != HappyQiaoComposition.Tongxin).Concat(peers).DistinctBy(c => (c.OwnerSeat, c.SkillId, c.BindingId, c.SkillInstanceId)).ToArray());
    }

    internal bool HappyQiaoRedQualification(int owner, string instance, HpChangeContext change) => _coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoRedHealthQualifiedEvent>().Count(e => e.ChangeId == change.Id && e.Owner == owner && e.Instance == instance && e.Target == change.TargetSeat && e.Kind == change.Kind) == 1;
    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 110)]
    internal bool IsHappyQiaoForeignHealthCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId == HappyQiaoComposition.Tongxin && candidate.BindingId == "pool-health" && context is { Window: SkillProgramTriggerWindow.AfterHealthChanged, HpChange: { } change } && context.OwnerSeat == candidate.OwnerSeat && context.TargetSeat == change.TargetSeat && context.ParentFrameId == change.Id && HasHappyQiaoHealth(candidate.OwnerSeat) && HappyQiaoRedQualification(candidate.OwnerSeat, candidate.SkillInstanceId, change) && _resolutionStackOperations._resolutionStack.OfType<HpChangedTriggerWindowFrame>().LastOrDefault()is { } window && window.Id == context.ParentFrameId && window.Change == change && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] == candidate;
    internal bool HappyQiaoShutuChange(int owner, HpChangeContext change) => HasHappyQiaoHealth(owner) && change.TargetSeat == owner && change.HpBefore != change.HpAfter && (change is { Kind: HpChangeKind.Recovery, HappyQiaoPool: HappyQiaoPool.DaQiao } && change.HpAfter > change.HpBefore || change is { Kind: HpChangeKind.Damage, HappyQiaoPool: HappyQiaoPool.XiaoQiao } && change.HpAfter < change.HpBefore);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 42400, SkillProgramEffectOp.ResolveHappyQiaoShutu, SkillProgramEffectOp.ResolveHappyQiaoTongxin)]
    internal bool HappyQiaoSkillQualified(ProgramTriggerLegalityContext gate) => gate.WindowContext?.HpChange is { } change && (gate.SkillId == HappyQiaoComposition.Shutu ? HappyQiaoShutuChange(gate.OwnerSeat, change) : gate.SkillId == HappyQiaoComposition.Tongxin && HappyQiaoRedQualification(gate.OwnerSeat, gate.Candidate.SkillInstanceId, change));
    internal SkillProgramStepOutcome BeginHappyQiaoSkill(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.HappyQiao is not null || frame.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHealthChanged, HpChange: { } change } context || !HappyQiaoOriginalHealthCandidate(frame) || !HasHappyQiaoHealth(frame.OwnerSeat) || !(operation == SkillProgramEffectOp.ResolveHappyQiaoShutu && frame.SkillId == HappyQiaoComposition.Shutu && HappyQiaoShutuChange(frame.OwnerSeat, change) || operation == SkillProgramEffectOp.ResolveHappyQiaoTongxin && frame.SkillId == HappyQiaoComposition.Tongxin && HappyQiaoRedQualification(frame.OwnerSeat, frame.SkillInstanceId, change)))
            throw new InvalidOperationException("Happy Qiao requires her original actual selected-pool HP change or her exact red native Use.");
        var stage = operation == SkillProgramEffectOp.ResolveHappyQiaoTongxin ? HappyQiaoStage.HealChoice : change.Kind == HpChangeKind.Recovery ? HappyQiaoStage.SuitChoice : HappyQiaoStage.UseChoice;
        var receipt = new HappyQiaoReceipt(HappyQiaoSource(frame), frame.GameplayHash, operation, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, change, stage)
        {
            Target = frame.OwnerSeat,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoStartedEvent(frame.Id, receipt.Source, receipt.Hash, operation, receipt.Parent, receipt.Turn, change.Id));
        ContinueHappyQiao(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool HappyQiaoOriginalHealthCandidate(ProgramSkillFrame frame)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is HpChangedTriggerWindowFrame hp && hp.Id == frame.WindowContext?.ParentFrameId && hp.Change == frame.WindowContext.HpChange && hp.CandidateIndex < hp.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, hp.Candidates[hp.CandidateIndex]);
    }

    internal bool HappyQiaoEqualHealable(int owner) => _coreMatchParticipantsPlayersAccess._players[owner].HappyQiaoHealth is { } h && h.DaQiaoHp == h.XiaoQiaoHp && h.AnyWounded;
    internal bool HappyQiaoEqualRedHand(int owner)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]);
        return hand.Count(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c) == Suit.Heart) == hand.Count(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c) == Suit.Diamond);
    }

    internal IReadOnlyList<PromptChoice> HappyQiaoDiscardChoices(ProgramSkillFrame frame) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, frame.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (_, card) => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card)is Suit.Heart or Suit.Diamond);
    internal PromptChoice HappyQiaoChoice(ProgramSkillFrame frame, string branch, string label, string value = "", IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"happy-qiao.{(frame.Id)}.{(frame.HappyQiao!.Stage)}.{(branch)}.{(value)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = HappyQiaoAction, ["branch"] = branch, ["value"] = value });
    internal IReadOnlyList<PromptChoice> HappyQiaoChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.HappyQiao!;
        var choices = new List<PromptChoice>();
        if (receipt.Stage == HappyQiaoStage.SuitChoice)
            choices.AddRange(new[] { ("HH", "取得两张红桃"), ("HD", "取得红桃和方片各一张"), ("DD", "取得两张方片") }.Select(x => HappyQiaoChoice(frame, "suits", x.Item2, x.Item1)));
        if (receipt.Stage == HappyQiaoStage.UseChoice)
            choices.AddRange(HappyQiaoUsePlans(frame).Select((plan, i) => HappyQiaoChoice(frame, "use", plan.Action.Description, i.ToString(CultureInfo.InvariantCulture), plan.Cards, plan.Targets)));
        if (receipt.Stage == HappyQiaoStage.HealChoice && HappyQiaoEqualHealable(frame.OwnerSeat))
            choices.Add(HappyQiaoChoice(frame, "heal", "回复1点体力"));
        if (receipt.Stage == HappyQiaoStage.DiscardChoice && HappyQiaoEqualRedHand(frame.OwnerSeat))
            choices.AddRange(HappyQiaoDiscardChoices(frame).Select(c => c with { Id = new($"happy-qiao-cost.{(frame.Id)}.{(c.Id.Value)}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = HappyQiaoAction, ["branch"] = "discard" } }));
        if (receipt.Stage == HappyQiaoStage.TargetChoice)
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => HappyQiaoChoice(frame, "target", "对" + p.Name + "造成1点伤害", p.Seat.ToString(CultureInfo.InvariantCulture), targets: [p.Seat])));
        if (receipt.Stage is HappyQiaoStage.UseChoice or HappyQiaoStage.HealChoice or HappyQiaoStage.DiscardChoice)
            choices.Add(HappyQiaoChoice(frame, "decline", "不发动此项效果"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PresentHappyQiao(ProgramSkillFrame frame)
    {
        var choices = HappyQiaoChoices(frame);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(frame, frame.OwnerSeat, choices, frame.SkillId == HappyQiaoComposition.Shutu ? "殊途：请选择。" : "同心：依次选择。", frame.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(HappyQiaoAction)]
    internal void AnswerHappyQiaoSkill(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyQiao: { } receipt } frame || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !HappyQiaoChoices(frame).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [answer])))
            throw new InvalidOperationException("Happy Qiao requires her exact original live private choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = answer.Parameters["branch"];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoBranchEvent(frame.Id, receipt.Stage, branch));
        if (receipt.Stage == HappyQiaoStage.HealChoice)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt with { Stage = branch == "heal" ? HappyQiaoStage.HealChildren : HappyQiaoStage.DiscardChoice } });
            ContinueHappyQiao(frame.Id);
            return;
        }

        if (branch == "decline")
        {
            FinishHappyQiaoSkill(frame);
            return;
        }

        if (receipt.Stage == HappyQiaoStage.SuitChoice)
        {
            var suits = answer.Parameters["value"].Select(ch => ch == 'H' ? Suit.Heart : Suit.Diamond).ToArray();
            var remaining = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).ToList();
            var gained = new List<int>();
            foreach (var suit in suits)
                if (remaining.FirstOrDefault(c => c.Suit == suit)is { } card)
                {
                    gained.Add(card.Id);
                    remaining.Remove(card);
                }

            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt with { Stage = HappyQiaoStage.GainChildren, Suits = suits, Gained = gained, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            if (gained.Count > 0)
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(gained, CardLocation.Hand(frame.OwnerSeat), new("skill.happy.shutu.chosen-red"), (_, _) => RecordHappyQiaoMovement(frame.Id));
            ContinueHappyQiao(frame.Id);
            return;
        }

        if (receipt.Stage == HappyQiaoStage.UseChoice)
        {
            var plan = HappyQiaoUsePlans(frame)[int.Parse(answer.Parameters["value"], CultureInfo.InvariantCulture)];
            var native = new HappyQiaoNativeUse(frame.Id, frame.InstructionIndex, receipt.Source, receipt.Hash, receipt.Turn, plan, 0, 0)
            {
                Costs = plan.Cards.Select(id =>
                {
                    var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
                    return new CardActionCost(id, card.Kind, _cardZonesOperations._cardZones.GetLocation(id));
                }).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt with { Stage = HappyQiaoStage.IssuingUse, Native = native } });
            IssueHappyQiaoShutuUse(frame);
            return;
        }

        if (receipt.Stage == HappyQiaoStage.DiscardChoice)
        {
            var location = new CardLocation(Enum.Parse<CardZoneKind>(answer.Parameters["source-zone"]), frame.OwnerSeat);
            var card = _cardZonesOperations._cardZones.CardsAt(location)[int.Parse(answer.Parameters["slot-index"], CultureInfo.InvariantCulture)];
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt with { Stage = HappyQiaoStage.TargetChoice, CardId = card.Id, CostFrom = location } });
            PresentHappyQiao(frame);
            return;
        }

        if (receipt.Stage == HappyQiaoStage.TargetChoice)
        {
            if (!HappyQiaoEqualRedHand(frame.OwnerSeat) || _cardZonesOperations._cardZones.GetLocation(receipt.CardId) != receipt.CostFrom || !HappyQiaoDiscardChoices(frame).Any(c => c.Cards.Contains(receipt.CardId)))
                throw new InvalidOperationException("Tongxin's live red hand equality or actual red discard material expired.");
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt with { Stage = HappyQiaoStage.DiscardChildren, Target = answer.Targets.Single(), Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
            ContinueHappyQiao(frame.Id);
            return;
        }

        throw new InvalidOperationException("Happy Qiao cannot repeat a completed native payment.");
    }

    internal void RecordHappyQiaoMovement(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var receipt = frame.HappyQiao!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyQiao = receipt with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoMovementEvent(frameId, receipt.Stage, receipt.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > receipt.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && receipt.Gained.Contains(m.CardId) && m.To == CardLocation.Hand(frame.OwnerSeat))));
    }

    internal bool DrainHappyQiaoSkill(ProgramSkillFrame frame)
    {
        var movement = frame.HappyQiao!.Stage is HappyQiaoStage.GainChildren or HappyQiaoStage.DiscardChildren;
        return _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 42400)]
    internal bool ContinueHappyQiao(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappyQiao: not null } current || current.Id != frameId)
            return false;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { HappyQiao: { } receipt } frame && frame.Id == frameId)
        {
            if (receipt.Stage is HappyQiaoStage.IssuingUse or HappyQiaoStage.Declaration or HappyQiaoStage.NativeUse)
                return true;
            if (frame.AttackAttempt is not null)
                return true;
            if (receipt.Stage == HappyQiaoStage.HealChildren && !receipt.HealIssued)
            {
                if (_characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(frame.Id, frame.OwnerSeat, frame.SkillId, "tongxin:" + frame.Id, frame.SkillInstanceId))
                    return true;
                frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
                receipt = frame.HappyQiao!;
                if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
                {
                    FinishHappyQiaoSkill(frame);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt with { HealIssued = true } });
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frame.Id, frame.OwnerSeat, frame.OwnerSeat, 1, null, null);
                continue;
            }

            if (receipt.Stage == HappyQiaoStage.DiscardChildren && receipt.CostBatch is null)
            {
                if (_characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(frame.Id, frame.OwnerSeat, frame.SkillId, "tongxin:" + frame.Id, frame.SkillInstanceId))
                    return true;
                frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
                receipt = frame.HappyQiao!;
                if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.GetLocation(receipt.CardId) != receipt.CostFrom || !HappyQiaoDiscardChoices(frame).Any(c => c.Cards.Contains(receipt.CardId)))
                {
                    FinishHappyQiaoSkill(frame);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt with { Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, frame.OwnerSeat, [receipt.CardId], new("skill.happy.tongxin.red-cost"), (batch, moves) =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
                    var paid = moves.Single();
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { HappyQiao = current.HappyQiao!with { After = paid.Sequence, CostBatch = batch } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoCostPaidEvent(frame.Id, batch, paid.CardId, paid.From, paid.To, paid.Sequence));
                });
                continue;
            }

            if (DrainHappyQiaoSkill(frame))
                return true;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
            receipt = frame.HappyQiao!;
            if (frame.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            {
                FinishHappyQiaoSkill(frame);
                return true;
            }

            if (receipt.Stage == HappyQiaoStage.HealChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyQiao = receipt with { Stage = HappyQiaoStage.DiscardChoice } });
                continue;
            }

            if (receipt.Stage == HappyQiaoStage.HealChoice && !HappyQiaoEqualHealable(frame.OwnerSeat))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyQiao = receipt with { Stage = HappyQiaoStage.DiscardChoice } });
                continue;
            }

            if (receipt.Stage == HappyQiaoStage.DiscardChoice && (!HappyQiaoEqualRedHand(frame.OwnerSeat) || HappyQiaoDiscardChoices(frame).Count == 0))
            {
                FinishHappyQiaoSkill(frame);
                return true;
            }

            if (receipt.Stage is HappyQiaoStage.GainChildren or HappyQiaoStage.ReturnedUse or HappyQiaoStage.DamageChildren)
            {
                FinishHappyQiaoSkill(frame);
                return true;
            }

            if (receipt.Stage == HappyQiaoStage.DiscardChildren)
            {
                if (receipt.CostBatch is null || !_coreProgramEventHistoryOperations.ProgramEventHistory<HappyQiaoCostPaidEvent>().Any(e => e.FrameId == frame.Id && e.BatchId == receipt.CostBatch && e.CardId == receipt.CardId && e.Sequence == receipt.After))
                    throw new InvalidOperationException("Tongxin cannot damage before its actual red logical discard completes.");
                if (!_coreMatchParticipantsPlayersAccess._players[receipt.Target].IsAlive)
                {
                    FinishHappyQiaoSkill(frame);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = receipt with { Stage = HappyQiaoStage.DamageChildren } });
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Damage(frame, receipt.Target, 1);
                return true;
            }

            PresentHappyQiao(frame);
            return true;
        }

        return true;
    }

    internal void FinishHappyQiaoSkill(ProgramSkillFrame frame)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyQiaoCompletedEvent(frame.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyQiao = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 42400)]
    internal bool ReturnHappyQiaoMovement(ProgramSkillFrame frame) => frame.HappyQiao is not null && ContinueHappyQiao(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 42400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 42400)]
    internal bool RetainHappyQiaoPaid(ProgramSkillFrame frame) => frame.HappyQiao is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyQiaoShutu)]
    internal PromptChoice HappyQiaoShutuAi(PendingDecision pending, ProgramSkillFrame frame) => pending.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyQiaoTongxin)]
    internal PromptChoice HappyQiaoTongxinAi(PendingDecision pending, ProgramSkillFrame frame) => pending.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "decline") ?? pending.Choices.First();
}
