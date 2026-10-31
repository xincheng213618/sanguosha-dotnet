#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuPanOlRuntime
{
    internal static IReadOnlyList<int> PijingFoldTargets(IReadOnlyList<int> original, IReadOnlyList<PijingToggle> toggles, CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_liu_pan_ol_PijingTargets.PijingFoldTargets(original, toggles, kind);
    internal IReadOnlyList<PijingToggle> PijingInitialOptions(CardUseFrame use, IReadOnlyList<PijingToggle> selected)
    {
        var targets = PijingFoldTargets(use.TargetSeats, selected, use.CardKind);
        var current = use with
        {
            TargetSeats = targets,
            Action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets)
        };
        var result = new List<PijingToggle>();
        var mains = use.CardKind == CardKind.BorrowedSword ? targets.Where((_, i) => i % 2 == 0) : targets;
        foreach (var seat in mains.Where(s => s != use.SourceSeat && _coreMatchParticipantsPlayersAccess._players[s].IsAlive && !selected.Any(t => t.Seat == s)))
        {
            var index = targets.ToList().IndexOf(seat);
            result.Add(new(seat, true, use.CardKind == CardKind.BorrowedSword ? [seat, targets[index + 1]] : [seat]));
        }

        foreach (var pair in _characterYangFuOlOperations.BingzhongLegalTargets(current).Where(p => p[0] != use.SourceSeat && !selected.Any(t => t.Seat == p[0])))
            result.Add(new(pair[0], false, pair));
        return Array.AsReadOnly(result.ToArray());
    }

    internal IReadOnlyList<IReadOnlyList<int>> PijingReciprocalOptions(CardUseFrame use, int issuer)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[issuer].IsAlive)
            return[];
        if (use.CardKind != CardKind.UnexpectedAssault)
            return Array.AsReadOnly(_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidates(use).Where(p => p[0] == issuer).ToArray());
        var a = use.Action!;
        var actor = _coreMatchParticipantsPlayersAccess._players[a.ActorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[issuer];
        return issuer != a.ActorSeat && !a.EffectiveDesignatedTargetSeats.Contains(issuer) && _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0 && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, issuer, use.CardKind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, use.CardKind, a.EffectiveSuit ?? Suit.None, global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime.ActualTargetPolicyColor(a)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, issuer, a.EffectiveSuit) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, use.CardKind, [issuer]) ? new IReadOnlyList<int>[]
        {
            Array.AsReadOnly(new[] { issuer })
        }

        : [];
    }

    internal IReadOnlyList<PromptChoice> PijingChoices(ProgramSkillFrame f)
    {
        var r = f.Pijing!;
        var use = RequirePijingUse(f, out _);
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Args(string branch) => new()
        {
            {
                "program-action",
                "pijing"
            },
            {
                "frame-id",
                f.Id.ToString(CultureInfo.InvariantCulture)
            },
            {
                "branch",
                branch
            }
        };
        if (r.Stage == PijingStage.Toggles)
        {
            if (r.Toggles.Count < r.X)
                foreach (var toggle in PijingInitialOptions(use, r.Toggles))
                    choices.Add(new(new($"pijing.{(f.Id)}.{(r.Toggles.Count)}.{((toggle.Remove ? "remove" : "add"))}.{(string.Join('.', toggle.Pair))}"), (toggle.Remove ? "移除此目标：" : "追加此目标：") + string.Join("、", toggle.Pair.Select(s => _coreMatchParticipantsPlayersAccess._players[s].Name)), [], toggle.Pair, Args(toggle.Remove ? "remove" : "add")));
            choices.Add(new(new($"pijing.{(f.Id)}.finish"), "完成目标选择", [], [], Args("finish")));
        }
        else if (r.Stage == PijingStage.Reciprocal)
        {
            foreach (var pair in PijingReciprocalOptions(use, f.OwnerSeat))
                choices.Add(new(new($"pijing.{(f.Id)}.reciprocal.{(string.Join('.', pair))}"), "额外指定" + _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name, [], pair, Args("add")));
            choices.Add(new(new($"pijing.{(f.Id)}.draw"), "摸一张牌", [], [], Args("draw")));
            choices.Add(new(new($"pijing.{(f.Id)}.decline"), "不执行", [], [], Args("decline")));
        }

        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishPijing(ProgramSkillFrame f)
    {
        var r = f.Pijing!;
        var actor = r.Reciprocal ? r.ActorSeat : f.OwnerSeat;
        var choices = PijingChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, actor, r.Reciprocal ? "披荆：可额外指定原刘磐，或摸一张牌。" : $"披荆：增减至多{(r.X)}名其他角色。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices,
            TargetSeat = actor,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("pijing")]
    internal void AnswerPijing(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Pijing: { } r } f || r.Stage is not (PijingStage.Toggles or PijingStage.Reciprocal) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Reciprocal ? r.ActorSeat : f.OwnerSeat) || !PijingChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("Pijing requires its original actual chooser and currently legal public target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (r.Stage == PijingStage.Toggles)
        {
            if (branch == "finish")
            {
                ApplyPijingTargets(f);
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = f.Pijing!with { Stage = PijingStage.Gifts } });
                ContinuePijing(f.Id);
                return;
            }

            var toggle = new PijingToggle(selected.Targets[0], branch == "remove", selected.Targets);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingToggleSelectedEvent(f.Id, r.Toggles.Count, toggle.Seat, toggle.Remove, toggle.Pair.Count));
            for (var i = 0; i < toggle.Pair.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingTogglePairEvent(f.Id, r.Toggles.Count, i, toggle.Pair[i]));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = r with { Toggles = r.Toggles.Append(toggle).ToArray() } });
            PublishPijing(f);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingNextChoiceEvent(f.Id, r.GroupId, branch));
        if (branch == "draw")
        {
            DrawPijingReciprocal(f);
            return;
        }

        if (branch == "add")
        {
            var toggle = new PijingToggle(f.OwnerSeat, false, selected.Targets);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingToggleSelectedEvent(f.Id, 0, toggle.Seat, false, toggle.Pair.Count));
            for (var i = 0; i < toggle.Pair.Count; i++)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingTogglePairEvent(f.Id, 0, i, toggle.Pair[i]));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Pijing = r with { Toggles = [toggle] } });
            ApplyPijingTargets(f);
        }

        FinishPijing(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ApplyPijingTargets(ProgramSkillFrame f)
    {
        var r = f.Pijing!;
        var use = RequirePijingUse(f, out var window);
        var targets = PijingFoldTargets(r.Original, r.Toggles, use.CardKind);
        var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(use.Action!, targets);
        var simple = use.AdjustedSimpleContinuation;
        if (r.Toggles.Count > 0 && window.Continuation == ProgramCardContinuation.FinalizedSimpleCard)
        {
            var continuation = window.SimpleContinuation ?? throw new InvalidOperationException("Pijing lost its native supportive basic effect.");
            if (continuation.Effect is not (SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol))
                throw new InvalidOperationException("Pijing cannot invent an equipment or basic-card effect.");
            simple ??= continuation with
            {
                RecoveryPolicySources = continuation.RecoveryPolicySources is { } p ? Array.AsReadOnly(p.ToArray()) : null
            };
        }

        for (var i = 0; i < targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingTargetSlotEvent(f.Id, true, i, targets[i]));
        var tail = new PijingTargetTail(f.Id, use.Id, r.ActionId, r.Source, r.GameplayHash, r.ActorSeat, r.ProviderSeat, r.Reciprocal, r.Original, targets);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PijingTargetsAppliedEvent(f.Id, use.Id, r.ActionId, r.Source, r.GameplayHash, r.ActorSeat, r.ProviderSeat, r.Original.Count, targets.Count, r.Reciprocal));
        var rebased = RebasePijingSlashTargets(use, targets);
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(rebased with { TargetSeats = targets, Action = action, PijingTail = tail, AdjustedSimpleContinuation = simple, TargetsAdjusted = use.TargetsAdjusted || r.Toggles.Count > 0, Enhancements = r.Toggles.Count > 0 ? use.Enhancements | CurrentCardEnhancement.ExtraTarget : use.Enhancements }, action);
        var added = r.Toggles.Where(t => !t.Remove).Select(t => t.Seat).ToArray();
        if (added.Length > 0)
            _coreCurrentUseTargetCandidateRefreshOperations.RefreshCurrentUseAddedTargetCandidates(_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!, added);
    }

    internal CardUseFrame RebasePijingSlashTargets(CardUseFrame use, IReadOnlyList<int> targets)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.CardAttack is not { } original)
            return use with
            {
                TargetIndex = 0,
                SequentialTrick = targets.Count == 0 ? null : use.SequentialTrick
            };
        CardAttackState Fresh(int seat) => new()
        {
            Active = original.Active,
            SourceSeat = use.SourceSeat,
            CardUserSeat = use.SourceSeat,
            TargetSeat = seat,
            DamageAmount = use.ProgramAdjustedSlashBaseDamage ?? use.Continuations.FangtianHalberd?.DamageAmount ?? original.DamageAmount,
            IgnoresArmor = use.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], _coreMatchParticipantsPlayersAccess._players[seat], use.CardKind),
            EffectiveCardKind = use.CardKind,
            IsDelayedJudgmentDamage = original.IsDelayedJudgmentDamage,
            IsSourceLess = original.IsSourceLess,
            ProgramJudgmentFrameId = original.ProgramJudgmentFrameId,
            ProgramSkillFrameId = original.ProgramSkillFrameId,
            ProgramSkillCardUseFrameId = original.ProgramSkillCardUseFrameId,
            DelayedJudgmentSeat = original.DelayedJudgmentSeat,
            DamageNatureOverride = original.DamageNatureOverride,
            ConversionSource = original.ConversionSource,
            CardId = original.CardId,
            AppearanceKind = original.AppearanceKind,
            AppearanceSuit = original.AppearanceSuit,
            AppearanceRank = original.AppearanceRank,
            PhysicalCardIds = Array.AsReadOnly(original.PhysicalCardIds.ToArray()),
            AdjustedSlashReturn = original.AdjustedSlashReturn
        };
        IReadOnlyList<CardAttackState>? prepared = use.PreparedTargetAttacks is { } old ? Array.AsReadOnly(targets.Select(seat => old.FirstOrDefault(a => a.TargetSeat == seat) ?? Fresh(seat)).ToArray()) : null;
        var first = targets.Count == 0 ? original : prepared is { Count: > 0 } ? prepared[0] : targets[0] == original.TargetSeat ? original : Fresh(targets[0]);
        return use with
        {
            TargetIndex = 0,
            CardAttack = first,
            PreparedTargetAttacks = prepared,
            SlashTargetsCancelled = targets.Count == 0,
            Continuations = use.Continuations with
            {
                FangtianHalberd = use.Continuations.FangtianHalberd is { Active: true } multi ? multi with
                {
                    TargetSeats = Array.AsReadOnly(targets.ToArray()),
                    CurrentAttackOwnerId = use.Id
                }

                : use.Continuations.FangtianHalberd
            }
        };
    }

    internal bool TryCompletePijingZeroTargetUse(ProgramCardTriggerWindowFrame window)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { } use || !HasPijingZeroTargetTail(use))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || window.Action.ActionId != use.Action!.ActionId || window.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick))
            throw new InvalidOperationException("Pijing's zero-target finish lost its true whole-use return.");
        if (window.Continuation == ProgramCardContinuation.Slash)
        {
            if (window.AttackOwnerFrameId != use.Id || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || attack.ResolutionId != use.Id)
                throw new InvalidOperationException("Pijing's removed Slash lost its actual attack.");
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
        else
        {
            var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(use.Id, use.CardId, requireProcessing: false);
            _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(use.Id, card);
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, card, use.CardKind);
        }

        return true;
    }
}
