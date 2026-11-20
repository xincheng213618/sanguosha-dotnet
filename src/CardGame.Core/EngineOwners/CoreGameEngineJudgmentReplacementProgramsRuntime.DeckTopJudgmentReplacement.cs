#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJudgmentReplacementProgramsRuntime
{
    private void BeginDeckTopJudgmentReplacementChoice(JudgmentFrame pending, JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        var old = _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending) ?? throw new InvalidOperationException("The deck-top retrial lost its current judgment.");
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

    private bool IsDeckTopJudgmentReplacementPromptValid(JudgmentFrame judgment, PendingDecision? decision)
    {
        var candidate = _coreNativeJudgmentOperations.CurrentJudgmentCandidate(judgment);
        if (candidate is null || decision is not { Kind: DecisionKind.ProgramJudgmentReplacement, IsPrivate: true, ValidCardIds.Count: 0, ValidTargetSeats.Count: 0, Choices.Count: 2 } || decision.PlayerSeat != candidate.OwnerSeat || decision.SourceSeat != judgment.TargetSeat || decision.TargetSeat != judgment.TargetSeat)
            return false;
        var(program, trigger) = GetProgramJudgmentReplacement(candidate);
        return IsDeckTopJudgmentReplacement(trigger) && decision.SkillPrompt?.SkillId == program.Id && decision.Choices.All(choice => choice.Cards.Count == 0 && choice.Targets.Count == 0) && decision.Choices[0].Id.Value == $"program-judgment-top.{(program.Id)}.{(trigger.Id)}.activate" && decision.Choices[0].Parameters.GetValueOrDefault("action") == "program-judgment-top-replace" && decision.Choices[1].Id.Value == $"program-judgment-top.{(program.Id)}.{(trigger.Id)}.skip" && decision.Choices[1].Parameters.GetValueOrDefault("action") == "program-judgment-replace-skip";
    }

    private bool TryResolveDeckTopJudgmentReplacement(PromptChoice selected, JudgmentFrame pending, JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger)
    {
        if (!IsDeckTopJudgmentReplacement(trigger) || selected.Parameters.GetValueOrDefault("action") != "program-judgment-top-replace")
            return false;
        if (!IsDeckTopJudgmentReplacementPromptValid(pending, _corePendingDecisionPolicyPipelineOperations._pendingDecision) || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], program.Id, candidate.SkillInstanceId!) || !MatchesProgramJudgmentReplacement(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], trigger, pending.TargetSeat, pending.Reason))
            throw new InvalidOperationException("The accepted deck-top retrial lost its original owning judgment candidate.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_corePhysicalDrawFacadesOperations.EnsureDrawPile())
        {
            _coreNativeJudgmentOperations.AdvanceJudgmentCandidate(pending);
            return true;
        }

        var old = _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(pending) ?? throw new InvalidOperationException("The accepted retrial lost its old judgment card.");
        var replacement = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile)[^1];
        var shared = new ProgramTriggerCandidate(candidate.OwnerSeat, program.Id, trigger.Id, candidate.SkillInstanceId!, program.GameplayHash, trigger.Priority);
        _coreProgramLifecycleOperations.BeginProgramBinding(shared, new(SkillProgramTriggerWindow.JudgmentReplacing, pending.Id, candidate.OwnerSeat, SourceSeat: pending.SourceSeat, TargetSeat: pending.TargetSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), JudgmentReplacement: new(pending.Id, pending.TargetSeat, pending.Reason, old.Id, replacement.Id)));
        return true;
    }

    private bool IsDeckTopJudgmentReplacementInput(JudgmentFrame judgment, CharacterState owner, Card card)
    {
        var candidate = _coreNativeJudgmentOperations.CurrentJudgmentCandidate(judgment);
        return candidate is not null && candidate.OwnerSeat == owner.Seat && judgment.TargetSeat == owner.Seat && IsDeckTopJudgmentReplacement(GetProgramJudgmentReplacement(candidate).Trigger) && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.DrawPile && _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault()?.Id == card.Id;
    }
}
