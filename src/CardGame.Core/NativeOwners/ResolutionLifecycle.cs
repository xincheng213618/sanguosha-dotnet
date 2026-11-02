#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ResolutionLifecycle(Func<IActualCardUseHistoryCapture> actualUseHistory, Func<RuntimeEntries.CoreDirectedTurnCardPoliciesEntries> directedPolicies, Func<RuntimeEntries.CoreGameEngineQinglongInsertionEntries> getCoreGameEngineQinglongInsertionRuntime, Func<RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries> getCoreGameEngineOriginalTargetAdditionsRuntime, Func<RuntimeEntries.CharacterMaWanOlEntries> getCharacterMaWanOlRuntime, Func<RuntimeEntries.CharacterQianZhaoOlEntries> getCharacterQianZhaoOlRuntime, Func<RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries> getCoreGameEngineLateFixedTargetDamageCapScopesRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramRecoveryReceiptsEntries> getCoreGameEngineProgramRecoveryReceiptsRuntime) : IResolutionLifecycle
{
    public void CardUsePushed(CardUseFrame frame, CardActionContext action)
    {
        directedPolicies().IssueDirectedUse(frame);
        getCoreGameEngineQinglongInsertionRuntime().BindQinglongInsertedUse(frame);
        getCoreGameEngineOriginalTargetAdditionsRuntime().IssueOriginalTargetAdditionPolicy(frame.Id, action);
        getCharacterMaWanOlRuntime().IssueMaWanHunjiangUse(frame.Id, action);
        getCharacterQianZhaoOlRuntime().IssueWeifuUse(frame.Id, action);
    }

    public void CardUseChanged(CardUseFrame previous, CardUseFrame current) => actualUseHistory().RecordNativeChanged(current);
    public void BeforeCardUseCompleted(CardUseFrame frame) => actualUseHistory().RecordNativeChanged(frame);

    public void BeforeDamageCompleted(DamageFrame frame) => getCoreGameEngineLateFixedTargetDamageCapScopesRuntime().CaptureLateFixedTargetDamageCapScope(frame);
    public void RecoveryCompleted(RecoveryFrame frame)
    {
        var change = getCoreGameEngineHpChangeProgramsRuntime().RecordHpChange(frame.ParentFrameId, frame.SourceSeat, frame.TargetSeat, frame.HpBefore, matchParticipantsState.ReferenceToPlayers[frame.TargetSeat].Hp, HpChangeKind.Recovery);
        if (change is not null)
            getCoreGameEngineProgramRecoveryReceiptsRuntime().RecordProgramRecoveryApplication(frame, change);
    }
}
