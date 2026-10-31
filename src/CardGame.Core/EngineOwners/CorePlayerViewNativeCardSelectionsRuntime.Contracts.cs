#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICorePlayerViewNativeCardSelectionsCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICorePlayerViewNativeCardSelectionsCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? ProjectShiLsCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePlayerViewNativeCardSelectionsCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICorePlayerViewNativeCardSelectionsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICorePlayerViewNativeCardSelectionsCharacterSunJianMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card[] MouSunJianOlYipoDiscardable(int seat);
}

internal interface ICorePlayerViewNativeCardSelectionsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICorePlayerViewNativeCardSelectionsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICorePlayerViewNativeCardSelectionsCharacterSunChenMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? ProjectShiScCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePlayerViewNativeCardSelectionsCharacterZhangYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? ProjectShiZyCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePlayerViewNativeCardSelectionsCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ChanshuangChooser(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICorePlayerViewNativeCardSelectionsCharacterQinLangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool QinLangCountUsed(int owner, int round, int count);
}

internal interface ICorePlayerViewNativeCardSelectionsCharacterSunJunMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? ProjectShiSjCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f);
}
