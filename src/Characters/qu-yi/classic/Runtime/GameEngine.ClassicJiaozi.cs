namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Classic Jiaozi has separate causing/receiving timings, even for self damage.
    // Other participant modifiers, including the local round-scaled variant, retain their own contracts.
    private static int ClassicJiaoziDamageOccurrences(SkillProgramDamageModifier modifier,
        int sourceSeat, int targetSeat, bool sourceLess) =>
        modifier.Condition == SkillProgramDamageModifierCondition.OwnerUniqueMaximumHand &&
        !sourceLess && sourceSeat == targetSeat ? 2 : 1;

    private int ClassicJiaoziPublicDamageBonus(GameSnapshot view, int sourceSeat, int targetSeat,
        bool sourceLess, int paidHandCards)
    {
        int HandCount(PlayerSnapshot player) => Math.Max(0,
            player.HandCount - (!sourceLess && player.Seat == sourceSeat ? paidHandCards : 0));
        var participants = sourceLess ? new[] { targetSeat } : new[] { sourceSeat, targetSeat }.Distinct();
        return participants.Where(seat => view.Players.Single(p => p.Seat == seat) is { IsAlive: true } owner &&
            view.Players.Where(p => p.IsAlive && p.Seat != seat).All(p => HandCount(p) < HandCount(owner)))
            .Sum(seat =>
            {
                var visible = VisiblePredictionSkillIds(view.Players.Single(p => p.Seat == seat));
                return GetSkillBindingShard(_players[seat]).ProgramInstances.Where(instance => visible.Contains(instance.SkillId))
                    .SelectMany(instance => instance.Program.DamageModifiers)
                    .Where(modifier => modifier.Condition == SkillProgramDamageModifierCondition.OwnerUniqueMaximumHand)
                    .Sum(modifier => modifier.Amount * ClassicJiaoziDamageOccurrences(modifier, sourceSeat, targetSeat, sourceLess));
            });
    }
}
