#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterDianWeiHappyGodCoreNativeCardClassificationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DamageNature GetDamageNature(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICharacterDianWeiHappyGodResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterDianWeiHappyGodCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
    global::System.Collections.Generic.IReadOnlyList<int> CurrentTableOrder();
}

internal interface ICharacterDianWeiHappyGodCoreRecipientScopedDamageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateRecipientScopedDamageRedirect(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICharacterDianWeiHappyGodKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICharacterDianWeiHappyGodCoreTeamActivityProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool PublicTeamEnemy(int owner, global::CardGame.Core.CharacterState target);
}

internal interface ICharacterDianWeiHappyGodCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertProgramAttackState();
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICharacterDianWeiHappyGodCoreDeathProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeathFrame GetCurrentDeathFrame(long expectedId);
    void UpdateDeathFrame(long frameId, global::System.Func<global::CardGame.Core.DeathFrame, global::CardGame.Core.DeathFrame> update);
    void CompleteDeathResolution(long deathFrameId);
}

internal interface ICharacterDianWeiHappyGodCoreProgramDodgeResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinAttackRange(int actorSeat, int targetSeat);
}

internal interface ICharacterDianWeiHappyGodCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICharacterDianWeiHappyGodCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMovementObserverEdge(int child);
    bool IsPaidRescueObserverSuffix(int child, global::CardGame.Core.DyingFrame dying);
}

internal interface ICharacterDianWeiHappyGodCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterDianWeiHappyGodContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterDianWeiHappyGodCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterDianWeiHappyGodKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterDianWeiHappyGodCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICharacterDianWeiHappyGodCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICharacterDianWeiHappyGodKernelCoreGameEngineProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId);
}

internal interface ICharacterDianWeiHappyGodProgramLifecycleWindowOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramLifecycleWindowRuntime Get();
}

internal interface ICharacterDianWeiHappyGodCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterDianWeiHappyGodCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame;
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICharacterDianWeiHappyGodCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterDianWeiHappyGodCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}
