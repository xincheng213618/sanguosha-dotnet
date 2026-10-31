#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDcLightningFacts(Func<FrameStore> getFrameStore, Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, Func<RuntimeEntries.CharacterDongChengMouEntries> getCharacterDongChengMouRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState) : IMouDcLightningFacts
{
    public MouDcDelayedCause? DyingCause(PlayerDyingEvent fact)
    {
        if (getFrameStore().LastOrDefault()is not DyingFrame { Continuation: DyingContinuationKind.Damage } dying || dying.Id != fact.ResolutionId || getFrameStore().OfType<DamageFrame>().SingleOrDefault(f => f.Id == dying.ParentFrameId)is not { Amount: > 0 } damage || getCoreGameEngineProgramAttackAttemptRuntime().CurrentDamageAttempt is not CardAttackHandle { IsDelayedJudgmentDamage: true, IsSourceLess: true, DamageWasApplied: true } attack || attack.ResolutionId != damage.ParentFrameId || attack.TargetSeat != fact.VictimSeat || getFrameStore().OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == attack.ResolutionId)is not { OriginalUseLightning: { } a } j || !getCharacterDongChengMouRuntime().MouDcNativeLightning(j) || a.Origin.Source is not { SkillId: MouDongChengRuntime.Douwei, BindingId: "discard-damage-use" } source || getContentRegistry().Skills.GetValueOrDefault(source.SkillId)?.Program?.GameplayHash != a.Origin.GameplayHash || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouDcUseIssuedEvent>().Count(x => x.UseId == a.Origin.UseId && x.ActionId == a.Origin.ActionId && x.Owner == source.OwnerSeat && x.Kind == CardKind.Lightning) != 1 || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<CardUseFinishedEvent>().Count(x => x.ResolutionId == a.Origin.UseId) != 1)
            return null;
        return new(a.Origin.UseId, a.Origin.ActionId, source, a.Origin.GameplayHash, a.Id, j.Id, damage.Id, fact.VictimSeat);
    }

    public MouDcSettlement? NextSettlement(long id)
    {
        if (getFrameStore().LastOrDefault()is not JudgmentFrame { NativeDelayedAttackCompletion: { } ret } j || j.Id != id || ret.ResolutionId != id || !ret.DelayedTurn || ret.ProgramFrameId is not null || !getCharacterDongChengMouRuntime().MouDcNativeLightning(j))
            return null;
        var claim = j.MouDongChengClaims?.FirstOrDefault(p => !p.Settled && matchParticipantsState.ReferenceToPlayers[p.Source.OwnerSeat].IsAlive);
        if (claim is not null)
            return new(id, claim.Source, claim.GameplayHash, false, claim.ProtectionFrameId, 0, 0);
        return j.MouDongChengReward is { Settled: false } reward && matchParticipantsState.ReferenceToPlayers[reward.Cause.Source.OwnerSeat].IsAlive ? new(id, reward.Cause.Source, reward.Cause.GameplayHash, true, 0, reward.DyingId, reward.Cause.OriginalUseId) : null;
    }

    public bool SettlementParent(ProgramSkillFrame f)
    {
        var index = getFrameStore().FindIndex(x => x.Id == f.Id);
        if (f.MouDongChengSettlement is not { } s || f.ActivationId != "native-delayed-settlement" || f.TriggerId is not null || f.WindowContext is not null || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.OwnerSeat != s.OriginalSource.OwnerSeat || f.SkillId != s.OriginalSource.SkillId || f.SkillInstanceId != s.OriginalSource.SkillInstanceId || f.GameplayHash != s.OriginalHash || index < 1 || getFrameStore()[index - 1] is not JudgmentFrame { NativeDelayedAttackCompletion: { } ret } j || j.Id != s.JudgmentId || ret.ResolutionId != j.Id || !ret.DelayedTurn || ret.ProgramFrameId is not null || !getCharacterDongChengMouRuntime().MouDcNativeLightning(j) || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouDcSettlementIssuedEvent>().Count(x => x.FrameId == f.Id && x.Return == s) != 1)
            return false;
        return s.Reward ? j.MouDongChengReward is { Settled: true } reward && reward.Cause.Source == s.OriginalSource && reward.Cause.GameplayHash == s.OriginalHash && reward.DyingId == s.DyingId && reward.Cause.OriginalUseId == s.OriginalUseId && getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouDcLightningRewardEvent>().Count(x => x.Cause == reward.Cause && x.DyingId == reward.DyingId) == 1 : j.MouDongChengClaims?.Any(p => p.ProtectionFrameId == s.ProtectionId && p.Source == s.OriginalSource && p.GameplayHash == s.OriginalHash) == true;
    }
}
