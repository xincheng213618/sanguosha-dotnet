#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangAcquisitionResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreWushuangAcquisitionCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginProgramBinding(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context);
}

internal interface ICoreWushuangAcquisitionCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreWushuangAcquisitionCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreWushuangAcquisitionCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICoreWushuangAcquisitionCoreWushuangOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.IWushuangModeFeature RequireWushuangFeature { get; }

    void AdvanceWushuangShopSeat(global::CardGame.Core.WushuangIntermissionFrame frame);
    void CompleteWushuangShopAction(long frameId);
}

internal interface ICoreWushuangAcquisitionContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}
