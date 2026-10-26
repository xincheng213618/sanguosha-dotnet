using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ZhouYuChecks
{
    public static void ChiyunFireOptionReturnsToOriginalPaidFrameAfterCold()
    {
        var s = Scene.Start("chiyun-fire-return", 4, [Keeper], NoEnemies(),
            [new(CardKind.Slash, Suit.Heart, 4), new(CardKind.Slash, Suit.Club, 5)]);
        // The real limited skill retains option one, so the independent NPC
        // must take the fire branch through its ordinary published decision.
        AnswerXiongzi(s, "keep1");
        s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.ProgramTrigger } &&
            s.P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "target"));
        s.Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "target" && choice.Targets.SequenceEqual([1]));
        s.Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "pick");
        s.Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "confirm");
        s.Reach(() => s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame =>
            frame.ShiZhouYu is { Operation: "chiyun-option", Stage: ShiZhouYuComposition.ChiyunStageOption }));
        var given = s.E<ShiZhouYuChiyunGivenEvent>().Single();
        var paidId = given.CardIds.Single();
        s.Need(s.P is { PlayerSeat: 1 } && s.P.Choices.All(choice => choice.Parameters["branch"] == "option1") &&
            s.V(1).Hand.Any(card => card.Id == paidId),
            "The real gift has settled on its original frame before the recipient receives its independent fire-only choice.");
        s.Cold();
        s.Reach(() => s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame =>
            frame.Id == given.FrameId && frame.ShiZhouYu is
                { Operation: "chiyun-option", Stage: ShiZhouYuComposition.ChiyunStageResolveDamage } && frame.AttackAttempt is not null));
        var issued = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == given.FrameId);
        var gainParentId = issued.WindowContext!.ParentFrameId;
        s.Need(issued.SkillId == ShiZhouYuComposition.ChiyunSkill && issued.ShiZhouYu!.PickedCards.SequenceEqual([paidId]) &&
            s.E<ShiZhouYuChiyunOptionEvent>() is [{ Option: 1, TargetSeat: 1 }] &&
            s.G.ResolutionStack.Any(frame => frame is CardsMovedTriggerWindowFrame && frame.Id == gainParentId),
            "The issued fire attempt retains the chiyun-option instruction, paid entity and suspended original gain-window parent.");
        s.Cold();
        s.Reach(() => s.G.ResolutionStack.All(frame => frame.Id != given.FrameId && frame.Id != gainParentId));
        s.Need(s.V(1).Hp == 7 && s.E<ShiZhouYuChiyunGivenEvent>().Length == 1 &&
            s.E<ShiZhouYuChiyunOptionEvent>().Length == 1 &&
            s.E<DamageAppliedEvent>().Count(damage => damage.TargetSeat == 1) == 1 &&
            s.G.CardMovements.Count(move => move.CardId == paidId && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Hand(1) && move.Reason.Value == ShiZhouYuComposition.ChiyunGiveReason) == 1,
            "Completing the real fire child finishes the original paid frame once instead of rejecting chiyun-option stage12 or repeating the gift/damage.");
        s.Cold(); s.Replay();
    }
}
