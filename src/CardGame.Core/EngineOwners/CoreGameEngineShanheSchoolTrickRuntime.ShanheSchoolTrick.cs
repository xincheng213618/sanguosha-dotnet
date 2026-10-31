#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSchoolTrickRuntime
{
    internal static bool IsShanheSchoolDamageTrick(CardKind kind) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && (global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(kind) || kind == CardKind.UnexpectedAssault);
    internal bool TryGetShanheSchoolTrickUse(int owner, ProgramSkillWindowContext? context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } identity } || !_coreShanheSchoolsOperations.HasShanheSchoolEffect(owner, 9) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.Action is not { Type: CardActionType.Use } action || action.ActorSeat != owner || w.TrickContinuation is null || w.CompletedResponseReturn is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { } actual, ShanheSchoolTrick: null } current || current.SourceSeat != owner || actual.ActionId != action.ActionId || identity.CardActionId != actual.ActionId || identity.ParentCardUseFrameId != current.Id || current.CardKind != actual.EffectiveKind || !IsShanheSchoolDamageTrick(current.CardKind))
            return false;
        use = current;
        window = w;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62024, SkillProgramEffectOp.RepeatCurrentDamageTrick)]
    internal bool CanOfferShanheSchoolTrick(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => candidate.SkillId == ShanheSchoolEffects.TrickRepeatSkillId && TryGetShanheSchoolTrickUse(candidate.OwnerSeat, context, out _, out _);
    void global::CardGame.Core.IShanheSchoolTrickHost.RepeatCurrentDamageTrick(ProgramSkillFrame frame) => IssueShanheSchoolTrick(frame);
    internal void IssueShanheSchoolTrick(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != ShanheSchoolEffects.TrickRepeatSkillId || frame.InstructionIndex != 1 || !TryGetShanheSchoolTrickUse(frame.OwnerSeat, frame.WindowContext, out var use, out var window))
            throw new InvalidOperationException("A school trick repeat requires its exact accepted original damage-trick use.");
        var source = new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        var native = new ExtraCardEffectState
        {
            Source = source,
            ProgramFrameId = frame.Id,
            ActionId = use.Action!.ActionId,
            AutomaticGameplayHash = frame.GameplayHash,
            Trick = window.TrickContinuation
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ShanheSchoolTrick = new(use.Action.ActionId, frame.Id, window.Id, frame.GameplayHash, native) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSchoolTrickIssuedEvent(use.Id, use.Action.ActionId, frame.Id, window.Id, source, frame.GameplayHash));
    }

    internal bool HasUnstartedShanheSchoolTrick(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { ShanheSchoolTrick: { Started: false } round, Action: { } action } use && round.ActionId == action.ActionId && !_coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(use) && _coreMatchOutcomeWinnerAccess._winner == Winner.None;
    internal void CompleteShanheSchoolTrick(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { ShanheSchoolTrick: { Active: true } round } use || _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id) || _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { ShanheSchoolTrick = round with { Active = false } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSchoolTrickCompletedEvent(id, round.ActionId, round.ProgramFrameId));
    }

    internal bool TryBeginShanheSchoolTrick(long id, bool afterAttack)
    {
        if (!HasUnstartedShanheSchoolTrick(id) || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { ShanheSchoolTrick: { } round, Action: { } action } use || use.ExtraCardEffect is { WholeGroupRepeated: false } || _characterFeiYiOlOperations.HasUnstartedFeiYiExtraEffect(id) || _characterDaQiaoLeOperations.HasUnstartedZixiExtraEffect(id) || _characterSunQuanOlMagicOperations.HasUnstartedMoQuanyuExtraRound(id) || _characterHuangYueyingOlMouOperations.HasUnstartedMouBingcaiExtraEffect(id) || _coreSoleIncomingEffectRepeatOperations.HasUnstartedSoleIncomingEffect(id) || _characterHuangQuanMouOperations.HasUnstartedMouHqRound(id) || _characterJiangJiMouOperations.HasUnstartedMouJjEffect(id) || _coreSequentialTrickTargetsOperations.HasRemainingSequentialTrickTargets(id) || _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(id))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("A school extra effect cannot pass an unfinished native child.");
        var previous = _characterFeiYiOlOperations.NativeExtraEffectState(use);
        var native = round.Native with
        {
            WholeGroupRepeated = true,
            MaterialClaims = previous?.MaterialClaims ?? round.Native.MaterialClaims,
            FrozenGroupDamageSourceSeat = previous?.FrozenGroupDamageSourceSeat ?? round.Native.FrozenGroupDamageSourceSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { ShanheSchoolTrick = round with { Started = true, Active = true, Native = native }, TargetIndex = 0, Step = ResolutionFrameStep.ResolvingEffect, FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSchoolTrickStartedEvent(id, action.ActionId, round.ProgramFrameId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ExtraCardEffectStartedEvent(id, action.ActionId, 0, use.TargetSeats.FirstOrDefault(use.SourceSeat), use.CardKind));
        if (afterAttack)
        {
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
            _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        if (use.TargetSeats.All(seat => !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(id, seat)))
        {
            CompleteShanheSchoolTrick(id);
            return false;
        }

        _coreNativeCardContinuationCursorsOperations.ActiveGroupCard = null;
        _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword = null;
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true);
        var trick = native.Trick ?? throw new InvalidOperationException("A school repeat lost its native trick continuation.");
        _coreNativeNullificationOperations.BeginNullificationWindow(id, card, use.SourceSeat, use.TargetSeats, trick.ActionKind, null, trick.RequiredCardKind, use.CardKind);
        return true;
    }
}
