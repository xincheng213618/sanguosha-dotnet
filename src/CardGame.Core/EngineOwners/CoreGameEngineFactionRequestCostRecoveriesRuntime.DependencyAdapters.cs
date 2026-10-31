#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreProgramSlashRequestAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreProgramSlashRequestOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSlashRequestRuntime> _targetFactory = targetFactory;
    public void CommitProgramChoiceResult(long frameId, string bind, string optionId, int chooserSeat, string label) => this._targetFactory().CommitProgramChoiceResult(frameId, bind, optionId, chooserSeat, label);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeOwners.FactionCardRequestHandle? ActiveFactionCardRequest { get => this._targetFactory().ActiveFactionCardRequest; set => this._targetFactory().ActiveFactionCardRequest = value; }
    public global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get => this._targetFactory().ActiveQinglongCrescentBlade; set => this._targetFactory().ActiveQinglongCrescentBlade = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreNativeCardUseFramesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreNativeCardUseFramesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _targetFactory = targetFactory;
    public void SetCardUseStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => this._targetFactory().SetCardUseStep(frameId, step);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreNativeFactionSlashAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreNativeFactionSlashOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionSlashRuntime> _targetFactory = targetFactory;
    public void CompleteFactionSlashFailure(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => this._targetFactory().CompleteFactionSlashFailure(pending);
    public void AdvanceFactionSlashCandidate(bool keepCurrentRequest = false) => this._targetFactory().AdvanceFactionSlashCandidate(keepCurrentRequest);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreQinglongInsertionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreQinglongInsertionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineQinglongInsertionRuntime> _targetFactory = targetFactory;
    public void RestoreFactionRequestAfterQinglongChoice(long ownerId) => this._targetFactory().RestoreFactionRequestAfterQinglongChoice(ownerId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreDeckEquipmentProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreDeckEquipmentProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeckEquipmentProgramsRuntime> _targetFactory = targetFactory;
    public bool RandomEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().RandomEquipmentFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreNativeBorrowedSwordAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreNativeBorrowedSwordOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeBorrowedSwordRuntime> _targetFactory = targetFactory;
    public void FinishBorrowedSword(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => this._targetFactory().FinishBorrowedSword(pending);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCorePublicPileEquipmentSequencesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCorePublicPileEquipmentSequencesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileEquipmentSequencesRuntime> _targetFactory = targetFactory;
    public bool PileEquipmentFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().PileEquipmentFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreNativeResponseWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreNativeResponseWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseWindowsRuntime> _targetFactory = targetFactory;
    public void PopResponseWindow(long parentFrameId) => this._targetFactory().PopResponseWindow(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreDamageObserverCursorQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreDamageObserverCursorQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime> _targetFactory = targetFactory;
    public bool DamageFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => this._targetFactory().DamageFrameRidesOn(ride, beneath);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime))]
internal sealed class CoreFactionRequestCostRecoveriesCoreNativeAttackCompletionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreFactionRequestCostRecoveriesCoreNativeAttackCompletionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeAttackCompletionRuntime> _targetFactory = targetFactory;
    public void CompleteAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().CompleteAttack(attack);
}
