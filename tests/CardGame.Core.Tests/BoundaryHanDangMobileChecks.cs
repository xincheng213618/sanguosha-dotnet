using CardGame.Core;
using static BoundaryHanDangScenario;

internal static class BoundaryHanDangMobileChecks
{
    public static void MountedRangeNonBasicCostAndOriginalForeignDiscardCold()
    {
        var (registry, game) = CreateScene(); var initialRange = game.GetAttackRange(0); Equip(game, CardKind.DefensiveHorse);
        Require(initialRange == 1 && game.GetAttackRange(0) > 4, "A real defensive mount immediately grants unlimited attack range through mobile Gongqi's continuous rule.");
        var horse = Player(game, 0).Equipment.Single(card => card.Kind == CardKind.DefensiveHorse).Id;
        var basic = EnsureCard(game, CardKind.Slash); var before = SnapshotJson.Serialize(game.CreateSnapshot(0));
        Require(!game.Submit(new UseProgramSkillCommand(0, Gongqi, "discard-nonbasic-and-target-card", [basic], [2], game.Revision, Decision(game)!.PromptId)).Accepted && before == SnapshotJson.Serialize(game.CreateSnapshot(0)),
            "A real basic card cannot pay this edition's nonbasic cost.");
        var peerHand = Player(game, 2).HandCount; Use(game, Gongqi, "discard-nonbasic-and-target-card", [horse], [2]); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Gongqi);
        Require(root.SelectedTargetSeats.SequenceEqual([2]) && game.GetAttackRange(0) == initialRange && Player(game, 0).Equipment.All(card => card.Id != horse), "Discarding the last actual mount removes unlimited range before the original cost child returns, retaining the same selected role.");
        Cold(ref game, registry); Continue(game);
        Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        Require(Decision(game)!.Choices.All(choice => choice.Parameters.GetValueOrDefault("card-owner-seat") == "2" && choice.Cards.Count == 0), "After the paid cost, only the originally selected role's opaque hand positions are offered; there is no optional decline.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"); Play(game);
        Require(Player(game, 2).HandCount == peerHand - 1 && game.CardMovements.Count(move => move.CardId == horse && move.From == CardLocation.Equipment(0) && move.To == CardLocation.DiscardPile && move.Reason.Value == CostReason) == 1 &&
            !game.GetLegalActions(0).Any(action => action.ProgramSkillId == Gongqi), "The original nonbasic equipment cost is paid once, its chosen target really loses one card, and this Play's opportunity is consumed.");
        Equip(game, CardKind.OffensiveHorse);
        Require(game.GetAttackRange(0) > 4 && Player(game, 0).TurnSlashSuitAllowances is null, "The other native mount slot also enables range without granting the OL same-suit Slash allowance.");
    }

    public static void FrozenRespondersHandEquippedWeaponsAndNativeDrawChildrenCold()
    {
        var (registry, game) = CreateScene(); Equip(game, CardKind.DefensiveHorse); var peerWeapon = EquipPeerWeapon(game, 1); var ownWeapon = EnsureCard(game, CardKind.Crossbow);
        Use(game, Jiefan, "aid-by-attack-range", targets: [2]); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Jiefan);
        var started = game.Events.Select(entry => entry.Payload).OfType<ProgramAttackRangeAidStartedEvent>().Single();
        Require(started.ResponderSeats.SequenceEqual([0, 1, 3]) && Decision(game)!.PlayerSeat == 0 && Decision(game)!.Choices.Any(choice => choice.Cards.SequenceEqual([ownWeapon])),
            "The genuine mounted owner and the two adjacent attackers are frozen in action order, and the owner's actual hand weapon is a legal response.");
        Answer(game, choice => choice.Cards.SequenceEqual([ownWeapon])); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Jiefan);
        Require(root.AttackRangeAid is { ResponderIndex: 0, Payment: { CardId: var paid, From: var from } } && paid == ownWeapon && from == CardLocation.Hand(0) &&
            Player(game, 0).Hand.All(card => card.Id != ownWeapon), "The first responder's hand weapon is actually discarded while the original cursor remains at that responder's native loss child.");
        Cold(ref game, registry); Continue(game); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Jiefan && prompt.PlayerSeat == 1);
        Answer(game, choice => choice.Cards.SequenceEqual([peerWeapon])); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch && prompt.PlayerSeat == 1);
        root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        Require(root.AttackRangeAid is { ResponderIndex: 1, Payment.From: var equipmentFrom } && equipmentFrom == CardLocation.Equipment(1) && Player(game, 1).Equipment.All(card => card.Id != peerWeapon),
            "An equipped weapon uses its real equipment source and native loss child before the next responder is exposed.");
        Continue(game); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Jiefan && prompt.PlayerSeat == 3); var hand = Player(game, 2).HandCount;
        Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "attack-range-aid-draw"); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch && prompt.PlayerSeat == 2);
        root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        Require(root.AttackRangeAid is { ResponderIndex: 2, Payment: { Stage: ProgramAttackRangeAidPaymentStage.MovementChildren, Draw: { RequestedCount: 1, ActualDrawIds.Count: 1, RecipientSeat: 2 } } } &&
            Player(game, 2).HandCount == hand + 1 && root.AttackRangeAid.ResponderSeats.SequenceEqual(started.ResponderSeats), "The final response returns one genuine native draw and pauses on its actual gained-card child without advancing or recalculating the original responder list.");
        Cold(ref game, registry); Continue(game); Play(game);
        var choices = game.Events.Select(entry => entry.Payload).OfType<ProgramAttackRangeAidChoiceResolvedEvent>().Where(entry => entry.FrameId == started.FrameId).ToArray();
        Require(choices.Length == 3 && choices.Count(entry => entry.DiscardedWeaponCardId == ownWeapon) == 1 && choices.Count(entry => entry.DiscardedWeaponCardId == peerWeapon) == 1 && choices.Sum(entry => entry.DrawnCardIds.Count) == 1 &&
            game.CardMovements.Count(move => move.CardId == ownWeapon && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing && move.Reason.Value == AidReason) == 1 &&
            !game.GetLegalActions(0).Any(action => action.ProgramSkillId == Jiefan), "Every original responder pays once across cold returns, and mobile Jiefan remains game-limited with no OL first-round refund.");
    }
    private static void Cold(ref GameEngine game, ContentRegistry registry)
    {
        var source = game; var before = Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(source.CreateSnapshot(viewer))).ToArray();
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(source.CreateCheckpoint())), registry);
        Require(before.SequenceEqual(Enumerable.Range(0, 4).Select(viewer => SnapshotJson.Serialize(restored.CreateSnapshot(viewer)))), "Original responders, paid HE material, native outcome and viewer snapshots survive a true JSON cold restore."); game = restored;
    }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
