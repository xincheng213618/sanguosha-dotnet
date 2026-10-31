using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class WushuangModeChecks
{
    public static void HandLimitExemptTacticsSkipBothManualAndAutomaticDiscard()
    {
        foreach (var (seed, tactic, kind) in new[] { (26, 2002, CardKind.Dodge), (28, 2003, CardKind.Peach) })
        foreach (var interactive in new[] { false, true })
        {
            var (game, registry) = Start(false, seed, initialHp: 3, peaches: kind == CardKind.Peach, interactiveDiscard: interactive);
            Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
            Buy(game, Shop(game).Offers.Single(offer => offer.TacticId == tactic).Id); Play(game);
            var hand = game.CreateSnapshot(0).Players[0].HandCount;
            Require(hand > 3 && game.CreateSnapshot(0).Players[0].Hand.All(card => card.Kind == kind),
                "The real low-HP owner holds more exempt physical cards than its ordinary hand limit.");
            var discarded = game.CardMovements.Count(move => move.From == CardLocation.Hand(0) && move.Reason == CardMoveReasons.HandLimitDiscard);
            End(game); Shopping(game);
            Require(game.CreateSnapshot(0).Players[0].HandCount == hand &&
                game.CardMovements.Count(move => move.From == CardLocation.Hand(0) && move.Reason == CardMoveReasons.HandLimitDiscard) == discarded,
                "Dodge/Peach exemptions must prevent both a manual discard debt and automatic card loss.");
            Cold(game, registry);
        }
    }
}
