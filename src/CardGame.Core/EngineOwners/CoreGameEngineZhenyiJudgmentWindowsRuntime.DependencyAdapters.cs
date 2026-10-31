#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().GetProgramTrigger(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IndexedSkillProgramTrigger> EnabledUniqueProgramTriggers(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window) => this._targetFactory().EnabledUniqueProgramTriggers(player, window);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCoreNativeJudgmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCoreNativeJudgmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeJudgmentRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId) => this._targetFactory().GetJudgmentFrame(frameId);
    public void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().ReplaceJudgmentFrame(frame);
    public bool? FinalizeJudgment(global::CardGame.Core.JudgmentFrame pending) => this._targetFactory().FinalizeJudgment(pending);
    public void ResumeCompletedJudgment(global::CardGame.Core.GameEngine.JudgmentCompletionReceipt receipt) => this._targetFactory().ResumeCompletedJudgment(receipt);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCoreResolutionIdentityBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCoreResolutionIdentityBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreResolutionIdentityBoundaryRuntime> _targetFactory = targetFactory;
    public long _resolutionSequence { get => this._targetFactory()._resolutionSequence; set => this._targetFactory()._resolutionSequence = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCharacterXiaoQiaoBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCharacterXiaoQiaoBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoBoundaryMobileRuntime> _targetFactory = targetFactory;
    public bool IsExactBoundaryXiaoQiaoBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame w, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().IsExactBoundaryXiaoQiaoBeforeResultProgramChild(w, f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCoreNativeDamageHandlesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCoreNativeDamageHandlesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame) => this._targetFactory().GetJudgmentCard(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCoreHellDizangProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCoreHellDizangProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellDizangProgramsRuntime> _targetFactory = targetFactory;
    public bool IsExactHellDayuanBeforeResultProgramChild(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.ProgramSkillFrame child) => this._targetFactory().IsExactHellDayuanBeforeResultProgramChild(window, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCharacterZhangQiYingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCharacterZhangQiYingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangQiYingOlRuntime> _targetFactory = targetFactory;
    public int EffectiveZhenyiJudgmentRank(global::CardGame.Core.JudgmentFrame frame, int fallback) => this._targetFactory().EffectiveZhenyiJudgmentRank(frame, fallback);
    public global::CardGame.Core.Suit EffectiveZhenyiJudgmentSuit(global::CardGame.Core.JudgmentFrame frame, global::CardGame.Core.Suit fallback) => this._targetFactory().EffectiveZhenyiJudgmentSuit(frame, fallback);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreZhenyiJudgmentWindowsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreZhenyiJudgmentWindowsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => this._targetFactory().PushRuntimeFrame(frame);
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
    public void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => this._targetFactory().PopResolutionFrame(frameId, expectedKind);
}
