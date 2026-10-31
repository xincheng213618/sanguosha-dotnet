#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreDyingSuitsAndHistoricalUseObserversAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreDyingSuitsAndHistoricalUseObserversOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime> _targetFactory = targetFactory;
    public bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => this._targetFactory().DyingSuitsStructuralEdge(parent, child);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreCharacterTurnedOverProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreCharacterTurnedOverProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> _targetFactory = targetFactory;
    public void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => this._targetFactory().RecordCharacterTurnedOver(targetSeat, wasFaceDown);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().HasRuntimeSkill(player, skillId);
    public string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId) => this._targetFactory().GetRuntimeSkillInstanceId(player, skillId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreProgramConversionPolarityAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreProgramConversionPolarityOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramConversionPolarityRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillPolarity CommitProgramConversionPolarity(global::CardGame.Core.ProgramSkillFrame frame) => this._targetFactory().CommitProgramConversionPolarity(frame);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreProgramAtomicCardMovesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreProgramAtomicCardMovesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAtomicCardMovesRuntime> _targetFactory = targetFactory;
    public void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default) => this._targetFactory().MoveProgramCardsFromMultipleSources(ids, destination, reason, afterPhysicalRecords, destinationForCard, disclosure, afterRequestSettled, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreSkillOwnershipWindowsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreSkillOwnershipWindowsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillOwnershipWindowsRuntime> _targetFactory = targetFactory;
    public bool TryBeginAdvancedSkillsChanged(long parentFrameId) => this._targetFactory().TryBeginAdvancedSkillsChanged(parentFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreProgramLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreProgramLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId) => this._targetFactory().GetActiveProgramFrame(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreProgramNativeDrawAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreProgramNativeDrawOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramNativeDrawRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().BeginOperationProgramDraw(frame, recipientSeat, requestedCount, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCorePrivateOfferReturnsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCorePrivateOfferReturnsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateOfferReturnsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().GetPausedPrivateOfferEffect(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreProgramIronChainTargetsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreProgramIronChainTargetsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramIronChainTargetsRuntime> _targetFactory = targetFactory;
    public void AddProgramIronChainUseActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> candidates, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => this._targetFactory().AddProgramIronChainUseActions(actions, actor, physicalCard, candidates, playedCardKind, conversionSource);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreObserverSubtreesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreObserverSubtreesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default) => this._targetFactory().FindProgramObserverRoot(firstChild, suffix, requiredRootId);
    public bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false) => this._targetFactory().TryAdvanceObserverSubtree(advanceRecoveryReplacement, incompleteAttackError, routeUnreturnedAttackThroughRuntime);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreSkillProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreSkillProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSkillProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome BeginProgramSkillDamage(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, int amount, global::CardGame.Core.ProgramParticipantReference? sourceReference = default, global::CardGame.Core.DamageNature? nature = default, bool sourceLess = false, int? explicitSourceSeat = default) => this._targetFactory().BeginProgramSkillDamage(frame, targetSeat, amount, sourceReference, nature, sourceLess, explicitSourceSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default) => this._targetFactory().TryBeginCardsMovedProgramWindow(instructionFrameId, subjectSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreHpChangeProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreHpChangeProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHpChangeProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginHpChangedProgramWindow(resumeFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreCardUseModulesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreCardUseModulesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime> _targetFactory = targetFactory;
    public bool IsTurnHandCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card) => this._targetFactory().IsTurnHandCardRestricted(player, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreRecoveryReplacementAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreRecoveryReplacementOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreRecoveryReplacementRuntime> _targetFactory = targetFactory;
    public bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginQueuedRecoveryReplacement(parentFrameId, continuation, cardId, cardKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreCharacterStateProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreCharacterStateProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterStateProgramsRuntime> _targetFactory = targetFactory;
    public bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default) => this._targetFactory().TryBeginCharacterStateProgramWindow(resumeFrameId, continuation, cardId, cardKind);
    public void RecordCharacterStateChange(int targetSeat, global::CardGame.Core.SkillProgramTriggerWindow window, long? parentFrameId = default, global::CardGame.Core.CharacterTurnedOverState? turnedOver = default) => this._targetFactory().RecordCharacterStateChange(targetSeat, window, parentFrameId, turnedOver);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreDeathProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreDeathProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDeathProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DeathFrame GetCurrentDeathFrame(long expectedId) => this._targetFactory().GetCurrentDeathFrame(expectedId);
    public void UpdateDeathFrame(long frameId, global::System.Func<global::CardGame.Core.DeathFrame, global::CardGame.Core.DeathFrame> update) => this._targetFactory().UpdateDeathFrame(frameId, update);
    public bool TryBeginOwnerDiedProgramWindow(global::CardGame.Core.DeathFrame death, global::CardGame.Core.CharacterState owner) => this._targetFactory().TryBeginOwnerDiedProgramWindow(death, owner);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime))]
internal sealed class CharacterPangTongAnniversaryGodCoreAdvancedLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICharacterPangTongAnniversaryGodCoreAdvancedLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdvancedLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAdvancedCard(int id) => this._targetFactory().GetAdvancedCard(id);
}
