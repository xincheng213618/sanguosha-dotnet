#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSoleIncomingEffectRepeatRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 50)]
    internal bool SoleIncomingEffectFinalizedBeforeActivityTargetWindow(ProgramTargetCardWindowQuery query) => HasSoleIncomingEffectCapability && global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime.ExtraEffectCardKind(query.Kind);
    internal bool HasSoleIncomingEffectCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RepeatIncomingSoleCardEffectAndDrawByRange);

    internal bool TryGetSoleIncomingEffect(int owner, ProgramSkillWindowContext? context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } id } || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != id.ParentCardUseFrameId || w.CompletedResponseReturn is not null || w.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard) || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } current || a.ActorSeat == owner || a.ActionId != id.CardActionId || a.ActorSeat != current.SourceSeat || a.EffectiveKind != current.CardKind || w.Action.ActionId != a.ActionId || !w.Action.TargetSeats.SequenceEqual(current.TargetSeats) || !global::CardGame.Core.EngineOwners.CoreGameEngineExtraEffectsAndRankGuessingRuntime.ExtraEffectCardKind(current.CardKind) || current.CardKind is CardKind.Dodge or CardKind.Nullification || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            return false;
        var actual = a.EffectiveDesignatedTargetSeats.Distinct().ToArray();
        if (actual is not [var target] || target != owner || !current.TargetSeats.Contains(owner) || current.SoleIncomingEffects is { Started: > 0 })
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == w.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != current.Id)
            return false;
        use = current;
        window = w;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1243, SkillProgramEffectOp.RepeatIncomingSoleCardEffectAndDrawByRange)]
    internal bool CanOfferSoleIncomingEffect(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => TryGetSoleIncomingEffect(candidate.OwnerSeat, context, out var use, out var window) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window) && use.SoleIncomingEffects?.Grants.Any(g => g.Source.OwnerSeat == candidate.OwnerSeat && g.Source.SkillId == candidate.SkillId && g.Source.SkillInstanceId == candidate.SkillInstanceId) != true;
    internal void IssueSoleIncomingEffect(ProgramSkillFrame supplied, int cap)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || !TryGetSoleIncomingEffect(f.OwnerSeat, context, out var use, out var window) || !CanOfferSoleIncomingEffect(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("An incoming repeat needs its exact sole target and original finalized native use.");
        var source = new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var grant = new SoleIncomingEffectGrant(f.Id, source, f.GameplayHash, window.Id, use.Id, use.Action!.ActionId, use.Action.ActorSeat, f.OwnerSeat, use.CardKind, cap);
        var previous = use.SoleIncomingEffects;
        var simple = window.SimpleContinuation is { } s ? s with
        {
            RecoveryPolicySources = s.RecoveryPolicySources is { } policies ? Array.AsReadOnly(policies.ToArray()) : null
        }

        : null;
        var native = previous?.Native ?? new ExtraCardEffectState
        {
            Source = source,
            ProgramFrameId = f.Id,
            ActionId = grant.ActionId,
            Trick = window.TrickContinuation,
            Simple = simple
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { SoleIncomingEffects = new(grant.ActionId, native) { Grants = (previous?.Grants ?? []).Append(grant).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleIncomingEffectIssuedEvent(grant));
    }

    internal bool HasUnstartedSoleIncomingEffect(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { SoleIncomingEffects: { } r, Action: { } a } use && a.ActionId == r.ActionId && r.Started < r.Grants.Count && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(use) && _coreMatchOutcomeWinnerAccess._winner == Winner.None;
    internal bool IsSoleIncomingExtraRound(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.SoleIncomingEffects is { Active: true };
    internal void CaptureSoleIncomingAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { SoleIncomingEffects: { Started: 0 } rounds, CardAttack: { } original } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || _characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(use.Id) != 0 || rounds.Native.OriginalAttacks.Any(s => s.TargetIndex == use.TargetIndex))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { SoleIncomingEffects = rounds with { Native = rounds.Native with { OriginalAttacks = rounds.Native.OriginalAttacks.Append(new(use.TargetIndex, original)).ToArray(), Fangtian = current.Continuations.FangtianHalberd ?? rounds.Native.Fangtian } } });
    }

    internal void CompleteSoleIncomingExtraRound(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { SoleIncomingEffects: { Active: true, Started: > 0 } r } use)
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { SoleIncomingEffects = r with { Active = false } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleIncomingEffectRoundCompletedEvent(id, r.ActionId, r.Grants[r.Started - 1].ProgramFrameId, r.Started));
    }

    internal bool TryBeginSoleIncomingExtraRound(long id, bool afterAttack)
    {
        if (!HasUnstartedSoleIncomingEffect(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { SoleIncomingEffects: { } r, Action: { } action } use || use.ExtraCardEffect is { WholeGroupRepeated: false } || _characterFeiYiOlOperations.HasUnstartedFeiYiExtraEffect(id) || _characterDaQiaoLeOperations.HasUnstartedZixiExtraEffect(id) || _characterSunQuanOlMagicOperations.HasUnstartedMoQuanyuExtraRound(id) || _characterHuangYueyingOlMouOperations.HasUnstartedMouBingcaiExtraEffect(id) || r.Native.Trick is not null && _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id) || r.Native.Simple is not null && _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(id))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("An incoming extra effect cannot pass an unfinished native child.");
        var previous = _characterFeiYiOlOperations.NativeExtraEffectState(use);
        var native = r.Native with
        {
            WholeGroupRepeated = true,
            MaterialClaims = previous?.MaterialClaims ?? r.Native.MaterialClaims,
            FrozenGroupDamageSourceSeat = previous?.FrozenGroupDamageSourceSeat ?? r.Native.FrozenGroupDamageSourceSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { SoleIncomingEffects = r with { Started = r.Started + 1, Active = true, Native = native }, TargetIndex = 0, Step = ResolutionFrameStep.ResolvingEffect, FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
        var grant = r.Grants[r.Started];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleIncomingEffectRoundStartedEvent(id, action.ActionId, grant.ProgramFrameId, r.Started + 1));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraCardEffectStartedEvent(id, action.ActionId, 0, use.TargetSeats.FirstOrDefault(use.SourceSeat), use.CardKind));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind))
        {
            var first = native.OriginalAttacks.OrderBy(s => s.TargetIndex).FirstOrDefault(s => _coreMatchParticipantsPlayersAccess._players[s.Attack.TargetSeat].IsAlive && !_coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, s.Attack.TargetSeat));
            if (first is null)
            {
                CompleteSoleIncomingExtraRound(id);
                return false;
            }

            var prepared = use.PreparedTargetAttacks?.Select((state, index) => native.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == index)?.Attack ?? state).ToArray();
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { TargetIndex = first.TargetIndex, PreparedTargetAttacks = prepared is null ? null : Array.AsReadOnly(prepared) });
            if (native.Fangtian is { } fang)
            {
                _coreCardAttackStateOperations.UpdateCardContinuations(id, state => state with { FangtianHalberd = fang with { Active = true, CurrentAttackOwnerId = id } });
                _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id);
            }

            _coreCardAttackStateOperations.UpdateCardAttackState(id, _ => global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime.ResetExtraCardEffectAttack(first.Attack));
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
            return true;
        }

        if (native.Trick is { } trick)
        {
            _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = null;
            _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
            var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true);
            _coreNativeNullificationOperations.BeginNullificationWindow(id, card, use.SourceSeat, use.TargetSeats, trick.ActionKind, null, trick.RequiredCardKind, use.CardKind);
            return true;
        }

        if (native.Simple is { } simple)
        {
            if (use.TargetSeats.Count > 1 && simple.Effect is SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol or SimpleCardUseEffect.GuanduIntoxication)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, current => current with { AdjustedSimpleContinuation = simple });
            _coreCardUseLifecycleOperations.ContinueSimpleCardUse(id, simple);
            return true;
        }

        throw new InvalidOperationException("An incoming repeat lost its accepted original effect continuation.");
    }

    internal bool ContinueSoleIncomingFangtian(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { SoleIncomingEffects: { Active: true } r } use)
            return false;
        var seed = r.Native.OriginalAttacks.SingleOrDefault(s => s.TargetIndex == use.TargetIndex);
        if (seed is null)
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return true;
        }

        _coreCardAttackStateOperations.UpdateCardAttackState(use.Id, _ => global::CardGame.Core.EngineOwners.CoreGameEngineExtraCardEffectRoundsRuntime.ResetExtraCardEffectAttack(seed.Attack));
        _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
        return true;
    }

    void global::CardGame.Core.ISoleIncomingEffectRepeatHost.IssueSoleIncomingEffect(ProgramSkillFrame frame, int maximumDrawCount) => IssueSoleIncomingEffect(frame, maximumDrawCount);
}
