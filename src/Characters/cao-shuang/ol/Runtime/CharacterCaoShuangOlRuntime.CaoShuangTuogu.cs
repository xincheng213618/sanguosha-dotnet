#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoShuangOlRuntime
{
    internal CardConversionSource CaoShuangSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome ICaoShuangProgramHost.InheritDeadGeneralChosenSkill(ProgramSkillFrame f) => BeginCaoShuangTuogu(f);
    SkillProgramStepOutcome ICaoShuangProgramHost.PlaceDamagedCardAsDelayedTrick(ProgramSkillFrame f) => BeginCaoShuangShanzhuan(f);
    internal string[] CaoShuangPrintedInheritanceSkills(int victim) => _coreMatchParticipantsPlayersAccess._players[victim].General.Skills.Concat(_coreMatchParticipantsPlayersAccess._players[victim].SecondaryGeneral?.Skills ?? []).Where(s => s.ContentId is not null && (s.Tags & (SkillTag.Limited | SkillTag.Awakening | SkillTag.Lord)) == 0 && !s.Name.Contains("隐匿", StringComparison.Ordinal) && !s.Description.Contains("隐匿", StringComparison.Ordinal)).Select(s => s.ContentId!).Distinct(StringComparer.Ordinal).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26060)]
    internal bool CanOfferCaoShuang(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.InheritDeadGeneralChosenSkill))
            return w.Window == SkillProgramTriggerWindow.CharacterDied && w.TargetSeat is { } victim && _coreTableSeatQueriesOperations.IsValidPlayerSeat(victim) && victim != c.OwnerSeat && !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().Any(p => p.Id == w.ParentFrameId && p.VictimSeat == victim) && CaoShuangPrintedInheritanceSkills(victim).Length > 0;
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.PlaceDamagedCardAsDelayedTrick))
            return w is { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } target } && w.SourceSeat == c.OwnerSeat && target != c.OwnerSeat && CaoShuangCanPlace(c.OwnerSeat, target);
        return true;
    }

    internal SkillProgramStepOutcome BeginCaoShuangTuogu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.CaoShuangTuogu is not null || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.CharacterDied, TargetSeat: { } victim } context || _resolutionStackOperations._resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } kill || kill.VictimSeat != victim || kill.CandidateIndex >= kill.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, kill.Candidates[kill.CandidateIndex]) || kill.Contexts[kill.CandidateIndex] != context || _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().SingleOrDefault(d => d.Id == kill.DeathFrameId)is not { KillerProgramsResolved: false } death || death.VictimSeat != victim || _coreMatchParticipantsPlayersAccess._players[victim].IsAlive || victim == f.OwnerSeat)
            throw new InvalidOperationException("Tuogu requires the exact current other-death candidate and deceased general.");
        var ids = CaoShuangPrintedInheritanceSkills(victim);
        if (ids.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoShuangTuogu = new(CaoShuangSource(f), f.GameplayHash, f.InstructionIndex, death.Id, kill.Id, victim) { SkillIds = ids }, Step = ResolutionFrameStep.AwaitingResponse });
        PublishCaoShuangTuogu(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CaoShuangTuoguChoices(ProgramSkillFrame f) => Array.AsReadOnly(f.CaoShuangTuogu!.SkillIds.Select(id => new PromptChoice(new($"caoshuang-tuogu.{(f.Id)}.{(id)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 获得【{(_contentRegistryOperations._contentRegistry.GetSkill(id).Name)}】", [], [], new Dictionary<string, string> { ["program-action"] = "caoshuang-tuogu", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["chosen-skill"] = id })).ToArray());
    internal void PublishCaoShuangTuogu(ProgramSkillFrame f)
    {
        var r = f.CaoShuangTuogu!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.VictimSeat, "托孤：为曹爽选择你武将牌上的一个技能", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = CaoShuangTuoguChoices(f),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("caoshuang-tuogu")]
    internal void AnswerCaoShuangTuogu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CaoShuangTuogu: { Granted: false } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != r.VictimSeat || _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], CaoShuangTuoguChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Only the exact deceased chooser can select its frozen printed Tuogu skill.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoShuangTuogu = null });
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        var skillId = choice.Parameters["chosen-skill"];
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        var previous = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CaoShuangTuoguInheritedEvent>().LastOrDefault(e => e.OwnerSeat == f.OwnerSeat);
        if (previous is not null)
            owner.SkillGrants.RemoveGrant(previous.GrantId);
        var existing = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.Where(g => g.SkillId == skillId).OrderBy(g => g.SourceId.StartsWith("turn:", StringComparison.Ordinal) ? 1 : 0).ThenBy(g => g.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
        var grantId = $"tuogu:{(f.OwnerSeat)}:{(f.Id)}:{(skillId)}";
        var instance = existing?.SkillInstanceId ?? grantId;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoShuangTuogu = r with { Granted = true }, Step = ResolutionFrameStep.ResolvingEffect });
        owner.SkillGrants.Grant(new(grantId, skillId, instance, "acquired:ol:tuogu"));
        _coreConversionStatesOperations.RegisterTaggedConversionSkill(owner, skillId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoShuangTuoguInheritedEvent(f.Id, r.DeathFrameId, f.OwnerSeat, r.VictimSeat, r.Source, skillId, grantId, instance, previous?.GrantId));
        if (existing is null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillsAcquiredEvent(f.OwnerSeat, f.SkillId, Array.AsReadOnly(new[] { skillId })));
        // Settle the same registered ownership/source-loss rules as normal rule
        // publication while this paid instruction still owns cleanup movements.
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoShuangTuogu = f.CaoShuangTuogu!with { SequenceBefore = before, SequenceAfter = before } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoShuangTuogu = f.CaoShuangTuogu!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueCaoShuangProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.InheritDeadGeneralChosenSkill)]
    internal PromptChoice ChooseAiCaoShuangTuogu(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
