namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheGuideSkillsHost(GameEngine engine) : IShanheGuideSkillsHost
    { public SkillProgramStepOutcome ResolveGuideSkill(ProgramSkillFrame frame, SkillProgramEffect effect) => engine.ResolveShanheGuideSkill(frame, effect); }
    private bool IsShanheFriendlySeat(int owner, int seat) => IsValidPlayerSeat(seat) &&
        (owner == seat || OtherRevealedPublicTeamAlly(_players[owner], _players[seat]));
    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 5409)]
    private IEnumerable<(CardUseEffectSource Source, int Amount)> ShanheBuwuDamageBonuses(IDamageAttempt attack)
    {
        if (attack.IsSourceLess || !IsValidPlayerSeat(attack.SourceSeat) || !IsValidPlayerSeat(attack.TargetSeat) ||
            !_contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheBuwu)) return [];
        // Use the shared additional-damage layer, before native armor and skill caps.
        return _players.Where(owner => owner.IsAlive && IsShanheFriendlySeat(owner.Seat, attack.SourceSeat) &&
            PublicTeamEnemy(owner.Seat, _players[attack.TargetSeat])).SelectMany(owner => GetSkillBindingShard(owner).ProgramInstances
                .Where(instance => instance.SkillId == "shanhe:buwu-3365")
                .Select(instance => (new CardUseEffectSource(instance.SkillId, "allied-damage", owner.Seat, instance.SkillInstanceId), 1)));
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5409)]
    private bool CanOfferShanheBuwu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheBuwu)) return true;
        if (candidate.SkillId != "shanhe:buwu-3365" || context.SourceSeat is not { } source || context.TargetSeat is not { } target ||
            !IsShanheFriendlySeat(candidate.OwnerSeat, source) || !IsValidPlayerSeat(target)) return false;
        return trigger.Effects[0].StateId == "before"
            ? CurrentDamageAttempt is { IsSourceLess: false } && IsShanheFriendlySeat(candidate.OwnerSeat, target)
            : context.Amount > 0 && PublicTeamEnemy(candidate.OwnerSeat, _players[target]);
    }
    private SkillProgramStepOutcome ResolveShanheGuideSkill(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (effect.Op == SkillProgramEffectOp.RecoverShanheShuanghun)
        {
            if (frame.SkillId != "shanhe:shuanghun-3366" || frame.WindowContext?.Window != SkillProgramTriggerWindow.SelfDyingResponse)
                throw new InvalidOperationException("Shuanghun recovery requires its owning dying binding.");
            RecoverProgramTargetTo(frame.Id, frame.OwnerSeat, frame.OwnerSeat, SkillProgramNumberExpression.IntegerConstant, _players[frame.OwnerSeat].MaxHp, true);
            return SkillProgramStepOutcome.Continue;
        }
        if (effect.Op == SkillProgramEffectOp.RestoreShanheShuanghunUse)
        {
            if (frame.SkillId != "shanhe:shuanghun-3366" || frame.WindowContext?.Window != SkillProgramTriggerWindow.SelfDyingResponse)
                throw new InvalidOperationException("Shuanghun's usage return requires its paid original dying binding.");
            if (_players.Any(player => player.IsAlive && OtherRevealedPublicTeamAlly(_players[frame.OwnerSeat], player)))
            {
                if (!_skillRuntimeState.ClearUsage(frame.OwnerSeat, frame.SkillId, "named-group:shuanghun", SkillUsageScope.Game))
                    throw new InvalidOperationException("Shuanghun cannot restore a use which was not consumed by this binding.");
                AdvanceEventRulesAndQueueFact(new ShanheShuanghunUseRestoredEvent(frame.Id, frame.OwnerSeat));
            }
            return SkillProgramStepOutcome.Continue;
        }
        if (frame.SkillId != "shanhe:buwu-3365" || frame.WindowContext is not { SourceSeat: { } source, TargetSeat: { } target } context ||
            !IsShanheFriendlySeat(frame.OwnerSeat, source)) throw new InvalidOperationException("Buwu requires its actual allied damage participants.");
        if (effect.StateId == "before")
        {
            if (_resolutionStack.Count < 2 || _resolutionStack[^2] is not BeforeDamageProgramWindowFrame window ||
                window.Id != context.ParentFrameId || window.SourceSeat != source || window.TargetSeat != target || window.Prevented ||
                CurrentDamageAttempt is not { IsSourceLess: false } attack || attack.SourceSeat != source || attack.TargetSeat != target)
                throw new InvalidOperationException("Buwu lost its live finalized damage attempt.");
            if (!IsShanheFriendlySeat(frame.OwnerSeat, target)) throw new InvalidOperationException("Buwu's prevention requires two actual friendly participants.");
            PreventProgramCurrentDamage(frame);
            return SkillProgramStepOutcome.Continue;
        }
        if (context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.Amount <= 0 || !PublicTeamEnemy(frame.OwnerSeat, _players[target]))
            throw new InvalidOperationException("Buwu's draw requires the final applied enemy damage receipt.");
        AdvanceEventRulesAndQueueFact(new ShanheBuwuDrawIssuedEvent(frame.Id, frame.OwnerSeat, context.DamageFrameId ?? context.ParentFrameId, context.Amount));
        return BeginOperationProgramDraw(frame, frame.OwnerSeat, context.Amount, new("skill-program.shanhe-buwu.draw"));
    }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheBuwu)]
    private void ReturnShanheBuwuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var issued = ProgramEventHistory<ShanheBuwuDrawIssuedEvent>().Single(item => item.FrameId == frame.Id);
        if (frame.ShanheBuwuDrawOutcome is not null || frame.NativeDraw is not null ||
            outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != issued.Amount)
            throw new InvalidOperationException("Buwu lost its original actual-damage draw return.");
        ReplaceRuntimeTop(frame = frame with { ShanheBuwuDrawOutcome = outcome });
        if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue) AdvanceRuntimeProgram(frame.Id);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 5409)]
    private bool IsShanheBuwuDrawMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement) =>
        effect is { Op: SkillProgramEffectOp.ResolveShanheBuwu, StateId: "after" } && frame.ShanheBuwuDrawOutcome is not null &&
        movement.SubjectSeat == frame.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Resume, 5409)]
    private bool ResumeShanheBuwuDraw(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheBuwuDrawOutcome: not null } frame || frame.Id != frameId) return false;
        ReplaceRuntimeTop(frame with { ShanheBuwuDrawOutcome = null, PendingMovementContinuation = null }); AdvanceRuntimeProgram(frame.Id); return true;
    }
}
