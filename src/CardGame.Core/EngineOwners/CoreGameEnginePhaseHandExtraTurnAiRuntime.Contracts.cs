#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePhaseHandExtraTurnAiResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePhaseHandExtraTurnAiCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICorePhaseHandExtraTurnAiSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICorePhaseHandExtraTurnAiCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateCompositionForAi(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SkillProgramEffect> sourceEffects, global::CardGame.Core.ProgramAiPublicContext publicContext, global::System.Collections.Generic.IReadOnlyList<int>? publishedTargets = default, global::CardGame.Core.ProgramSkillWindowContext? windowContext = default);
}

internal interface ICorePhaseHandExtraTurnAiCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICorePhaseHandExtraTurnAiCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
