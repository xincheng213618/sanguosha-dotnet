#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ResolutionLifecycle(Func<RuntimeEntries.CoreGameEngineQinglongInsertionEntries> getCoreGameEngineQinglongInsertionRuntime, Func<RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries> getCoreGameEngineOriginalTargetAdditionsRuntime, Func<RuntimeEntries.CharacterMaWanOlEntries> getCharacterMaWanOlRuntime, Func<RuntimeEntries.CharacterQianZhaoOlEntries> getCharacterQianZhaoOlRuntime, Func<RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries> getCoreGameEngineLateFixedTargetDamageCapScopesRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramRecoveryReceiptsEntries> getCoreGameEngineProgramRecoveryReceiptsRuntime) : IResolutionLifecycle
{
    public void CardUsePushed(CardUseFrame frame, CardActionContext action)
    {
        getCoreGameEngineQinglongInsertionRuntime().BindQinglongInsertedUse(frame);
        getCoreGameEngineOriginalTargetAdditionsRuntime().IssueOriginalTargetAdditionPolicy(frame.Id, action);
        getCharacterMaWanOlRuntime().IssueMaWanHunjiangUse(frame.Id, action);
        getCharacterQianZhaoOlRuntime().IssueWeifuUse(frame.Id, action);
    }

    public void BeforeDamageCompleted(DamageFrame frame) => getCoreGameEngineLateFixedTargetDamageCapScopesRuntime().CaptureLateFixedTargetDamageCapScope(frame);
    public void RecoveryCompleted(RecoveryFrame frame)
    {
        var change = getCoreGameEngineHpChangeProgramsRuntime().RecordHpChange(frame.ParentFrameId, frame.SourceSeat, frame.TargetSeat, frame.HpBefore, matchParticipantsState.ReferenceToPlayers[frame.TargetSeat].Hp, HpChangeKind.Recovery);
        if (change is not null)
            getCoreGameEngineProgramRecoveryReceiptsRuntime().RecordProgramRecoveryApplication(frame, change);
    }
}
