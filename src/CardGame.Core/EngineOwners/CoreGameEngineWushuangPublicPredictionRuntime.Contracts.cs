#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreWushuangPublicPredictionCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreWushuangPublicPredictionCorePublicDamagePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PublicDamagePrediction PredictVisibleDamage(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.CardKind? kind, bool cardUse, bool sourceLess, bool chain, bool finalized, int paidHandCards, bool ignoresEquipmentArmor, global::CardGame.Core.DamageNature? damageNature = default, global::CardGame.Core.GameEngine.IDamageAttempt? issuedAttack = default, bool wushuangTactics = false, global::CardGame.Core.Suit prospectiveSuit = (global::CardGame.Core.Suit)500, int additionalIncomingSegments = 0, int priorTargetSegments = 0);
}

internal interface ICoreWushuangPublicPredictionCoreWushuangModeProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangMode { get; }
}

internal interface ICoreWushuangPublicPredictionCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
    global::CardGame.Core.Suit? CaptureUsedCardSuit(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.Card card);
}

internal interface ICoreWushuangPublicPredictionCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreWushuangPublicPredictionCoreNativeEquipmentAndFactionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasArmorBypass(global::CardGame.Core.CharacterState player);
}

internal interface ICoreWushuangPublicPredictionCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}
