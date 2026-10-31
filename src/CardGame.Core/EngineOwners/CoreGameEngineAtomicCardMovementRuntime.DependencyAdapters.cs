#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed class CoreAtomicCardMovementCharacterZhongHuiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCharacterZhongHuiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeLishiDiscardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().NormalizeLishiDiscardDestination(card, from, to, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime))]
internal sealed class CoreAtomicCardMovementCoreActualDiscardPhaseLedgerAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreActualDiscardPhaseLedgerOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualDiscardPhaseLedgerRuntime> _targetFactory = targetFactory;
    public void CollectDiscardPhaseHandDiscard(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().CollectDiscardPhaseHandDiscard(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime))]
internal sealed class CoreAtomicCardMovementCoreHellSoulCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreHellSoulCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellSoulCardsRuntime> _targetFactory = targetFactory;
    public void DestroyHellReturnSoulIfDiscarded(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation to) => this._targetFactory().DestroyHellReturnSoulIfDiscarded(card, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime))]
internal sealed class CoreAtomicCardMovementCoreWushuangShopCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreWushuangShopCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> _targetFactory = targetFactory;
    public bool IsWushuangShopEquipment(int cardId) => this._targetFactory().IsWushuangShopEquipment(cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime))]
internal sealed class CoreAtomicCardMovementCoreAlternativeViewAsCostsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreAlternativeViewAsCostsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeProgramViewAsCostDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().NormalizeProgramViewAsCostDestination(card, from, to, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.AtomicCardMovementRuntime))]
internal sealed class CoreAtomicCardMovementAtomicCardMovementAdapter(global::System.Func<global::CardGame.Core.AtomicCardMovementRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementAtomicCardMovementOperations
{
    private readonly global::System.Func<global::CardGame.Core.AtomicCardMovementRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.AtomicCardMovementRuntime Get() => this._targetFactory();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime))]
internal sealed class CoreAtomicCardMovementCoreHellMengpoProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreHellMengpoProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHellMengpoProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HellShiyouTransferPlan PrepareHellShiyouDiscardTransfers(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardTransfer> input, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().PrepareHellShiyouDiscardTransfers(input, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed class CoreAtomicCardMovementCoreShanheStartingResourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreShanheStartingResourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _targetFactory = targetFactory;
    public bool IsShanheStartingResource(int id) => this._targetFactory().IsShanheStartingResource(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed class CoreAtomicCardMovementCharacterZhugeLiangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCharacterZhugeLiangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeWuZglCopyDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().NormalizeWuZglCopyDestination(card, from, to, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime))]
internal sealed class CoreAtomicCardMovementCoreCardDeclarationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreCardDeclarationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDeclarationsRuntime> _targetFactory = targetFactory;
    public bool ClaimDeclarationPayment(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ClaimDeclarationPayment(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime))]
internal sealed class CoreAtomicCardMovementCoreEquipmentSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreEquipmentSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentSkillsRuntime> _targetFactory = targetFactory;
    public void ResolveEquipmentSkillGrant(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ResolveEquipmentSkillGrant(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed class CoreAtomicCardMovementCharacterZuoCiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCharacterZuoCiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? PlanHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().PlanHappyZuoCiPhantomDestination(card, from, requestedTo, reason);
    public global::CardGame.Core.CardLocation NormalizeHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().NormalizeHappyZuoCiPhantomDestination(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime))]
internal sealed class CoreAtomicCardMovementCharacterWeiYanHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCharacterWeiYanHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeHappyWeiYanGainDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().NormalizeHappyWeiYanGainDestination(card, from, to, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime))]
internal sealed class CoreAtomicCardMovementCoreHellActivitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreHellActivitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineHellActivitiesRuntime> _targetFactory = targetFactory;
    public bool CanPlanHellDiscard(global::CardGame.Core.CardLocation destination) => this._targetFactory().CanPlanHellDiscard(destination);
    public void RecordHellInterceptedMovement(global::CardGame.Core.HellShiyouTransferPlan plan, global::CardGame.Core.CardMovementRecord move, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact) => this._targetFactory().RecordHellInterceptedMovement(plan, move, beforeFact);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed class CoreAtomicCardMovementCoreHappyWeiYanGainDestinationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreHappyWeiYanGainDestinationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyWeiYanGainDestinationPlan? PlanHappyWeiYanGainDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation requestedTo, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().PlanHappyWeiYanGainDestination(card, from, requestedTo, reason);
    public bool TrySettleHappyWeiYanRetainedGain(global::CardGame.Core.HappyWeiYanGainDestinationPlan plan, long? producerFrameId = default) => this._targetFactory().TrySettleHappyWeiYanRetainedGain(plan, producerFrameId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreAtomicCardMovementCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public bool IsDiouCarrier(int id) => this._targetFactory().IsDiouCarrier(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime))]
internal sealed class CoreAtomicCardMovementCoreAdjacentDiscardStorageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreAdjacentDiscardStorageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> _targetFactory = targetFactory;
    public bool TracksAdjacentDiscardStorage { get => this._targetFactory().TracksAdjacentDiscardStorage; }

    public global::CardGame.Core.CardKind? FreezeAdjacentDiscardPaymentIdentity(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().FreezeAdjacentDiscardPaymentIdentity(card, from, to, reason);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime))]
internal sealed class CoreAtomicCardMovementCorePhysicalMovementFacadesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCorePhysicalMovementFacadesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalMovementFacadesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardMovementRecord RecordMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason, global::System.Action<global::CardGame.Core.CardMovementRecord>? beforeFact = default, global::CardGame.Core.CardKind? adjacentDiscardIdentity = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::CardGame.Core.HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = default, global::CardGame.Core.HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = default) => this._targetFactory().RecordMovement(card, from, to, reason, beforeFact, adjacentDiscardIdentity, disclosure, happyWeiYanGainDestination, happyZuoCiPhantomDestination);
    public void ClearJudgmentEffectiveKindAfterMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ClearJudgmentEffectiveKindAfterMove(card, from, to);
    public void ResolveSilverLionRemoval(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardMoveReason reason) => this._targetFactory().ResolveSilverLionRemoval(card, from, reason);
    public void ResolveWoodenOxMove(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().ResolveWoodenOxMove(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed class CoreAtomicCardMovementCoreEphemeralCardMaterialsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreAtomicCardMovementCoreEphemeralCardMaterialsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeEphemeralCardDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason = default) => this._targetFactory().NormalizeEphemeralCardDestination(card, from, to, reason);
}
