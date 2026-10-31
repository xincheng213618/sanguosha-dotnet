#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardUseModulesEntries
{
    internal IReadOnlyList<int> ApplyTurnCardGroupTargetRestrictions(CharacterState source, IReadOnlyList<int> targets) => _resolve().ApplyTurnCardGroupTargetRestrictions(source, targets);
    internal bool DistanceCardUseExemption130HasUnlimitedTurnRuleModifier(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption130HasUnlimitedTurnRuleModifier(q);
    internal void ExpireDeadOwnerDamageModifiers(int ownerSeat) => _resolve().ExpireDeadOwnerDamageModifiers(ownerSeat);
    internal void ExpireOwnerTurnStartDamageModifiers(int ownerSeat) => _resolve().ExpireOwnerTurnStartDamageModifiers(ownerSeat);
    internal void ExpireTurnCardUseEffects(int turnNumber, int turnSeat) => _resolve().ExpireTurnCardUseEffects(turnNumber, turnSeat);
    internal IReadOnlyList<LegalAction> FilterTurnCardUseRestrictions(CharacterState actor, IEnumerable<LegalAction> actions, Card? nativeOriginalCard = default) => _resolve().FilterTurnCardUseRestrictions(actor, actions, nativeOriginalCard);
    internal int GetAdditiveTurnRuleModifier(int actorSeat, SkillRuleQuery query) => _resolve().GetAdditiveTurnRuleModifier(actorSeat, query);
    internal void GrantProgramTurnCardActionProhibition(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, IReadOnlyList<CardActionType> actionTypes) => _resolve().GrantProgramTurnCardActionProhibition(frame, cardKinds, actionTypes);
    internal void GrantProgramTurnCardConversion(ProgramSkillFrame frame, string sourceBind, SkillProgramCardColorRelation colorRelation, CardKind outputKind) => _resolve().GrantProgramTurnCardConversion(frame, sourceBind, colorRelation, outputKind);
    internal void GrantProgramTurnCardDamageModifier(ProgramSkillFrame frame, IReadOnlyList<CardKind> cardKinds, int amount, SkillProgramDamageModifierExpiration expiration, SkillProgramDamageModifierSourceScope sourceScope) => _resolve().GrantProgramTurnCardDamageModifier(frame, cardKinds, amount, expiration, sourceScope);
    internal void GrantProgramTurnCardTargetRestriction(ProgramSkillFrame frame, SkillProgramCardTargetRestriction restriction, int targetSeat) => _resolve().GrantProgramTurnCardTargetRestriction(frame, restriction, targetSeat);
    internal void GrantProgramTurnHandCardProhibition(ProgramSkillFrame frame, int targetSeat) => _resolve().GrantProgramTurnHandCardProhibition(frame, targetSeat);
    internal void GrantProgramTurnHandColorRestriction(ProgramSkillFrame frame, string sourceBind, int targetSeat, bool useFrozenSuit = false) => _resolve().GrantProgramTurnHandColorRestriction(frame, sourceBind, targetSeat, useFrozenSuit);
    internal void GrantProgramTurnRuleModifier(ProgramSkillFrame frame, SkillRuleQuery query, SkillRuleOperation operation, int amount, IReadOnlyList<CardKind> cardKinds, PlayerMarkerKind? amountFromMarker = default, SkillProgramDamageModifierExpiration expiration = (SkillProgramDamageModifierExpiration)0) => _resolve().GrantProgramTurnRuleModifier(frame, query, operation, amount, cardKinds, amountFromMarker, expiration);
    internal bool HasTurnCardTargetRestriction(int actorSeat, SkillProgramCardTargetRestriction restriction, int? targetSeat = default) => _resolve().HasTurnCardTargetRestriction(actorSeat, restriction, targetSeat);
    internal bool HasUnlimitedTurnRuleModifier(int actorSeat, SkillRuleQuery query, CardKind? effectiveCardKind = default) => _resolve().HasUnlimitedTurnRuleModifier(actorSeat, query, effectiveCardKind);
    internal bool IsCardUseForbidden(int actorSeat, CardKind effectiveKind, CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default) => _resolve().IsCardUseForbidden(actorSeat, effectiveKind, actionType, ignoreIssuedPlayBan, existingUseFrameId);
    internal bool IsTurnHandCardRestricted(CharacterState player, Card card) => _resolve().IsTurnHandCardRestricted(player, card);
    internal bool TryGetLegalActionEffectiveCardKind(CharacterState actor, LegalAction action, scoped out CardKind effectiveKind, Card? nativeOriginalCard = default) => _resolve().TryGetLegalActionEffectiveCardKind(actor, action, out effectiveKind, nativeOriginalCard);
    internal void ValidateProgramTurnEffectGrant(ProgramSkillFrame frame) => _resolve().ValidateProgramTurnEffectGrant(frame);
}
