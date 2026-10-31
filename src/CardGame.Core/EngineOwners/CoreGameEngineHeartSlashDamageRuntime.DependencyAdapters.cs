#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreHeartSlashDamageCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHeartSlashDamageCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed class CoreHeartSlashDamageCharacterWuJingOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHeartSlashDamageCharacterWuJingOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Suit? WuJingLiubingUsedSuit(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? conversions, global::CardGame.Core.Suit? ordinary) => this._targetFactory().WuJingLiubingUsedSuit(actorSeat, card, effectiveKind, ids, conversions, ordinary);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreHeartSlashDamageCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreHeartSlashDamageCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().ApplyProgramUseAppearance(owner, card, source);
    public global::CardGame.Core.Suit? CaptureUsedCardSuit(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.Card card) => this._targetFactory().CaptureUsedCardSuit(actorSeat, physicalIds, card);
}
