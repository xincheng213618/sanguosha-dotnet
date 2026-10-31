#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheGuideSkillsRuntime
{
    SkillProgramStepOutcome IShanheGuideSkillsHost.ResolveGuideSkill(ProgramSkillFrame frame, SkillProgramEffect effect) => ResolveShanheGuideSkill(frame, effect);
    internal bool IsShanheFriendlySeat(int owner, int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && (owner == seat || _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[owner], _coreMatchParticipantsPlayersAccess._players[seat]));
    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 5409)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> ShanheBuwuDamageBonuses(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsSourceLess || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheBuwu))
            return[];
        // Use the shared additional-damage layer, before native armor and skill caps.
        return _coreMatchParticipantsPlayersAccess._players.Where(owner => owner.IsAlive && IsShanheFriendlySeat(owner.Seat, attack.SourceSeat) && _coreTeamActivityProgramsOperations.PublicTeamEnemy(owner.Seat, _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat])).SelectMany(owner => _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(instance => instance.SkillId == "shanhe:buwu-3365").Select(instance => (new CardUseEffectSource(instance.SkillId, "allied-damage", owner.Seat, instance.SkillInstanceId), 1)));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5409)]
    internal bool CanOfferShanheBuwu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheBuwu))
            return true;
        if (candidate.SkillId != "shanhe:buwu-3365" || context.SourceSeat is not { } source || context.TargetSeat is not { } target || !IsShanheFriendlySeat(candidate.OwnerSeat, source) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target))
            return false;
        return trigger.Effects[0].StateId == "before" ? _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false } && IsShanheFriendlySeat(candidate.OwnerSeat, target) : context.Amount > 0 && _coreTeamActivityProgramsOperations.PublicTeamEnemy(candidate.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[target]);
    }

    internal SkillProgramStepOutcome ResolveShanheGuideSkill(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (effect.Op == SkillProgramEffectOp.RecoverShanheShuanghun)
        {
            if (frame.SkillId != "shanhe:shuanghun-3366" || frame.WindowContext?.Window != SkillProgramTriggerWindow.SelfDyingResponse)
                throw new InvalidOperationException("Shuanghun recovery requires its owning dying binding.");
            _coreProgramLifecycleOperations.RecoverProgramTargetTo(frame.Id, frame.OwnerSeat, frame.OwnerSeat, SkillProgramNumberExpression.IntegerConstant, _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp, true);
            return SkillProgramStepOutcome.Continue;
        }

        if (effect.Op == SkillProgramEffectOp.RestoreShanheShuanghunUse)
        {
            if (frame.SkillId != "shanhe:shuanghun-3366" || frame.WindowContext?.Window != SkillProgramTriggerWindow.SelfDyingResponse)
                throw new InvalidOperationException("Shuanghun's usage return requires its paid original dying binding.");
            if (_coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && _coreTeamActivityProgramsOperations.OtherRevealedPublicTeamAlly(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], player)))
            {
                if (!_skillRuntimeStateOperations._skillRuntimeState.ClearUsage(frame.OwnerSeat, frame.SkillId, "named-group:shuanghun", SkillUsageScope.Game))
                    throw new InvalidOperationException("Shuanghun cannot restore a use which was not consumed by this binding.");
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheShuanghunUseRestoredEvent(frame.Id, frame.OwnerSeat));
            }

            return SkillProgramStepOutcome.Continue;
        }

        if (frame.SkillId != "shanhe:buwu-3365" || frame.WindowContext is not { SourceSeat: { } source, TargetSeat: { } target } context || !IsShanheFriendlySeat(frame.OwnerSeat, source))
            throw new InvalidOperationException("Buwu requires its actual allied damage participants.");
        if (effect.StateId == "before")
        {
            if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || window.SourceSeat != source || window.TargetSeat != target || window.Prevented || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } attack || attack.SourceSeat != source || attack.TargetSeat != target)
                throw new InvalidOperationException("Buwu lost its live finalized damage attempt.");
            if (!IsShanheFriendlySeat(frame.OwnerSeat, target))
                throw new InvalidOperationException("Buwu's prevention requires two actual friendly participants.");
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(frame);
            return SkillProgramStepOutcome.Continue;
        }

        if (context.Window != SkillProgramTriggerWindow.AfterDamageApplied || context.Amount <= 0 || !_coreTeamActivityProgramsOperations.PublicTeamEnemy(frame.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[target]))
            throw new InvalidOperationException("Buwu's draw requires the final applied enemy damage receipt.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheBuwuDrawIssuedEvent(frame.Id, frame.OwnerSeat, context.DamageFrameId ?? context.ParentFrameId, context.Amount));
        return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, context.Amount, new("skill-program.shanhe-buwu.draw"));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheBuwu)]
    internal void ReturnShanheBuwuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheBuwuDrawIssuedEvent>().Single(item => item.FrameId == frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShanheBuwuDrawOutcome = outcome });
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 5409)]
    internal bool ResumeShanheBuwuDraw(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheBuwuDrawOutcome: not null } frame || frame.Id != frameId)
            return false;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ShanheBuwuDrawOutcome = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }
}
