#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFangOlRuntime
{
    internal SkillProgramStepOutcome YoudiProgramBaitDiscard(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var chooserSeat] || chooserSeat == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[chooserSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "诱敌的目标已失效，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat]).Count == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "诱敌的手牌已空，没有可弃置的手牌，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "诱敌的技能拥有者已失效，剩余结算取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        PresentYoudiBaitPrompt(active, chooserSeat);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PresentYoudiBaitPrompt(ProgramSkillFrame frame, int chooserSeat)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var choices = hand.Select((_, slot) => new PromptChoice(new ChoiceId($"youdi-bait.frame-{(frame.Id)}.slot-{(slot)}"), $"弃置 {(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 的第 {(slot + 1)} 张手牌。", [], [frame.OwnerSeat], new Dictionary<string, string> { ["program-action"] = "youdi-bait-discard", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["slot-index"] = slot.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, chooserSeat, $"【{(presentation.Name)}】请弃置 {(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 的一张手牌。", [], [frame.OwnerSeat], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooserSeat,
            SkillPrompt = new SkillPromptPresentation(frame.SkillId, presentation.Name, $"{(presentation.Name)} · 弃置手牌", $"暗置选择 {(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 的一张手牌弃置；若弃置的牌不为【杀】，其获得你的一张手牌，黑色牌其摸一张牌。"),
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("youdi-bait-discard")]
    internal void ResolveYoudiBaitChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Youdi bait choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.YoudiBaitDiscard } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Youdi bait choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var chooserSeat] || !int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), System.Globalization.CultureInfo.InvariantCulture, out var slot))
            throw new InvalidOperationException("The Youdi bait choice lost its chooser or slot.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != chooserSeat)
            throw new InvalidOperationException("The Youdi bait chooser changed while suspended.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner);
        if (!_coreMatchParticipantsPlayersAccess._players[chooserSeat].IsAlive || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "诱敌的参与者或技能实例已失效，剩余结算取消。");
            return;
        }

        if (slot < 0 || slot >= hand.Count)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "公布的诱敌手牌牌位已失效，剩余结算取消。");
            return;
        }

        var card = hand[slot];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, chooserSeat, card, CardLocation.Hand(owner.Seat), new($"skill-program.{(frame.SkillId)}.youdi-discard"));
        var took = false;
        var chooser = _coreMatchParticipantsPlayersAccess._players[chooserSeat];
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind))
        {
            var chooserHand = _corePhysicalOwnedCardQueriesOperations.GetHand(chooser);
            if (chooserHand.Count > 0)
            {
                var taken = chooserHand[_randomOperations._random.Next(chooserHand.Count)];
                _corePhysicalMovementFacadesOperations.MoveCard(taken, CardLocation.Hand(chooserSeat), CardLocation.Processing, new($"skill-program.{(frame.SkillId)}.youdi-take"), movement => _characterBianFuRenOlOperations.RecordBianFuRenOwnedTransferProducer(movement, owner.Seat, frame.Id));
                _corePhysicalMovementFacadesOperations.MoveCard(taken, CardLocation.Processing, CardLocation.Hand(owner.Seat), new($"skill-program.{(frame.SkillId)}.youdi-take"));
                took = true;
            }
        }

        var drawn = 0;
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, card)))
        {
            _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, new($"skill-program.{(frame.SkillId)}.youdi-draw"));
            drawn = 1;
        }

        _coreMatchLogOperations.AddLog("SkillEffect", took && drawn > 0 ? $"{(owner.Name)} 诱敌弃置【{(card.DisplayName)}】：获得 {(_coreMatchParticipantsPlayersAccess._players[chooserSeat].Name)} 一张手牌并摸一张牌。" : took ? $"{(owner.Name)} 诱敌弃置【{(card.DisplayName)}】：获得 {(_coreMatchParticipantsPlayersAccess._players[chooserSeat].Name)} 一张手牌。" : drawn > 0 ? $"{(owner.Name)} 诱敌弃置【{(card.DisplayName)}】：摸一张牌。" : $"{(owner.Name)} 诱敌弃置【{(card.DisplayName)}】。", owner.Seat, chooserSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYoudiBaitEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, chooserSeat, card.Id, took, drawn));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }
}
