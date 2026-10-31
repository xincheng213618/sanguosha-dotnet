#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime))]
internal sealed class CoreOwnPublicPileConversionsCharacterPangTongFriendAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCharacterPangTongFriendOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongFriendRuntime> _targetFactory = targetFactory;
    public bool IsFriendYangmingOriginalUseChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => this._targetFactory().IsFriendYangmingOriginalUseChoice(owner, card, kind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime))]
internal sealed class CoreOwnPublicPileConversionsCharacterHuangZuMobileShiAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCharacterHuangZuMobileShiOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZuMobileShiRuntime> _targetFactory = targetFactory;
    public bool IsShiHzOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsShiHzOriginalActionCost(action, cost);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime))]
internal sealed class CoreOwnPublicPileConversionsCharacterLuBuOlMagicAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCharacterLuBuOlMagicOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuOlMagicRuntime> _targetFactory = targetFactory;
    public bool MoGangquanPhysicalFire(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().MoGangquanPhysicalFire(owner, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime))]
internal sealed class CoreOwnPublicPileConversionsCoreCardActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCoreCardActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, global::CardGame.Core.ProgramCardContinuation? continuation = default) => this._targetFactory().CollectSharedCardActionCandidates(action, window, eventTargets, cardUseCausedDamage, continuation);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreOwnPublicPileConversionsCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source) => this._targetFactory().ViewAsRule(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime))]
internal sealed class CoreOwnPublicPileConversionsCharacterGuanYuWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCharacterGuanYuWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuanYuWumiaoRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WuyouIdentity(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card) => this._targetFactory().WuyouIdentity(holder, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed class CoreOwnPublicPileConversionsCoreNativePublicPileOriginalCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCoreNativePublicPileOriginalCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _targetFactory = targetFactory;
    public bool IsNativePublicPileOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsNativePublicPileOriginalActionCost(action, cost);
    public bool IsNativePublicPileOriginalChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool response) => this._targetFactory().IsNativePublicPileOriginalChoice(owner, card, kind, response);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime))]
internal sealed class CoreOwnPublicPileConversionsCoreNationalSkillsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCoreNationalSkillsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNationalSkillsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => this._targetFactory().GetSkillBindingShard(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime))]
internal sealed class CoreOwnPublicPileConversionsCoreActionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCoreActionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActionQueriesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false) => this._targetFactory().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime))]
internal sealed class CoreOwnPublicPileConversionsCoreTableOrderAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCoreTableOrderOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTableOrderRuntime> _targetFactory = targetFactory;
    public int RelativeTableOrder(int from, int target) => this._targetFactory().RelativeTableOrder(from, target);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed class CoreOwnPublicPileConversionsCoreRuntimeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCoreRuntimeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _targetFactory = targetFactory;
    public void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => this._targetFactory().ReplaceRuntimeFrame(expectedFrameId, next);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime))]
internal sealed class CoreOwnPublicPileConversionsCorePublicPileSourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCorePublicPileSourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PublicPersistentPileSource> ReferencedPublicPileSources(int seat, string skill, string? consumerInstance = default) => this._targetFactory().ReferencedPublicPileSources(seat, skill, consumerInstance);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source) => this._targetFactory().PublicPileCards(source);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime))]
internal sealed class CoreOwnPublicPileConversionsCharacterGongsunZanMouMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCharacterGongsunZanMouMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> _targetFactory = targetFactory;
    public bool IsMouGszHuActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsMouGszHuActionCost(action, cost);
    public bool IsMouGszHuCard(global::CardGame.Core.CharacterState owner, int id) => this._targetFactory().IsMouGszHuCard(owner, id);
    public bool ValidateMouGszHuConversion(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversion, bool forResponse, global::CardGame.Core.CardKind? effectiveKind) => this._targetFactory().ValidateMouGszHuConversion(provider, materials, conversion, forResponse, effectiveKind);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreOwnPublicPileConversionsCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime))]
internal sealed class CoreOwnPublicPileConversionsCoreSourceDingxiNativeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreOwnPublicPileConversionsCoreSourceDingxiNativeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> _targetFactory = targetFactory;
    public bool IsSourceDingxiOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost) => this._targetFactory().IsSourceDingxiOriginalActionCost(action, cost);
}
