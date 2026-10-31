#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRevealRecastProgramsRuntime
{
    internal void RecordCompletedCardDamageParticipant(long cardUseFrameId, int targetSeat)
    {
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(item => item.Id == cardUseFrameId);
        if (use?.Action?.ConversionChain.FirstOrDefault()is not { } conversion || _contentRegistryOperations._contentRegistry.GetSkill(conversion.SkillId).Program?.Triggers.Any(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.DrawCompletedCardParticipants)) != true)
            return;
        if (!use.CompletedDamageParticipants.Contains(targetSeat))
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { CompletedDamageParticipants = Array.AsReadOnly(use.CompletedDamageParticipants.Append(targetSeat).ToArray()) });
    }

    internal void DrawProgramCompletedCardParticipants(ProgramSkillFrame frame, string stateId, int threshold)
    {
        var use = frame.WindowContext?.CardUse ?? throw new InvalidOperationException("Completed-card participant draws require a card-use context.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var participants = (_coreCardUseLifecycleOperations.LifecycleCardUse(use.ParentCardUseFrameId) ?? throw new InvalidOperationException("The participant draw lost its card-use owner.")).CompletedDamageParticipants.Order().ToArray();
        var ownerDrawn = 0;
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.completed-participant-draw");
        if (owner.IsAlive)
            ownerDrawn += _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, reason).Count;
        foreach (var seat in participants)
            if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            {
                var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], 1, true, reason).Count;
                if (seat == owner.Seat)
                    ownerDrawn += drawn;
            }

        var key = (frame.OwnerSeat, frame.SkillId, "participant-draw-count:" + stateId);
        var count = checked(_coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key) + ownerDrawn);
        _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = count;
        if (count >= threshold)
            _coreProgramSkillChangesOperations.SetProgramBooleanState(frame, stateId, true);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.ParentCardUseFrameId, parent => parent with { CompletedDamageParticipants = [] });
    }

    internal SkillProgramStepOutcome RecastProgramSelectedCards(ProgramSkillFrame frame, string stateId, int threshold)
    {
        if (frame.TriggerId is not null || frame.SelectedCardIds is not [var id] || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat))
            throw new InvalidOperationException("Recasting requires one selected physical owner hand card.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Single(item => item.Id == id);
        var recast = _coreNativeRecastInvocationsOperations.CaptureNativeRecastInvocation(owner.Seat, owner.Seat, [card.Id], frame.Id, frame.InstructionIndex);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(owner.Seat), CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(owner, 1, true, CardMoveReasons.RecastDraw);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(owner.Seat, card.Id, card.Kind, drawn.Count));
        _coreNativeRecastInvocationsOperations.CompleteNativeRecastInvocation(recast, drawn.Count);
        var key = (frame.OwnerSeat, frame.SkillId, "recast-card-count:" + stateId);
        var count = checked(_coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key) + 1);
        _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = count;
        if (count >= threshold)
            _coreProgramSkillChangesOperations.SetProgramBooleanState(frame, stateId, true);
        return SkillProgramStepOutcome.Continue;
    }

    internal IReadOnlyList<PromptChoice> PairedHandRevealChoices(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[frame.SelectedTargetSeats.Single()];
        return _corePhysicalOwnedCardQueriesOperations.GetHand(target).Select(card => new PromptChoice(new ChoiceId("paired-hand-reveal." + card.Id), "确定同时展示的手牌：" + card.DisplayName, [card.Id], [target.Seat], new Dictionary<string, string> { ["program-action"] = "paired-hand-reveal", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = effect.ResultBind! })).ToArray();
    }

    internal SkillProgramStepOutcome RevealProgramSelectedHandAgainstTarget(ProgramSkillFrame frame, string resultBind)
    {
        if (frame.TriggerId is not null || frame.SelectedCardIds is not [var ownerCard] || frame.SelectedTargetSeats is not [var targetSeat] || targetSeat == frame.OwnerSeat || _cardZonesOperations._cardZones.GetLocation(ownerCard) != CardLocation.Hand(frame.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[targetSeat]).Count == 0)
            throw new InvalidOperationException("Paired hand reveal requires one physical owner card and another living nonempty hand.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, targetSeat, "双方同时展示：秘密确定一张手牌。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = targetSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = PairedHandRevealChoices(frame, effect)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[targetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("paired-hand-reveal")]
    internal void ResolvePairedHandRevealChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Paired reveal lost its program frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var canonical = PairedHandRevealChoices(frame, effect).SingleOrDefault(choice => choice.Id == selected.Id);
        if (effect.Op != SkillProgramEffectOp.RevealSelectedHandAgainstTarget || canonical is null || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([canonical], [selected]) || frame.ChoiceBindings.Any(binding => binding.Name == effect.ResultBind))
            throw new InvalidOperationException("Paired reveal choice changed its physical card, target or result binding.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[frame.SelectedTargetSeats.Single()];
        var ownerCard = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Single(card => card.Id == frame.SelectedCardIds.Single());
        var targetCard = _corePhysicalOwnedCardQueriesOperations.GetHand(target).Single(card => card.Id == canonical.Cards.Single());
        var ownerKind = _coreActionQueriesOperations.GetProgramCardIdentityMatches(owner, ownerCard).FirstOrDefault()?.Identity.OutputKind ?? ownerCard.Kind;
        var targetKind = _coreActionQueriesOperations.GetProgramCardIdentityMatches(target, targetCard).FirstOrDefault()?.Identity.OutputKind ?? targetCard.Kind;
        var outcome = global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(ownerKind) && targetKind != CardKind.Dodge ? "damage" : !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(ownerKind) && targetKind == CardKind.Dodge ? "obtain" : "none";
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(frame.Id, frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), owner.Seat, effect.ResultBind!, [_coreSnapshotValueProjectionOperations.ToSnapshot(ownerCard), _coreSnapshotValueProjectionOperations.ToSnapshot(targetCard)]));
        _coreProgramSlashRequestOperations.CommitProgramChoiceResult(frame.Id, effect.ResultBind!, outcome, target.Seat, "双方同时公开实体手牌。");
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(frame.Id, PublicHandRevealReturnKind.Program))
            return;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    SkillProgramStepOutcome global::CardGame.Core.IRevealRecastProgramHost.RecastSelectedCards(ProgramSkillFrame frame, string stateId, int threshold) => RecastProgramSelectedCards(frame, stateId, threshold);
    void global::CardGame.Core.IRevealRecastProgramHost.DrawCompletedCardParticipants(ProgramSkillFrame frame, string stateId, int threshold) => DrawProgramCompletedCardParticipants(frame, stateId, threshold);
    SkillProgramStepOutcome global::CardGame.Core.IRevealRecastProgramHost.RevealSelectedHandAgainstTarget(ProgramSkillFrame frame, string resultBind) => RevealProgramSelectedHandAgainstTarget(frame, resultBind);
}
