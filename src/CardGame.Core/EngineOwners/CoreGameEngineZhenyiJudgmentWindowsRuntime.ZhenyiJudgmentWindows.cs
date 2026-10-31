#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineZhenyiJudgmentWindowsRuntime
{
    internal bool ZhenyiJudgmentSucceeded(JudgmentFrame frame, Suit suit, int rank) => frame.Continuation switch
    {
        JudgmentContinuationKind.Lightning => suit == Suit.Spade && rank is >= 2 and <= 9,
        JudgmentContinuationKind.Indulgence => suit == Suit.Heart,
        JudgmentContinuationKind.SupplyShortage => suit == Suit.Club,
        _ => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(suit)};
    internal SkillProgramTriggerWindow CurrentZhenyiJudgmentWindowKind(long judgmentId) => _resolutionStackOperations._resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().LastOrDefault()is { ZhenyiBeforeResult: true } frame && frame.Judgment.JudgmentFrameId == judgmentId ? SkillProgramTriggerWindow.JudgmentBeforeResult : SkillProgramTriggerWindow.JudgmentFinalized;
    internal bool IsExactZhenyiBeforeResultProgramChild(ProgramJudgmentTriggerWindowFrame window, ProgramSkillFrame child)
    {
        if (_characterXiaoQiaoBoundaryMobileOperations.IsExactBoundaryXiaoQiaoBeforeResultProgramChild(window, child))
            return true;
        if (_coreHellDizangProgramsOperations.IsExactHellDayuanBeforeResultProgramChild(window, child))
            return true;
        if (window.ZhenyiBeforeResult != true || !window.Activated || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || child.WindowContext is not { Window: SkillProgramTriggerWindow.JudgmentBeforeResult, Judgment: { } result } context || context.ParentFrameId != window.Id || result != window.Judgment || _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(f => f.Id == window.ParentFrameId)is not { } pending || pending.ZhenyiBeforeResultCompleted == true || pending.Id != result.JudgmentFrameId || _coreNativeDamageHandlesOperations.GetJudgmentCard(pending)?.Id != result.CardId)
            return false;
        var candidate = window.Candidates[window.CandidateIndex];
        return candidate.OwnerSeat == child.OwnerSeat && candidate.SkillId == child.SkillId && candidate.TriggerId == child.TriggerId && candidate.SkillInstanceId == child.SkillInstanceId && candidate.GameplayHash == child.GameplayHash && _coreProgramLifecycleOperations.GetProgramTrigger(child).Effects is [{ Op: SkillProgramEffectOp.ChooseFinalJudgmentFiveByZiwei }];
    }

    internal bool TryBeginZhenyiBeforeResultWindow(JudgmentFrame pending, Card card, Suit suit)
    {
        if (pending.ZhenyiBeforeResultCompleted == true || !(_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseFinalJudgmentFiveByZiwei) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseHellDayuanJudgment) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RewriteBoundaryXiaoQiaoJudgment)))
            return false;
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => p.Seat).SelectMany(owner => _coreSkillProgramsOperations.EnabledUniqueProgramTriggers(owner, SkillProgramTriggerWindow.JudgmentBeforeResult).Where(b => ProgramInstructionResolver.Default.Features(b.Trigger).HasOperation(SkillProgramEffectOp.ChooseHellDayuanJudgment) || ProgramInstructionResolver.Default.Features(b.Trigger).HasOperation(SkillProgramEffectOp.RewriteBoundaryXiaoQiaoJudgment) || owner.Markers.GetValueOrDefault(PlayerMarkerKind.Ziwei) == 1 && ProgramInstructionResolver.Default.Features(b.Trigger).HasOperation(SkillProgramEffectOp.ChooseFinalJudgmentFiveByZiwei)).OrderByDescending(b => b.Trigger.Priority).ThenBy(b => b.SkillId, StringComparer.Ordinal).ThenBy(b => b.Trigger.Id, StringComparer.Ordinal).Select(b => new ProgramJudgmentTriggerCandidate(owner.Seat, b.SkillId, b.Trigger.Id, b.SkillInstanceId, b.Program.GameplayHash))).ToArray();
        if (candidates.Length == 0)
            return false;
        var rank = _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentRank(pending, card.Rank);
        var effectiveSuit = _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentSuit(pending, suit);
        var context = new JudgmentFinalizedContext(pending.Id, pending.TargetSeat, pending.Reason, card.Id, card.Kind, effectiveSuit, rank, ZhenyiJudgmentSucceeded(pending, effectiveSuit, rank), pending.SourceSeat);
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramJudgmentTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, pending.Id, context, Array.AsReadOnly(candidates)) { ZhenyiBeforeResult = true });
        return true;
    }

    internal ProgramJudgmentTriggerWindowFrame RefreshZhenyiBeforeResultWindow(ProgramJudgmentTriggerWindowFrame frame, JudgmentFrame pending)
    {
        if (frame.ZhenyiBeforeResult != true || frame.ParentFrameId != pending.Id || frame.Judgment.JudgmentFrameId != pending.Id || _coreNativeDamageHandlesOperations.GetJudgmentCard(pending)is not { } card || card.Id != frame.Judgment.CardId || card.Kind != frame.Judgment.CardKind)
            throw new InvalidOperationException("The before-result judgment lost its exact native physical card and parent.");
        var suit = _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentSuit(pending, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], card));
        var rank = _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentRank(pending, card.Rank);
        var updated = frame with
        {
            Judgment = frame.Judgment with
            {
                Suit = suit,
                Rank = rank,
                Succeeded = ZhenyiJudgmentSucceeded(pending, suit, rank)
            }
        };
        if (updated != frame)
            _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, updated);
        return updated;
    }

    internal void CompleteZhenyiBeforeResultWindow(ProgramJudgmentTriggerWindowFrame frame, JudgmentFrame pending)
    {
        if (frame.ZhenyiBeforeResult != true || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || frame.ParentFrameId != pending.Id || pending.ZhenyiBeforeResultCompleted == true)
            throw new InvalidOperationException("The before-result judgment must return to its exact native parent once.");
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramJudgmentTriggerWindow);
        pending = _coreNativeJudgmentOperations.GetJudgmentFrame(pending.Id)with
        {
            ZhenyiBeforeResultCompleted = true
        };
        _coreNativeJudgmentOperations.ReplaceJudgmentFrame(pending);
        var completed = _coreNativeJudgmentOperations.FinalizeJudgment(pending);
        if (completed is { } result)
            _coreNativeJudgmentOperations.ResumeCompletedJudgment(new global::CardGame.Core.GameEngine.JudgmentCompletionReceipt(GetJudgmentFrameOrCompletedZhenyi(pending), result));
    }

    internal JudgmentFrame GetJudgmentFrameOrCompletedZhenyi(JudgmentFrame fallback) => _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(f => f.Id == fallback.Id) ?? fallback;
}
