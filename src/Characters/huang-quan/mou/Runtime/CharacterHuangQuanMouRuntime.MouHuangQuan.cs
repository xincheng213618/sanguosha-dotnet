#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangQuanMouRuntime
{
    internal const string MouHqQiaodui = MouHuangQuanRuntime.MouHqQiaodui, MouHqTuicheng = MouHuangQuanRuntime.MouHqTuicheng;
    internal MouHuangQuanRuntime MouHuangQuan => _mouHuangQuanOperations.Get();

    internal bool ValidMouHq(ProgramSkillFrame f) => MouHuangQuan.Valid(f);
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedBeforeActivity, 35700)]
    internal bool MouHqFinalizedWindow(ProgramTargetCardWindowQuery q) => HasMouHuangQuan;
    internal bool TryMouHqOriginalUse(int owner, ProgramSkillWindowContext? context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } info } || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != info.ParentCardUseFrameId || CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) != context.Window || _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } u || a.ActionId != info.CardActionId || a.ActionId != w.Action.ActionId || a.ActorSeat != u.SourceSeat || a.EffectiveKind != u.CardKind || !w.Action.TargetSeats.SequenceEqual(u.TargetSeats) || !(a.ActorSeat == owner ? a.EffectiveDesignatedTargetSeats.Any(t => t != owner) : a.EffectiveDesignatedTargetSeats.Contains(owner)) || w.CompletedResponseReturn is not null)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == w.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != u.Id)
            return false;
        use = u;
        window = w;
        return true;
    }

    internal bool MouHqParent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null || f.ActivationId != f.TriggerId || f.WindowContext is not { } context || context.OwnerSeat != f.OwnerSeat || f.InstructionIndex != 1 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || _resolutionStackOperations._resolutionStack[at - 1].Id != context.ParentFrameId)
            return false;
        if (f.SkillId == MouHqQiaodui && TryMouHqOriginalUse(f.OwnerSeat, context, out _, out var w) && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count)
        {
            var c = w.Candidates[w.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        return f.SkillId == MouHqTuicheng && _resolutionStackOperations._resolutionStack[at - 1] is CardsMovedTriggerWindowFrame moved && moved.CandidateIndex >= 0 && moved.CandidateIndex < moved.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, moved.Candidates[moved.CandidateIndex]) && context.MouHqGift is { } g && MouHqGiftsForBatch(moved.Batch, f.OwnerSeat).Any(x => x.Id == g.Id);
    }
}
