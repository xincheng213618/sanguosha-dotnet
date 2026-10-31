#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenDengOlRuntime
{
    internal int GetFengjiRoundAdjustment(int seat, SkillRuleQuery query)
    {
        if (_coreActualRoundProgressRoundNumberAccess._roundNumber == 0)
            return 0;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory();
        var options = facts.OfType<ProgramFengjiOptionChosenEvent>().Where(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber).ToArray();
        if (options.Length == 0)
            return 0;
        var swapsByOwner = facts.OfType<ProgramXuanhuiEffectsSwappedEvent>().Where(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber).GroupBy(e => e.OwnerSeat).ToDictionary(group => group.Key, group => group.Count());
        var total = 0;
        foreach (var option in options)
        {
            if (query == SkillRuleQuery.DrawCount && option.Option != FengjiOptionKind.Draw || query == SkillRuleQuery.SlashLimit && option.Option != FengjiOptionKind.Slash)
                continue;
            var swapCount = swapsByOwner.GetValueOrDefault(option.OwnerSeat);
            if (option.Accepted)
            {
                if (option.OwnerSeat == seat)
                    total += swapCount % 2 == 0 ? -1 : 2;
                else if (option.RecipientSeat == seat)
                    total += swapCount % 2 == 0 ? 2 : -1;
            }
            else if (option.OwnerSeat == seat)
            {
                total += 1;
            }
        }

        return total;
    }

    SkillProgramStepOutcome IChenDengProgramHost.FengjiRoundChoice(ProgramSkillFrame frame, SkillProgramEffect effect) => FengjiProgramRoundChoice(frame, effect);
    SkillProgramStepOutcome IChenDengProgramHost.XuanhuiSwapEffects(ProgramSkillFrame frame, SkillProgramEffect effect) => XuanhuiProgramSwapEffects(frame, effect);
}
