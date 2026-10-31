#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiYanHappyGodRuntime
{
    internal const string HappyEzhiDraw = "skill-program.happy:ezhi.draw", HappyEzhiDiscard = "skill-program.happy:ezhi.discard", HappyGujingDiscard = "skill-program.happy:gujing.discard";
    internal bool HasHappyWeiYanEzhiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveHappyWeiYanEzhi);

    internal IReadOnlyList<HappyWeiYanBone> HappyBones(int holder) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(holder).Select(card => HappyBoneAt(card.Id, CardLocation.Equipment(holder), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)is { } bone ? bone with { Holder = holder } : null).OfType<HappyWeiYanBone>().ToArray();
    internal bool TryHappyEzhiAction(ProgramSkillWindowContext context, out CardActionContext action, out long parent)
    {
        action = null !;
        parent = 0;
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.CardUse is not { } info)
            return false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } normal } use && normal.ActionId == info.CardActionId)
        {
            action = normal;
            parent = use.Id;
        }
        else if (_resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { CompletedResponseReturn: { IsCommitted: true } response } window && window.Action.ActionId == info.CardActionId && _coreResponseUseCompletionOperations.IsCompletedResponseUse(window.Action, response.Kind))
        {
            action = window.Action;
            parent = window.ParentFrameId;
        }

        return parent > 0 && (action.Type == CardActionType.Use || action.ActorSeat == action.ProviderSeat && action.RequesterSeat is null) && action.PhysicalCards.Count > 0 && action.PhysicalCards.Select(c => c.CardId).Distinct().Count() == action.PhysicalCards.Count;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 42301, SkillProgramEffectOp.ResolveHappyWeiYanEzhi, SkillProgramEffectOp.ResolveHappyWeiYanBoneReturn)]
    internal bool HappyEzhiEligible(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => trigger.Id == "bone-use" ? TryHappyEzhiAction(context, out var action, out _) && action.ActorSeat != candidate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive && HappyBones(action.ActorSeat).Count > 0 : DiscardedHappyBones(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber).Count > 0;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.AfterCommonStart, 42303, SkillProgramEffectOp.ResolveHappyWeiYanGujing)]
    internal bool HappyGujingCanActivate(CharacterState owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat && HappyBoneAvailableSlots(p.Seat).Any());
    internal SkillProgramStepOutcome BeginHappyWeiYanAdditional(ProgramSkillFrame frame, SkillProgramEffectOp operation)
    {
        if (frame.InstructionIndex != 1 || frame.HappyWeiYan is not null)
            throw new InvalidOperationException("Happy Wei Yan requires one original unpaid instruction.");
        CardActionContext? action = null;
        long nativeParent = 0;
        var target = frame.OwnerSeat;
        HappyWeiYanStage stage;
        if (operation == SkillProgramEffectOp.ResolveHappyWeiYanEzhi)
        {
            if (frame.WindowContext is not { } context || !TryHappyEzhiAction(context, out var actual, out nativeParent) || actual.ActorSeat == frame.OwnerSeat || HappyBones(actual.ActorSeat).Count == 0)
                throw new InvalidOperationException("Ezhi lost its other bone holder's actual physical Use.");
            action = actual;
            target = actual.ActorSeat;
            stage = HappyWeiYanStage.Drawing;
        }
        else if (operation == SkillProgramEffectOp.ResolveHappyWeiYanBoneReturn)
        {
            if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } ended || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(ended))
                throw new InvalidOperationException("Discarded bones require their genuine completed actual turn.");
            stage = HappyWeiYanStage.ReturnChoice;
        }
        else
        {
            if (operation != SkillProgramEffectOp.ResolveHappyWeiYanGujing || frame.ActivationId != "bone-path" || frame.TriggerId is not null || frame.SelectedTargetSeats is not [var selected] || selected == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[selected].IsAlive || !HappyBoneAvailableSlots(selected).Any())
                throw new InvalidOperationException("Gujing requires its paid limited Play activation and selected other living slot holder.");
            target = selected;
            stage = HappyWeiYanStage.Hand;
        }

        var receipt = new HappyWeiYanReceipt(HappyWeiYanSource(frame), frame.GameplayHash, operation, frame.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber)
        {
            Stage = stage,
            Target = target,
            NativeParentId = nativeParent,
            OriginalAction = action,
            ActionId = action?.ActionId ?? 0,
            UseId = action?.Type == CardActionType.Use ? nativeParent : 0,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            Discarded = operation == SkillProgramEffectOp.ResolveHappyWeiYanBoneReturn ? DiscardedHappyBones(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber) : []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappyWeiYanStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, operation, receipt.ParentId, receipt.ActualTurn, target, receipt.UseId, receipt.ActionId));
        if (stage == HappyWeiYanStage.Drawing)
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, 2, new(HappyEzhiDraw));
        else
            ContinueHappyWeiYan(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveHappyWeiYanEzhi)]
    internal void ReturnHappyEzhiDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (frame.HappyWeiYan is not { Stage: HappyWeiYanStage.Drawing, Draw: null } receipt || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != 2 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Ezhi lost its original once-issued two-card native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { Stage = HappyWeiYanStage.DrawChildren, Draw = outcome, Before = checked((int)outcome.SequenceBefore), After = checked((int)outcome.SequenceAfter), Paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter && m.Reason.Value == HappyEzhiDraw).ToArray() }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        ContinueHappyWeiYan(frame.Id);
    }

    internal bool ContinueHappyEzhi(ProgramSkillFrame initial)
    {
        var id = initial.Id;
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
        {
            var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            RequireHappyWeiYan(frame);
            var receipt = frame.HappyWeiYan!;
            if (receipt.Stage == HappyWeiYanStage.Drawing)
                return true;
            if (receipt.Stage is HappyWeiYanStage.DrawChildren or HappyWeiYanStage.BoneDiscardChildren)
            {
                if (DrainHappyWeiYan(frame))
                    return true;
                frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                receipt = frame.HappyWeiYan!;
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
                if (receipt.Stage == HappyWeiYanStage.BoneDiscardChildren)
                {
                    CompleteHappyEzhiPayment(frame);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { Stage = HappyWeiYanStage.BoneChoice, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Paid = [] } });
                receipt = frame.HappyWeiYan!;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[receipt.Target].IsAlive || HappyBones(receipt.Target).Count == 0)
            {
                FinishHappyWeiYan(frame);
                return true;
            }

            var choices = HappyWeiYanAdditionalChoices(frame);
            if (choices.Count == 0)
            {
                FinishHappyWeiYan(frame);
                return true;
            }

            if (choices.Count == 1)
            {
                PayHappyEzhiBone(frame, int.Parse(choices[0].Parameters["option"][5..], System.Globalization.CultureInfo.InvariantCulture));
                continue;
            }

            PresentHappyWeiYanAdditional(frame, receipt.Target, choices);
            return true;
        }

        return true;
    }

    internal IReadOnlyList<PromptChoice> HappyWeiYanAdditionalChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.HappyWeiYan!;
        PromptChoice Choice(string option, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"happy-wei-yan.{(frame.Id)}.{(receipt.Stage)}.{(receipt.Cursor)}.{(option)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "happy-wei-yan", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["option"] = option });
        if (receipt.Stage == HappyWeiYanStage.BoneChoice)
        {
            var discardable = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, receipt.Target, receipt.Target, [CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).SelectMany(c => c.Cards).ToHashSet();
            return HappyBones(receipt.Target).Where(b => discardable.Contains(b.CardId)).Select(b => Choice("bone:" + b.CardId, "弃置“" + EquipmentCatalog.GetSlotName(b.Slot) + "”的骨", [b.CardId], [receipt.Target])).ToArray();
        }

        if (receipt.Stage == HappyWeiYanStage.GujingSelection)
        {
            var result = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat && receipt.SelectedBones.All(b => b.Holder != p.Seat)).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, p.Seat)).SelectMany(p =>
            {
                var allowed = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, p.Seat, [CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).SelectMany(c => c.Cards).ToHashSet();
                return HappyBones(p.Seat).Where(b => allowed.Contains(b.CardId)).Select(b => Choice("abolish:" + b.CardId, "弃置“" + p.Name + "”的骨并废除“" + EquipmentCatalog.GetSlotName(b.Slot) + "”", [b.CardId], [p.Seat]));
            }).ToList();
            result.Add(Choice("finish", "完成选择"));
            return result;
        }

        if (receipt.Stage != HappyWeiYanStage.ReturnChoice || receipt.Cursor >= receipt.Discarded.Count)
            return[];
        var plans = HappyBoneReturnPlans(frame);
        return plans.Select((plan, index) => Choice("use:" + index, plan.Description, [receipt.Discarded[receipt.Cursor].CardId], plan.TargetSeats)).Append(Choice("skip", "不使用这张骨")).ToArray();
    }

    internal void PresentHappyWeiYanAdditional(ProgramSkillFrame frame, int chooser, IReadOnlyList<PromptChoice> choices)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, skill.Name + "：请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void AnswerHappyWeiYanAdditional(ProgramSkillFrame frame, PromptChoice chosen)
    {
        var receipt = frame.HappyWeiYan!;
        var option = chosen.Parameters["option"];
        if (receipt.Stage == HappyWeiYanStage.BoneChoice)
        {
            PayHappyEzhiBone(frame, chosen.Cards.Single());
            ContinueHappyWeiYan(frame.Id);
            return;
        }

        if (receipt.Stage == HappyWeiYanStage.GujingSelection)
        {
            if (option == "finish")
            {
                BeginHappyGujingPayments(frame);
                return;
            }

            var bone = HappyBones(chosen.Targets.Single()).Single(b => b.CardId == chosen.Cards.Single());
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyWeiYan = receipt with { SelectedBones = receipt.SelectedBones.Append(bone).ToArray() } });
            ContinueHappyWeiYan(frame.Id);
            return;
        }

        if (option == "skip")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HappyWeiYan = receipt with { Cursor = receipt.Cursor + 1 } });
            ContinueHappyWeiYan(frame.Id);
            return;
        }

        var index = int.Parse(option[4..], System.Globalization.CultureInfo.InvariantCulture);
        var action = HappyBoneReturnPlans(frame)[index];
        var native = new HappyWeiYanBoneUse(frame.Id, receipt.Source, receipt.GameplayHash, receipt.ActualTurn, receipt.Cursor, receipt.Discarded[receipt.Cursor], action, 0, 0);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { Stage = HappyWeiYanStage.IssuingUse, NativeUse = native } });
        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], action);
    }

    internal void PayHappyEzhiBone(ProgramSkillFrame frame, int id)
    {
        var receipt = frame.HappyWeiYan!;
        var bone = HappyBones(receipt.Target).Single(b => b.CardId == id);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HappyWeiYan = receipt with { Stage = HappyWeiYanStage.BoneDiscardChildren, SelectedBone = bone, CardId = id, Before = before, After = before, Paid = [] }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, receipt.Target, [id], new(HappyEzhiDiscard), (_, _) => SaveHappyWeiYanMove(frame.Id, HappyWeiYanStage.BoneDiscardChildren, before));
    }

    internal void CompleteHappyEzhiPayment(ProgramSkillFrame frame)
    {
        var receipt = frame.HappyWeiYan!;
        var bone = receipt.SelectedBone ?? throw new InvalidOperationException("Ezhi requires its paid original bone category.");
        var action = receipt.OriginalAction!;
        if (receipt.Paid is not [var paid] || paid.CardId != bone.CardId || paid.From != CardLocation.Equipment(receipt.Target) || paid.Reason.Value != HappyEzhiDiscard || HappyBoneAt(bone.CardId, paid.From, receipt.Before)is not { } original || original.Sequence != bone.Sequence || original.PrintedKind != bone.PrintedKind)
            throw new InvalidOperationException("Ezhi must actually discard its frozen genuine bone before comparing the original categories.");
        var cancel = global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(bone.PrintedKind) == global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind);
        var fact = new HappyWeiYanEzhiPaidEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.ActualTurn, receipt.ParentId, receipt.NativeParentId, action.ActionId, action.ActorSeat, action.EffectiveKind, bone.CardId, bone.PrintedKind, receipt.Before, receipt.After, cancel);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        if (cancel)
        {
            var proof = new HappyWeiYanCancellation(frame.Id, receipt.Source, receipt.GameplayHash, receipt.ActualTurn, receipt.ParentId, receipt.NativeParentId, action, bone, receipt.Before, receipt.After)
            {
                Draw = receipt.Draw
            };
            if (action.Type == CardActionType.Use)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(receipt.NativeParentId, use => use with { HappyWeiYanCancellation = use.HappyWeiYanCancellation ?? proof });
            else if (action.EffectiveKind == CardKind.Dodge)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(receipt.NativeParentId, use => use with { HappyWeiYanResponses = (use.HappyWeiYanResponses ?? []).Append(proof).ToArray() });
            else if (action.EffectiveKind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } pending && pending.Id == receipt.NativeParentId && pending.NamedAmbushResponse?.ActionId == action.ActionId)
                _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { HappyWeiYanCancellation = proof });
            else
                throw new InvalidOperationException("Ezhi lost its actual true response Use owner.");
        }

        FinishHappyWeiYan(frame);
    }

    internal bool HappyEzhiDistance(int actor, int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<HappyWeiYanEzhiPaidEvent>().Any(e => !e.Canceled && e.Source.OwnerSeat == actor && e.Actor == target && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program!.GameplayHash);
    [ProgramDistanceRule(ProgramDistanceStage.CardUseExemption, 42300)]
    internal bool HappyEzhiCardDistance(ProgramCardDistanceQuery query) => HappyEzhiDistance(query.Actor.Seat, query.Target.Seat);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 42300)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 42300)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 42300)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 42300)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 42300)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 42300)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 42300)]
    internal bool HappyEzhiSlashDistance(ProgramSlashDistanceQuery query) => HappyEzhiDistance(query.Actor.Seat, query.Target.Seat);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyWeiYanEzhi)]
    internal PromptChoice HappyEzhiAi(PendingDecision decision) => decision.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyWeiYanGujing)]
    internal PromptChoice HappyGujingAi(PendingDecision decision) => decision.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveHappyWeiYanBoneReturn)]
    internal PromptChoice HappyBoneReturnAi(PendingDecision decision) => decision.Choices.First();
}
