#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreWushuangPublicPredictionCoreProgramEventHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangPublicPredictionCoreProgramEventHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<T> ProgramEventHistory<T>()
        where T : global::CardGame.Core.IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime))]
internal sealed class CoreWushuangPublicPredictionCorePublicDamagePredictionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangPublicPredictionCorePublicDamagePredictionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicDamagePredictionRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.PublicDamagePrediction PredictVisibleDamage(global::CardGame.Core.GameSnapshot view, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.CardKind? kind, bool cardUse, bool sourceLess, bool chain, bool finalized, int paidHandCards, bool ignoresEquipmentArmor, global::CardGame.Core.DamageNature? damageNature = default, global::CardGame.Core.GameEngine.IDamageAttempt? issuedAttack = default, bool wushuangTactics = false, global::CardGame.Core.Suit prospectiveSuit = (global::CardGame.Core.Suit)500, int additionalIncomingSegments = 0, int priorTargetSegments = 0) => this._targetFactory().PredictVisibleDamage(view, sourceSeat, targetSeat, amount, kind, cardUse, sourceLess, chain, finalized, paidHandCards, ignoresEquipmentArmor, damageNature, issuedAttack, wushuangTactics, prospectiveSuit, additionalIncomingSegments, priorTargetSegments);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime))]
internal sealed class CoreWushuangPublicPredictionCoreWushuangModeProjectionAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangPublicPredictionCoreWushuangModeProjectionOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreWushuangModeProjectionRuntime> _targetFactory = targetFactory;
    public bool IsWushuangMode { get => this._targetFactory().IsWushuangMode; }
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime))]
internal sealed class CoreWushuangPublicPredictionCorePlayCardHistoryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangPublicPredictionCorePlayCardHistoryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePlayCardHistoryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => this._targetFactory().ApplyProgramUseAppearance(owner, card, source);
    public global::CardGame.Core.Suit? CaptureUsedCardSuit(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> physicalIds, global::CardGame.Core.Card card) => this._targetFactory().CaptureUsedCardSuit(actorSeat, physicalIds, card);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreWushuangPublicPredictionCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangPublicPredictionCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().GetSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime))]
internal sealed class CoreWushuangPublicPredictionCoreNativeEquipmentAndFactionQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangPublicPredictionCoreNativeEquipmentAndFactionQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeEquipmentAndFactionQueriesRuntime> _targetFactory = targetFactory;
    public bool HasArmorBypass(global::CardGame.Core.CharacterState player) => this._targetFactory().HasArmorBypass(player);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime))]
internal sealed class CoreWushuangPublicPredictionCoreCardAttackStateAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreWushuangPublicPredictionCoreCardAttackStateOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardAttackStateRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.Card GetAttackCard(int id) => this._targetFactory().GetAttackCard(id);
}
