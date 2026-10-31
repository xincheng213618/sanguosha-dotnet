#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CharacterXuanGongZhuOlCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CharacterXuanGongZhuOlCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime))]
internal sealed class CharacterXuanGongZhuOlCoreZhuijiProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCoreZhuijiProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineZhuijiProgramsRuntime> _targetFactory = targetFactory;
    public bool ValidZhuijiIssued(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidZhuijiIssued(f);
    public bool IsZhuijiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => this._targetFactory().IsZhuijiMovement(f, e, p);
    public global::CardGame.Core.SkillProgramStepOutcome ArmZhuijiActualPlay(global::CardGame.Core.ProgramSkillFrame input) => this._targetFactory().ArmZhuijiActualPlay(input);
    public global::CardGame.Core.SkillProgramStepOutcome SettleZhuijiActualPlayDebt(global::CardGame.Core.ProgramSkillFrame input) => this._targetFactory().SettleZhuijiActualPlayDebt(input);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CharacterXuanGongZhuOlCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public bool ValidGaolingRecovery(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidGaolingRecovery(f);
    public global::CardGame.Core.SkillProgramStepOutcome ArmGaolingRecovery(global::CardGame.Core.ProgramSkillFrame input) => this._targetFactory().ArmGaolingRecovery(input);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime))]
internal sealed class CharacterXuanGongZhuOlCoreQimeiProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCoreQimeiProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQimeiProgramsRuntime> _targetFactory = targetFactory;
    public void RequireQimeiDraw(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().RequireQimeiDraw(f);
    public bool ValidQimeiDraw(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ValidQimeiDraw(f);
    public global::CardGame.Core.SkillProgramStepOutcome ArmQimeiPair(global::CardGame.Core.ProgramSkillFrame input) => this._targetFactory().ArmQimeiPair(input);
    public global::CardGame.Core.SkillProgramStepOutcome SettleQimeiPairEquality(global::CardGame.Core.ProgramSkillFrame input) => this._targetFactory().SettleQimeiPairEquality(input);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime))]
internal sealed class CharacterXuanGongZhuOlCorePrivateOfferReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCorePrivateOfferReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GetPausedPrivateOfferEffect(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime))]
internal sealed class CharacterXuanGongZhuOlCoreTypedRecoveryDamageCursorAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCoreTypedRecoveryDamageCursorOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTypedRecoveryDamageCursorRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? TypedRecoveryPausedEffect(global::CardGame.Core.ProgramSkillFrame producer) => this._targetFactory().TypedRecoveryPausedEffect(producer);
    public bool DirectProgramRecoveryReturnMatches(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.RecoverProgramInstruction instruction, global::CardGame.Core.RecoveryReplacementFrame recovery) => this._targetFactory().DirectProgramRecoveryReturnMatches(producer, effect, instruction, recovery);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime))]
internal sealed class CharacterXuanGongZhuOlCoreAlternatingBenefitsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCoreAlternatingBenefitsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternatingBenefitsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory() => this._targetFactory().CompleteProgramEventHistory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CharacterXuanGongZhuOlCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterXuanGongZhuOlCoreObserverSubtreesOperations2
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default) => this._targetFactory().FindProgramObserverRoot(firstChild, suffix, requiredRootId);
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}
