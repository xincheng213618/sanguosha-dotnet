namespace CardGame.Core;

public sealed partial class GameEngine
{
    private PublicSlashTraits CreatePublicSlashTraits(CharacterState actor, LegalAction action,
        CardKind kind, bool? declaredColor, GameSnapshot? ownerView)
    {
        // Only the acting player's own capabilities and public mode/debt state
        // contribute. Reuse native nature sources without creating an attack,
        // capturing a use receipt or consuming a once-per-turn allowance.
        var nature = GetDamageNature(kind);
        if (ShenwuEnabled(actor, "shenwu:zhaohuo") || HasWeaponAbility(actor, CardKind.CrimsonSoulZither))
            nature = DamageNature.Fire;
        nature = _modeRuntime.TransformDamageNature(nature);
        if (action.TargetSeat is { } target && HasShouliDamageDebt(target))
            nature = DamageNature.Thunder;
        if (HasRuntimeSkill(actor, MobileMaChaoYuli))
            nature = DamageNature.Thunder;
        var ignoresArmor = action.TargetSeat is { } armorTarget
            ? HasCardArmorBypass(actor, _players[armorTarget], kind)
            : HasArmorBypass(actor);
        if (!ignoresArmor && declaredColor == false && HasYuanchouCardPolicy)
        {
            ignoresArmor = HasCardPolicy(actor, SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections);
            if (!ignoresArmor && action.TargetSeat is { } directedTarget && ownerView is not null)
            {
                // The native rule also grants bypass against a policy owner.
                // Only policy sources already visible to this exact actor can
                // influence the estimate; hidden opponent grants stay hidden.
                var visible = VisiblePredictionSkillIds(ownerView.Players.Single(player => player.Seat == directedTarget));
                ignoresArmor = CardPolicies(_players[directedTarget],
                        SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections)
                    .Any(binding => visible.Contains(binding.Source.SkillId));
            }
        }
        return new(declaredColor, ignoresArmor, nature);
    }
}
