#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramTurnWindowEntryRuntimePortsProgramPipelineHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ProgramPipelineHooks _programPipelineHooks { get; }
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCharacterZhangXunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedAnniversaryZhangXunPlayEndingCandidates(global::CardGame.Core.CharacterState phaseActor);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCharacterLuZhiWeiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedQingzhongEndingCandidates(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.QingzhongPlayPhaseKey? CaptureQingzhongEndingPhase(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCorePhaseHandSeizureOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedPhaseHandDebtCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrence);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCharacterZhangShiPingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHongjiCapability { get; }

    bool HasOutstandingHongjiPhase(global::CardGame.Core.CharacterState recipient, global::CardGame.Core.SkillProgramTriggerWindow window);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(global::CardGame.Core.CharacterState turnOwner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> candidates);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCorePhaseNamePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AppendPhaseNamePredictionCandidates(global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates);
    bool TracksPhaseNamePredictions { get; }
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCharacterLvQianOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedWeiluEndingCandidates(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.WeiluPhaseKey? CaptureWeiluEndingPhase(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCoreZhuijiProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedZhuijiEndingCandidates(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.ZhuijiPlayPhaseKey? CaptureZhuijiEndingPhase(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCharacterCaoHongMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedMouCaoHongPlayEndingCandidates(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCharacterWangKuangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedWangKuangRenxiaEndingCandidates(global::CardGame.Core.CharacterState owner);
    global::CardGame.Core.WangKuangRenxiaPhaseKey? CaptureWangKuangRenxiaEndingPhase(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramTurnWindowEntryRuntimePortsCharacterLiuBaOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedTongduPlayEndingCandidates(global::CardGame.Core.CharacterState owner);
}
