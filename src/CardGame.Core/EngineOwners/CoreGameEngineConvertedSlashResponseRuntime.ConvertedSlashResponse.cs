#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineConvertedSlashResponseRuntime
{
    internal bool HasConvertedSlashSameColorResponseRestriction(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (attack.ConversionSource is null && attack.ProgramSkillCardUseFrameId is null)
            return false;
        return _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat], SkillProgramCardPolicyKind.ConvertedSlashSameColorResponseOnly, attack.EffectiveCardKind).Any();
    }

    internal bool? ConvertedSlashSameColorResponseColor(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (!HasConvertedSlashSameColorResponseRestriction(attack))
            return null;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.CardUserSeat];
        if (attack.PhysicalCards.Count == 0)
            return null;
        var colors = attack.PhysicalCards.Select(card => global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, card))).ToArray();
        return colors.All(color => color == true) ? true : colors.All(color => color == false) ? false : null;
    }
}
