#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActionQueriesEntries
{
    internal global::CardGame.Core.NativeOwners.ActionQueryScope BeginActionQuery() => _resolve().BeginActionQuery();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildMobileWumouPrintedTargetActions(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card) => _resolve().BuildMobileWumouPrintedTargetActions(actor, card);
    internal global::CardGame.Core.RuleQueryEvaluation EvaluateAttackRange(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => _resolve().EvaluateAttackRange(player, excludedEquipmentId, excludedEquipmentSlot);
    internal global::CardGame.Core.RuleQueryEvaluation EvaluateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => _resolve().EvaluateDistance(source, target, excludedEquipmentId, excludedEquipmentSlot);
    internal global::CardGame.Core.RuleQueryEvaluation EvaluateSlashUseLimit(global::CardGame.Core.CharacterState player, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default) => _resolve().EvaluateSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().GetProgramCardIdentityMatches(owner, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetProgramViewAsConversions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind outputKind, bool forResponse, bool dyingUse = false) => _resolve().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSlashUseCards(global::CardGame.Core.CharacterState owner, bool ignoreSuitUseProhibition = false) => _resolve().GetSlashUseCards(owner, ignoreSuitUseProhibition);
}
