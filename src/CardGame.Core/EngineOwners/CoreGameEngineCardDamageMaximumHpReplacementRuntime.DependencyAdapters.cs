#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCharacterPanZhangMaZhongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCharacterPanZhangMaZhongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanZhangMaZhongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryAnjianDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BoundaryAnjianDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreNativePlayerDeathAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreNativePlayerDeathOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativePlayerDeathRuntime> _targetFactory = targetFactory;
    public void BeginPlayerDeath(long parentFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState? killer, global::CardGame.Core.GameEngine.IDamageAttempt? attack, global::CardGame.Core.DyingFrame? dying, long? causingProgramSkillFrameId = default, long? causingCardDamageReplacementId = default) => this._targetFactory().BeginPlayerDeath(parentFrameId, victim, killer, attack, dying, causingProgramSkillFrameId, causingCardDamageReplacementId);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCharacterDaQiaoXiaoQiaoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCharacterDaQiaoXiaoQiaoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyQiaoCardMaximumProof? PrepareHappyQiaoCardMaximumHp(global::CardGame.Core.GameEngine.IDamageAttempt attack, int newMaximum) => this._targetFactory().PrepareHappyQiaoCardMaximumHp(attack, newMaximum);
    public bool ApplyHappyQiaoCardMaximumHp(long ownerId, global::CardGame.Core.CardDamageMaximumHpReplacementReceipt receipt) => this._targetFactory().ApplyHappyQiaoCardMaximumHp(ownerId, receipt);
    public bool TryBeginHappyQiaoCardMaximumHealth(long attackId) => this._targetFactory().TryBeginHappyQiaoCardMaximumHealth(attackId);
    public bool PublishHappyQiaoCardMaximumHp(long ownerId, global::CardGame.Core.CardDamageMaximumHpReplacementReceipt receipt) => this._targetFactory().PublishHappyQiaoCardMaximumHp(ownerId, receipt);
    public bool MatchesHappyQiaoCardMaximumHp(global::CardGame.Core.CardDamageMaximumHpReplacementReceipt receipt) => this._targetFactory().MatchesHappyQiaoCardMaximumHp(receipt);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreNativeDamagePoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreNativeDamagePoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> GetPassiveProgramDamageModifiers(global::CardGame.Core.GameEngine.IDamageAttempt attack, bool cardDamageOnly = false) => this._targetFactory().GetPassiveProgramDamageModifiers(attack, cardDamageOnly);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCharacterZhangFeiBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCharacterZhangFeiBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryZhangFeiLiyongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BoundaryZhangFeiLiyongCardDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCharacterHuangZhongBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCharacterHuangZhongBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryHuangZhongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BoundaryHuangZhongCardDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreTableSeatQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreTableSeatQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _targetFactory = targetFactory;
    public bool IsValidPlayerSeat(int seat) => this._targetFactory().IsValidPlayerSeat(seat);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreProgramCardPoliciesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreProgramCardPoliciesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.IndexedSkillProgramInstance Source, global::CardGame.Core.SkillProgramCardPolicy Policy)> CardPolicies(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.CardKind? requiredKind = default, bool? eventTargetIsFemale = default, bool? eventSourceIsFemale = default) => this._targetFactory().CardPolicies(owner, kind, effectiveKind, requiredKind, eventTargetIsFemale, eventSourceIsFemale);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreIdentityConcealmentAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreIdentityConcealmentOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineIdentityConcealmentRuntime> _targetFactory = targetFactory;
    public bool TryBeginIdentityAppearanceForDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, int amount) => this._targetFactory().TryBeginIdentityAppearanceForDamage(attack, amount);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCharacterChengPuBoundaryMobileAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCharacterChengPuBoundaryMobileOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime> _targetFactory = targetFactory;
    public int BoundaryChengPuLihuoDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => this._targetFactory().BoundaryChengPuLihuoDamage(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.CardAttackState GetCardAttackState(long id) => this._targetFactory().GetCardAttackState(id);
    public void UpdateCardAttackState(long id, global::System.Func<global::CardGame.Core.CardAttackState?, global::CardGame.Core.CardAttackState> update) => this._targetFactory().UpdateCardAttackState(id, update);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreExtendedViewAsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreExtendedViewAsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> _targetFactory = targetFactory;
    public int ProgramConversionDamageBonus(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => this._targetFactory().ProgramConversionDamageBonus(attack);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreMatchLogRuntime))]
internal sealed class CoreCardDamageMaximumHpReplacementCoreMatchLogAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreCardDamageMaximumHpReplacementCoreMatchLogOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchLogRuntime> _targetFactory = targetFactory;
    public void AddLog(string type, string message, int? actor = default, int? target = default) => this._targetFactory().AddLog(type, message, actor, target);
}
