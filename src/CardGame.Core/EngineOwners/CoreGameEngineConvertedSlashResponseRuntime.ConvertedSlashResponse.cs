#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineConvertedSlashResponseRuntime
{
    internal bool HasConvertedSlashSameColorResponseRestriction(NativeOwners.CardAttackHandle attack)
    {
        if (attack.ConversionSource is null && attack.ProgramSkillCardUseFrameId is null)
            return false;
        return _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat], SkillProgramCardPolicyKind.ConvertedSlashSameColorResponseOnly, attack.EffectiveCardKind).Any();
    }

    internal bool? ConvertedSlashSameColorResponseColor(NativeOwners.CardAttackHandle attack)
    {
        if (!HasConvertedSlashSameColorResponseRestriction(attack))
            return null;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat];
        if (attack.PhysicalCards.Count == 0)
            return null;
        var colors = attack.PhysicalCards.Select(card => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, card))).ToArray();
        return colors.All(color => color == true) ? true : colors.All(color => color == false) ? false : null;
    }
}
