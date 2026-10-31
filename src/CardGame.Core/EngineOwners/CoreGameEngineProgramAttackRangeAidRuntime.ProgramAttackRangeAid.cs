#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramAttackRangeAidRuntime
{
    internal SkillProgramStepOutcome RequestProgramAttackRangeAid(ProgramSkillFrame frame, CardMoveReason reason)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.AttackRangeAid is not null || active.SelectedTargetSeats.Count != 1)
            throw new InvalidOperationException("Attack-range aid requires one selected target and no active response chain.");
        var targetSeat = active.SelectedTargetSeats.Single();
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            throw new InvalidOperationException("The attack-range aid target is no longer alive.");
        var responders = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != targetSeat && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(player.Seat, targetSeat)).OrderBy(player => _coreTableOrderOperations.RelativeTableOrder(active.OwnerSeat, player.Seat)).Select(player => player.Seat).ToArray();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAttackRangeAidStartedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, targetSeat, Array.AsReadOnly(responders)));
        _coreMatchLogOperations.AddLog("ActiveSkill", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，" + $"共有 {(responders.Length)} 名角色依次响应。", active.OwnerSeat, targetSeat);
        if (responders.Length == 0)
            return SkillProgramStepOutcome.Continue;
        active = active with
        {
            AttackRangeAid = new ProgramAttackRangeAid(targetSeat, Array.AsReadOnly(responders), ResponderIndex: 0)
            {
                InstructionIndex = active.InstructionIndex
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishProgramAttackRangeAid(active, reason);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishProgramAttackRangeAid(ProgramSkillFrame frame, CardMoveReason reason)
    {
        var aid = frame.AttackRangeAid ?? throw new InvalidOperationException("Missing attack-range aid state.");
        var responderIndex = aid.ResponderIndex;
        while (responderIndex < aid.ResponderSeats.Count && !_coreMatchParticipantsPlayersAccess._players[aid.ResponderSeats[responderIndex]].IsAlive)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAttackRangeAidResponderSkippedEvent(frame.Id, responderIndex, aid.ResponderSeats[responderIndex]));
            responderIndex++;
        }

        if (responderIndex >= aid.ResponderSeats.Count || !_coreMatchParticipantsPlayersAccess._players[aid.TargetSeat].IsAlive)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { AttackRangeAid = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (responderIndex != aid.ResponderIndex)
        {
            aid = aid with
            {
                ResponderIndex = responderIndex
            };
            frame = frame with
            {
                AttackRangeAid = aid
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        }

        var responder = _coreMatchParticipantsPlayersAccess._players[aid.ResponderSeats[aid.ResponderIndex]];
        var target = _coreMatchParticipantsPlayersAccess._players[aid.TargetSeat];
        var choices = _characterHanDangBoundaryMobileOperations.ProgramAttackRangeAidWeapons(responder.Seat).OrderBy(card => card.Id).Select(card => new PromptChoice(new ChoiceId($"program-attack-range-aid.frame-{(frame.Id)}.index-{(aid.ResponderIndex)}.weapon-{(card.Id)}"), $"弃置武器【{(card.DisplayName)}】。", [card.Id], [], AttackRangeAidParameters(frame, aid, reason, "attack-range-aid-discard-weapon"))).Append(new PromptChoice(new ChoiceId($"program-attack-range-aid.frame-{(frame.Id)}.index-{(aid.ResponderIndex)}.draw-{(target.Seat)}"), $"令 {(target.Name)} 摸一张牌。", [], [target.Seat], AttackRangeAidParameters(frame, aid, reason, "attack-range-aid-draw"))).ToArray();
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, responder.Seat, $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 发动【{(skill.Name)}】：弃置一张武器牌，或令 {(target.Name)} 摸一张牌。", choices.SelectMany(choice => choice.Cards).ToArray(), [target.Seat], SourceSeat: frame.OwnerSeat)
        {
            PromptId = responder.IsHuman ? _coreNativePromptBoundaryOperations.CreatePromptId() : default,
            IsPrivate = true,
            TargetSeat = target.Seat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 响应方式", "你在技能发动时能攻击到受益角色，因此必须选择：弃置一张武器，或令受益角色摸一张牌。"),
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = responder.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal static Dictionary<string, string> AttackRangeAidParameters(ProgramSkillFrame frame, ProgramAttackRangeAid aid, CardMoveReason reason, string action) => RuntimeSkills.RuntimeRules_Core_ProgramAttackRangeAid.AttackRangeAidParameters(frame, aid, reason, action);
    internal void ResolveProgramAttackRangeAid(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected)
    {
        var aid = frame.AttackRangeAid ?? throw new InvalidOperationException("Missing attack-range aid state.");
        var expectedReason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        if (effect.Op != SkillProgramEffectOp.RequestAttackRangeAid || aid.ResponderIndex < 0 || aid.ResponderIndex >= aid.ResponderSeats.Count || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("responder-index") != aid.ResponderIndex.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("target-seat") != aid.TargetSeat.ToString(System.Globalization.CultureInfo.InvariantCulture) || selected.Parameters.GetValueOrDefault("move-reason") != expectedReason.Value)
            throw new InvalidOperationException("The attack-range aid choice does not match its suspended instruction.");
        var responderSeat = aid.ResponderSeats[aid.ResponderIndex];
        var responder = _coreMatchParticipantsPlayersAccess._players[responderSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[aid.TargetSeat];
        if (aid.Payment is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != responderSeat || !_corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices.Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Attack-range aid must pay the current original responder exactly once.");
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (selected.Parameters.GetValueOrDefault("program-action"))
        {
            case "attack-range-aid-discard-weapon" when selected.Cards.Count == 1 && selected.Targets.Count == 0:
            {
                var weapon = _characterHanDangBoundaryMobileOperations.ProgramAttackRangeAidWeapons(responderSeat).SingleOrDefault(card => card.Id == selected.Cards.Single()) ?? throw new InvalidOperationException("The selected aid-response weapon is no longer in its owner's actual hand or equipment.");
                var from = _cardZonesOperations._cardZones.GetLocation(weapon.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect, AttackRangeAid = aid with { Payment = new(aid.ResponderIndex, responderSeat, ProgramAttackRangeAidPaymentStage.MovementChildren, before, before, weapon.Id, weapon.Kind, from) }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
                var discardInvocation = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(frame.OwnerSeat, responderSeat, [weapon.Id], expectedReason, frame.Id);
                _corePhysicalMovementFacadesOperations.MoveCard(weapon, from, CardLocation.Processing, expectedReason);
                _corePhysicalMovementFacadesOperations.MoveCard(weapon, CardLocation.Processing, CardLocation.DiscardPile, expectedReason);
                _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(discardInvocation);
                frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { AttackRangeAid = frame.AttackRangeAid!with { Payment = frame.AttackRangeAid!.Payment!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAttackRangeAidChoiceResolvedEvent(frame.Id, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, responderSeat, aid.TargetSeat, weapon.Id, Array.AsReadOnly(Array.Empty<int>())));
                _characterHanDangBoundaryMobileOperations.ContinueProgramAttackRangeAidPayment(frame.Id);
                return;
            }

            case "attack-range-aid-draw" when selected.Cards.Count == 0 && selected.Targets.SequenceEqual([aid.TargetSeat]):
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect, AttackRangeAid = aid with { Payment = new(aid.ResponderIndex, responderSeat, ProgramAttackRangeAidPaymentStage.Drawing, before, before) } });
                _characterHanDangBoundaryMobileOperations.ContinueProgramAttackRangeAidPayment(frame.Id);
                return;
            default:
                throw new InvalidOperationException("The attack-range aid choice is malformed.");
        }
    }

    [ProgramAiChoice(SkillProgramEffectOp.RequestAttackRangeAid)]
    internal PromptChoice SelectAiProgramAttackRangeAid(PendingDecision decision, ProgramSkillFrame frame)
    {
        var aid = frame.AttackRangeAid ?? throw new InvalidOperationException("Missing AI attack-range aid state.");
        var responderSeat = aid.ResponderSeats[aid.ResponderIndex];
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var(choice, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[responderSeat].ChooseProgramAttackRangeAidResponse(_corePlayerViewProjectionOperations.CreateSnapshot(responderSeat), frame.SkillId, skill.Name, aid.TargetSeat, decision.Choices, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        return choice;
    }
}
