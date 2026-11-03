#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangZhongBoundaryMobileRuntime
{
    internal const string BoundaryHuangZhongMobileSkill = "boundary:liegong-mobile";
    internal bool IsBoundaryHuangZhongOrdinaryRange(CharacterState actor, CharacterState target, CardKind kind, int? excludedEquipmentId, EquipmentSlot? excludedEquipmentSlot) => CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.Any(i => i.SkillId == BoundaryHuangZhongMobileSkill) && _attackRanges.IsWithinAttackRange(actor.Seat, target.Seat, excludedEquipmentId, excludedEquipmentSlot);
    internal bool IsBoundaryHuangZhongUnrespondable(long frameId) => _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == frameId && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && _coreFinalTargetSlashOperations.FinalTargetSlashReceipts(frameId, attack.TargetSeat).Any(r => r.SkillId == BoundaryHuangZhongMobileSkill && r.PreventCancellation);
}
