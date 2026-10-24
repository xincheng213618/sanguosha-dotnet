namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryHuangZhongMobileSkill = "boundary:liegong-mobile";

    // Mobile grants an additional rank-based permission. The existing rank
    // policy remains responsible for its native physical/virtual/provider paths.
    private bool IsBoundaryHuangZhongOrdinaryRange(CharacterState actor, CharacterState target, CardKind kind,
        int? excludedEquipmentId, EquipmentSlot? excludedEquipmentSlot) =>
        IsSlashCard(kind) && GetSkillBindingShard(actor).ProgramInstances.Any(i => i.SkillId == BoundaryHuangZhongMobileSkill) &&
        (excludedEquipmentId is null && excludedEquipmentSlot is null ? IsWithinAttackRange(actor.Seat, target.Seat) :
            IsGameFactionAttackRangeTarget(actor.Seat, target.Seat) || ConvertRuleValue(EvaluateDistance(actor, target, excludedEquipmentId, excludedEquipmentSlot)) <=
                ConvertRuleValue(EvaluateAttackRange(actor, excludedEquipmentId, excludedEquipmentSlot)));

    // Keep the issued right on the original native use/target receipt. Mobile
    // forbids the Dodge itself; old OL may still accept a paid, ineffective Dodge.
    private bool IsBoundaryHuangZhongUnrespondable(long frameId) => ActiveCardAttack is { } attack && attack.ResolutionId == frameId &&
        IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && FinalTargetSlashReceipts(frameId, attack.TargetSeat)
            .Any(r => r.SkillId == BoundaryHuangZhongMobileSkill && r.PreventCancellation);

    // The ordinary additive path already consumes these same native receipts.
    // This projection is used only before card-damage replacement finalization.
    private int BoundaryHuangZhongCardDamage(IDamageAttempt attack) => !attack.IsChainPropagation && attack is CardAttackHandle &&
        IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) ? FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat)
            .Where(r => r.SkillId == BoundaryHuangZhongMobileSkill).Sum(r => r.DamageBonus) : 0;
}
