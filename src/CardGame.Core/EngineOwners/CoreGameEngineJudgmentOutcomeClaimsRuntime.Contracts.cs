#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreJudgmentOutcomeClaimsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreJudgmentOutcomeClaimsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetJudgmentCard(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreJudgmentOutcomeClaimsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreJudgmentOutcomeClaimsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreJudgmentOutcomeClaimsCoreJudgmentOutcomeDamageEdgesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool JudgmentOutcomeDamageEdge(int index, global::CardGame.Core.ProgramSkillFrame root);
}

internal interface ICoreJudgmentOutcomeClaimsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidProgramJudgmentContinuation(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.ProgramSkillFrame? frame);
}

internal interface ICoreJudgmentOutcomeClaimsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreJudgmentOutcomeClaimsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreJudgmentOutcomeClaimsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.JudgmentFrame GetJudgmentFrame(long frameId);
    void ReplaceJudgmentFrame(global::CardGame.Core.JudgmentFrame frame);
}

internal interface ICoreJudgmentOutcomeClaimsCoreProgramJudgmentOutcomesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? JudgmentOutcomeInstruction(global::CardGame.Core.ProgramSkillFrame frame, int instructionIndex);
}
