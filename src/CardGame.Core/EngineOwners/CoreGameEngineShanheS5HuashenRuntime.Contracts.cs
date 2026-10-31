#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheS5HuashenResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreShanheS5HuashenCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreShanheS5HuashenContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreShanheS5HuashenCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreShanheS5HuashenKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreShanheS5HuashenCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreShanheS5HuashenCorePrivateGeneralLibrariesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PrivateGeneralLibrarySource? ResolvePrivateGeneralLibrarySource(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect e);
    bool LibrarySourceQualified(global::CardGame.Core.PrivateGeneralLibrarySource source);
    global::System.Collections.Generic.IReadOnlyList<string> PrivateGeneralDeclarableSkills(string generalId, global::CardGame.Core.PrivateGeneralLibraryPolicy policy);
    global::System.Collections.Generic.IEnumerable<string> PrivateGeneralCandidates();
    global::CardGame.Core.SkillProgramStepOutcome ExecutePrivateGeneralLibrary(global::CardGame.Core.SkillProgramEffect e, global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.PrivateGeneralLibrary DrawPrivateGeneralAvatars(global::CardGame.Core.PrivateGeneralLibrary library, int amount, global::System.Collections.Generic.IReadOnlyCollection<string>? excludedIds = default);
}

internal interface ICoreShanheS5HuashenCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}
