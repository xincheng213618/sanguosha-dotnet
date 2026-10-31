#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MouDcLightningNative(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalUseLightningEntries> getCoreGameEngineOriginalUseLightningRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IMouDcLightningNative
{
    public ProgramSkillFrame BindSettlement(ProgramSkillFrame child, long judgment) => getCoreGameEngineOriginalUseLightningRuntime().BindNativeDelayedAttackChild(child, judgment);
    public void MarkReward(MouDcDelayedCause cause, long dying)
    {
        if (getFrameStore().OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == cause.JudgmentId)is not { OriginalUseLightning: { } a } j || a.Id != cause.AttachmentId || a.Origin.UseId != cause.OriginalUseId || a.Origin.Source != cause.Source || a.Origin.GameplayHash != cause.GameplayHash || j.MouDongChengReward is not null)
            throw new InvalidOperationException("Only the first causal native Lightning dying may earn its original whole-Use reward.");
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(j.Id, j with { MouDongChengReward = new(cause, dying, actualTurnProgressState.ReferenceToTurnNumber) });
        matchParticipantsState.ReferenceToPlayers[cause.Source.OwnerSeat].MouDongChengDisabledTurn = actualTurnProgressState.ReferenceToTurnNumber;
    }

    public void ResumeCleanup(long judgment) => getCoreGameEngineOriginalUseLightningRuntime().ResumeNativeDelayedAttackCompletion(judgment);
    public void MarkSettlement(MouDcSettlement s)
    {
        if (getFrameStore().OfType<JudgmentFrame>().SingleOrDefault(j => j.Id == s.JudgmentId)is not { NativeDelayedAttackCompletion: not null } j)
            throw new InvalidOperationException("Settlement must retain its complete native delayed return.");
        if (s.Reward)
        {
            if (j.MouDongChengReward is not { Settled: false } reward || reward.DyingId != s.DyingId)
                throw new InvalidOperationException("A delayed reward cannot settle twice.");
            getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(j.Id, j with { MouDongChengReward = reward with { Settled = true } });
        }
    }
}
