#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterXuanGongZhuOlCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterXuanGongZhuOlCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterXuanGongZhuOlCoreZhuijiProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidZhuijiIssued(global::CardGame.Core.ProgramSkillFrame f);
    bool IsZhuijiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p);
    global::CardGame.Core.SkillProgramStepOutcome ArmZhuijiActualPlay(global::CardGame.Core.ProgramSkillFrame input);
    global::CardGame.Core.SkillProgramStepOutcome SettleZhuijiActualPlayDebt(global::CardGame.Core.ProgramSkillFrame input);
}

internal interface ICharacterXuanGongZhuOlCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidGaolingRecovery(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.SkillProgramStepOutcome ArmGaolingRecovery(global::CardGame.Core.ProgramSkillFrame input);
}

internal interface ICharacterXuanGongZhuOlCoreQimeiProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RequireQimeiDraw(global::CardGame.Core.ProgramSkillFrame f);
    bool ValidQimeiDraw(global::CardGame.Core.ProgramSkillFrame f);
    global::CardGame.Core.SkillProgramStepOutcome ArmQimeiPair(global::CardGame.Core.ProgramSkillFrame input);
    global::CardGame.Core.SkillProgramStepOutcome SettleQimeiPairEquality(global::CardGame.Core.ProgramSkillFrame input);
}

internal interface ICharacterXuanGongZhuOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterXuanGongZhuOlCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMovementObserverSuffix(int rootIndex);
}

internal interface ICharacterXuanGongZhuOlCharacterXuanGongZhuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool XuanGongZhuFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterXuanGongZhuOlCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterXuanGongZhuOlCoreTypedRecoveryDamageCursorOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? TypedRecoveryPausedEffect(global::CardGame.Core.ProgramSkillFrame producer);
    bool DirectProgramRecoveryReturnMatches(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.RecoverProgramInstruction instruction, global::CardGame.Core.RecoveryReplacementFrame recovery);
}

internal interface ICharacterXuanGongZhuOlCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICharacterXuanGongZhuOlCoreObserverSubtreesOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}
