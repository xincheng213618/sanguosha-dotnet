using System.Globalization;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

using static BoundaryLiDianChecks;

internal static class AnniversaryHuZhaoMiduDrawChecks
{
    private const string Hero = "anniversary:hu-zhao", Midu = "anniversary:midu";
    private const string Peer = "fixture:midu-peer", DrawWatch = "fixture:midu-native-draw", GainWatch = "fixture:midu-native-gain", Deck = "fixture:midu-deck";
    private const string DrawReason = "skill-program.anniversary:midu.draw";

    internal static void PaidAreasReturnOriginalForeignDrawCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        VerifyPaidDraw(registry, mask: 1, count: 1);
        VerifyPaidDraw(registry, mask: 3, count: 2);
    }

    private static void VerifyPaidDraw(ContentRegistry registry, int mask, int count)
    {
        const int owner = 0, recipient = 1;
        var setup = new ShanheBattleSetup(43, 1, "hu-zhao-midu-draw", Hero, 3, 0, 0, 0, 0, [], [],
            [new(CardKind.Crossbow, Suit.Club, 1)],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer, ExtraSkillIds: [DrawWatch, GainWatch]) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] }], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        int[] Hand(int seat) => Player(seat).Hand.Select(card => card.Id).ToArray();
        bool IsDrawWatch(PendingDecision prompt) => prompt is { PlayerSeat: recipient, SkillPrompt.SkillId: DrawWatch };

        Accept(game, new StartGameCommand());
        Settle(game);
        var source = Player(owner);
        var weapon = source.Equipment.Single(card => card.Kind == CardKind.Crossbow);
        var recipientHand = Hand(recipient);
        Require((source.EquipmentSlotCapacities?.GetValueOrDefault(EquipmentSlot.Weapon, 1) ?? 1) == 1 &&
            (source.EquipmentSlotCapacities?.GetValueOrDefault(EquipmentSlot.Armor, 1) ?? 1) == 1 &&
            recipientHand.Length == 1, "Midu begins with two available slots, an equipped Crossbow, and the original foreign hand.");
        Accept(game, new UseProgramSkillCommand(owner, Midu, "abolish-or-restore", [], [], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: Midu });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("mode") == "abolish" &&
            choice.Parameters.GetValueOrDefault("value") == mask.ToString(CultureInfo.InvariantCulture));
        Reach(game, prompt => prompt is { PlayerSeat: owner, SkillPrompt.SkillId: Midu } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("mode") == "recipient"));
        Answer(game, choice => choice.Targets.SequenceEqual([recipient]));
        Reach(game, IsDrawWatch);

        var paid = Facts<HuZhaoMiduPaidEvent>().Single();
        var stack = game.ResolutionStack.ToArray();
        var parent = stack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var attempt = stack.OfType<NativeDrawAttemptFrame>().Single();
        var index = Array.IndexOf(stack, attempt);
        var discarded = game.CardMovements.Single(move => move.CardId == weapon.Id && move.Reason.Value == "equipment.slot-abolished");
        var capacities = Player(owner).EquipmentSlotCapacities!;
        Require(paid is { Source.OwnerSeat: owner, Source.SkillId: Midu, Restore: false, Recipient: recipient } && paid.SelectedMask == mask &&
            parent.HuZhaoMidu is { Paid: true, Stage: HuZhaoMiduStage.Drawing, Recipient: recipient } receipt && receipt.SelectedMask == mask &&
            parent.NativeOperationDrawReturns.Count == 0 && capacities[EquipmentSlot.Weapon] == 0 &&
            (capacities.GetValueOrDefault(EquipmentSlot.Armor, 1) == 0) == ((mask & 2) != 0) &&
            discarded.From == CardLocation.Equipment(owner) && discarded.To == CardLocation.DiscardPile &&
            discarded.Sequence > paid.Before && discarded.Sequence <= attempt.Context.SequenceBefore &&
            index > 0 && stack[index - 1].Id == parent.Id && parent.NativeDraw?.ActiveAttemptId == attempt.Id &&
            attempt.Context.ParentFrameId == parent.Id && attempt.Context.RecipientSeat == recipient && attempt.Context.RequestedCount == count &&
            attempt.Context.Reason.Value == DrawReason && attempt.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation &&
            Hand(recipient).SequenceEqual(recipientHand) && Facts<HuZhaoMiduCompletedEvent>().Length == 0,
            "The selected slot costs are paid before the original foreign native request for the matching number of cards.");

        var promptId = P(game)!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredAttempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == attempt.Id);
        var runtime = game.GetRuntimeModule<CharacterHuZhaoAnniversaryRuntime>();
        Require(P(game) is { } restoredPrompt && IsDrawWatch(restoredPrompt) && restoredPrompt.PromptId == promptId &&
            restoredAttempt.Context == attempt.Context && Facts<HuZhaoMiduPaidEvent>().Single() == paid &&
            runtime.HuZhaoFirstChild(restoredParent, restoredAttempt) &&
            !runtime.HuZhaoFirstChild(restoredParent with { NativeDraw = null }, restoredAttempt) &&
            !runtime.HuZhaoFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { RecipientSeat = owner } }) &&
            !runtime.HuZhaoFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { RequestedCount = count + 1 } }),
            "Cold restore keeps the original paid foreign request and rejects a detached cursor, different recipient or different amount.");

        Reach(game, prompt => prompt is { PlayerSeat: recipient, SkillPrompt.SkillId: GainWatch });
        var gainPromptId = P(game)!.PromptId;
        var gainParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var gainWindow = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.ParentFrameId == parent.Id);
        var returned = gainParent.NativeOperationDrawReturns.Single();
        var drawn = game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();
        Require(gainParent.HuZhaoMidu is { Stage: HuZhaoMiduStage.DrawChildren } paidAreas &&
            paidAreas.Before == paid.Before && paidAreas.After < returned.Outcome.SequenceAfter &&
            gainParent.NativeDraw is null && returned.Operation == SkillProgramEffectOp.ResolveAnniversaryHuZhaoMidu &&
            returned.Reason.Value == DrawReason && returned.Outcome.RecipientSeat == recipient && returned.Outcome.RequestedCount == count &&
            returned.Outcome.AttemptFrameId == attempt.Id && returned.Outcome.ActualDrawIds.SequenceEqual(drawn.Select(move => move.CardId)) &&
            runtime.HuZhaoFirstChild(gainParent, gainWindow) && Facts<HuZhaoMiduCompletedEvent>().Length == 0,
            "The original program frame keeps the shared Draw return while its foreign gain observer is pending.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredGainParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredGainWindow = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Id == gainWindow.Id);
        runtime = game.GetRuntimeModule<CharacterHuZhaoAnniversaryRuntime>();
        Require(P(game)?.PromptId == gainPromptId &&
            JsonSerializer.Serialize(restoredGainParent.NativeOperationDrawReturns.Single()) == JsonSerializer.Serialize(returned) &&
            runtime.HuZhaoFirstChild(restoredGainParent, restoredGainWindow) &&
            !runtime.HuZhaoFirstChild(restoredGainParent with { NativeOperationDrawReturns = [] }, restoredGainWindow),
            "Cold restore keeps that same returned native draw on its issuer until the actual gain child returns.");
        Settle(game);

        Require(Facts<HuZhaoMiduCompletedEvent>().Single() == new HuZhaoMiduCompletedEvent(parent.Id, false, recipient, count) &&
            Facts<HuZhaoMiduPaidEvent>().Single() == paid &&
            Facts<NativeDrawAttemptDecidedEvent>().Count(entry => entry.AttemptFrameId == attempt.Id && entry.ParentFrameId == parent.Id &&
                entry.RecipientSeat == recipient && entry.RequestedCount == count && !entry.Replaced) == 1 &&
            game.CardMovements.Where(move => move.Reason.Value == DrawReason).SequenceEqual(drawn) &&
            drawn.Length == count && drawn.Select(move => move.CardId).Distinct().Count() == count &&
            drawn.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(recipient) &&
                move.Sequence > attempt.Context.SequenceBefore) &&
            Hand(recipient).Order().SequenceEqual(recipientHand.Concat(drawn.Select(move => move.CardId)).Order()) &&
            Hand(owner).Length == source.HandCount,
            "The original request gives the exact physical cards to the foreign hand and completes on its issuing frame once.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-midu-native-draw", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{DrawWatch}}","revision":1,"triggers":[
                  {"id":"midu-foreign-draw","window":"nativeDrawStarting","subject":"owner","condition":{"kind":"phaseIsPlay"},
                   "optional":true,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen",
                    "options":[{"id":"continue"}]}]}]},
                  {"id":"{{GainWatch}}","revision":1,"triggers":[
                   {"id":"midu-foreign-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch",
                    "movementReasons":["{{DrawReason}}"],"optional":true,"isSkillInvocation":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{DrawWatch}}":{
                  "name":"观察弥笃摸牌","description":"暂停在异席的原生摸牌请求。","optionLabels":{"continue":"继续"} },
                  "{{GainWatch}}":{"name":"观察弥笃得牌","description":"暂停在异席实际获得弥笃摸牌之后。","optionLabels":{"continue":"继续"} } } }
                """);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Peer, "固定同桌", "", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "弥笃固定实体牌堆", 0, 0, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "弥笃异席摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
