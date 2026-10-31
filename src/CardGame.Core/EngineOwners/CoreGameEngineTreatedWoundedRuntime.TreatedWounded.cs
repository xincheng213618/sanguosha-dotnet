#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTreatedWoundedRuntime
{
    internal bool UsesTreatedWounded => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.OtherWuTreatedWoundedDuringOwnTurn);

    internal bool IsRuleWounded(CharacterState player) => player.HasHpValue && ((player.HappyQiaoHealth is { } health && _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(player.Seat)is null ? health.AnyWounded : player.Hp < player.MaxHp) || UsesTreatedWounded && _characterSunHaoOlOperations.IsGuimingTreatedWounded(player));
}
