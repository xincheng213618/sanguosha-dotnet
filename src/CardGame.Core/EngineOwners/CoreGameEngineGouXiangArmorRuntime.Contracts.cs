#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGouXiangArmorCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsArmorIneffectiveForTurn(global::CardGame.Core.CharacterState target);
}

internal interface ICoreGouXiangArmorKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long ownerId);
}

internal interface ICoreGouXiangArmorCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreGouXiangArmorRandomOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.DeterministicRandom _random { get; }
}

internal interface ICoreGouXiangArmorCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGouXiangArmorCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat);
}

internal interface ICoreGouXiangArmorCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment? LiveDeclarationPayment(int cardId);
}

internal interface ICoreGouXiangArmorCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreGouXiangArmorCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreGouXiangArmorCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreGouXiangArmorCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreGouXiangArmorContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreGouXiangArmorCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, global::CardGame.Core.Card card);
}

internal interface ICoreGouXiangArmorCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreGouXiangArmorResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreGouXiangArmorKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}
