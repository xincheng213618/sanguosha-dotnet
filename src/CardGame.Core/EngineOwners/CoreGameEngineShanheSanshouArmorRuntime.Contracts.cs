#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreShanheSanshouArmorCoreEquipmentSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsActiveProgramSourceEquipmentCard(int ownerSeat, string skillId, string skillInstanceId, global::CardGame.Core.Card card);
}

internal interface ICoreShanheSanshouArmorCharacterHuCheErMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> KongwuEffectiveEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreShanheSanshouArmorCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}

internal interface ICoreShanheSanshouArmorCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsArmorIneffectiveForTurn(global::CardGame.Core.CharacterState target);
}

internal interface ICoreShanheSanshouArmorCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat);
}
