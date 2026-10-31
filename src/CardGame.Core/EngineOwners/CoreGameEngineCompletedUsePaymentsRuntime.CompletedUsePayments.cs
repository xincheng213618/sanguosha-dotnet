#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCompletedUsePaymentsRuntime
{
    internal SkillProgramStepOutcome BeginCompletedUsePayment(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.CompletedUsePayment is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } context } || context.ActorSeat != f.OwnerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(context.ParentCardUseFrameId)is not { Step: ResolutionFrameStep.Completed, CausedDamage: true } use || use.Action?.ActionId != context.CardActionId)
            throw new InvalidOperationException("Completion payment lost its accumulated, completed owning use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CompletedUsePayment = new(f.InstructionIndex, use.Id, CompletedUsePaymentStage.Choosing) });
        PublishCompletedUsePayment(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishCompletedUsePayment(ProgramSkillFrame f)
    {
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Parameters(string branch) => new()
        {
            ["program-action"] = "completed-use-payment",
            ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        foreach (var zone in new[]
        {
            CardZoneKind.Hand,
            CardZoneKind.Equipment
        }

        )
            foreach (var card in _cardZonesOperations._cardZones.CardsAt(new(zone, f.OwnerSeat)))
            {
                if (zone == CardZoneKind.Equipment && (card.IsGeneralWeapon || _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, card)))
                    continue;
                choices.Add(new(new($"completed-use-payment.{(f.Id)}.{(card.Id)}"), $"弃置【{(card.DisplayName)}】", [card.Id], [], Parameters("discard")));
            }

        choices.Add(new(new($"completed-use-payment.{(f.Id)}.hp"), "失去1点体力", [], [], Parameters("lose-hp")));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "此牌已造成伤害：弃置一张手牌或装备牌，或失去1点体力。", Array.AsReadOnly(choices.SelectMany(c => c.Cards).ToArray()), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("completed-use-payment")]
    internal void ResolveCompletedUsePayment(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.CompletedUsePayment is not { Stage: CompletedUsePaymentStage.Choosing } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || choice.Targets.Count != 0)
            throw new InvalidOperationException("Completion payment lost its exact unpublished owner.");
        AssertCompletedUsePayment(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "结算付款者或未付费技能来源失效。");
            return;
        }

        var branch = choice.Parameters.GetValueOrDefault("branch");
        if (branch == "discard")
        {
            if (choice.Cards.Count != 1)
                throw new InvalidOperationException("Completion discard requires one held entity.");
            var from = _cardZonesOperations._cardZones.GetLocation(choice.Cards[0]);
            if (from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                throw new InvalidOperationException("Completion discard left its owner's HE zones.");
            var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == choice.Cards[0]);
            if (from.Zone == CardZoneKind.Equipment && (card.IsGeneralWeapon || _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(f.OwnerSeat, f.SkillId, f.SkillInstanceId, card)))
                throw new InvalidOperationException("Completion payment cannot discard its active source equipment.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { CompletedUsePayment = r with { Stage = CompletedUsePaymentStage.Paid, CardId = card.Id, From = from } });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.PayCompletedUseDiscardOrLoseHp)}"), move =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { CompletedUsePayment = active.CompletedUsePayment!with { MovementSequence = move.Sequence } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCompletedUsePaymentEvent(f.Id, r.CardUseFrameId, f.OwnerSeat, card.Id, 0));
            });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (branch != "lose-hp" || choice.Cards.Count != 0)
            throw new InvalidOperationException("Invalid completion payment choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CompletedUsePayment = r with { Stage = CompletedUsePaymentStage.Paid, LostHp = true, HpBefore = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCompletedUsePaymentEvent(f.Id, r.CardUseFrameId, f.OwnerSeat, null, 1));
        var host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        host.LoseHp(f.Id, f.SkillId, f.OwnerSeat, 1);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 530)]
    internal bool ResumeCompletedUsePayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.CompletedUsePayment is not { } r)
            return false;
        AssertCompletedUsePayment(f);
        if (r.Stage == CompletedUsePaymentStage.Choosing)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CompletedUsePayment = null });
        return false;
    }

    [ProgramAiChoice(SkillProgramEffectOp.PayCompletedUseDiscardOrLoseHp)]
    internal PromptChoice SelectAiCompletedUsePayment(PendingDecision decision) => decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard" && _cardZonesOperations._cardZones.GetLocation(c.Cards[0]).Zone == CardZoneKind.Hand) ?? decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ?? decision.Choices.Single();
    internal void AssertCompletedUsePayment(ProgramSkillFrame f)
    {
        if (f.CompletedUsePayment is not { } r)
            return;
        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.PayCompletedUseDiscardOrLoseHp || r.InstructionIndex != f.InstructionIndex || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } context } || context.ParentCardUseFrameId != r.CardUseFrameId || _coreCardUseLifecycleOperations.LifecycleCardUse(r.CardUseFrameId)is not { CausedDamage: true, Step: ResolutionFrameStep.Completed } use || use.Action?.ActionId != context.CardActionId || r.Stage == CompletedUsePaymentStage.Paid && (r.LostHp ? r.CardId is not null || r.From is not null || r.MovementSequence != 0 : r.CardId is not { } cardId || r.From is not { } from || from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == r.MovementSequence && m.CardId == cardId && m.From == from && m.To == CardLocation.DiscardPile && m.Reason.Value == $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.PayCompletedUseDiscardOrLoseHp)}")) || r.Stage == CompletedUsePaymentStage.Paid && r.LostHp && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 1 && e.RemainingHp == r.HpBefore - 1) || r.Stage == CompletedUsePaymentStage.Paid && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramCompletedUsePaymentEvent>().Any(e => e.ProgramFrameId == f.Id && e.CardUseFrameId == r.CardUseFrameId && e.OwnerSeat == f.OwnerSeat && e.DiscardedCardId == r.CardId && e.HpLost == (r.LostHp ? 1 : 0)))
            throw new InvalidOperationException("Completion payment receipt lost its exact use, cost or public payment fact.");
    }
}
