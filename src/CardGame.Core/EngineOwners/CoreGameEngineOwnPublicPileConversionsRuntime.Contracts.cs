#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreOwnPublicPileConversionsCharacterPangTongFriendOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsFriendYangmingOriginalUseChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind);
}

internal interface ICoreOwnPublicPileConversionsCharacterHuangZuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiHzOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost);
}

internal interface ICoreOwnPublicPileConversionsCharacterLuBuOlMagicOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MoGangquanPhysicalFire(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreOwnPublicPileConversionsCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, global::CardGame.Core.ProgramCardContinuation? continuation = default);
}

internal interface ICoreOwnPublicPileConversionsCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreOwnPublicPileConversionsCharacterGuanYuWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WuyouIdentity(global::CardGame.Core.CharacterState holder, global::CardGame.Core.Card card);
}

internal interface ICoreOwnPublicPileConversionsCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNativePublicPileOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost);
    bool IsNativePublicPileOriginalChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, bool response);
}

internal interface ICoreOwnPublicPileConversionsCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICoreOwnPublicPileConversionsContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreOwnPublicPileConversionsCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreOwnPublicPileConversionsCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICoreOwnPublicPileConversionsCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreOwnPublicPileConversionsCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreOwnPublicPileConversionsCorePublicPileSourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PublicPersistentPileSource> ReferencedPublicPileSources(int seat, string skill, string? consumerInstance = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> PublicPileCards(global::CardGame.Core.PublicPersistentPileSource source);
}

internal interface ICoreOwnPublicPileConversionsCharacterGongsunZanMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGszHuActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost);
    bool IsMouGszHuCard(global::CardGame.Core.CharacterState owner, int id);
    bool ValidateMouGszHuConversion(global::CardGame.Core.CharacterState provider, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? conversion, bool forResponse, global::CardGame.Core.CardKind? effectiveKind);
}

internal interface ICoreOwnPublicPileConversionsResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreOwnPublicPileConversionsCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreOwnPublicPileConversionsCoreSourceDingxiNativeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSourceDingxiOriginalActionCost(global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardActionCost cost);
}
