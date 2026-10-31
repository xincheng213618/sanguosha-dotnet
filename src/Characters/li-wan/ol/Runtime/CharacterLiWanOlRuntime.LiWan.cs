#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiWanOlRuntime
{
    internal const string LianjuGainReason = "skill-program.li-wan.lianju.gain", SilvDrawReason = "skill-program.li-wan.silv.draw", SilvRefundReason = "skill-program.li-wan.silv.refund";
    SkillProgramStepOutcome global::CardGame.Core.ILiWanProgramHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginLiWan(frame, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26720)]
    internal bool CanOfferLiWan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [{ } effect] || !LiWanComposition.Handles(effect.Op))
            return true;
        if (effect.Op is SkillProgramEffectOp.ResolveLianjuOppositeColorReturn or SkillProgramEffectOp.ReturnDiscardedSilvEntity)
            return CanRunIssuedLiWanCandidate(c, context);
        return effect.Op == SkillProgramEffectOp.OfferLianjuSameColorGift ? context.Window == SkillProgramTriggerWindow.TurnEnding && context.SourceSeat == c.OwnerSeat && context.TargetSeat == c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat) : context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == c.OwnerSeat && context.Amount > 0 && context.DamageFrameId is not null;
    }

    internal SkillProgramStepOutcome BeginLiWan(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.LiWan is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !CanOfferLiWan(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Li Wan requires her exact unissued native event candidate.");
        var r = new LiWanReceipt(LiWanSource(f), f.GameplayHash, op, f.InstructionIndex, context.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, LianjuEndingSerial, LiWanStage.Color);
        if (op == SkillProgramEffectOp.ResolveLianjuOppositeColorReturn)
        {
            var promise = LianjuDuePromises()[context.OccurrenceIndex];
            r = r with
            {
                IsRed = !promise.IsRed,
                RecipientSeat = f.OwnerSeat,
                PromiseFrameId = promise.FrameId,
                Stage = LiWanStage.Cards,
                ChosenCards = []
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LianjuPromiseConsumedEvent(promise.FrameId, context.ParentFrameId, LianjuEndingSerial));
        }

        if (op == SkillProgramEffectOp.ReturnDiscardedSilvEntity)
        {
            var move = context.MovementBatch!.Movements[context.MovementIndex!.Value];
            r = r with
            {
                RecipientSeat = _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(move)!.Value.OwnerSeat!.Value,
                RefundSequence = move.Sequence,
                ChosenCards = [move.CardId]
            };
        }

        if (op == SkillProgramEffectOp.DrawSilvMarkedCardAfterDamage)
        {
            var damage = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == context.DamageFrameId) ?? throw new InvalidOperationException("Silv requires its real applied damage frame.");
            r = r with
            {
                RecipientSeat = f.OwnerSeat,
                Damage = new(damage.Id, damage.ParentFrameId, context.Amount, f.OwnerSeat)
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiWan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiWanStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.ActualTurn, r.EndingSerial, r.PromiseFrameId, r.RefundSequence));
        if (op is SkillProgramEffectOp.DrawSilvMarkedCardAfterDamage or SkillProgramEffectOp.ReturnDiscardedSilvEntity)
        {
            PayLiWan(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishLiWan(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> LiWanChoices(ProgramSkillFrame f)
    {
        var r = f.LiWan!;
        var list = new List<PromptChoice>();
        void Add(string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => list.Add(new(new($"li-wan.{(f.Id)}.{(branch)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "li-wan", ["branch"] = branch }));
        if (r.Stage == LiWanStage.Color)
        {
            Add("red", "选择红色牌（可以不选牌）");
            Add("black", "选择黑色牌（可以不选牌）");
        }
        else if (r.Stage == LiWanStage.Recipient)
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                Add($"recipient.{(p.Seat)}", $"令{(p.Name)}获得牌", targets: [p.Seat]);
        else if (r.Stage == LiWanStage.Cards)
        {
            if (r.ChosenCards is not { } selected || r.IsRed is not { } red)
                throw new InvalidOperationException("Lianju lost its frozen color draft.");
            if (selected.Count < 2)
                foreach (var id in LianjuEligibleCards(r.Op == SkillProgramEffectOp.OfferLianjuSameColorGift ? f.OwnerSeat : _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, red).Except(selected))
                    Add($"card.{(id)}", $"选择【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", [id]);
            Add("finish", $"确定获得{(selected.Count)}张牌");
        }

        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishLiWan(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = LiWanChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "联句：选择颜色、获得者及至多两张同色的本回合使用牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("li-wan")]
    internal void AnswerLiWan(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiWan: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !LiWanChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Li Wan lost her exact public native draft.");
        AssertLiWan(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (r.Stage == LiWanStage.Color)
            r = r with
            {
                IsRed = branch == "red",
                Stage = LiWanStage.Recipient
            };
        else if (r.Stage == LiWanStage.Recipient)
            r = r with
            {
                RecipientSeat = choice.Targets.Single(),
                Stage = LiWanStage.Cards,
                ChosenCards = []
            };
        else if (r.Stage == LiWanStage.Cards && branch != "finish")
            r = r with
            {
                ChosenCards = r.ChosenCards!.Concat(choice.Cards).ToArray()
            };
        else
        {
            PayLiWan(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiWan = r });
        PublishLiWan(f);
    }

    internal void PayLiWan(ProgramSkillFrame f)
    {
        var r = f.LiWan!;
        var recipient = r.RecipientSeat ?? throw new InvalidOperationException("Li Wan has no frozen recipient.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, LiWan = r with { Stage = LiWanStage.Children, Before = before, After = before }, PendingMovementContinuation = new(recipient, 0, null) });
        var count = 0;
        if (_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
        {
            if (r.Op == SkillProgramEffectOp.DrawSilvMarkedCardAfterDamage)
                count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[recipient], 1, true, new(SilvDrawReason)).Count;
            else
            {
                var ids = r.ChosenCards!.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DiscardPile).ToArray();
                if (ids.Length != r.ChosenCards!.Count)
                    throw new InvalidOperationException("Lianju cannot substitute or recycle a selected entity before payment.");
                if (ids.Length > 0)
                    _corePhysicalMovementFacadesOperations.MoveCards(ids.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), CardLocation.DiscardPile, CardLocation.Hand(recipient), new(r.Op == SkillProgramEffectOp.ReturnDiscardedSilvEntity ? SilvRefundReason : LianjuGainReason));
                count = ids.Length;
            }
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiWan = f.LiWan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiWanPaidEvent(f.Id, recipient, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
        if (r.Op == SkillProgramEffectOp.OfferLianjuSameColorGift)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LianjuPromiseGrantedEvent(f.Id, r.Source, r.GameplayHash, r.EndingSerial, r.ActualTurn, recipient, r.IsRed!.Value));
        if (r.Op != SkillProgramEffectOp.ReturnDiscardedSilvEntity)
        {
            var issuers = r.Op == SkillProgramEffectOp.DrawSilvMarkedCardAfterDamage ? new[]
            {
                (Source: r.Source, Hash: r.GameplayHash)
            }

            : _coreSkillProgramsOperations.EnabledUniqueProgramTriggers(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], SkillProgramTriggerWindow.AfterDamageApplied).Where(b => b.Trigger.Effects is [{ Op: SkillProgramEffectOp.DrawSilvMarkedCardAfterDamage }]).Select(b => (Source: new CardConversionSource(b.SkillId, b.Trigger.Id, f.OwnerSeat, b.SkillInstanceId), Hash: b.Program.GameplayHash)).ToArray();
            foreach (var move in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && m.To == CardLocation.Hand(recipient) && m.Reason.Value is LianjuGainReason or SilvDrawReason).ToArray())
                foreach (var issuer in issuers.Take(1))
                    if (SilvMark(move.CardId)is null)
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SilvEntityMarkedEvent(move.CardId, issuer.Source, issuer.Hash, f.Id, move.Sequence));
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainLiWanChildren(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 26720)]
    internal bool ResumeLiWan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiWan: { } r } f || f.Id != id)
            return false;
        AssertLiWan(f);
        if (r.Stage != LiWanStage.Children)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishLiWan(f);
            return true;
        }

        if (DrainLiWanChildren(id))
            return true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiWanCompletedEvent(id));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiWan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26720)]
    internal bool ReturnLiWanMovement(ProgramSkillFrame f) => f.LiWan is not null && ResumeLiWan(f.Id);
    [ProgramAiChoice(SkillProgramEffectOp.OfferLianjuSameColorGift, SkillProgramEffectOp.ResolveLianjuOppositeColorReturn)]
    internal PromptChoice AiLiWan(PendingDecision prompt, ProgramSkillFrame f) => f.LiWan!.Stage switch
    {
        LiWanStage.Color => prompt.Choices.First(c => c.Parameters["branch"] == (LianjuEligibleCards(f.OwnerSeat, true).Count >= LianjuEligibleCards(f.OwnerSeat, false).Count ? "red" : "black")),
        LiWanStage.Recipient => prompt.Choices.OrderBy(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets[0]]).Count).First(),
        _ => prompt.Choices.FirstOrDefault(c => c.Cards.Count > 0) ?? prompt.Choices.Single()};
}
