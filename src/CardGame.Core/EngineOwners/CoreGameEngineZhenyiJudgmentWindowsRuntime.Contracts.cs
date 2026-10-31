#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreZhenyiJudgmentWindowsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreZhenyiJudgmentWindowsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreZhenyiJudgmentWindowsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window);
}

internal interface ICoreZhenyiJudgmentWindowsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId);
    void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame);
    bool? FinalizeJudgment(global::CardGame.Core.JudgmentFrame pending);
    void ResumeCompletedJudgment(global::CardGame.Core.GameEngine.JudgmentCompletionReceipt receipt);
}

internal interface ICoreZhenyiJudgmentWindowsCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreZhenyiJudgmentWindowsCharacterXiaoQiaoBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactBoundaryXiaoQiaoBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame w, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreZhenyiJudgmentWindowsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreZhenyiJudgmentWindowsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreZhenyiJudgmentWindowsCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreZhenyiJudgmentWindowsCoreHellDizangProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsExactHellDayuanBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.ProgramSkillFrame child);
}

internal interface ICoreZhenyiJudgmentWindowsCharacterZhangQiYingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int EffectiveZhenyiJudgmentRank(global::CardGame.Core.JudgmentFrame frame, int fallback);
    global::CardGame.Core.Suit EffectiveZhenyiJudgmentSuit(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit fallback);
}

internal interface ICoreZhenyiJudgmentWindowsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
}
