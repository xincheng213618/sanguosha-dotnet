using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

using static BoundaryLiDianChecks;

internal static class AnniversaryFengXiBoyanDrawChecks
{
    private const string Hero = "anniversary:feng-xi", Boyan = "anniversary:boyan";
    private const string Peer = "fixture:boyan-peer", DrawWatch = "fixture:boyan-native-draw", Deck = "fixture:boyan-deck";
    private const string Reason = "skill.anniversary.boyan.draw";

    internal static void CappedForeignNativeDrawReturnsCold()
    {
        const int owner = 0, target = 1, amount = 3;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(67, 1, "feng-xi-boyan-native-draw", Hero, 3, 0, 0, 0, 0, [],
            [new(CardKind.Dodge, Suit.Heart, 7)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer, ExtraSkillIds: [DrawWatch])
            {
                StartingHand = [new(CardKind.Slash, Suit.Club, 7), new(CardKind.Slash, Suit.Club, 8)]
            }], new string('f', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, registry);
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        bool IsDrawWatch(PendingDecision prompt) => prompt is { PlayerSeat: target, SkillPrompt.SkillId: DrawWatch };

        Accept(game, new StartGameCommand());
        Settle(game);
        var originalHand = Player(target).Hand.Select(card => card.Id).ToArray();
        var sourceHand = Player(owner).HandCount;
        Require(Player(target).MaxHp == 6 && originalHand.Length == 2,
            "The chosen foreign recipient starts with two physical cards and a maximum HP above Boyan's hand cap.");
        Accept(game, new UseProgramSkillCommand(owner, Boyan, "fill-and-seal-hand", [], [target], game.Revision, P(game)!.PromptId));
        Reach(game, IsDrawWatch);

        var started = Facts<AnniversaryFengXiStartedEvent>().Single();
        var stack = game.ResolutionStack.ToArray();
        var parent = stack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        var attempt = stack.OfType<NativeDrawAttemptFrame>().Single();
        var window = stack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(frame => frame.Id == attempt.ActiveWindowFrameId);
        var index = Array.IndexOf(stack, attempt);
        Require(started.Source is { OwnerSeat: owner, SkillId: Boyan } && started.Target == target &&
            parent.AnniversaryFengXi is { Stage: AnniversaryFengXiStage.Drawing, Target: target } &&
            parent.NativeDraw is { Operation: SkillProgramEffectOp.ResolveAnniversaryFengXiBoyan, Recipients: [target], RequestedCount: amount } &&
            index > 0 && index + 1 < stack.Length && stack[index - 1].Id == parent.Id && stack[index + 1].Id == window.Id &&
            window is { OwnerSeat: target, Window: SkillProgramTriggerWindow.NativeDrawStarting,
                Continuation: ProgramLifecycleContinuation.ResumeNativeDrawAttempt } &&
            attempt.Context is { ParentFrameId: var parentId, RecipientSeat: target, RequestedCount: amount } && parentId == parent.Id &&
            attempt.Context.Reason.Value == Reason && window.NativeDrawAttempt == attempt.Context &&
            attempt.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation && parent.NativeDraw?.ActiveAttemptId == attempt.Id &&
            Facts<NativeDrawAttemptStartedEvent>().Count(entry => entry.Context == attempt.Context && entry.Return == attempt.Return) == 1 &&
            Player(target).Hand.Select(card => card.Id).SequenceEqual(originalHand) &&
            game.CardMovements.All(move => move.Reason.Value != Reason) &&
            Facts<HandCardColorRestrictionGrantedEvent>().All(entry => entry.Restriction.ParentFrameId != parent.Id),
            "Boyan reaches the recipient's real native draw window with a capped three-card request before any refill or hand seal.");

        var before = P(game)!;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredParent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredAttempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == attempt.Id);
        var runtime = game.GetRuntimeModule<CharacterFengXiAnniversaryRuntime>();
        Require(P(game) is { } restoredPrompt && IsDrawWatch(restoredPrompt) && restoredPrompt.PromptId == before.PromptId &&
            restoredAttempt.Context == attempt.Context && runtime.FengXiFirstChild(restoredParent, restoredAttempt) &&
            !runtime.FengXiFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { RecipientSeat = owner } }) &&
            !runtime.FengXiFirstChild(restoredParent, restoredAttempt with { Context = restoredAttempt.Context with { RequestedCount = amount + 1 } }),
            "Cold restore accepts the original foreign request and rejects replacing its recipient with Feng Xi or raising its count.");

        Reach(game, prompt => IsDrawWatch(prompt) && game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>()
            .Any(frame => frame.Batch.ParentFrameId == parent.Id));
        var gainPromptId = P(game)!.PromptId;
        var paid = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var gain = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.ParentFrameId == parent.Id);
        var receipt = paid.NativeOperationDrawReturns.Single();
        var outcome = receipt.Outcome;
        var returned = Facts<AnniversaryFengXiDrawReturnedEvent>().Single();
        var drawn = game.CardMovements.Where(move => move.Reason.Value == Reason).ToArray();
        var gainedIds = drawn.Select(move => move.CardId).ToArray();
        Require(paid.AnniversaryFengXi is { Stage: AnniversaryFengXiStage.DrawChildren } && paid.NativeDraw is null &&
            receipt.InstructionIndex == parent.InstructionIndex && receipt.Operation == SkillProgramEffectOp.ResolveAnniversaryFengXiBoyan &&
            receipt.Reason.Value == Reason && outcome is { RecipientSeat: target, RequestedCount: amount, Replacement: null } &&
            outcome.AttemptFrameId == attempt.Id && outcome.ActualDrawIds.SequenceEqual(gainedIds) &&
            outcome.SequenceBefore == returned.Before && outcome.SequenceAfter == returned.After &&
            Facts<HandCardColorRestrictionGrantedEvent>().All(entry => entry.Restriction.ParentFrameId != parent.Id),
            "The original completed Draw receipt backs the real gain window before either hand restriction is granted.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredPaid = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredGain = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Id == gain.Id);
        runtime = game.GetRuntimeModule<CharacterFengXiAnniversaryRuntime>();
        Require(P(game)?.PromptId == gainPromptId && JsonSerializer.Serialize(restoredPaid.NativeOperationDrawReturns.Single()) == JsonSerializer.Serialize(receipt) &&
            runtime.FengXiFirstChild(restoredPaid, restoredGain) &&
            !runtime.FengXiFirstChild(restoredPaid with { NativeOperationDrawReturns = [] }, restoredGain),
            "Cold restore keeps the original native return and the gain child requires that receipt on its parent.");
        Settle(game);

        Require(returned is { Target: target, Requested: amount, Actual: amount } && returned.FrameId == parent.Id &&
            returned.AttemptId == attempt.Id && returned.Before == attempt.Context.SequenceBefore &&
            Facts<AnniversaryFengXiDrawReturnedEvent>().SequenceEqual([returned]) &&
            game.CardMovements.Where(move => move.Reason.Value == Reason).SequenceEqual(drawn) &&
            drawn.Length == amount && gainedIds.Distinct().Count() == amount && drawn.All(move =>
                move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(target) &&
                move.Sequence > returned.Before && move.Sequence <= returned.After) &&
            Player(target).Hand.Select(card => card.Id).Order().SequenceEqual(originalHand.Concat(gainedIds).Order()) &&
            Player(owner).HandCount == sourceHand && Player(target).HandCount == 5 &&
            Facts<HandCardColorRestrictionGrantedEvent>().Count(entry =>
                entry.Restriction.ParentFrameId == parent.Id && entry.Restriction.AffectedSeat == target) == 2 &&
            Facts<ProgramSkillResolvedEvent>().Count(entry => entry.FrameId == parent.Id && entry.Completed) == 1,
            "The original Boyan draw moves three distinct real cards into the foreign hand, reaches five, and seals that hand once.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-boyan-native-draw", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{DrawWatch}}","revision":1,"triggers":[
                  {"id":"boyan-foreign-draw","window":"nativeDrawStarting","subject":"owner","condition":{"kind":"phaseIsPlay"},
                   "optional":true,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen",
                    "options":[{"id":"continue"}]}]},
                  {"id":"boyan-foreign-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                   "movementOccurrence":"perBatch","movementReasons":["{{Reason}}"],"optional":true,"isSkillInvocation":false,
                   "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen",
                    "options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [DrawWatch] = new { name = "观察驳言摸牌", description = "在受领者的摸牌请求和获得牌时暂停。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } }
                }));
            builder.AddSkill(new(DrawWatch, catalog.Presentations[DrawWatch].Name, catalog.Presentations[DrawWatch].Description)
                { Program = catalog.Programs[DrawWatch], ProgramPresentation = catalog.Presentations[DrawWatch] });
            builder.AddGeneral(new(Peer, "固定六体力同桌", "", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "驳言固定实体牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7), 24).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "驳言异席补牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
