namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasNativeDamageArmorPolicy(int sourceSeat, bool sourceLess,
        SkillProgramCardPolicyKind kind, IReadOnlySet<string>? visibleSkillIds = null) =>
        !sourceLess && IsValidPlayerSeat(sourceSeat) && _players[sourceSeat].IsAlive &&
        CardPolicies(_players[sourceSeat], kind)
            .Any(binding => visibleSkillIds is null || visibleSkillIds.Contains(binding.Source.SkillId));

    private bool BypassesNativeDamageArmor(IDamageAttempt attack) =>
        WushuangArmorBypass(attack) || attack.SourceSeat != attack.TargetSeat &&
        HasNativeDamageArmorPolicy(attack.SourceSeat, attack.IsSourceLess,
            SkillProgramCardPolicyKind.BypassOtherParticipantArmor);

    private int NativeDamageArmorMultiplier(IDamageAttempt attack) =>
        Math.Max(WushuangArmorDamageMultiplier(attack),
            HasNativeDamageArmorPolicy(attack.SourceSeat, attack.IsSourceLess,
                SkillProgramCardPolicyKind.DoubleArmorDamage) ? 2 : 1);
}
