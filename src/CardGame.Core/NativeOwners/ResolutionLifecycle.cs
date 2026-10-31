#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ResolutionLifecycle(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineQinglongInsertionEntries> getCoreGameEngineQinglongInsertionRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalTargetAdditionsEntries> getCoreGameEngineOriginalTargetAdditionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMaWanOlEntries> getCharacterMaWanOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries> getCharacterQianZhaoOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries> getCoreGameEngineLateFixedTargetDamageCapScopesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramRecoveryReceiptsEntries> getCoreGameEngineProgramRecoveryReceiptsRuntime) : IResolutionLifecycle
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
