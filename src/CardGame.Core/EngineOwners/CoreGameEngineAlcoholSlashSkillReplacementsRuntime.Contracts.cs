#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAlcoholSlashSkillReplacementsCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreAlcoholSlashSkillReplacementsObservedSkillGrantRevisionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, long> _observedSkillGrantRevisions { get; }
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreDamageObserverCursorQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DamageProgramMatchesWindow(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DamageTriggerWindowFrame window);
}

internal interface ICoreAlcoholSlashSkillReplacementsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreAlcoholSlashSkillReplacementsKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreOrderedPrintedSkillLossObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ExactOrderedPrintedSkillLossParent(global::CardGame.Core.ProgramSkillFrame f, out global::CardGame.Core.DamageTriggerWindowFrame window, out global::CardGame.Core.DamageFrame damage, out global::CardGame.Core.GameEngine.IDamageAttempt attack);
    global::CardGame.Core.ProgramSkillFrame? OrderedPrintedSkillLossObserverRoot(long? ownerRootId = default);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreHellSoulCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HellWanghunGrantLostEvent[] HellSoulOutstandingLosses(int seat);
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreConversionStatesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RegisterTaggedConversionSkill(global::CardGame.Core.CharacterState player, string skillId);
}

internal interface ICoreAlcoholSlashSkillReplacementsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreAlcoholSlashSkillReplacementsCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}
