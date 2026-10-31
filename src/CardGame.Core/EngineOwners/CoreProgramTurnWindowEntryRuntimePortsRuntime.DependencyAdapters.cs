#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCharacterZhangXunAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCharacterZhangXunAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangXunAnniversaryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedAnniversaryZhangXunPlayEndingCandidates(global::CardGame.Core.CharacterState phaseActor) => this._targetFactory().IssuedAnniversaryZhangXunPlayEndingCandidates(phaseActor);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCharacterLuZhiWeiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCharacterLuZhiWeiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiWeiOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedQingzhongEndingCandidates(global::CardGame.Core.CharacterState owner) => this._targetFactory().IssuedQingzhongEndingCandidates(owner);
    public global::CardGame.Core.QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureQingzhongEndingPhase(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCorePhaseHandSeizureAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCorePhaseHandSeizureOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedPhaseHandDebtCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrence) => this._targetFactory().IssuedPhaseHandDebtCandidates(owner, window, occurrence);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCharacterZhangShiPingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCharacterZhangShiPingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangShiPingOlRuntime> _targetFactory = targetFactory;
    public bool HasHongjiCapability { get => this._targetFactory().HasHongjiCapability; }

    public bool HasOutstandingHongjiPhase(global::CardGame.Core.CharacterState recipient, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().HasOutstandingHongjiPhase(recipient, window);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(global::CardGame.Core.CharacterState turnOwner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> candidates) => this._targetFactory().IncludeHongjiPhaseCandidates(turnOwner, window, candidates);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCorePhaseNamePredictionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCorePhaseNamePredictionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePhaseNamePredictionRuntime> _targetFactory = targetFactory;
    public void AppendPhaseNamePredictionCandidates(global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates) => this._targetFactory().AppendPhaseNamePredictionCandidates(candidates);
    public bool TracksPhaseNamePredictions { get => this._targetFactory().TracksPhaseNamePredictions; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCharacterLvQianOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCharacterLvQianOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvQianOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedWeiluEndingCandidates(global::CardGame.Core.CharacterState owner) => this._targetFactory().IssuedWeiluEndingCandidates(owner);
    public global::CardGame.Core.WeiluPhaseKey? CaptureWeiluEndingPhase(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureWeiluEndingPhase(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCoreZhuijiProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCoreZhuijiProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedZhuijiEndingCandidates(global::CardGame.Core.CharacterState owner) => this._targetFactory().IssuedZhuijiEndingCandidates(owner);
    public global::CardGame.Core.ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureZhuijiEndingPhase(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterCaoHongMouRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCharacterCaoHongMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoHongMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCharacterCaoHongMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoHongMouRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedMouCaoHongPlayEndingCandidates(global::CardGame.Core.CharacterState owner) => this._targetFactory().IssuedMouCaoHongPlayEndingCandidates(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCharacterWangKuangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCharacterWangKuangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangKuangOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedWangKuangRenxiaEndingCandidates(global::CardGame.Core.CharacterState owner) => this._targetFactory().IssuedWangKuangRenxiaEndingCandidates(owner);
    public global::CardGame.Core.WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(global::CardGame.Core.CharacterState owner) => this._targetFactory().CaptureWangKuangRenxiaEndingPhase(owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime))]
internal sealed class CoreProgramTurnWindowEntryRuntimePortsCharacterLiuBaOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramTurnWindowEntryRuntimePortsCharacterLiuBaOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedTongduPlayEndingCandidates(global::CardGame.Core.CharacterState owner) => this._targetFactory().IssuedTongduPlayEndingCandidates(owner);
}
