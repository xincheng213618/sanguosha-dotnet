#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreEphemeralCardMaterialsCharacterZhangFenAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeDagongCheDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsDagongCheDestroyedMaterial(global::CardGame.Core.CardMovementRecord m);
    bool IsDagongCheDestroyedMaterial(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsDagongCheDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreEphemeralCardMaterialsCoreWushuangShopCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeWushuangShopEquipmentDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsWushuangShopDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreEphemeralCardMaterialsCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreEphemeralCardMaterialsCoreCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsPaidDestroyedGeneralWeaponUseMaterial(long useId, int cardId);
}

internal interface ICoreEphemeralCardMaterialsCharacterZuoCiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeHappyZuoCiPhantomDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsHappyZuoCiDestroyedMaterial(global::CardGame.Core.CardMovementRecord m);
    bool IsHappyZuoCiDestroyedMaterial(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsHappyZuoCiDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreEphemeralCardMaterialsCharacterDengAiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeShenDaCarrierDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsShenDaCarrierDestroyedMaterial(global::CardGame.Core.CardMovementRecord m);
    bool IsShenDaCarrierDestroyedMaterial(int id, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsShenDaCarrierDestroyedMaterial(long use, int id);
}

internal interface ICoreEphemeralCardMaterialsCoreShanheStartingResourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeShanheStartingResourceDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason? reason);
    bool IsShanheStartingResourceDestroyedMaterial(global::CardGame.Core.CardMovementRecord move);
}

internal interface ICoreEphemeralCardMaterialsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardLocation NormalizeDiouCarrierDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsDiouCarrierDestroyedMaterial(global::CardGame.Core.CardMovementRecord movement);
    bool IsDiouCarrierDestroyedMaterial(int cardId, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
    bool IsDiouCarrierDestroyedMaterial(long useId, int cardId);
}

internal interface ICoreEphemeralCardMaterialsCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}
