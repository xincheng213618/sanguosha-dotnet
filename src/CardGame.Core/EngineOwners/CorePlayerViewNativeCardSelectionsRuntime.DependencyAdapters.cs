#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCorePhysicalOwnedCardQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCorePhysicalOwnedCardQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player) => this._targetFactory().GetHand(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCharacterLuSuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCharacterLuSuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? ProjectShiLsCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ProjectShiLsCardSelection(decision, f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCharacterSunJianMouOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCharacterSunJianMouOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJianMouOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card[] MouSunJianOlYipoDiscardable(int seat) => this._targetFactory().MouSunJianOlYipoDiscardable(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCharacterSunChenMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCharacterSunChenMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? ProjectShiScCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ProjectShiScCardSelection(decision, f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCharacterZhangYanMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCharacterZhangYanMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangYanMobileShiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? ProjectShiZyCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ProjectShiZyCardSelection(decision, f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCharacterLiYiOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCharacterLiYiOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> _targetFactory = targetFactory;
    public int ChanshuangChooser(global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ChanshuangChooser(f);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCharacterQinLangOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCharacterQinLangOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _targetFactory = targetFactory;
    public bool QinLangCountUsed(int owner, int round, int count) => this._targetFactory().QinLangCountUsed(owner, round, count);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime))]
internal sealed class CorePlayerViewNativeCardSelectionsCharacterSunJunMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICorePlayerViewNativeCardSelectionsCharacterSunJunMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PendingDecision? ProjectShiSjCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => this._targetFactory().ProjectShiSjCardSelection(decision, f);
}
