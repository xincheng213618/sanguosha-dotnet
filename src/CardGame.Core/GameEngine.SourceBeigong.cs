using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IReadOnlyList<PublicPersistentPileSource> SourceDingxiPiles(int owner) => PublicPileSources(owner).Where(p => p.SkillId == SourceBeigongIds.Dingxi).ToArray();
    private int SourceDingxiCount(int owner) => SourceDingxiPiles(owner).Sum(p => PublicPileCards(p).Count);
    private static bool IsSourceBeigongOp(SkillProgramEffectOp op) => op is SkillProgramEffectOp.ResolveSourceDingxiUsedCard or
        SkillProgramEffectOp.DrawSourceDingxiPile or SkillProgramEffectOp.ResolveSourceHuojie or SkillProgramEffectOp.ResolveSourceXingxia;
    private static bool HasSourceBeigongState(ProgramSkillFrame f) => f.SourceDingxi is not null || f.SourceHuojie is not null || f.SourceXingxia is not null;
    private bool DrainSourceBeigong(long id) => TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) ||
        TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) ||
        TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || TryBeginAdvancedSkillsChanged(id) || TryBeginCardsMovedProgramWindow(id);

    [ProgramCapability]
    private sealed class SourceBeigongHost(GameEngine engine) : ISourceBeigongHost
    {
        public SkillProgramStepOutcome Run(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
        {
            SkillProgramEffectOp.ResolveSourceDingxiUsedCard => engine.StartSourceDingxi(f),
            SkillProgramEffectOp.DrawSourceDingxiPile => engine.DrawSourceDingxi(f),
            SkillProgramEffectOp.ResolveSourceHuojie => engine.StartSourceHuojie(f),
            SkillProgramEffectOp.ResolveSourceXingxia => engine.StartSourceXingxia(f),
            _ => throw new InvalidOperationException("Unrecognized original Beigong instruction.")
        };
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24510)]
    private bool SourceBeigongCanOffer(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveSourceDingxiUsedCard))
            return SourceDingxiCandidates(candidate.OwnerSeat, context).Any(id => SourceDingxiPlan(candidate.OwnerSeat, id, SourceDingxiLeft(candidate.OwnerSeat)) is not null);
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveSourceHuojie)) return SourceDingxiCount(candidate.OwnerSeat) > _players.Count;
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveSourceXingxia))
            return _players.Any(p => p.IsAlive && OtherRevealedPublicTeamAlly(_players[candidate.OwnerSeat], p)) &&
                ProgramEventHistory<SourceXingxiaIssuedEvent>().Where(e => e.OwnerSeat == candidate.OwnerSeat).All(e => _roundNumber - e.Round >= 2);
        return true;
    }

    private SkillProgramStepOutcome DrawSourceDingxi(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id); var count = SourceDingxiCount(f.OwnerSeat);
        if (count == 0) return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { SourceDingxi = new(0, 0, 0, -1, [], SourceDingxiStage.DrawChildren, Before: _movementSequence, After: _movementSequence),
            PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        DrawProgramCards(f.Id, f.OwnerSeat, count, null, null, SkillProgramCardSetVisibility.Private, new("skill-program.source-dingxi.draw"));
        f = GetActiveProgramFrame(f.Id); ReplaceRuntimeTop(f with { SourceDingxi = f.SourceDingxi! with { After = _movementSequence } });
        ContinueSourceBeigong(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24510)]
    private bool ResumeSourceBeigong(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame f || f.Id != id || !HasSourceBeigongState(f)) return false;
        ContinueSourceBeigong(id); return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 24510)]
    private bool ReturnSourceBeigongMovement(ProgramSkillFrame f)
    {
        if (!HasSourceBeigongState(f)) return false;
        ContinueSourceBeigong(f.Id); return true;
    }
    private void ContinueSourceBeigong(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame f || f.Id != id || _pendingDecision is not null || f.AttackAttempt is not null) return;
        if (DrainSourceBeigong(id)) return;
        f = GetActiveProgramFrame(id);
        if (f.PendingMovementContinuation is not null) ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (f.SourceDingxi is not null) ContinueSourceDingxi(f);
        else if (f.SourceHuojie is not null) ContinueSourceHuojie(f);
        else if (f.SourceXingxia is not null) ContinueSourceXingxia(f);
    }
    private void FinishSourceBeigong(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(f = f with { SourceDingxi = null, SourceHuojie = null, SourceXingxia = null, PendingMovementContinuation = null });
        FinishProgramSkill(f, true);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 24510)]
    private bool SourceBeigongMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) =>
        HasSourceBeigongState(f) && effect is not null && IsSourceBeigongOp(effect.Op) && pending == f.PendingMovementContinuation &&
        pending.BeforeCount == 0 && pending.CoverageResultBind is null &&
        (pending.SubjectSeat == f.OwnerSeat || f.SourceXingxia is { Stage: SourceXingxiaStage.PaymentChildren } x &&
            x.Index >= 0 && x.Index < x.Enemies.Count && pending.SubjectSeat == x.Enemies[x.Index]);
}
