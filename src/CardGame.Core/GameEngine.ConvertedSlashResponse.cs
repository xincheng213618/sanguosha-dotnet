namespace CardGame.Core;

public sealed partial class GameEngine
{
    // 父魂 applies to every converted Slash, including the granted 武圣.
    // A mixed-color conversion is colorless; null is its color, not permission
    // to respond freely. Test the restriction separately before using this value.
    private bool HasConvertedSlashSameColorResponseRestriction(CardAttackHandle attack)
    {
        if (attack.ConversionSource is null && attack.ProgramSkillCardUseFrameId is null)
            return false;
        return CardPolicies(_players[attack.CardUserSeat],
            SkillProgramCardPolicyKind.ConvertedSlashSameColorResponseOnly,
            attack.EffectiveCardKind).Any();
    }

    private bool? ConvertedSlashSameColorResponseColor(CardAttackHandle attack)
    {
        if (!HasConvertedSlashSameColorResponseRestriction(attack)) return null;
        var source = _players[attack.CardUserSeat];
        if (attack.PhysicalCards.Count == 0) return null;
        var colors = attack.PhysicalCards.Select(card => SuitColor(EffectiveSuit(source, card))).ToArray();
        return colors.All(color => color == true) ? true : colors.All(color => color == false) ? false : null;
    }
}
