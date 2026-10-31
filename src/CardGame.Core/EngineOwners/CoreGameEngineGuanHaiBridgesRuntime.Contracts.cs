#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreGuanHaiBridgesCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? PhysicalGroupSuit(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreGuanHaiBridgesCoreWushuangJinzhiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangJinzhiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreGuanHaiBridgesCharacterGuanHaiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsXieweiMultiTargetUse(global::CardGame.Core.CardUseFrame use);
    bool IsXieweiRemovedMaterial(long useId, int cardId);
    bool IsXieweiSelection(global::CardGame.Core.ProgramMultiCardViewAsSelection selection);
}

internal interface ICoreGuanHaiBridgesCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? PhysicalGroupColor(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards);
}

internal interface ICoreGuanHaiBridgesCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreGuanHaiBridgesCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetFangtianEffectiveTarget(global::CardGame.Core.NativeOwners.FangtianHalberdHandle pending, int index);
}
