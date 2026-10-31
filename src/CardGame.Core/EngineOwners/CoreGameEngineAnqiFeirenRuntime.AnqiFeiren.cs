#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAnqiFeirenRuntime
{
    internal const string AnqiFeirenSkill = "shanhe:anqi-feiren";
    internal bool HasAnqiFeirenCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnqiFeiren);

    internal Card? ActiveAnqiFeiren(int owner, string skillId, string instanceId) => skillId != AnqiFeirenSkill ? null : _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[owner]).SingleOrDefault(card => card.Kind == CardKind.AnqiFeiren && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, skillId, instanceId, card));
    internal string[] AnqiFeirenCandidates(int target) => _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[target]).ActiveGrants.Where(grant => !grant.IsModeRule && !_contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked)).Select(grant => grant.SkillId).Where(id => id != "standard:none").Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
    internal bool HasSpentAnqiFeiren(int owner, int target) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnqiFeirenSuppressedEvent>().Any(e => e.Source.OwnerSeat == owner && e.TargetSeat == target && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5781, SkillProgramEffectOp.ResolveAnqiFeiren)]
    internal bool CanOfferAnqiFeiren(ProgramTriggerLegalityContext gate) => ActiveAnqiFeiren(gate.OwnerSeat, gate.SkillId, gate.Candidate.SkillInstanceId)is not null && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && gate.WindowContext is { Window: SkillProgramTriggerWindow.BeforeDamageApplied, SourceSeat: { } source, TargetSeat: { } target, Amount: > 0 } && source == gate.OwnerSeat && source != target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false } attack && attack.SourceSeat == source && attack.TargetSeat == target && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().SingleOrDefault(w => w.Id == gate.Context.ParentFrameId)is { Prevented: false, Amount: > 0 } && !HasSpentAnqiFeiren(source, target) && AnqiFeirenCandidates(target).Length > 0;
    internal SkillProgramStepOutcome ResolveAnqiFeiren(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } target, Amount: > 0 } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || window.Prevented || window.SourceSeat != f.OwnerSeat || window.TargetSeat != target || window.Amount <= 0 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } damage || damage.SourceSeat != f.OwnerSeat || damage.TargetSeat != target || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber < 1 || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || ActiveAnqiFeiren(f.OwnerSeat, f.SkillId, f.SkillInstanceId)is not { } weapon || HasSpentAnqiFeiren(f.OwnerSeat, target))
            throw new InvalidOperationException("Anqi Feiren lost its actual equipped source, positive native damage window or per-target actual-turn opportunity.");
        var candidates = AnqiFeirenCandidates(target);
        if (candidates.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var index = _randomOperations._random.Next(candidates.Length);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnqiFeirenSuppressedEvent(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, window.Id, window.ParentFrameId, weapon.Id, target, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat, candidates[index], index, candidates.Length));
        _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsAnqiFeirenGrantQualified(CharacterState owner, SkillGrant grant) => !HasAnqiFeirenCapability || grant.IsModeRule || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked) || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnqiFeirenSuppressedEvent>().Any(e => e.TargetSeat == owner.Seat && e.SuppressedSkillId == grant.SkillId && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnqiFeirenExpiredEvent>().Any(expired => expired.SuppressionFrameId == e.FrameId));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 5781)]
    internal void ExpireAnqiFeiren(TurnEndedEvent turn)
    {
        if (!HasAnqiFeirenCapability)
            return;
        foreach (var suppressed in _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnqiFeirenSuppressedEvent>().Where(e => e.TargetSeat == turn.ActorSeat && turn.TurnNumber >= e.TurnNumber && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<AnqiFeirenExpiredEvent>().Any(x => x.SuppressionFrameId == e.FrameId)).ToArray())
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnqiFeirenExpiredEvent(suppressed.FrameId, suppressed.TargetSeat, turn.TurnNumber));
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        }
    }

    SkillProgramStepOutcome IAnqiFeirenHost.ResolveAnqiFeiren(ProgramSkillFrame frame) => ResolveAnqiFeiren(frame);
}
