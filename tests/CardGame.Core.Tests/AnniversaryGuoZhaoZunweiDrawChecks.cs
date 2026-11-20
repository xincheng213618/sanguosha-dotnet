using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

using static BoundaryLiDianChecks;

internal static class AnniversaryGuoZhaoZunweiDrawChecks
{
    private const string Hero = "anniversary:guo-zhao", Zunwei = "anniversary:zunwei", Pianchong = "anniversary:pianchong";
    private const string Peer = "fixture:zunwei-peer", DrawWatch = "fixture:zunwei-native-draw", Deck = "fixture:zunwei-deck";
    private const string Reason = "skill-program.anniversary:zunwei.draw";

    internal static void CappedOwnerNativeDrawReturnsCold()
    {
        const int owner = 0, target = 1, amount = 5;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(47, 1, "guo-zhao-zunwei-draw", Hero, 3, 0, 0, 0, 0, [DrawWatch],
            [new(CardKind.Dodge, Suit.Heart, 7)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer)
            {
                StartingHand = Enumerable.Range(1, 7).Select(rank => new ShanheStartingCard(CardKind.Slash, Suit.Club, rank)).ToArray()
            }], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        int[] Hand(int seat) => game.CreateSnapshot(seat).Players[seat].Hand.Select(card => card.Id).ToArray();
        bool IsWatch(PendingDecision prompt) => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: DrawWatch };

        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.PlayerSeat == owner &&
            (prompt.Kind == DecisionKind.PlayCard || prompt.SkillPrompt?.SkillId == Pianchong));
        if (P(game)?.SkillPrompt?.SkillId == Pianchong)
            Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
        Settle(game);
        var ownerHand = Hand(owner);
        var targetHand = Hand(target);
        Require(ownerHand.Length == 1 && targetHand.Length == 7, "The actual hand difference is larger than Zunwei's cap.");
        Accept(game, new UseProgramSkillCommand(owner, Zunwei, "three-permanent-options", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: Zunwei });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == "hand" && choice.Targets.SequenceEqual([target]));
        Reach(game, IsWatch);

        var paid = Facts<AnniversaryGuoZhaoOptionPaidEvent>().Single();
        var stack = game.ResolutionStack.ToArray();
        var parent = stack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var attempt = stack.OfType<NativeDrawAttemptFrame>().Single();
        var index = Array.IndexOf(stack, attempt);
        Require(paid is { Source.OwnerSeat: owner, Source.SkillId: Zunwei, Target: target, Option: "hand", Amount: amount } &&
            parent.AnniversaryGuoZhao is { Stage: AnniversaryGuoZhaoStage.Drawing, Paid: true, Target: target, Amount: amount } &&
            index > 0 && stack[index - 1].Id == parent.Id && parent.NativeDraw?.ActiveAttemptId == attempt.Id &&
            attempt.Context.ParentFrameId == parent.Id && attempt.Context.RecipientSeat == owner &&
            attempt.Context.RequestedCount == amount && attempt.Context.Reason.Value == Reason &&
            attempt.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation &&
            Hand(owner).SequenceEqual(ownerHand) && Hand(target).SequenceEqual(targetHand) &&
            Facts<AnniversaryGuoZhaoDrawReturnedEvent>().Length == 0 && game.CardMovements.All(move => move.Reason.Value != Reason),
            "Zunwei spends its hand option against the real foreign hand before the original owner's five-card request pays.");

        var watch = P(game)!;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredAttempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == attempt.Id);
        var runtime = game.GetRuntimeModule<CharacterGuoZhaoAnniversaryRuntime>();
        Require(P(game) is { } restoredPrompt && IsWatch(restoredPrompt) && restoredPrompt.PromptId == watch.PromptId &&
            Facts<AnniversaryGuoZhaoOptionPaidEvent>().Single() == paid && restoredAttempt.Context == attempt.Context &&
            runtime.GuoZhaoFirstChild(restoredParent, restoredAttempt) &&
            !runtime.GuoZhaoFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { RecipientSeat = target } }) &&
            !runtime.GuoZhaoFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { RequestedCount = amount + 1 } }),
            "Cold restore retains the paid choice and accepts only the original recipient and count.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Settle(game);

        var returned = Facts<AnniversaryGuoZhaoDrawReturnedEvent>().Single();
        var drawn = game.CardMovements.Where(move => move.Reason.Value == Reason).ToArray();
        Require(returned is { Requested: amount, Actual: amount } && returned.FrameId == parent.Id &&
            returned.Before == attempt.Context.SequenceBefore && drawn.Length == amount &&
            drawn.Select(move => move.CardId).Distinct().Count() == amount &&
            drawn.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner) &&
                move.Sequence > returned.Before && move.Sequence <= returned.After) &&
            Hand(owner).Order().SequenceEqual(ownerHand.Concat(drawn.Select(move => move.CardId)).Order()) &&
            Hand(target).SequenceEqual(targetHand) && Facts<AnniversaryGuoZhaoOptionPaidEvent>().Single() == paid &&
            Facts<NativeDrawAttemptDecidedEvent>().Count(entry => entry.AttemptFrameId == attempt.Id &&
                entry.ParentFrameId == parent.Id && entry.RecipientSeat == owner && entry.RequestedCount == amount && !entry.Replaced) == 1,
            "The original native request adds five distinct physical cards to Guo Zhao's hand once.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-zunwei-native-draw", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{DrawWatch}}","revision":1,"triggers":[
                  {"id":"zunwei-owner-draw","window":"nativeDrawStarting","subject":"owner","condition":{"kind":"phaseIsPlay"},
                   "optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen",
                    "options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [DrawWatch] = new { name = "观察尊位摸牌", description = "暂停在郭照的原生摸牌请求。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } }
                }));
            builder.AddSkill(new(DrawWatch, catalog.Presentations[DrawWatch].Name, catalog.Presentations[DrawWatch].Description)
                { Program = catalog.Programs[DrawWatch], ProgramPresentation = catalog.Presentations[DrawWatch] });
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "尊位固定实体牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7), 24).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "尊位本人摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
