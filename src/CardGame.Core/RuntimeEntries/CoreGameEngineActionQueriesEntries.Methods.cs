#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActionQueriesEntries
{
    internal NativeOwners.ActionQueryScope BeginActionQuery() => _resolve().BeginActionQuery();
    internal IReadOnlyList<LegalAction> BuildMobileWumouPrintedTargetActions(CharacterState actor, Card card) => _resolve().BuildMobileWumouPrintedTargetActions(actor, card);
    internal RuleQueryEvaluation EvaluateAttackRange(CharacterState player, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().EvaluateAttackRange(player, excludedEquipmentId, excludedEquipmentSlot);
    internal RuleQueryEvaluation EvaluateDistance(CharacterState source, CharacterState target, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().EvaluateDistance(source, target, excludedEquipmentId, excludedEquipmentSlot);
    internal RuleQueryEvaluation EvaluateSlashUseLimit(CharacterState player, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().EvaluateSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
    internal IReadOnlyList<GameEngine.ProgramCardIdentityMatch> GetProgramCardIdentityMatches(CharacterState owner, Card card) => _resolve().GetProgramCardIdentityMatches(owner, card);
    internal IReadOnlyList<CardConversionSource> GetProgramViewAsConversions(CharacterState owner, Card card, CardKind outputKind, bool forResponse, bool dyingUse = false) => _resolve().GetProgramViewAsConversions(owner, card, outputKind, forResponse, dyingUse);
    internal IReadOnlyList<Card> GetSlashUseCards(CharacterState owner, bool ignoreSuitUseProhibition = false) => _resolve().GetSlashUseCards(owner, ignoreSuitUseProhibition);
}
