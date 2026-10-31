#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFenAnniversaryRuntime
{
    internal void AppendDagongCheIssuedDamageCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window, List<DamageTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.AfterDamageApplied || attack.IsSourceLess || !attack.DamageWasApplied || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { DagongCheSlash: { } o } use || !_coreMatchParticipantsPlayersAccess._players[o.Source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive || !_coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(o.Source.OwnerSeat, attack.TargetSeat) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.FrameId)is not { } parent || !ExactDagongCheUse(use, parent))
            return;
        var t = _contentRegistryOperations._contentRegistry.GetSkill(ZhangFenComposition.Cart).Program!.Triggers.Single(t => t.Id == "cart-slash-discard");
        result.RemoveAll(c => c.OwnerSeat == o.Source.OwnerSeat && c.ProgramId == ZhangFenComposition.Cart && c.ProgramTriggerId == t.Id);
        result.Add(new(o.Source.OwnerSeat, $"dagong-che:{(use.Id)}:{(attack.TargetSeat)}", t.Priority, false, ZhangFenComposition.Cart, t.Id, o.Source.SkillInstanceId!, o.Hash, 0));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 43500)]
    internal bool IsIssuedDagongCheDamageCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != ZhangFenComposition.Cart || c.BindingId != "cart-slash-discard" || c.OccurrenceIndex != 0 || w.OccurrenceIndex != 0 || w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.Amount is not > 0 || w.DamageFrameId is not { } damageId || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is not { } parent || parent.ParentFrameId != damageId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex].ToProgramCandidate() != c || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damageId)is not { } damage || damage.TargetSeat != w.TargetSeat || damage.Amount != w.Amount || _coreCardUseLifecycleOperations.LifecycleCardUse(damage.ParentFrameId)is not { DagongCheSlash: { } o } use || c.OwnerSeat != o.Source.OwnerSeat || w.OwnerSeat != c.OwnerSeat || w.SourceSeat != c.OwnerSeat || c.SkillInstanceId != o.Source.SkillInstanceId || c.GameplayHash != o.Hash || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[damage.TargetSeat].IsAlive || !_coreForeignEquipmentDiscardPoliciesOperations.HasDiscardableHeBy(c.OwnerSeat, damage.TargetSeat) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.FrameId)is not { } f || !ExactDagongCheUse(use, f))
            return false;
        return !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangFenStartedEvent>().Any(e => e.Op == SkillProgramEffectOp.ResolveAnniversaryDagongCheAttack && e.ParentId == parent.Id);
    }

    internal bool IsUnbegunIssuedDagongCheDamage(ProgramSkillFrame f) => f.ZhangFen is null && f.InstructionIndex == 0 && f.WindowContext is { } w && IsIssuedDagongCheDamageCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, OccurrenceIndex: w.OccurrenceIndex), w);
    internal bool HasBoundDagongCheDiscardOrigin(ProgramSkillFrame f) => f.ZhangFen is { } r && f.WindowContext is { DamageFrameId: { } damageId } w && _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damageId)is { } damage && damage.SourceSeat == f.OwnerSeat && damage.TargetSeat == r.TargetSeat && damage.TargetSeat == w.TargetSeat && damage.Amount == w.Amount && _coreCardUseLifecycleOperations.LifecycleCardUse(damage.ParentFrameId)is { DagongCheSlash: { } o } use && o.CartId == r.CartId && o.Source.OwnerSeat == f.OwnerSeat && o.Source.SkillInstanceId == f.SkillInstanceId && o.Hash == r.Hash && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(p => p.Id == o.FrameId)is { } producer && ExactDagongCheUse(use, producer);
}
