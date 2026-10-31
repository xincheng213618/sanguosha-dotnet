#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGhostActivityProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IGhostActivityProgramHost.ExecuteGhostActivity(ProgramSkillFrame frame, SkillProgramEffect effect) => ExecuteGhostActivity(frame, effect);
    internal SkillProgramStepOutcome ExecuteGhostActivity(ProgramSkillFrame input, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        switch (effect.Op)
        {
            case SkillProgramEffectOp.DiscardRandomJudgment:
            {
                var cards = _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
                if (cards.Count == 0)
                    return SkillProgramStepOutcome.Continue;
                var card = cards[_randomOperations._random.Next(cards.Count)];
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, frame.OwnerSeat, card, CardLocation.Judgment(frame.OwnerSeat), reason, parentFrameId: frame.Id);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RandomJudgmentDiscardedEvent(frame.Id, frame.OwnerSeat, card.Id));
                return SkillProgramStepOutcome.Continue;
            }

            case SkillProgramEffectOp.RewardOtherRecoverySource:
            {
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { Kind: HpChangeKind.Recovery } change } || change.TargetSeat != frame.OwnerSeat)
                    throw new InvalidOperationException("A recovery-source reward lost its actual recovery.");
                if (change.SourceSeat is { } source && source != frame.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive)
                    _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, source, 1, null, null, SkillProgramCardSetVisibility.Private, reason);
                return SkillProgramStepOutcome.Continue;
            }

            case SkillProgramEffectOp.DiscardDeathSourceCards:
            {
                if (frame.WindowContext?.Window != SkillProgramTriggerWindow.OwnerDied)
                    throw new InvalidOperationException("A death-source discard lost its actual death window.");
                if (frame.WindowContext.SourceSeat is not { } source || source == frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(source) || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive)
                    return SkillProgramStepOutcome.Continue;
                var ids = effect.Zones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, source)).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(source, card, new(zone, source), OwnedCardMoveIntent.Discard))).Select(card => card.Id).ToArray();
                if (ids.Length != 0)
                    _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, source, ids, reason);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeathSourceCardsDiscardedEvent(frame.Id, frame.OwnerSeat, source, ids.Length));
                return SkillProgramStepOutcome.Continue;
            }

            case SkillProgramEffectOp.SelectRedJudgmentFireTarget:
            {
                var red = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
                if (red.CardIds.Count == 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = [] });
                    return SkillProgramStepOutcome.Continue;
                }

                if (red.CardIds.Count != 1)
                    throw new InvalidOperationException("A red-judgment target requires one filtered judgment card.");
                var targets = _coreProgramLifecycleOperations.GetProgramTargetSeats(frame.OwnerSeat, SkillProgramTargetKind.OtherLiving, frame.WindowContext);
                if (targets.Count == 0)
                {
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = [] });
                    return SkillProgramStepOutcome.Continue;
                }

                var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
                _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "选择受到1点火焰伤害的角色。", [], targets, frame.OwnerSeat)
                {
                    PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                    SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description),
                    Choices = Array.AsReadOnly(targets.Select(target => new PromptChoice(new ChoiceId($"red-judgment-fire.{(frame.Id)}.{(target)}"), $"对 {(_coreMatchParticipantsPlayersAccess._players[target].Name)} 造成1点火焰伤害。", [], [target], new Dictionary<string, string> { ["program-action"] = "red-judgment-fire-target", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture) })).ToArray())
                };
                _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
                return SkillProgramStepOutcome.AwaitChoice;
            }

            default:
                throw new InvalidOperationException("Unsupported ghost activity operation.");
        }
    }

    [ProgramAnswer("red-judgment-fire-target")]
    internal void ResolveRedJudgmentFireTarget(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The red judgment lost its program.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.SelectRedJudgmentFireTarget || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture) || choice.Cards.Count != 0 || choice.Targets is not [var target] || _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!).CardIds.Count != 1 || !_coreProgramLifecycleOperations.GetProgramTargetSeats(frame.OwnerSeat, SkillProgramTargetKind.OtherLiving, frame.WindowContext).Contains(target))
            throw new InvalidOperationException("The selected red-judgment target lost its exact instruction or legal target.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = Array.AsReadOnly(new[] { target }) });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool IsValidRedJudgmentFireSelection(ProgramSkillFrame frame, SkillProgramEffect effect, bool awaiting)
    {
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.CardsMoved || effect.SourceBind is null)
            return false;
        var red = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind);
        if (red.CardIds.Count == 0)
            return frame.SelectedTargetSeats.Count == 0;
        if (red.CardIds.Count != 1)
            return false;
        return frame.SelectedTargetSeats is [var target] ? _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && target != frame.OwnerSeat : frame.SelectedTargetSeats.Count == 0 && (awaiting || !_coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != frame.OwnerSeat));
    }
}
