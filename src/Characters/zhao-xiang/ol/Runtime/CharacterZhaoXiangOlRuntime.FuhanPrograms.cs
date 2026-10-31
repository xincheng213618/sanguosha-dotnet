#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoXiangOlRuntime
{
    internal string[] ZhaoXiangAvailableShuGenerals(int owner) => _corePrivateGeneralLibrariesOperations.PrivateGeneralCandidates().Where(id => _contentRegistryOperations._contentRegistry.Generals[id].FactionId == "shu" && _contentRegistryOperations._contentRegistry.Generals[id].SkillIds.All(s => _contentRegistryOperations._contentRegistry.Skills.TryGetValue(s, out var skill) && skill.ImplementationStatus == SkillImplementationStatus.Complete)).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 610)]
    internal bool CanOfferZhaoXiangExchange(int owner, SkillProgramTrigger trigger) => !ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.ExchangeFiveShuBySpentMeiYing) || _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[owner].HasHpValue && ZhaoXiangAvailableShuGenerals(owner).Length >= 5;
    internal bool ZhaoXiangPreparationParentMatches(ProgramSkillFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return f.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } c && f.TriggerId is not null && f.ActivationId == f.TriggerId && f.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && c.OwnerSeat == f.OwnerSeat && c.SourceSeat == f.OwnerSeat && index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame parent && parent.Id == c.ParentFrameId && parent.OwnerSeat == f.OwnerSeat && parent.Window == c.Window && parent.Continuation == ProgramLifecycleContinuation.NormalTurnStart && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]);
    }

    internal static string ZhaoXiangPoolFingerprint(IReadOnlyList<string> ids) => RuntimeSkills.RuntimeRules_zhao_xiang_ol_Fuhan_Programs.ZhaoXiangPoolFingerprint(ids);
    internal SkillProgramStepOutcome ExchangeFiveShuBySpentMeiYing(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ZhaoXiang is not null || f.InstructionIndex != 1 || !ZhaoXiangPreparationParentMatches(f) || !CanOfferZhaoXiangExchange(f.OwnerSeat, _coreProgramLifecycleOperations.GetProgramTrigger(f)))
            throw new InvalidOperationException("Five-Shu replacement lost its exact limited own preparation.");
        var available = ZhaoXiangAvailableShuGenerals(f.OwnerSeat).ToList();
        var candidates = new List<string>();
        for (var i = 0; i < 5; i++)
        {
            var at = _randomOperations._random.Next(available.Count);
            candidates.Add(available[at]);
            available.RemoveAt(at);
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var removed = owner.Markers.GetValueOrDefault(PlayerMarkerKind.MeiYing);
        var r = new ZhaoXiangReceipt
        {
            Source = new(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            ParentFrameId = f.WindowContext!.ParentFrameId,
            Turn = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            InstructionIndex = f.InstructionIndex,
            Stage = "selection",
            Removed = removed,
            CumulativeRemoved = checked(ZhaoXiangCumulativeRemoved(f.OwnerSeat) + removed),
            Generals = candidates,
            OriginalGeneralId = owner.General.Id,
            MaximumBefore = owner.MaxHp
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoXiang = r });
        if (removed > 0)
            _coreProgramMarkerPaymentsOperations.PayProgramMarkerCost(owner, new(PlayerMarkerKind.MeiYing, removed), f.SkillId, f.TriggerId!);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaoXiangExchangePaidEvent(f.Id, r.Source, r.GameplayHash, r.ParentFrameId, r.Turn, r.Removed, r.CumulativeRemoved, ZhaoXiangPoolFingerprint(candidates)));
        PublishZhaoXiangExchange(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ZhaoXiangExchangeChoices(ProgramSkillFrame f) => Array.AsReadOnly(f.ZhaoXiang!.Generals.Select(id => new PromptChoice(new($"zhao-xiang.{(f.Id)}.{(id)}"), $"替换为 {(_contentRegistryOperations._contentRegistry.Generals[id].Name)}", [], [], new Dictionary<string, string> { ["program-action"] = "zhao-xiang-exchange", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["general-id"] = id }) { ContentIds = Array.AsReadOnly(new[] { id }) }).ToArray());
    internal void PublishZhaoXiangExchange(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishZhaoXiang(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "扶汉：从实际抽取的五张蜀势力武将牌中选择替换。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            ValidContentIds = f.ZhaoXiang!.Generals,
            Choices = ZhaoXiangExchangeChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("zhao-xiang-exchange")]
    internal void ResolveZhaoXiangExchangeChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhaoXiang: { Stage: "selection" } } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ZhaoXiangExchangeChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Five-Shu choice lost its original private sampled templates.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var r = f.ZhaoXiang!;
        var generalId = choice.Parameters["general-id"];
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishZhaoXiang(f);
            return;
        }

        var maximum = Math.Clamp(r.CumulativeRemoved, 2, 8);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZhaoXiang = r = r with { Stage = "replacement-children", SelectedGeneralId = generalId, MaximumAfter = maximum } });
        owner.General = _coreMatchDefinitionGeneralPoolAccess._generalPool.Single(g => g.Id == generalId);
        owner.GeneralRevealed = true;
        _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, maximum - owner.MaxHp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhaoXiangExchangeSelectedEvent(f.Id, f.OwnerSeat, r.OriginalGeneralId, generalId, r.MaximumBefore, maximum));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ExchangeFiveShuBySpentMeiYing)]
    internal PromptChoice SelectAiZhaoXiangExchange(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderByDescending(c => _contentRegistryOperations._contentRegistry.Generals[c.Parameters["general-id"]].BaseHp).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
}
