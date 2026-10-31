#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreForcedPhaseAlcoholCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordMouZhaJiangAwareAlcoholDebit(long useId, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreForcedPhaseAlcoholCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
    global::CardGame.Core.ProgramSkillCardSetBinding GetProgramCardSet(global::CardGame.Core.ProgramSkillFrame frame, string bind);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
}

internal interface ICoreForcedPhaseAlcoholKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreForcedPhaseAlcoholCoreAfterTurnEndedProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActualAfterTurnEndedParent(global::CardGame.Core.DeferredTurnEndFrame parent);
    bool CanRunAfterTurnEndedCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.SkillProgramTrigger trigger);
}

internal interface ICoreForcedPhaseAlcoholCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreForcedPhaseAlcoholCoreVirtualBasicOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardKind Kind, int Target)> VirtualBasicOptions(int owner, bool anyRegisteredBasic = false);
}

internal interface ICoreForcedPhaseAlcoholResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreForcedPhaseAlcoholCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreForcedPhaseAlcoholKernelCoreGameEngineProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramAttackHandle CreateProgramAttackHandle(long ownerFrameId);
}

internal interface ICoreForcedPhaseAlcoholCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreForcedPhaseAlcoholCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksPlayCardHistory { get; }
}

internal interface ICoreForcedPhaseAlcoholContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreForcedPhaseAlcoholCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreForcedPhaseAlcoholCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreForcedPhaseAlcoholCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreForcedPhaseAlcoholCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSimpleCardUse(long frameId, global::CardGame.Core.ProgramSimpleCardContinuation continuation);
}

internal interface ICoreForcedPhaseAlcoholCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreForcedPhaseAlcoholCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordYingboCardUse(long resolutionId, int sourceSeat, global::CardGame.Core.CardKind cardKind);
}

internal interface ICoreForcedPhaseAlcoholCoreNativeDamageCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDamageAttack(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreForcedPhaseAlcoholCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default);
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreForcedPhaseAlcoholCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreForcedPhaseAlcoholCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasFirstActualPlayUseDistance(global::CardGame.Core.CharacterState actor);
}
