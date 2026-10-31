#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTreatedWoundedRuntime
{
    internal bool UsesTreatedWounded => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.OtherWuTreatedWoundedDuringOwnTurn);

    internal bool IsRuleWounded(CharacterState player) => player.HasHpValue && ((player.HappyQiaoHealth is { } health && _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(player.Seat)is null ? health.AnyWounded : player.Hp < player.MaxHp) || UsesTreatedWounded && _characterSunHaoOlOperations.Value.IsGuimingTreatedWounded(player));
}
