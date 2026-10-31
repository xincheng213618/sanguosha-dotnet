#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunJianMouOlRuntime
{
    internal static bool IsMouSunJianOlHulieKind(CardKind kind) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || kind == CardKind.Duel;
    internal bool MouSunJianOlHulieAvailable(int owner, int turn, bool counter) => !_coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlHulieUsedEvent>().Any(e => e.OwnerSeat == owner && e.Turn == turn && e.Counter == counter);
    internal CardUseFrame? MouSunJianOlContextUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } info || _coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } a } use || a.ActionId != info.CardActionId || !IsMouSunJianOlHulieKind(use.CardKind) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.ParentFrameId != use.Id || window.Action.ActionId != a.ActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } current || current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash)
            return null;
        return use;
    }

    internal bool CanOfferMouSunJianOlHulie(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || MouSunJianOlContextUse(c, w)is not { Action: { } a, TargetSeats: [var target] } use || use.SourceSeat != c.OwnerSeat || a.ActorSeat != c.OwnerSeat || target == c.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId) || use.MouSunJianOlHulieEntries?.Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId) == true || a.EffectiveDesignatedTargetSeats.Distinct().Count() != 1 || !a.EffectiveDesignatedTargetSeats.Contains(target))
            return false;
        return MouSunJianOlHulieAvailable(c.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, false) || MouSunJianOlHulieAvailable(c.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, true);
    }

    internal bool ValidMouSunJianOlHulieEntry(CardUseFrame use, MouSunJianOlHulieEntry e) => use.Action is { Type: CardActionType.Use } a && e.UseId == use.Id && e.ActionId == a.ActionId && e.Source.SkillId == MouSunJianOlComposition.Hulie && e.Source.BindingId == "single-target-damage" && e.Source.OwnerSeat == e.ActorSeat && e.TargetSeat != e.ActorSeat && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlHulieQualifiedEvent>().Count(q => q.Entry == e with { Boosted = false }) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(b => b.FrameId == e.FrameId && b.OwnerSeat == e.Source.OwnerSeat && b.SkillId == e.Source.SkillId && b.SkillInstanceId == e.Source.SkillInstanceId && b.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1 && (!e.Boosted || _coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlHulieUsedEvent>().Count(b => b.FrameId == e.FrameId && b.OwnerSeat == e.Source.OwnerSeat && b.Turn == e.Turn && !b.Counter && b.OriginalUseId == e.UseId) == 1);
    internal MouSunJianOlHulieEntry? MouSunJianOlHulieEntry(CardUseFrame use, ProgramTriggerCandidate c) => use.MouSunJianOlHulieEntries?.SingleOrDefault(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.Source.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash && ValidMouSunJianOlHulieEntry(use, e));
    [ProgramIssuedTrigger(620)]
    internal bool CanRunIssuedMouSunJianOlHulie(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == MouSunJianOlComposition.Hulie && c.BindingId == "undamaged-counter" && w.Window == SkillProgramTriggerWindow.CardUseCompleted && MouSunJianOlContextUse(c, w)is { Step: ResolutionFrameStep.Completed } use && MouSunJianOlHulieEntry(use, c)is { } e && _coreMatchParticipantsPlayersAccess._players[e.ActorSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[e.TargetSeat].IsAlive && MouSunJianOlHulieAvailable(e.ActorSeat, e.Turn, true) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlUseDamageEvent>().Any(d => d.UseId == use.Id && d.Amount > 0) && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(d => d.ResolutionId == use.Id) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlCounterConsideredEvent>().Any(d => d.OwnerSeat == c.OwnerSeat && d.OriginalUseId == use.Id);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 32501)]
    internal bool QualifyMouSunJianOlHulie(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => !t.Effects.Any(e => e.Op is SkillProgramEffectOp.OfferMouSunJianHulieDamage or SkillProgramEffectOp.ResolveMouSunJianHulieCounter) || (t.Effects[0].Op == SkillProgramEffectOp.OfferMouSunJianHulieDamage ? CanOfferMouSunJianOlHulie(c, w) : CanRunIssuedMouSunJianOlHulie(c, w));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 32501)]
    internal void CollectMouSunJianOlHulieCounters(CardActionContext a, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted || _coreCardUseLifecycleOperations.LifecycleCardUse(_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id ?? 0)is not { MouSunJianOlHulieEntries: { } entries } use || use.Action?.ActionId != a.ActionId || _coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlUseDamageEvent>().Any(d => d.UseId == use.Id && d.Amount > 0))
            return;
        foreach (var e in entries.Where(e => ValidMouSunJianOlHulieEntry(use, e) && _coreMatchParticipantsPlayersAccess._players[e.ActorSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[e.TargetSeat].IsAlive && MouSunJianOlHulieAvailable(e.ActorSeat, e.Turn, true)))
        {
            var t = _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program!.Triggers.Single(t => t.Id == "undamaged-counter");
            if (!result.Any(c => c.OwnerSeat == e.Source.OwnerSeat && c.SkillId == e.Source.SkillId && c.SkillInstanceId == e.Source.SkillInstanceId && c.TriggerId == t.Id))
                result.Add(new(e.Source.OwnerSeat, e.Source.OwnerSeat, e.Source.SkillId, t.Id, e.GameplayHash, e.Source.SkillInstanceId, t.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(a, window, 0, e.Source.OwnerSeat, e.Source.OwnerSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat], a))));
        }
    }

    internal SkillProgramStepOutcome BeginMouSunJianOlHulie(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0);
        if (f.MouSunJianOlHulie is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || !(op == SkillProgramEffectOp.OfferMouSunJianHulieDamage ? CanOfferMouSunJianOlHulie(c, w) : CanRunIssuedMouSunJianOlHulie(c, w)))
            throw new InvalidOperationException("Hulie requires its exact original accepted single-target native Use.");
        var use = MouSunJianOlContextUse(c, w)!;
        var entry = op == SkillProgramEffectOp.OfferMouSunJianHulieDamage ? new MouSunJianOlHulieEntry(f.Id, use.Id, use.Action!.ActionId, MouSunJianOlSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, f.OwnerSeat, use.TargetSeats.Single()) : MouSunJianOlHulieEntry(use, c)!;
        if (op == SkillProgramEffectOp.OfferMouSunJianHulieDamage)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouSunJianOlHulieEntries = (u.MouSunJianOlHulieEntries ?? []).Append(entry).ToArray() });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlHulieQualifiedEvent(entry));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlHulie = new(entry, op == SkillProgramEffectOp.OfferMouSunJianHulieDamage ? MouSunJianOlHulieStage.DamageChoice : MouSunJianOlHulieStage.CounterChoice) });
        if (op == SkillProgramEffectOp.ResolveMouSunJianHulieCounter)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlCounterConsideredEvent(f.Id, f.OwnerSeat, entry.UseId));
        if (op == SkillProgramEffectOp.OfferMouSunJianHulieDamage && !MouSunJianOlHulieAvailable(f.OwnerSeat, entry.Turn, false))
            FinishMouSunJianOl(f);
        else
            PublishMouSunJianOlHulie(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MouSunJianOlHulieChoices(ProgramSkillFrame f)
    {
        var r = f.MouSunJianOlHulie!;
        var counter = r.Stage == MouSunJianOlHulieStage.CounterChoice;
        var result = new List<PromptChoice>();
        void Add(string branch, string label) => result.Add(new(new($"mou-hulie.{(f.Id)}.{(branch)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = "mou-sun-jian-hulie", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch }));
        if (MouSunJianOlHulieAvailable(f.OwnerSeat, r.Entry.Turn, counter) && (!counter || CanMouSunJianOlCounter(f)))
            Add("accept", counter ? "令原目标视为对你使用一张【杀】" : "此牌伤害+1");
        Add("decline", "不发动虎烈");
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishMouSunJianOlHulie(ProgramSkillFrame f)
    {
        var choices = MouSunJianOlHulieChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.MouSunJianOlHulie!.Stage == MouSunJianOlHulieStage.DamageChoice ? "虎烈：是否令此牌伤害+1？" : "虎烈：此牌未造成伤害，是否令原目标反击？", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-sun-jian-hulie")]
    internal void AnswerMouSunJianOlHulie(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouSunJianOlHulie: { } r } f || !ValidMouSunJianOlHulie(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !MouSunJianOlHulieChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Hulie changed its exact published original-use decision.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["branch"] == "decline")
        {
            FinishMouSunJianOl(f);
            return;
        }

        var counter = r.Stage == MouSunJianOlHulieStage.CounterChoice;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlHulieUsedEvent(f.Id, f.OwnerSeat, r.Entry.Turn, r.Entry.TurnOwner, counter, r.Entry.UseId));
        if (!counter)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(r.Entry.UseId, u => u with { MouSunJianOlHulieEntries = u.MouSunJianOlHulieEntries!.Select(e => e == r.Entry ? e with { Boosted = true } : e).ToArray() });
            FinishMouSunJianOl(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSunJianOlHulie = r with { Accepted = true } });
        IssueMouSunJianOlCounter(f);
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 32501)]
    internal int MouSunJianOlHulieDamageBonus(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is NativeOwners.CardAttackHandle && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { MouSunJianOlHulieEntries: { } entries } use ? entries.Count(e => e.Boosted && ValidMouSunJianOlHulieEntry(use, e)) : 0;
    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 32501)]
    internal bool RecordMouSunJianOlUseDamage(GameEngine.IDamageAttempt attack, long damageId)
    {
        if (attack is not NativeOwners.CardAttackHandle card || !card.DamageWasApplied || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouSunJianOlHulieEntries.Count: > 0 } use || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damageId)is not DamageFrame { Amount: > 0 } damage || damage.ParentFrameId != use.Id)
            return false;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouSunJianOlUseDamageEvent>().Any(e => e.DamageId == damageId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSunJianOlUseDamageEvent(use.Id, damageId, damage.Amount));
        return false;
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferMouSunJianHulieDamage, SkillProgramEffectOp.ResolveMouSunJianHulieCounter)]
    internal PromptChoice AiMouSunJianOlHulie(PendingDecision p, ProgramSkillFrame f) => p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "accept") ?? p.Choices.Single();
}
