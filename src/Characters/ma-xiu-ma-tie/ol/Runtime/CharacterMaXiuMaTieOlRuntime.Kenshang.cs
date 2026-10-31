#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaXiuMaTieOlRuntime
{
    internal const string KenshangDrawReason = "skill-program.ma-xiu-ma-tie.kenshang.draw";
    internal void IssueKenshangUse(long id, CardActionContext action)
    {
        if (action.Type != CardActionType.Use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.PhysicalCards.Count < 2)
            return;
        var source = action.ConversionChain.SingleOrDefault(s => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(s.SkillId)?.Program?.ViewAs.Any(r => r.Id == s.BindingId && r.KenshangUse == true) == true);
        if (source is null)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id) ?? throw new InvalidOperationException("Kenshang lost its native issuance owner.");
        if (use.Kenshang is not null || source.OwnerSeat != action.ProviderSeat || action.ActorSeat != source.OwnerSeat || use.Action?.ActionId != action.ActionId || !use.PhysicalCardIds!.SequenceEqual(action.PhysicalCards.Select(c => c.CardId)))
            throw new InvalidOperationException("Kenshang cannot issue another use or another provider's materials.");
        var hash = _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { Kenshang = new(id, action.ActionId, source, hash, action.ActorSeat, action.ProviderSeat, action.PhysicalCards.Count, use.TargetSeats) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangUseIssuedEvent(id, action.ActionId, source, hash, action.ActorSeat, action.ProviderSeat, action.PhysicalCards.Count));
        for (var i = 0; i < action.PhysicalCards.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangMaterialEvent(id, i, action.PhysicalCards[i]));
    }

    internal bool KenshangUseProof(CardUseFrame use)
    {
        if (use.Kenshang is not { } r || use.Action is not { Type: CardActionType.Use } a || r.UseId != use.Id || r.ActionId != a.ActionId || r.Source.OwnerSeat != r.ActorSeat || r.ProviderSeat != r.ActorSeat || a.ProviderSeat != r.ProviderSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || a.EffectiveKind != use.CardKind || r.MaterialCount < 2 || a.PhysicalCards.Count != r.MaterialCount || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, r.ActorSeat, r.ProviderSeat) || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program is not { } p || p.GameplayHash != r.GameplayHash || !p.ViewAs.Any(v => v.Id == r.Source.BindingId && v.KenshangUse == true) || !a.ConversionChain.Contains(r.Source) || _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangUseIssuedEvent>().Where(e => e.UseId == use.Id).ToArray()is not [var issued] || issued.ActionId != a.ActionId || issued.Source != r.Source || issued.GameplayHash != r.GameplayHash || issued.ActorSeat != r.ActorSeat || issued.ProviderSeat != r.ProviderSeat || issued.MaterialCount != r.MaterialCount)
            return false;
        var costs = _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangMaterialEvent>().Where(e => e.UseId == use.Id).OrderBy(e => e.Index).ToArray();
        return costs.Length == r.MaterialCount && costs.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, r.MaterialCount)) && costs.Select(e => e.Cost).SequenceEqual(a.PhysicalCards) && a.PhysicalCards.All(c => c.From.OwnerSeat == r.ProviderSeat && c.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment) && use.PhysicalCardIds!.SequenceEqual(a.PhysicalCards.Select(c => c.CardId));
    }

    internal void RecordKenshangDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageId, int amount)
    {
        if (amount <= 0 || !attack.DamageWasApplied || attack is not global::CardGame.Core.NativeOwners.CardAttackHandle card || attack.IsDelayedJudgmentDamage || attack.IsProgramJudgmentDamage || card.IsProgramSkillDamage || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Kenshang: { } r } use)
            return;
        if (!KenshangUseProof(use) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame d || d.Id != damageId || d.ParentFrameId != use.Id || d.Amount != amount || d.SourceSeat != attack.SourceSeat || d.TargetSeat != attack.TargetSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangDamageEvent>().Any(e => e.DamageFrameId == damageId))
            throw new InvalidOperationException("Kenshang damage requires the exact once-applied native use damage, including its real chain propagation.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangDamageEvent(use.Id, r.ActionId, damageId, attack.SourceSeat, attack.TargetSeat, amount, attack.IsChainPropagation));
    }

    internal int KenshangDamageTotal(CardUseFrame use) => _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangDamageEvent>().Where(e => e.UseId == use.Id && e.ActionId == use.Kenshang!.ActionId).Sum(e => e.Amount);
    internal int[] KenshangLegalTargets(CardUseFrame use)
    {
        var action = use.Action!;
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        // This is the target frontier of an already-issued Slash, whose quota and
        // equipment payment have completed. Rechecking a new Use quota here would
        // erase its own legal targets after the ordinary Slash debit.
        return _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, t, new(-1, use.CardKind, action.EffectiveSuit ?? Suit.None, action.EffectiveRank ?? 0), action.ConversionChain.FirstOrDefault(), use.CardKind, existingUseFrameId: use.Id, specificEffectiveRank: action.EffectiveRank, physicalCardIds: action.PhysicalCards.Select(c => c.CardId).ToArray()) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, use.CardKind, action.EffectiveSuit, global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(action)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(use.SourceSeat, t.Seat, action.EffectiveSuit) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, use.CardKind, [t.Seat])).Select(t => t.Seat).ToArray();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 12)]
    internal void CollectIssuedKenshangCandidates(CardActionContext a, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseCompleted) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { Kenshang: { } r } u || u.Action?.ActionId != a.ActionId || !KenshangUseProof(u) || !_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat].IsAlive || window == SkillProgramTriggerWindow.CardUseCommitted && r.Offered || window == SkillProgramTriggerWindow.CardUseCompleted && (r.DrawIssued || KenshangDamageTotal(u) >= r.MaterialCount))
            return;
        var op = window == SkillProgramTriggerWindow.CardUseCommitted ? SkillProgramEffectOp.ResolveKenshangTargetRewrite : SkillProgramEffectOp.DrawForKenshangShortDamage;
        var p = _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!;
        var t = p.Triggers.Single(t => t.Effects is [var e] && e.Op == op);
        if (result.Any(c => c.OwnerSeat == r.Source.OwnerSeat && c.SkillId == p.Id && c.SkillInstanceId == r.Source.SkillInstanceId && c.TriggerId == t.Id))
            return;
        result.Add(new(r.Source.OwnerSeat, r.Source.OwnerSeat, p.Id, t.Id, p.GameplayHash, r.Source.SkillInstanceId, t.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(a, window, 0, r.Source.OwnerSeat, r.Source.OwnerSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[r.Source.OwnerSeat], a))));
    }

    internal bool IsIssuedKenshangCandidate(ProgramTriggerCandidate c) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(c.SkillId)?.Program?.Triggers.Any(t => t.Id == c.BindingId && t.Effects.Any(e => KenshangComposition.Handles(e.Op))) == true;
    internal bool CanRunIssuedKenshangCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!IsIssuedKenshangCandidate(c) || context.CardUse is not { } card || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Kenshang: { } r, Action: { } a } u || !KenshangUseProof(u) || card.CardActionId != a.ActionId || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || c.OwnerSeat != r.Source.OwnerSeat || c.SkillId != r.Source.SkillId || c.SkillInstanceId != r.Source.SkillInstanceId || c.GameplayHash != r.GameplayHash || context.OwnerSeat != c.OwnerSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } w || w.ParentFrameId != u.Id || w.Action.ActionId != a.ActionId || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count)
            return false;
        var current = w.Candidates[w.CandidateIndex];
        var t = _coreProgramLifecycleOperations.GetProgramTrigger2(c);
        return current.OwnerSeat == c.OwnerSeat && current.SkillId == c.SkillId && current.TriggerId == c.BindingId && current.SkillInstanceId == c.SkillInstanceId && current.GameplayHash == c.GameplayHash && _coreCardActionsOperations.CreateCardActionProgramContext2(w, current) == context && (context.Window == SkillProgramTriggerWindow.CardUseCommitted && t.Effects is [{ Op: SkillProgramEffectOp.ResolveKenshangTargetRewrite }] && !r.Offered || context.Window == SkillProgramTriggerWindow.CardUseCompleted && t.Effects is [{ Op: SkillProgramEffectOp.DrawForKenshangShortDamage }] && !r.DrawIssued && KenshangDamageTotal(u) < r.MaterialCount);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27110)]
    internal bool QualifyKenshang(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => !t.Effects.Any(e => KenshangComposition.Handles(e.Op)) || CanRunIssuedKenshangCandidate(c, context);
    SkillProgramStepOutcome global::CardGame.Core.IKenshangHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginKenshang(f, op);
    internal CardUseFrame RequireKenshangUse(ProgramSkillFrame f)
    {
        if (f.WindowContext?.CardUse is not { } card || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is not { Kenshang: { } r } u || !KenshangUseProof(u) || r.Source.OwnerSeat != f.OwnerSeat || r.Source.SkillId != f.SkillId || r.Source.SkillInstanceId != f.SkillInstanceId || r.GameplayHash != f.GameplayHash || r.ActionId != card.CardActionId)
            throw new InvalidOperationException("Kenshang lost its exact issued native conversion.");
        return u;
    }

    internal SkillProgramStepOutcome BeginKenshang(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var c = f.WindowContext;
        if (f.Kenshang is not null || f.InstructionIndex != 1 || c is null || !CanRunIssuedKenshangCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c))
            throw new InvalidOperationException("Kenshang must start at its exact unconsumed use candidate.");
        var u = RequireKenshangUse(f);
        var r = u.Kenshang!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kenshang = new(u.Id, r.ActionId, r.Source, r.GameplayHash, f.InstructionIndex, op == SkillProgramEffectOp.ResolveKenshangTargetRewrite ? KenshangProgramStage.Targets : KenshangProgramStage.DrawChildren) });
        if (op == SkillProgramEffectOp.ResolveKenshangTargetRewrite)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(u.Id, x => x with { Kenshang = x.Kenshang!with { Offered = true } });
            if (KenshangLegalTargets(u).Length < r.MaterialCount)
                FinishKenshang(f);
            else
                PublishKenshangTargets(f);
        }
        else
        {
            var total = KenshangDamageTotal(u);
            if (total >= r.MaterialCount)
                throw new InvalidOperationException("Kenshang cannot draw after full or excess actual damage.");
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(u.Id, x => x with { Kenshang = x.Kenshang!with { DrawIssued = true } });
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var count = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, true, new(KenshangDrawReason)).Count;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kenshang = f.Kenshang!with { Before = before, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, DrawCount = count } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangDrawIssuedEvent(f.Id, u.Id, r.ActionId, total, r.MaterialCount, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
            ContinueKenshang(f.Id);
        }

        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> KenshangTargetChoices(ProgramSkillFrame f)
    {
        var r = f.Kenshang!;
        var u = RequireKenshangUse(f);
        var choices = new List<PromptChoice>();
        if (r.Selected.Count < u.Kenshang!.MaterialCount)
            foreach (var seat in KenshangLegalTargets(u).Except(r.Selected))
                choices.Add(new(new($"kenshang.{(f.Id)}.target.{(seat)}"), "重选目标：" + _coreMatchParticipantsPlayersAccess._players[seat].Name, [], [seat], new Dictionary<string, string> { { "program-action", "kenshang-target" }, { "branch", "select" } }));
        if (r.Selected.Count == u.Kenshang!.MaterialCount)
            choices.Add(new(new($"kenshang.{(f.Id)}.commit"), "将目标改为所选的等量角色", [], [], new Dictionary<string, string> { { "program-action", "kenshang-target" }, { "branch", "commit" } }));
        choices.Add(new(new($"kenshang.{(f.Id)}.keep"), "保留原目标", [], [], new Dictionary<string, string> { { "program-action", "kenshang-target" }, { "branch", "keep" } }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishKenshangTargets(ProgramSkillFrame f)
    {
        var choices = KenshangTargetChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "垦伤：可保留原目标，或重新指定恰等于实体牌数的合法目标。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("kenshang-target")]
    internal void AnswerKenshangTarget(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Kenshang: { Stage: KenshangProgramStage.Targets } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !KenshangTargetChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Kenshang needs its current exact target choice.");
        var u = RequireKenshangUse(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (choice.Parameters["branch"] == "select")
        {
            var seat = choice.Targets.Single();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangTargetSelectedEvent(f.Id, u.Id, r.Selected.Count, seat));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kenshang = r with { Selected = r.Selected.Append(seat).ToArray() } });
            PublishKenshangTargets(f);
            return;
        }

        if (choice.Parameters["branch"] == "commit")
        {
            if (r.Selected.Count != u.Kenshang!.MaterialCount || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Any(s => !KenshangLegalTargets(u).Contains(s)))
                throw new InvalidOperationException("Kenshang must replace the entire frontier with exactly as many distinct currently legal targets as original materials.");
            var targets = Array.AsReadOnly(r.Selected.ToArray());
            var action = _coreProgramCardUseRolesOperations.CloneRoleAction(u.Action!, u.SourceSeat, targets);
            var previous = u.TargetSeats;
            var updated = u with
            {
                TargetSeats = targets,
                TargetIndex = 0,
                Action = action,
                TargetsAdjusted = true,
                Kenshang = u.Kenshang!with
                {
                    RewriteFrameId = f.Id,
                    RewrittenTargets = targets
                },
                Enhancements = u.Enhancements | CurrentCardEnhancement.ExtraTarget
            };
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(updated, action);
            var attack = _coreCardActionsOperations.ProgramCardAttack ?? throw new InvalidOperationException("Kenshang lost its retained native Slash.");
            attack.SetDamageParticipants(u.SourceSeat, targets[0]);
            attack.SetIgnoresArmor(_coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat]) || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[u.SourceSeat], _coreMatchParticipantsPlayersAccess._players[targets[0]], u.CardKind));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangTargetsCommittedEvent(f.Id, u.Id, action.ActionId, targets.Count));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SlashTargetsReplacedEvent(f.Id, f.SkillId, f.OwnerSeat, u.Id, previous, targets));
        }

        FinishKenshang(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void FinishKenshang(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Kenshang = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal void ContinueKenshang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Kenshang: { } r } f || f.Id != id)
            return;
        if (r.Stage == KenshangProgramStage.Targets)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishKenshangTargets(f);
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        FinishKenshang(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27110)]
    internal bool ResumeKenshang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Kenshang: not null } f || f.Id != id)
            return false;
        ContinueKenshang(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27110)]
    internal bool ReturnKenshangMovement(ProgramSkillFrame f)
    {
        if (f.Kenshang is null)
            return false;
        ContinueKenshang(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveKenshangTargetRewrite, SkillProgramEffectOp.DrawForKenshangShortDamage)]
    internal PromptChoice AiKenshang(PendingDecision p, ProgramSkillFrame f) => p.Choices.Single(c => c.Parameters["branch"] == "keep");
    [ProgramIssuedTrigger(580, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger580(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsIssuedKenshangCandidate(candidate) ? CanRunIssuedKenshangCandidate(candidate, context) : null;
}
