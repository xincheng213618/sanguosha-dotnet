#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal bool HasMoQuanyuSlash => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMoSunQuanQuanyuSlashEffects);

    internal int MoQuanyuSlashMask(int owner, int target) => MoQiangangEntered(owner) ? MoQuanyuHistoryMask(owner, target) : MoQuanyuBit(MoQuanyuCurrentChoice(owner, owner));
    internal void CaptureMoQuanyuSingleton(ProgramCardTriggerWindowFrame window, SkillProgramTriggerWindow kind)
    {
        if (!HasMoQuanyuSlash || kind != SkillProgramTriggerWindow.CardUseTargetsFinalized || window.CompletedResponseReturn is not null || window.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard) || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { Type: CardActionType.Use } a, MoQuanyuSoleUse: null } use || !_coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat]).ProgramInstances.Any(i => i.SkillId is "ol:mo-quanyu" or "ol:mo-tianen") || a.ActionId != window.Action.ActionId || a.ActorSeat != use.SourceSeat || a.EffectiveKind != use.CardKind || !a.TargetSeats.SequenceEqual(use.TargetSeats))
            return;
        var actual = use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Where((_, i) => i % 2 == 0).ToArray() : use.TargetSeats.ToArray();
        if (actual is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || use.CardKind == CardKind.BorrowedSword && use.TargetSeats.Count != 2)
            return;
        var original = new MoQuanyuSoleUse(use.Id, a.ActionId, window.Id, use.SourceSeat, target, use.CardKind);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MoQuanyuSoleUse = original });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuSoleUseCapturedEvent(original));
    }

    internal bool TryGetMoQuanyuSingletonUse(int actor, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window, out int target)
    {
        use = null !;
        window = null !;
        target = -1;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } id } || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != id.ParentCardUseFrameId || w.CompletedResponseReturn is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { Type: CardActionType.Use } a, MoQuanyuSoleUse: { } original } u || original != new MoQuanyuSoleUse(u.Id, a.ActionId, w.Id, actor, original.TargetSeat, u.CardKind) || a.ActorSeat != actor || u.SourceSeat != actor || a.EffectiveKind != u.CardKind || a.ActionId != id.CardActionId || w.Action.ActionId != a.ActionId || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSoleUseCapturedEvent>().Count(e => e.Original == original) != 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == w.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != u.Id)
            return false;
        use = u;
        window = w;
        target = original.TargetSeat;
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IMoSunQuanSlashHost.Resolve(ProgramSkillFrame frame) => BeginMoQuanyuSlash(frame);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28912)]
    internal bool CanOfferMoQuanyuSlash(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMoSunQuanQuanyuSlashEffects))
            return true;
        return candidate.SkillId == "ol:mo-quanyu" && candidate.BindingId == "singleton-slash-effects" && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && TryGetMoQuanyuSingletonUse(candidate.OwnerSeat, context, out var use, out var window, out var target) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window) && MoQuanyuSlashMask(candidate.OwnerSeat, target) != 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashStartedEvent>().Any(e => e.Original.UseId == use.Id && e.Source.OwnerSeat == candidate.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginMoQuanyuSlash(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MoQuanyuSlash is not null || f.WindowContext is not { } context || f.SkillId != "ol:mo-quanyu" || f.TriggerId != "singleton-slash-effects" || f.InstructionIndex != 1 || !TryGetMoQuanyuSingletonUse(f.OwnerSeat, context, out var use, out _, out var target) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
            throw new InvalidOperationException("Quanyu requires its original sole-target native Slash and exact finalized-window producer.");
        var mask = MoQuanyuSlashMask(f.OwnerSeat, target);
        if (mask == 0 || use.MoQuanyuSlashPolicy is not null)
            throw new InvalidOperationException("A Slash cannot execute Quanyu twice.");
        var r = new MoQuanyuSlashReceipt(new(f.SkillId, f.TriggerId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, use.MoQuanyuSoleUse!, mask, MoQiangangEntered(f.OwnerSeat), MoQuanyuSlashStage.AdditionalTarget);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQuanyuSlash = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuSlashStartedEvent(f.Id, r.Source, r.GameplayHash, r.Original, mask, r.Entered));
        if ((mask & MoQuanyuBit(MoQuanyuEffect.Qingming)) != 0)
        {
            var targets = _coreMatchParticipantsPlayersAccess._players.Where(p => MoQuanyuAdditionalTarget(use, p)).Select(p => p.Seat).ToArray();
            if (targets.Length > 0)
            {
                Dictionary<string, string> Parameters() => new()
                {
                    ["program-action"] = "mo-quanyu-extra-target",
                    ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
                };
                var choices = targets.Select(t => new PromptChoice(new($"mo-quanyu.{(f.Id)}.{(t)}"), $"青冥：额外指定 {(_coreMatchParticipantsPlayersAccess._players[t].Name)}", [], [t], Parameters())).ToList();
                choices.Add(new(new($"mo-quanyu.{(f.Id)}.decline"), "不追加目标", [], [], Parameters()));
                r = r with
                {
                    Choices = choices
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQuanyuSlash = r, Step = ResolutionFrameStep.AwaitingResponse });
                _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "青冥：可额外指定一个合法目标。", [], targets, f.OwnerSeat)
                {
                    PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                    IsPrivate = false,
                    TargetSeat = f.OwnerSeat,
                    Choices = r.Choices,
                    SkillPrompt = new(f.SkillId, "权御", "青冥", _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Description)
                };
                _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                return SkillProgramStepOutcome.AwaitChoice;
            }
        }

        ApplyMoQuanyuSlash(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool MoQuanyuAdditionalTarget(CardUseFrame use, CharacterState target) => !use.TargetSeats.Contains(target.Seat) && _coreDesignatedExtraTargetsOperations.CanBeDistanceFreeDesignatedTarget(use, target) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat], target, new(-1, use.CardKind, use.Action!.EffectiveSuit ?? Suit.None, use.Action.EffectiveRank ?? 0), use.Action.ConversionChain.FirstOrDefault(), use.CardKind, existingUseFrameId: use.Id, specificEffectiveRank: use.Action.EffectiveRank, physicalCardIds: use.Action.PhysicalCards.Select(c => c.CardId).ToArray());
    [ProgramAnswer("mo-quanyu-extra-target")]
    internal void AnswerMoQuanyuAdditionalTarget(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoQuanyuSlash: { Stage: MoQuanyuSlashStage.AdditionalTarget } r } f || r.Choices.SingleOrDefault(c => c.Id == selected.Id)is not { } choice || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected]) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.Original.UseId)is not { Action: { } a } use)
            throw new InvalidOperationException("Quanyu extra target answer lost its frozen original Slash choice.");
        if (choice.Targets is [var added])
        {
            if (!MoQuanyuAdditionalTarget(use, _coreMatchParticipantsPlayersAccess._players[added]))
                throw new InvalidOperationException("Quanyu cannot add an illegal native target.");
            var targets = Array.AsReadOnly(use.TargetSeats.Append(added).ToArray());
            var action = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(a, targets);
            _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuTargetAddedEvent(f.Id, use.Id, a.ActionId, use.TargetSeats.Count, added, r.Source, r.GameplayHash));
            r = r with
            {
                AddedTargetSeat = added
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQuanyuSlash = r });
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        ApplyMoQuanyuSlash(f);
    }

    internal void ApplyMoQuanyuSlash(ProgramSkillFrame f)
    {
        var r = f.MoQuanyuSlash ?? throw new InvalidOperationException("Quanyu lost its issued sword effects.");
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(r.Original.UseId) ?? throw new InvalidOperationException("Quanyu lost its one accepted Use.");
        if (r.Stage != MoQuanyuSlashStage.AdditionalTarget || use.MoQuanyuSlashPolicy is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == f.GameplayHash && e.Original == r.Original && e.Mask == r.Mask) != 1)
            throw new InvalidOperationException("Quanyu's accepted effects cannot be repeated or retargeted.");
        r = r with
        {
            Stage = MoQuanyuSlashStage.Applied
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MoQuanyuSlashPolicy = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuSlashAppliedEvent(f.Id, use.Id, r.Original.ActionId, r.Mask, r.AddedTargetSeat));
        if ((r.Mask & MoQuanyuBit(MoQuanyuEffect.Baili)) != 0)
        {
            var native = new ExtraCardEffectState
            {
                Source = r.Source,
                ProgramFrameId = f.Id,
                ActionId = r.Original.ActionId,
                PaidCardId = 0
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { MoQuanyuExtraRound = new(r.Original.ActionId, f.Id, r.Source, r.GameplayHash, native) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoQuanyuExtraRoundIssuedEvent(f.Id, use.Id, r.Original.ActionId, r.Source, r.GameplayHash));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MoQuanyuSlash = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    internal void ApplyMoQuanyuAttackPolicy(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.MoQuanyuSlashPolicy is not { Stage: MoQuanyuSlashStage.Applied } r)
            return;
        if ((r.Mask & MoQuanyuBit(MoQuanyuEffect.Bixie)) != 0)
            attack.SetIgnoresArmor(true);
        if ((r.Mask & MoQuanyuBit(MoQuanyuEffect.Zidian)) != 0)
            attack.ProhibitDodgeBy("权御·紫电");
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 60)]
    internal int MoQuanyuDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.MoQuanyuSlashPolicy is { Stage: MoQuanyuSlashStage.Applied } r && (r.Mask & MoQuanyuBit(MoQuanyuEffect.Baihong)) != 0 ? 1 : 0;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMoSunQuanQuanyuSlashEffects)]
    internal PromptChoice ChooseMoQuanyuExtraTargetAi(PendingDecision prompt, ProgramSkillFrame f) => prompt.Choices.FirstOrDefault(c => c.Targets is [var target] && !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target])) ?? prompt.Choices.Single(c => c.Targets.Count == 0);
}
