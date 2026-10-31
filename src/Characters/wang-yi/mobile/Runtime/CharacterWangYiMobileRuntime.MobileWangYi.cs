#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangYiMobileRuntime
{
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 73010)]
    internal IEnumerable<CardSnapshot> MobileWangYiPrivateMijiCards(int viewerSeat)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SkillId: "mobile:miji", InstructionIndex: 4 } frame || frame.OwnerSeat != viewerSeat || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != viewerSeat || frame.CardSetBindings.SingleOrDefault(binding => binding.Name == "miji-cards")is not { Visibility: SkillProgramCardSetVisibility.Private } cards)
            return[];
        return cards.CardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(card => card.Id == id))).ToArray();
    }

    internal static bool IsDeckTopJudgmentReplacement(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.ReplaceJudgment, Zones: [CardZoneKind.DrawPile] }];
    internal bool HasDeckTopJudgmentReplacementMaterial() => _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) + _cardZonesOperations._cardZones.Count(CardLocation.DiscardPile) > 0;
    internal void BeginDeckTopJudgmentReplacementChoice(JudgmentFrame pending, JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        var old = _coreNativeDamageHandlesOperations.GetJudgmentCard(pending) ?? throw new InvalidOperationException("The deck-top retrial lost its current judgment.");
        _coreNativeJudgmentOperations.SetJudgmentFrameStep(pending.Id, ResolutionFrameStep.AwaitingResponse);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new JudgmentReplacementRequestedEvent(pending.Id, pending.Id, pending.TargetSeat, owner.Seat, pending.Reason, old.Id, old.Kind, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], old)));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramJudgmentReplacement, owner.Seat, $"【{(_contentRegistryOperations._contentRegistry.GetSkill(program.Id).Name)}】：是否亮出未知的牌堆顶牌代替当前判定牌？", [], [], pending.TargetSeat, old.Kind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = pending.TargetSeat,
            SkillPrompt = new(program.Id, _contentRegistryOperations._contentRegistry.GetSkill(program.Id).Name, "以未知顶牌改判", _contentRegistryOperations._contentRegistry.GetSkill(program.Id).Description),
            Choices = Array.AsReadOnly(new[] { new PromptChoice(new($"program-judgment-top.{(program.Id)}.{(trigger.Id)}.activate"), "亮出牌堆顶的一张牌代替之。", [], [], new Dictionary<string, string> { ["action"] = "program-judgment-top-replace" }), new PromptChoice(new($"program-judgment-top.{(program.Id)}.{(trigger.Id)}.skip"), "不发动，保留当前判定牌。", [], [], new Dictionary<string, string> { ["action"] = "program-judgment-replace-skip" }) })
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal bool IsDeckTopJudgmentReplacementPromptValid(JudgmentFrame judgment, PendingDecision? decision)
    {
        var candidate = _coreNativeJudgmentOperations.CurrentJudgmentCandidate(judgment);
        if (candidate is null || decision is not { Kind: DecisionKind.ProgramJudgmentReplacement, IsPrivate: true, ValidCardIds.Count: 0, ValidTargetSeats.Count: 0, Choices.Count: 2 } || decision.PlayerSeat != candidate.OwnerSeat || decision.SourceSeat != judgment.TargetSeat || decision.TargetSeat != judgment.TargetSeat)
            return false;
        var(program, trigger) = _coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacement(candidate);
        return IsDeckTopJudgmentReplacement(trigger) && decision.SkillPrompt?.SkillId == program.Id && decision.Choices.All(choice => choice.Cards.Count == 0 && choice.Targets.Count == 0) && decision.Choices[0].Id.Value == $"program-judgment-top.{(program.Id)}.{(trigger.Id)}.activate" && decision.Choices[0].Parameters.GetValueOrDefault("action") == "program-judgment-top-replace" && decision.Choices[1].Id.Value == $"program-judgment-top.{(program.Id)}.{(trigger.Id)}.skip" && decision.Choices[1].Parameters.GetValueOrDefault("action") == "program-judgment-replace-skip";
    }

    internal bool TryResolveDeckTopJudgmentReplacement(PromptChoice selected, JudgmentFrame pending, JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger)
    {
        if (!IsDeckTopJudgmentReplacement(trigger) || selected.Parameters.GetValueOrDefault("action") != "program-judgment-top-replace")
            return false;
        if (!IsDeckTopJudgmentReplacementPromptValid(pending, _corePendingDecisionPolicyPipelineOperations._pendingDecision) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], program.Id, candidate.SkillInstanceId!) || !_coreJudgmentReplacementProgramsOperations.MatchesProgramJudgmentReplacement(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], trigger, pending.TargetSeat, pending.Reason))
            throw new InvalidOperationException("The accepted deck-top retrial lost its original owning judgment candidate.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_corePhysicalDrawFacadesOperations.EnsureDrawPile())
        {
            _coreNativeJudgmentOperations.AdvanceJudgmentCandidate(pending);
            return true;
        }

        var old = _coreNativeDamageHandlesOperations.GetJudgmentCard(pending) ?? throw new InvalidOperationException("The accepted retrial lost its old judgment card.");
        var replacement = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
        var shared = new ProgramTriggerCandidate(candidate.OwnerSeat, program.Id, trigger.Id, candidate.SkillInstanceId!, program.GameplayHash, trigger.Priority);
        _coreProgramLifecycleOperations.BeginProgramBinding(shared, new(SkillProgramTriggerWindow.JudgmentReplacing, pending.Id, candidate.OwnerSeat, SourceSeat: pending.SourceSeat, TargetSeat: pending.TargetSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), JudgmentReplacement: new(pending.Id, pending.TargetSeat, pending.Reason, old.Id, replacement.Id)));
        return true;
    }

    internal bool IsDeckTopJudgmentReplacementInput(JudgmentFrame judgment, CharacterState owner, Card card)
    {
        var candidate = _coreNativeJudgmentOperations.CurrentJudgmentCandidate(judgment);
        return candidate is not null && candidate.OwnerSeat == owner.Seat && judgment.TargetSeat == owner.Seat && IsDeckTopJudgmentReplacement(_coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacement(candidate).Trigger) && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.DrawPile && _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault()?.Id == card.Id;
    }

    internal bool TryResolveAiDeckTopJudgmentReplacement()
    {
        if (_coreJudgmentCursorRuntimeOperations.ActiveJudgment is not { } judgment || _coreNativeJudgmentOperations.CurrentJudgmentCandidate(judgment)is not { } candidate || !IsDeckTopJudgmentReplacement(_coreJudgmentReplacementProgramsOperations.GetProgramJudgmentReplacement(candidate).Trigger))
            return false;
        var current = _coreNativeDamageHandlesOperations.GetJudgmentCard(judgment)!;
        // The choice only uses the current public judgment, never the hidden
        // replacement material. Miji wants black; an already-black result stays.
        var activate = judgment.Reason == "skill.mobile-wang-yi.miji" && _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[judgment.TargetSeat], current)is Suit.Heart or Suit.Diamond;
        _coreJudgmentReplacementProgramsOperations.ResolveProgramJudgmentReplacementChoice(_corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices[activate ? 0 : 1]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }
}
