#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime))]
internal sealed class CoreActionQueriesCoreCardConversionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActionQueriesCoreCardConversionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> FindProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => this._targetFactory().FindProgramCardIdentityMatches(owner, card);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> FindProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default) => this._targetFactory().FindProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse, requestedUseTarget);
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> FindSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false) => this._targetFactory().FindSlashUseCards(owner, ignoreSuitUseProhibition);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime))]
internal sealed class CoreActionQueriesCoreNativeLegalActionsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActionQueriesCoreNativeLegalActionsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeLegalActionsRuntime> _targetFactory = targetFactory;
    public global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActionsCore(global::CardGame.Core.CharacterState actor, bool includeProgramActions, int? selectedPhysicalCardId, global::CardGame.Core.Card? nativeOriginalCard, bool ignoreForcedHandSelection = false) => this._targetFactory().BuildLegalActionsCore(actor, includeProgramActions, selectedPhysicalCardId, nativeOriginalCard, ignoreForcedHandSelection);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime))]
internal sealed class CoreActionQueriesCoreRuleQueriesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreActionQueriesCoreRuleQueriesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.RuleQueryEvaluation CalculateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().CalculateDistance(source, target, excludedEquipmentId, excludedEquipmentSlot);
    public global::CardGame.Core.RuleQueryEvaluation CalculateAttackRange(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().CalculateAttackRange(player, excludedEquipmentId, excludedEquipmentSlot);
    public global::CardGame.Core.RuleQueryEvaluation CalculateSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => this._targetFactory().CalculateSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
}
