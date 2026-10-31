#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime))]
internal sealed class CoreProgramAtomicCardMovesCoreHellMengpoProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCoreHellMengpoProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HellShiyouTransferPlan PrepareHellShiyouDiscardTransfers(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardTransfer> input, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().PrepareHellShiyouDiscardTransfers(input, reason);
    public void CompleteHellShiyouDiscardTransfers(global::CardGame.Core.HellShiyouTransferPlan plan, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => this._targetFactory().CompleteHellShiyouDiscardTransfers(plan, movements);
    public bool IsHellShiyouPaidDiscardMovement(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardLocation expectedSource, global::CardGame.Core.CardMoveReason expectedReason) => this._targetFactory().IsHellShiyouPaidDiscardMovement(move, expectedSource, expectedReason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed class CoreProgramAtomicCardMovesCharacterZuoCiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCharacterZuoCiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? PlanHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().PlanHappyZuoCiPhantomDestination(card, from, requestedTo, reason);
    public global::CardGame.Core.CardLocation NormalizeHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().NormalizeHappyZuoCiPhantomDestination(card, from, to);
    public bool IsHappyZuoCiDestroyedMaterial(global::CardGame.Core.CardMovementRecord m) => this._targetFactory().IsHappyZuoCiDestroyedMaterial(m);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime))]
internal sealed class CoreProgramAtomicCardMovesCoreActualDiscardPhaseLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCoreActualDiscardPhaseLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> _targetFactory = targetFactory;
    public void CollectDiscardPhaseHandDiscard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().CollectDiscardPhaseHandDiscard(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime))]
internal sealed class CoreProgramAtomicCardMovesCoreHellActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCoreHellActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> _targetFactory = targetFactory;
    public bool CanPlanHellDiscard(global::CardGame.Core.CardLocation destination) => this._targetFactory().CanPlanHellDiscard(destination);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreProgramAtomicCardMovesCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public void DestroyHellReturnSoulIfDiscarded(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation to) => this._targetFactory().DestroyHellReturnSoulIfDiscarded(card, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CoreProgramAtomicCardMovesCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default) => this._targetFactory().NormalizeEphemeralCardDestination(card, from, to, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed class CoreProgramAtomicCardMovesCharacterZhongHuiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCharacterZhongHuiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeLishiDiscardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().NormalizeLishiDiscardDestination(card, from, to, reason);
    public bool IsLishiPaidDiscardMovement(global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardLocation expectedSource, global::CardGame.Core.CardMoveReason expectedReason) => this._targetFactory().IsLishiPaidDiscardMovement(move, expectedSource, expectedReason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreProgramAtomicCardMovesCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementRecord RecordMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, global::CardGame.Core.CardKind? adjacentDiscardIdentity = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::CardGame.Core.HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = default, global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = default) => this._targetFactory().RecordMovement(card, from, to, reason, beforeFact, adjacentDiscardIdentity, disclosure, happyWeiYanGainDestination, happyZuoCiPhantomDestination);
    public void ClearJudgmentEffectiveKindAfterMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ClearJudgmentEffectiveKindAfterMove(card, from, to);
    public void ResolveSilverLionRemoval(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().ResolveSilverLionRemoval(card, from, reason);
    public void ResolveWoodenOxMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ResolveWoodenOxMove(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed class CoreProgramAtomicCardMovesCoreHappyWeiYanGainDestinationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCoreHappyWeiYanGainDestinationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyWeiYanGainDestinationPlan? PlanHappyWeiYanGainDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().PlanHappyWeiYanGainDestination(card, from, requestedTo, reason);
    public global::CardGame.Core.HappyWeiYanRetainedGainSettlement SettleHappyWeiYanRetainedGain(global::CardGame.Core.HappyWeiYanGainDestinationPlan plan, long? producerFrameId = default) => this._targetFactory().SettleHappyWeiYanRetainedGain(plan, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed class CoreProgramAtomicCardMovesCoreCardMovementProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCoreCardMovementProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementBatchBuilder BeginCardMovementBatch(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> sources, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> destinations) => this._targetFactory().BeginCardMovementBatch(sources, destinations);
    public void CompleteCardMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements, bool committed) => this._targetFactory().CompleteCardMovementBatch(batch, movements, committed);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime))]
internal sealed class CoreProgramAtomicCardMovesCoreEquipmentSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreProgramAtomicCardMovesCoreEquipmentSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> _targetFactory = targetFactory;
    public void ResolveEquipmentSkillGrant(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ResolveEquipmentSkillGrant(card, from, to);
}
