#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreActionQueriesCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> FindProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> FindProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false, int? requestedUseTarget = default);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> FindSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false);
}

internal interface ICoreActionQueriesKernelCoreGameEngineActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ActionQueryScope CreateActionQueryScope(global::CardGame.Core.NativeOwners.ActionQuery? previous = default);
}

internal interface ICoreActionQueriesCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActionsCore(global::CardGame.Core.CharacterState actor, bool includeProgramActions, int? selectedPhysicalCardId, global::CardGame.Core.Card? nativeOriginalCard, bool ignoreForcedHandSelection = false);
}

internal interface ICoreActionQueriesCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation CalculateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    global::CardGame.Core.RuleQueryEvaluation CalculateAttackRange(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
    global::CardGame.Core.RuleQueryEvaluation CalculateSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}
