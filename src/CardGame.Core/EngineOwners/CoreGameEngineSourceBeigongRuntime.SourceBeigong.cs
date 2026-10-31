#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceBeigongRuntime
{
    internal IReadOnlyList<PublicPersistentPileSource> SourceDingxiPiles(int owner) => _corePublicPileSourcesOperations.PublicPileSources(owner).Where(p => p.SkillId == SourceBeigongIds.Dingxi).ToArray();
    internal int SourceDingxiCount(int owner) => SourceDingxiPiles(owner).Sum(p => _corePublicPileSourcesOperations.PublicPileCards(p).Count);
    internal static bool IsSourceBeigongOp(SkillProgramEffectOp op) => op is SkillProgramEffectOp.ResolveSourceDingxiUsedCard or SkillProgramEffectOp.DrawSourceDingxiPile or SkillProgramEffectOp.ResolveSourceHuojie or SkillProgramEffectOp.ResolveSourceXingxia;
    internal static bool HasSourceBeigongState(ProgramSkillFrame f) => f.SourceDingxi is not null || f.SourceHuojie is not null || f.SourceXingxia is not null;
    internal bool DrainSourceBeigong(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    SkillProgramStepOutcome global::CardGame.Core.ISourceBeigongHost.Run(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveSourceDingxiUsedCard => _coreSourceDingxiOperations.StartSourceDingxi(f),
        SkillProgramEffectOp.DrawSourceDingxiPile => DrawSourceDingxi(f),
        SkillProgramEffectOp.ResolveSourceHuojie => _coreSourceHuojieOperations.StartSourceHuojie(f),
        SkillProgramEffectOp.ResolveSourceXingxia => _coreSourceXingxiaOperations.StartSourceXingxia(f),
        _ => throw new InvalidOperationException("Unrecognized original Beigong instruction.")};
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24510)]
    internal bool SourceBeigongCanOffer(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveSourceDingxiUsedCard))
            return _coreSourceDingxiOperations.SourceDingxiCandidates(candidate.OwnerSeat, context).Any(id => _coreSourceDingxiOperations.SourceDingxiPlan(candidate.OwnerSeat, id, _coreSourceDingxiOperations.SourceDingxiLeft(candidate.OwnerSeat))is not null);
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveSourceHuojie))
            return SourceDingxiCount(candidate.OwnerSeat) > _coreMatchParticipantsPlayersAccess._players.Count;
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveSourceXingxia))
            return _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], p)) && _coreProgramEventHistoryOperations.ProgramEventHistory<SourceXingxiaIssuedEvent>().Where(e => e.OwnerSeat == candidate.OwnerSeat).All(e => _coreActualRoundProgressRoundNumberAccess._roundNumber - e.Round >= 2);
        return true;
    }

    internal SkillProgramStepOutcome DrawSourceDingxi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var count = SourceDingxiCount(f.OwnerSeat);
        if (count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceDingxi = new(0, 0, 0, -1, [], SourceDingxiStage.DrawChildren, Before: _coreCardMovementJournalMovementSequenceAccess._movementSequence, After: _coreCardMovementJournalMovementSequenceAccess._movementSequence), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, count, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.source-dingxi.draw"));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SourceDingxi = f.SourceDingxi!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        ContinueSourceBeigong(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24510)]
    internal bool ResumeSourceBeigong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || !HasSourceBeigongState(f))
            return false;
        ContinueSourceBeigong(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 24510)]
    internal bool ReturnSourceBeigongMovement(ProgramSkillFrame f)
    {
        if (!HasSourceBeigongState(f))
            return false;
        ContinueSourceBeigong(f.Id);
        return true;
    }

    internal void ContinueSourceBeigong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || f.AttackAttempt is not null)
            return;
        if (DrainSourceBeigong(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (f.SourceDingxi is not null)
            _coreSourceDingxiOperations.ContinueSourceDingxi(f);
        else if (f.SourceHuojie is not null)
            _coreSourceHuojieOperations.ContinueSourceHuojie(f);
        else if (f.SourceXingxia is not null)
            _coreSourceXingxiaOperations.ContinueSourceXingxia(f);
    }

    internal void FinishSourceBeigong(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SourceDingxi = null, SourceHuojie = null, SourceXingxia = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 24510)]
    internal bool SourceBeigongMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => HasSourceBeigongState(f) && effect is not null && IsSourceBeigongOp(effect.Op) && pending == f.PendingMovementContinuation && pending.BeforeCount == 0 && pending.CoverageResultBind is null && (pending.SubjectSeat == f.OwnerSeat || f.SourceXingxia is { Stage: SourceXingxiaStage.PaymentChildren } x && x.Index >= 0 && x.Index < x.Enemies.Count && pending.SubjectSeat == x.Enemies[x.Index]);
}
