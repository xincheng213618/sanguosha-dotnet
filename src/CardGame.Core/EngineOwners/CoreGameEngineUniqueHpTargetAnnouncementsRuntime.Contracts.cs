#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreUniqueHpTargetAnnouncementsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreUniqueHpTargetAnnouncementsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardActionContext action);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts);
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreSameTypeActualUseAidOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesSameTypeAidLegacyProducer(global::CardGame.Core.CardUseFrame use, long producer);
    bool IsSameTypeAidTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SameTypeAidTargetAddedEvent fact);
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool StartActualUseTargetWindow(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.ActualUseTargetReturnKind kind, global::CardGame.Core.ActualUseTargetTrickReturn? trick);
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreUniqueHpTargetAnnouncementsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreUniqueHpTargetAnnouncementsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
}
