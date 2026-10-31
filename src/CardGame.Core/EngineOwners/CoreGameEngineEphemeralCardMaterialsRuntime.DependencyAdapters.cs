#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime))]
internal sealed class CoreEphemeralCardMaterialsCharacterZhangFenAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCharacterZhangFenAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFenAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeDagongCheDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().NormalizeDagongCheDestination(card, from, to);
    public bool IsDagongCheDestroyedMaterial(global::CardGame.Core.CardMovementRecord m) => this._targetFactory().IsDagongCheDestroyedMaterial(m);
    public bool IsDagongCheDestroyedMaterial(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().IsDagongCheDestroyedMaterial(cardId, from, to);
    public bool IsDagongCheDestroyedMaterial(long useId, int cardId) => this._targetFactory().IsDagongCheDestroyedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime))]
internal sealed class CoreEphemeralCardMaterialsCoreWushuangShopCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCoreWushuangShopCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeWushuangShopEquipmentDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().NormalizeWushuangShopEquipmentDestination(card, from, to);
    public bool IsWushuangShopDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().IsWushuangShopDestroyedMaterial(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreEphemeralCardMaterialsCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime))]
internal sealed class CoreEphemeralCardMaterialsCoreCardUsePaymentsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCoreCardUsePaymentsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUsePaymentsRuntime> _targetFactory = targetFactory;
    public bool IsPaidDestroyedGeneralWeaponUseMaterial(long useId, int cardId) => this._targetFactory().IsPaidDestroyedGeneralWeaponUseMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed class CoreEphemeralCardMaterialsCharacterZuoCiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCharacterZuoCiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().NormalizeHappyZuoCiPhantomDestination(card, from, to);
    public bool IsHappyZuoCiDestroyedMaterial(global::CardGame.Core.CardMovementRecord m) => this._targetFactory().IsHappyZuoCiDestroyedMaterial(m);
    public bool IsHappyZuoCiDestroyedMaterial(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().IsHappyZuoCiDestroyedMaterial(cardId, from, to);
    public bool IsHappyZuoCiDestroyedMaterial(long useId, int cardId) => this._targetFactory().IsHappyZuoCiDestroyedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime))]
internal sealed class CoreEphemeralCardMaterialsCharacterDengAiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCharacterDengAiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeShenDaCarrierDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().NormalizeShenDaCarrierDestination(card, from, to);
    public bool IsShenDaCarrierDestroyedMaterial(global::CardGame.Core.CardMovementRecord m) => this._targetFactory().IsShenDaCarrierDestroyedMaterial(m);
    public bool IsShenDaCarrierDestroyedMaterial(int id, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().IsShenDaCarrierDestroyedMaterial(id, from, to);
    public bool IsShenDaCarrierDestroyedMaterial(long use, int id) => this._targetFactory().IsShenDaCarrierDestroyedMaterial(use, id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed class CoreEphemeralCardMaterialsCoreShanheStartingResourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCoreShanheStartingResourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeShanheStartingResourceDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason) => this._targetFactory().NormalizeShanheStartingResourceDestination(card, from, to, reason);
    public bool IsShanheStartingResourceDestroyedMaterial(global::CardGame.Core.CardMovementRecord move) => this._targetFactory().IsShanheStartingResourceDestroyedMaterial(move);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreEphemeralCardMaterialsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardLocation NormalizeDiouCarrierDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().NormalizeDiouCarrierDestination(card, from, to);
    public bool IsDiouCarrierDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().IsDiouCarrierDestroyedMaterial(movement);
    public bool IsDiouCarrierDestroyedMaterial(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().IsDiouCarrierDestroyedMaterial(cardId, from, to);
    public bool IsDiouCarrierDestroyedMaterial(long useId, int cardId) => this._targetFactory().IsDiouCarrierDestroyedMaterial(useId, cardId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed class CoreEphemeralCardMaterialsCoreCardUseLifecycleAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreEphemeralCardMaterialsCoreCardUseLifecycleOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId) => this._targetFactory().LifecycleCardUse(frameId);
}
