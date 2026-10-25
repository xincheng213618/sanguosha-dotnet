using CardGame.Core;
using static BoundaryFuHuangHouScenario;

namespace CardGame.Core.Tests;

internal static class BoundaryFuHuangHouMobileChecks
{
    public static void ClassicQiuyuanUsesEachActualMultiTargetAndDodgeOnlyDistantAidCold()
    {
        const string classic = "classic:qiuyuan";
        var (registry, game) = CreateScene(classicAid: true);
        Need(new[] { 0, 2 }.All(seat => Player(game, seat).Skills?.Any(skill => skill.ContentId == classic) == true),
            "Both original target seats are the real generals holding classic Qiuyuan before this multi-target Slash is issued.");
        var actor = new[] { 1, 3 }.First(seat => Player(game, seat).Hand.Any(c => c.Kind == CardKind.Slash) &&
            Player(game, seat == 1 ? 3 : 1).Hand.Any(c => c.Kind == CardKind.Dodge) && Player(game, seat == 1 ? 3 : 1).Hand.Any(c => c.Kind == CardKind.Peach));
        var donor = actor == 1 ? 3 : 1;
        Reach(game, p => p is { Kind: DecisionKind.PlayCard } && p.PlayerSeat == actor);
        Accept(game, new UseProgramSkillCommand(actor, "fixture:boundary-fuhuanghou-quiet", "two-original-targets", [], [], game.Revision, Decision(game)!.PromptId));
        Reach(game, p => p is { Kind: DecisionKind.PlayCard } && p.PlayerSeat == actor);
        var slash = Player(game, actor).Hand.First(c => c.Kind == CardKind.Slash).Id;
        Need(!game.Submit(new PlayCardCommand(actor, slash, [donor], game.Revision, Decision(game)!.PromptId)).Accepted,
            "The aid recipient is outside the actual user's normal Slash distance even though the native use allows two original targets.");
        var action = game.GetLegalActions(actor).Single(a => a.CardId == slash && a.Kind == LegalActionKind.Slash &&
            a.TargetSeats.Count == 2 && a.TargetSeats.Order().SequenceEqual([0, 2]));
        Accept(game, new PlayCardCommand(actor, slash, action.TargetSeats, game.Revision, Decision(game)!.PromptId, action.PlayedCardKind, action.TargetCardId)
            { ConversionSource = action.ConversionSource });
        Reach(game, p => Optional(p, classic)); var first = Decision(game)!.PlayerSeat; Activate(game);
        Reach(game, p => p?.Choices.Any(c => c.Parameters.GetValueOrDefault("aid-option") == "recipient") == true);
        Answer(game, c => c.Targets.SequenceEqual([donor]));
        Reach(game, p => p?.PlayerSeat == donor && p.Choices.Any(c => c.Parameters.GetValueOrDefault("aid-option") == "give"));
        var dodge = Player(game, donor).Hand.First(c => c.Kind == CardKind.Dodge).Id;
        var peach = Player(game, donor).Hand.First(c => c.Kind == CardKind.Peach).Id;
        Need(Decision(game)!.Choices.Any(c => c.Cards.SequenceEqual([dodge])) && Decision(game)!.Choices.All(c => !c.Cards.Contains(peach)),
            "Classic Qiuyuan accepts only an actual hand Dodge; the same donor's Peach is not the boundary variant's eligible payment.");
        Answer(game, c => c.Cards.SequenceEqual([dodge])); Reach(game, p => p?.SkillPrompt?.SkillId == Watch);
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == classic);
        var use = root.SameTypeAid!.Use.CardUseFrameId;
        Need(root.OwnerSeat == first && root.WindowContext!.TargetSeat == first && root.SameTypeAid is { Stage: SameTypeAidStage.GiftChildren, Payment.Delivered: true } &&
            Player(game, first).Hand.Any(c => c.Id == dodge), "The real finalized target owns its paid Dodge and exact native gain child.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry); Continue(game);
        Reach(game, p => Optional(p, classic)); var second = Decision(game)!.PlayerSeat;
        Need(first != second && new[] { first, second }.Order().SequenceEqual([0, 2]),
            "Each of the two actual finalized targets gets one independent offer, including the nonprimary target.");
        Activate(game); Reach(game, p => p?.Choices.Any(c => c.Parameters.GetValueOrDefault("aid-option") == "recipient") == true);
        Answer(game, c => c.Targets.SequenceEqual([donor]));
        Reach(game, p => p?.PlayerSeat == donor && p.Choices.Any(c => c.Parameters.GetValueOrDefault("aid-option") == "target"));
        Answer(game, c => c.Parameters.GetValueOrDefault("aid-option") == "target");
        Reach(game, p => p is { Kind: DecisionKind.PlayCard } && p.PlayerSeat == actor);
        Need(Events<SameTypeAidOfferedEvent>(game).Where(e => e.Source.SkillId == classic && e.Use.CardUseFrameId == use).Select(e => e.Source.OwnerSeat).Order().SequenceEqual([0, 2]) &&
            Events<SameTypeAidTargetAddedEvent>(game).Single(e => e.Source.SkillId == classic).RecipientSeat == donor &&
            new[] { 0, 2, donor }.All(target => Events<DamageAppliedEvent>(game).Any(e => e.SourceSeat == actor && e.TargetSeat == target && e.Amount == 1)) &&
            game.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(actor) && m.To == CardLocation.Processing) == 1 &&
            game.CardMovements.Count(m => m.CardId == dodge && m.From == CardLocation.Hand(donor) && m.To == CardLocation.Hand(first) && m.Reason.Value == GiftReason) == 1,
            "Cold returns the actual Dodge once; refusal adds the distant character to the same multi-target Slash, and all three targets resolve without repaying the Slash.");
    }
    public static void HealthyHpThresholdAndOneContestPerRoundIssueExactForeignTurnRestriction()
    {
        var (_, game) = CreateScene(); Need(Player(game, 0).Hp == Player(game, 0).MaxHp && Player(game, 1).Hp == Player(game, 0).Hp,
            "The mobile contest owner is genuinely healthy and the other actual turn owner's HP is exactly equal, exercising the inclusive mobile threshold.");
        End(game); Reach(game, prompt => Optional(prompt, Zhuikong)); Contest(game, win: true);
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        var restricted = Events<ForeignTurnContestRestrictedEvent>(game).Single();
        var grant = Events<CardTargetRestrictionGrantedEvent>(game).Single(item => item.Restriction.GrantSequence == restricted.GrantSequence).Restriction;
        Need(restricted.Origin.Result.SourceWon && grant.SubjectSeat == 1 && grant.TurnSeat == 1 &&
            grant.Restriction == SkillProgramCardTargetRestriction.SelfOnly && !Events<ForeignTurnContestSlashIssuedEvent>(game).Any(),
            "A real healthy-owner win binds self-only use to the exact foreign actual turn and never creates the failed-contest Slash.");
        End(game); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(Events<ForeignTurnContestRestrictedEvent>(game).Count() == 1 && Events<PindianResultDeterminedEvent>(game).Count(item => item.SkillId == Zhuikong) == 1,
            "Other eligible actual turns in that same native round do not offer or repay a second mobile contest.");
        var (_, low) = CreateScene(weakPeers: true); End(low); Reach(low, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        Need(!Events<PindianResultDeterminedEvent>(low).Any(item => item.SkillId == Zhuikong), "Peers below the healthy owner's HP never qualify for the mobile contest.");
    }

    public static void TiedContestClaimsTheOriginalOpponentCardBeforeItsNativeSlashCold()
    {
        var (registry, game) = CreateScene(); End(game); Reach(game, prompt => Optional(prompt, Zhuikong)); Contest(game, win: false);
        Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch);
        var claim = Events<ForeignTurnContestCardClaimedEvent>(game).Single();
        Need(claim.Origin.Result.SourceRank == claim.Origin.Result.OpponentRank && !claim.Origin.Result.SourceWon &&
            Player(game, 0).Hand.Any(card => card.Id == claim.CardId) && claim.CardId == claim.Origin.Result.OpponentCardId &&
            !Events<ForeignTurnContestSlashIssuedEvent>(game).Any(),
            "A genuine tie is not a win: its exact opponent Pindian entity is already obtained while the real gain child still precedes the virtual Slash.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Continue(game); Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        var slash = Events<ForeignTurnContestSlashIssuedEvent>(game).Single().Return;
        Need(game.CardMovements.Count(move => move.CardId == claim.CardId && move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0) && move.Reason.Value == ClaimReason) == 1 &&
            slash.Source.SkillId == Zhuikong && slash.Origin == claim.Origin && Events<ForeignTurnContestSlashReturnedEvent>(game).Count() == 1 &&
            Events<DamageAppliedEvent>(game).Any(item => item.SourceSeat == 1 && item.TargetSeat == 0 && item.Amount == 1),
            "Cold returns the paid claim once, then executes and returns the original other actor's real material-free ordinary Slash once.");
    }

    public static void SlashAidGivesANonSlashBasicOrAddsTheOriginalDistantTargetCold()
    {
        foreach (var give in new[] { true, false })
        {
            var (registry, game) = CreateScene();
            var actor = new[] { 1, 3 }.First(seat => Player(game, seat).Hand.Any(card => card.Kind == CardKind.Slash) && Player(game, seat == 1 ? 3 : 1).Hand.Any(card => card.Kind == CardKind.Peach));
            var donor = actor == 1 ? 3 : 1;
            Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard } && prompt.PlayerSeat == actor, skipContests: true);
            var original = Player(game, actor).Hand.First(card => card.Kind == CardKind.Slash).Id;
            var peach = Player(game, donor).Hand.First(card => card.Kind == CardKind.Peach).Id;
            Need(!game.Submit(new PlayCardCommand(actor, original, [donor], game.Revision, Decision(game)!.PromptId)).Accepted,
                "The prospective recipient is genuinely outside the original physical Slash user's normal distance.");
            Accept(game, new PlayCardCommand(actor, original, [0], game.Revision, Decision(game)!.PromptId)); Reach(game, prompt => Optional(prompt, Qiuyuan)); Activate(game);
            Reach(game, prompt => prompt?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("aid-option") == "recipient") == true);
            Need(Decision(game)!.Choices.All(choice => choice.Targets.All(target => target != 0 && target != actor)), "Mobile aid excludes the original Slash user and Fu Huang Hou herself.");
            Answer(game, choice => choice.Targets.SequenceEqual([donor]));
            Reach(game, prompt => prompt?.PlayerSeat == donor && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("aid-option") == "target"));
            var forbidden = Player(game, donor).Hand.Where(card => card.Kind is CardKind.Slash or CardKind.Dismantlement).Select(card => card.Id).ToArray();
            Need(Decision(game)!.Choices.Any(choice => choice.Cards.SequenceEqual([peach])) && Decision(game)!.Choices.All(choice => !choice.Cards.Any(forbidden.Contains)),
                "A real Peach may pay mobile Qiuyuan, while Slash and a nonbasic card cannot pay the same original choice.");
            if (give)
            {
                Answer(game, choice => choice.Cards.SequenceEqual([peach])); Reach(game, prompt => prompt?.SkillPrompt?.SkillId == Watch);
                Need(Player(game, 0).Hand.Any(card => card.Id == peach) && !Events<SameTypeAidTargetAddedEvent>(game).Any(), "The original basic card is really given before its gain child; giving does not add its donor as a target.");
                game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry); Continue(game);
            }
            else Answer(game, choice => choice.Parameters.GetValueOrDefault("aid-option") == "target");
            Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard } && prompt.PlayerSeat == actor);
            Need(game.CardMovements.Count(move => move.CardId == original && move.From == CardLocation.Hand(actor) && move.To == CardLocation.Processing) == 1 &&
                Events<DamageAppliedEvent>(game).Any(item => item.SourceSeat == actor && item.TargetSeat == 0 && item.Amount == 1) &&
                (give ? game.CardMovements.Count(move => move.CardId == peach && move.From == CardLocation.Hand(donor) && move.To == CardLocation.Hand(0) && move.Reason.Value == GiftReason) == 1 && !Events<SameTypeAidTargetAddedEvent>(game).Any()
                    : Events<SameTypeAidTargetAddedEvent>(game).Single().RecipientSeat == donor && Events<DamageAppliedEvent>(game).Any(item => item.SourceSeat == actor && item.TargetSeat == donor)),
                "The original physical Slash is paid once: the real basic gift returns once cold, or refusal adds the distant recipient to that same native use and resolves its damage.");
        }
    }
    private static IEnumerable<T> Events<T>(GameEngine game) => game.Events.Select(item => item.Payload).OfType<T>();
    private static void Need(bool value, string reason) { if (!value) throw new InvalidOperationException(reason); }
}
