#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterSunHaoMobileCorePrivateGeneralLibrariesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlySet<string> PrivateGeneralLibrarySuppressionInputs(global::CardGame.Core.CharacterState owner);
    bool LibraryBaseGrantLive(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    bool IsGeneralLibraryGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICharacterSunHaoMobileCoreMatchSetupDefinitionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterSunHaoMobileCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterSunHaoMobileCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterSunHaoMobileCharacterSunHaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SunHaoActualTurnOpen { get; }

    bool DrainSunHao(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterSunHaoMobileCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterSunHaoMobileResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterSunHaoMobileCoreCurrentTurnSkillSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentTurnSkillGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICharacterSunHaoMobileCoreHpConditionalSkillGrantsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHpConditionalGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICharacterSunHaoMobileContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterSunHaoMobileKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterSunHaoMobileCharacterHeQiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsEquipmentSuitGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
}

internal interface ICharacterSunHaoMobileCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterSunHaoMobileCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterSunHaoMobileCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterSunHaoMobileCoreLordSkillProjectionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProjectedGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant);
    bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role);
}
