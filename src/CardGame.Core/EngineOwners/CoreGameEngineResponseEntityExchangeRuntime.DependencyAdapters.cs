#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CoreResponseEntityExchangeCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetEquipment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetJudgment(global::CardGame.Core.CharacterState player) => this._targetFactory().GetJudgment(player);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime))]
internal sealed class CoreResponseEntityExchangeCoreDyingCursorRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreDyingCursorRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDyingCursorRuntimeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.DyingFrame? ActiveDying { get => this._targetFactory().ActiveDying; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreResponseEntityExchangeCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreResponseEntityExchangeCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeTop(next);
    public void AdvanceRuntimeProgram(long frameId) => this._targetFactory().AdvanceRuntimeProgram(frameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreResponseEntityExchangeCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public void MoveCard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, bool emitCardMovedEvents = true, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0) => this._targetFactory().MoveCard(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime))]
internal sealed class CoreResponseEntityExchangeCoreNativeCardContinuationCursorsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreNativeCardContinuationCursorsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NullificationWindowFrame? ActiveNullificationWindow { get => this._targetFactory().ActiveNullificationWindow; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime))]
internal sealed class CoreResponseEntityExchangeCoreWushuangJinzhiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreWushuangJinzhiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangJinzhiRuntime> _targetFactory = targetFactory;
    public bool IsWushuangJinzhiSource(global::CardGame.Core.CardConversionSource source) => this._targetFactory().IsWushuangJinzhiSource(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime))]
internal sealed class CoreResponseEntityExchangeCoreBoundCardMovementContinuationAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreBoundCardMovementContinuationOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundCardMovementContinuationRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramStepOutcome AwaitProgramBoundCardMovements(long frameId, int ownerSeat) => this._targetFactory().AwaitProgramBoundCardMovements(frameId, ownerSeat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreResponseEntityExchangeCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime))]
internal sealed class CoreResponseEntityExchangeCoreNativePromptBoundaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreNativePromptBoundaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePromptBoundaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PromptId CreatePromptId() => this._targetFactory().CreatePromptId();
    public void ClearPendingDecision() => this._targetFactory().ClearPendingDecision();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime))]
internal sealed class CoreResponseEntityExchangeCoreProgramSkillChangesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreProgramSkillChangesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramSkillChangesRuntime> _targetFactory = targetFactory;
    public void CommitProgramSkillChangeFact(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.IGameEvent fact) => this._targetFactory().CommitProgramSkillChangeFact(frame, fact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime))]
internal sealed class CoreResponseEntityExchangeCorePhysicalDrawFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCorePhysicalDrawFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalDrawFacadesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<int> DrawCards(global::CardGame.Core.CharacterState player, int count, bool log, global::CardGame.Core.CardMoveReason? reason = default, global::CardGame.Core.CardConversionSource? directProducer = default) => this._targetFactory().DrawCards(player, count, log, reason, directProducer);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime))]
internal sealed class CoreResponseEntityExchangeCoreNativeCardAppearanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreNativeCardAppearanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardAppearanceRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().EffectiveSuit(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime))]
internal sealed class CoreResponseEntityExchangeCorePendingDecisionPolicyPipelineAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCorePendingDecisionPolicyPipelineOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePendingDecisionPolicyPipelineRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? _pendingDecision { get => this._targetFactory()._pendingDecision; set => this._targetFactory()._pendingDecision = value; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime))]
internal sealed class CoreResponseEntityExchangeCoreNativeLogicalDiscardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreResponseEntityExchangeCoreNativeLogicalDiscardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeLogicalDiscardsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.NativeLogicalDiscardInvocationDraft? CaptureNativeLogicalDiscardInvocation(int initiatorSeat, int executorSeat, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardMoveReason reason, long? parentFrameId = default) => this._targetFactory().CaptureNativeLogicalDiscardInvocation(initiatorSeat, executorSeat, cardIds, reason, parentFrameId);
    public void CompleteNativeLogicalDiscardInvocation(global::CardGame.Core.NativeLogicalDiscardInvocationDraft? draft, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>? prepaidMaterials = default) => this._targetFactory().CompleteNativeLogicalDiscardInvocation(draft, prepaidMaterials);
}
