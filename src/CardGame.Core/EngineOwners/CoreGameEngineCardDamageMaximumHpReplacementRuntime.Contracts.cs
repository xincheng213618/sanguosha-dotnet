#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardDamageMaximumHpReplacementCharacterPanZhangMaZhongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryAnjianDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreCardDamageMaximumHpReplacementResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreCardDamageMaximumHpReplacementCoreNativePlayerDeathOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default);
}

internal interface ICoreCardDamageMaximumHpReplacementCharacterDaQiaoXiaoQiaoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyQiaoCardMaximumProof? PrepareHappyQiaoCardMaximumHp(global::CardGame.Core.GameEngine.IDamageAttempt attack, int newMaximum);
    bool ApplyHappyQiaoCardMaximumHp(long ownerId, global::CardGame.Core.CardDamageMaximumHpReplacementReceipt receipt);
    bool TryBeginHappyQiaoCardMaximumHealth(long attackId);
    bool PublishHappyQiaoCardMaximumHp(long ownerId, global::CardGame.Core.CardDamageMaximumHpReplacementReceipt receipt);
    bool MatchesHappyQiaoCardMaximumHp(global::CardGame.Core.CardDamageMaximumHpReplacementReceipt receipt);
}

internal interface ICoreCardDamageMaximumHpReplacementCoreProgramEventHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent;
}

internal interface ICoreCardDamageMaximumHpReplacementCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> GetPassiveProgramDamageModifiers(global::CardGame.Core.GameEngine.IDamageAttempt attack, bool cardDamageOnly = false);
}

internal interface ICoreCardDamageMaximumHpReplacementCharacterZhangFeiBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryZhangFeiLiyongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreCardDamageMaximumHpReplacementSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICoreCardDamageMaximumHpReplacementCharacterHuangZhongBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryHuangZhongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreCardDamageMaximumHpReplacementCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreCardDamageMaximumHpReplacementCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default);
}

internal interface ICoreCardDamageMaximumHpReplacementKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreCardDamageMaximumHpReplacementCoreIdentityConcealmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginIdentityAppearanceForDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount);
}

internal interface ICoreCardDamageMaximumHpReplacementCharacterChengPuBoundaryMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int BoundaryChengPuLihuoDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack);
}

internal interface ICoreCardDamageMaximumHpReplacementCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardAttackState GetCardAttackState(long id);
    void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update);
}

internal interface ICoreCardDamageMaximumHpReplacementContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreCardDamageMaximumHpReplacementCoreExtendedViewAsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ProgramConversionDamageBonus(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreCardDamageMaximumHpReplacementTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreCardDamageMaximumHpReplacementCoreMatchLogOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddLog(string type, string message, int? actor = default, int? target = default);
}
