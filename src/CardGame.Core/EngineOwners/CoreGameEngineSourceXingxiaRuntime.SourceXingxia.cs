#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceXingxiaRuntime
{
    internal SkillProgramStepOutcome StartSourceXingxia(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SelectedTargetSeats is not [var friend] || !_coreMatchParticipantsPlayersAccess._players[friend].IsAlive || !_coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[friend]))
            throw new InvalidOperationException("Source1117 requires its selected living other public-team ally.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceXingxia = new(_coreActualRoundProgressRoundNumberAccess._roundNumber, friend, [], 0, SourceXingxiaStage.FriendlyDamage) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceXingxiaIssuedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, _coreActualRoundProgressRoundNumberAccess._roundNumber, friend));
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, friend, 2, nature: DamageNature.Fire);
        _coreSourceBeigongOperations.ContinueSourceBeigong(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<(Card Card, CardLocation From)> SourceXingxiaRedCards(int payer) => new[]
    {
        CardZoneKind.Hand,
        CardZoneKind.Equipment
    }.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, payer)).Where(c => _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(_coreMatchParticipantsPlayersAccess._players[payer], c)is Suit.Heart or Suit.Diamond && !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(payer, c, new(zone, payer), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(payer, c, new(zone, payer), OwnedCardMoveIntent.Discard)).Select(c => (c, new CardLocation(zone, payer)))).ToArray();
    internal void ContinueSourceXingxia(ProgramSkillFrame f)
    {
        var r = f.SourceXingxia!;
        if (r.Stage == SourceXingxiaStage.FriendlyDamage)
        {
            var enemies = Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreTeamActivityProgramsOperations.PublicTeamEnemy(f.OwnerSeat, p)).Select(p => p.Seat).Order().ToArray());
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceXingxia = r = r with { Enemies = enemies, Index = 0, Stage = SourceXingxiaStage.EnemyChoice } });
        }
        else if (r.Stage is SourceXingxiaStage.PaymentChildren or SourceXingxiaStage.EnemyDamage)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceXingxia = r = r with { Index = r.Index + 1, Stage = SourceXingxiaStage.EnemyChoice } });
        while (r.Index < r.Enemies.Count && !_coreMatchParticipantsPlayersAccess._players[r.Enemies[r.Index]].IsAlive)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceXingxia = r = r with { Index = r.Index + 1 } });
        if (r.Index >= r.Enemies.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreSourceBeigongOperations.FinishSourceBeigong(f);
            return;
        }

        var payer = r.Enemies[r.Index];
        var choices = SourceXingxiaRedCards(payer).Select(p => new PromptChoice(new ChoiceId($"source-xingxia.{(f.Id)}.{(r.Index)}.{(p.Card.Id)}"), $"弃置【{(p.Card.DisplayName)}】", [p.Card.Id], [], new Dictionary<string, string> { ["program-action"] = "source-xingxia-red", ["branch"] = "discard" })).ToList();
        choices.Add(new(new ChoiceId($"source-xingxia.{(f.Id)}.{(r.Index)}.fire"), "受到1点火焰伤害", [], [], new Dictionary<string, string> { ["program-action"] = "source-xingxia-red", ["branch"] = "fire" }));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, payer, "行夏：弃置一张红色手牌或装备，或受到1点火焰伤害。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[payer].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("source-xingxia-red")]
    internal void AnswerSourceXingxiaRed(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SourceXingxia: { Stage: SourceXingxiaStage.EnemyChoice } r } f || r.Index < 0 || r.Index >= r.Enemies.Count || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.Enemies[r.Index])
            throw new InvalidOperationException("Source1117 lost its current original enemy choice.");
        var payer = r.Enemies[r.Index];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (selected.Parameters.GetValueOrDefault("branch") == "fire")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceXingxia = r with { Stage = SourceXingxiaStage.EnemyDamage } });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, payer, 1, nature: DamageNature.Fire);
            _coreSourceBeigongOperations.ContinueSourceBeigong(f.Id);
            return;
        }

        var id = selected.Cards.Single();
        if (!SourceXingxiaRedCards(payer).Any(p => p.Card.Id == id))
            throw new InvalidOperationException("Source1117 cannot discard a missing or non-red original entity.");
        var reason = new CardMoveReason("skill-program.source-xingxia.discard");
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceXingxia = r with { Stage = SourceXingxiaStage.PaymentChildren, PaymentCardId = id, Before = before, After = before }, PendingMovementContinuation = new(payer, 0, null) });
        var logical = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(payer, payer, [id], reason, f.Id);
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], CardLocation.DiscardPile, reason, (_, moves) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { SourceXingxia = current.SourceXingxia!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SourceXingxiaPaymentEvent(f.Id, f.OwnerSeat, payer, id, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
        });
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logical);
        _coreSourceBeigongOperations.ContinueSourceBeigong(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveSourceXingxia)]
    internal PromptChoice SourceXingxiaAiChoice(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ?? p.Choices.Single(c => c.Parameters.GetValueOrDefault("branch") == "fire");
}
