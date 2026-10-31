#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramDodgeResponsesRuntime
{
    internal IEnumerable<PromptChoice> ProgramDodgeResponseChoices(CharacterState owner)
    {
        if (_coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(owner.Seat) && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(owner, CardKind.Dodge))
            yield break;
        foreach (var choice in _coreChainedStateBasicConversionsOperations.ChainedStateBasicDodgeChoices(owner))
            yield return choice;
        foreach (var choice in _coreRoundOnceCardlessNativeResponsesOperations.RoundOnceCardlessResponseChoices(owner, CardKind.Dodge))
            yield return choice;
        foreach (var choice in _coreTieredRoundZeroResponsesOperations.TieredRoundZeroResponseChoices(owner, CardKind.Dodge))
            yield return choice;
        foreach (var choice in _characterXiaoQiaoMouOlOperations.MouXqJueyanResponseChoices(owner, CardKind.Dodge))
            yield return choice;
        foreach (var program in _coreSkillProgramsOperations.EnabledActivationPrograms(owner))
            foreach (var activation in program.Activations.Where(item => item.Effects is [{ Op: SkillProgramEffectOp.DrawTurnOwnerThenDiscardMaximumHandForDodge }]))
                yield return new PromptChoice(new ChoiceId($"respond.program-dodge.{(program.Id)}.{(activation.Id)}"), $"发动【{(_contentRegistryOperations._contentRegistry.GetSkill(program.Id).Name)}】尝试提供闪。", [], [], new Dictionary<string, string> { ["response"] = "program-dodge", ["skill"] = program.Id, ["activation"] = activation.Id });
    }

    internal void BeginProgramDodgeResponse(PromptChoice choice, bool advanceToHumanBoundary)
    {
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.RespondDodge } pending ? pending : throw new InvalidOperationException("A configured Dodge requires its current response prompt.");
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("Configured Dodge lost its attack.");
        var owner = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        if ((owner.Seat != attack.TargetSeat && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense?.CurrentCandidateSeat != owner.Seat) || !owner.IsAlive || !ProgramDodgeResponseChoices(owner).Any(item => item.Id == choice.Id && CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([item], [choice])))
            throw new InvalidOperationException("Configured Dodge changed its living responder or published activation.");
        var program = _coreSkillProgramsOperations.GetEnabledSkillProgram(owner, choice.Parameters["skill"]);
        var activation = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Activation, choice.Parameters["activation"]).Activation!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var frame = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Seat, program.Id, activation.Id, program.GameplayHash, 0, [], [])
        {
            SkillInstanceId = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, program.Id),
            ResponseDecision = decision
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(frame.Id, owner.Seat, program.Id, activation.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void CompleteProgramMaximumHandDodge(ProgramSkillFrame frame, bool succeeded)
    {
        var decision = frame.ResponseDecision ?? throw new InvalidOperationException("Configured Dodge lost its saved response.");
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("Configured Dodge lost its parent attack.");
        if (frame != _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id) || decision.PlayerSeat != frame.OwnerSeat || (attack.TargetSeat != frame.OwnerSeat && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense?.CurrentCandidateSeat != frame.OwnerSeat) || frame.HandControlDraft is not null)
            throw new InvalidOperationException("Configured Dodge changed its parent or uncommitted physical cost.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillResolvedEvent(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId, true));
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramSkill);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        if (!succeeded)
        {
            var cards = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(owner, CardKind.Dodge);
            var freshChoices = _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } defense ? _coreNativeFactionDefenseOperations.CreateFactionDefenseResponseChoices(owner, defense, cards, _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(owner) && decision.Choices.Any(item => item.Parameters.GetValueOrDefault("response") == "faction-defense-bagua")) : _coreNativePromptBoundaryOperations.CreateResponseChoices(cards, CardKind.Dodge, decision.IncomingCard, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(owner, card, CardKind.Dodge), includeBagua: decision.Choices.Any(item => item.Parameters.GetValueOrDefault("response") == "bagua"), includeFactionDefense: decision.Choices.Any(item => item.Parameters.GetValueOrDefault("response") == "faction-defense-request"), responder: owner);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = decision with
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                ValidCardIds = cards.Select(card => card.Id).ToArray(),
                Choices = freshChoices.Where(item => item.Parameters.GetValueOrDefault("skill") != frame.SkillId || item.Parameters.GetValueOrDefault("response") != "program-dodge").ToArray()
            };
            _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return;
        }

        var source = new CardConversionSource(frame.SkillId, frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId);
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } factionDefense)
        {
            _coreNativeFactionDefenseOperations.CompleteFactionDefenseResponse(factionDefense, owner, null, false, source);
            return;
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(-1, owner.Seat, attack.SourceSeat, CardKind.Dodge));
        var continuation = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is null ? ProgramCardContinuation.Dodge : ProgramCardContinuation.GroupResponse;
        if (!_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, owner, owner, null, attack.SourceSeat, CardKind.Dodge, [], continuation, source))
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is null)
                _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
            else
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
    }

    internal bool IsWithinAttackRange(int actorSeat, int targetSeat) => _characterTaishiCiMobileGodOperations.HasShenTaishiCiPoweiRange(actorSeat, targetSeat) || _characterSunYuanOlOperations.HasSayingRoundRange(actorSeat, targetSeat) || _coreProgramPersistentCombatPoliciesOperations.IsGameFactionAttackRangeTarget(actorSeat, targetSeat) || _coreNativeRuleQueriesOperations.GetCombatDistance(actorSeat, targetSeat) <= _coreNativeRuleQueriesOperations.GetAttackRange(actorSeat);
}
