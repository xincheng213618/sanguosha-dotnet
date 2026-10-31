#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePublicSlashTraitsRuntime
{
    internal PublicSlashTraits CreatePublicSlashTraits(CharacterState actor, LegalAction action, CardKind kind, bool? declaredColor, GameSnapshot? ownerView)
    {
        // Only the acting player's own capabilities and public mode/debt state
        // contribute. Reuse native nature sources without creating an attack,
        // capturing a use receipt or consuming a once-per-turn allowance.
        var nature = CoreNativeCardClassificationsRuntime.GetDamageNature(kind);
        if (_coreShenwuNpcProgramsOperations.ShenwuEnabled(actor, "shenwu:zhaohuo") || _coreAdvancedWeaponsOperations.HasWeaponAbility(actor, CardKind.CrimsonSoulZither))
            nature = DamageNature.Fire;
        nature = _modeRuntimeOperations._modeRuntime.TransformDamageNature(nature);
        if (action.TargetSeat is { } target && _characterMaChaoAnniversaryGodOperations.HasShouliDamageDebt(target))
            nature = DamageNature.Thunder;
        if (_coreNationalSkillsOperations.HasRuntimeSkill(actor, "mobile-god:yuli"))
            nature = DamageNature.Thunder;
        var ignoresArmor = action.TargetSeat is { } armorTarget ? _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[armorTarget], kind) : _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(actor);
        if (!ignoresArmor && declaredColor == false && _coreDirectedCardPolicyQueriesOperations.HasYuanchouCardPolicy)
        {
            ignoresArmor = _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections);
            if (!ignoresArmor && action.TargetSeat is { } directedTarget && ownerView is not null)
            {
                // The native rule also grants bypass against a policy owner.
                // Only policy sources already visible to this exact actor can
                // influence the estimate; hidden opponent grants stay hidden.
                var visible = CoreGameEnginePublicDamagePredictionRuntime.VisiblePredictionSkillIds(ownerView.Players.Single(player => player.Seat == directedTarget));
                ignoresArmor = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[directedTarget], SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections).Any(binding => visible.Contains(binding.Source.SkillId));
            }
        }

        return new(declaredColor, ignoresArmor, nature);
    }
}
