using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class HappyTaishiCiPoweiHuishiChecks
{
    private const string GuoJia = "mobile:shen-guo-jia", TaishiCi = "happy:shen-taishi-ci";
    private const string Huishi = "mobile:huishi-gift", Powei = "happy:powei", Shenzhu = "happy:shenzhu", Chongwei = "happy:chongwei";
    private const string Peer = "fixture:powei-huishi-peer", Deck = "fixture:powei-huishi-deck";

    internal static void PaidForeignHuishiAwakensAtActualEndingWithWeiCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        VerifyFirstActualEnding(registry, grantAwakening: false);
        VerifyFirstActualEnding(registry, grantAwakening: true);
    }

    private static void VerifyFirstActualEnding(ContentRegistry registry, bool grantAwakening)
    {
        const int source = 0, owner = 1;
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(7, 1, "powei-paid-huishi", GuoJia, 5, 3, 0, 0, 0,
            [], [], [], [new(TaishiCi) { StartingHand = [] }, peer, peer], [], new string('c', 64))
            { DefaultGodFactionId = "wei" };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = source, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Reach(SourcePlay);
        var firstTurn = game.State.TurnNumber;
        var beforeCost = game.CreateSnapshot(source).Players[source].MaxHp;
        var marked = WeiBySeat();
        Need(beforeCost >= 4 && Facts<TurnStartedEvent>().Single() == new TurnStartedEvent(firstTurn, source) &&
            marked.SequenceEqual([1, 0, 1, 1]) && Facts<PlayerMarkerChangedEvent>().Count(fact =>
                fact.Marker == PlayerMarkerKind.ShenTaishiCiWei && fact.SkillOwnerSeat == owner && fact.Delta == 1) == 3 &&
            Facts<HappyTaishiCiAwakenedEvent>().Length == 0 && !HasSkill(Shenzhu),
            "The original Chongwei setup has already marked all three other seats when Huishi first becomes playable.");

        ShenGuoJiaAwakeningOverrideEvent? grant = null;
        if (grantAwakening)
        {
            var action = game.GetLegalActions(source).Single(candidate => candidate.ProgramSkillId == Huishi &&
                candidate.ProgramActivationId == "grant-awakening");
            Need(action.SelectableTargetSeats.Contains(owner), "The original Huishi can select the living foreign Powei owner.");
            Submit(new UseProgramSkillCommand(source, Huishi, "grant-awakening", [], [owner], game.Revision, Prompt()!.PromptId));
            Reach(() => Prompt()?.SkillPrompt?.SkillId == Huishi &&
                Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("value") == Powei));
            var offer = Prompt()!;
            var choice = offer.Choices.Single(candidate => candidate.Parameters.GetValueOrDefault("value") == Powei);
            Submit(new AnswerPromptCommand(source, offer.PromptId, choice.Id, game.Revision));
            Reach(SourcePlay);
            grant = Facts<ShenGuoJiaAwakeningOverrideEvent>().Single();
            var payment = Facts<ShenGuoJiaMaximumPaidEvent>().Single();
            Need(grant is { Recipient: owner, SkillId: Powei, Source: { OwnerSeat: source, SkillId: Huishi, BindingId: "grant-awakening" } } &&
                payment is { Seat: source, Purpose: "huishi-gift" } && grant.FrameId == payment.FrameId &&
                payment.Before == beforeCost && payment.After == beforeCost - 2 && game.CreateSnapshot(source).Players[source].MaxHp == payment.After &&
                Facts<HappyTaishiCiAwakenedEvent>().Length == 0 && WeiBySeat().SequenceEqual(marked),
                "Huishi pays two maximum HP for the original foreign Powei instance while the field still holds its original Wei.");

            var playPrompt = Prompt()!.PromptId;
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(Prompt()?.PromptId == playPrompt && Facts<ShenGuoJiaAwakeningOverrideEvent>().Single() == grant &&
                Facts<ShenGuoJiaMaximumPaidEvent>().Single() == payment && WeiBySeat().SequenceEqual(marked),
                "Cold replay preserves the original paid awakening grant, source Play and all three Wei markers.");
        }

        Submit(new EndPlayPhaseCommand(source, game.Revision, Prompt()!.PromptId));
        Reach(() => Facts<ActualTurnCursorReturnedEvent>().Any(fact => fact.ActualTurn == firstTurn && fact.OwnerSeat == source));
        var awakenings = Facts<HappyTaishiCiAwakenedEvent>();
        Need(Facts<TurnEndedEvent>().Any(fact => fact.ActorSeat == source && fact.TurnNumber == firstTurn) &&
            WeiBySeat().SequenceEqual(marked) && HasSkill(Chongwei) && Facts<HappyTaishiCiWeiRemovedEvent>().Length == 0,
            "The source's actual turn finishes while Chongwei and every original Wei marker remain in play.");
        if (grant is null)
        {
            Need(awakenings.Length == 0 && !HasSkill(Shenzhu), "Powei remains unawakened when real Wei remain and Huishi was unused.");
            return;
        }

        Need(awakenings is [{ DrawCount: 0 } awake] && awake.Source.OwnerSeat == owner && awake.Source.SkillId == Powei &&
            awake.Source.SkillInstanceId == grant.InstanceId && awake.FrameId > grant.FrameId && HasSkill(Shenzhu),
            "The paid Huishi must let the exact foreign Powei awaken at the original actual end with Wei still present and no round removals.");
        var accepted = awakenings.Single();
        var start = Facts<HappyTaishiCiStartedEvent>().Single(fact => fact.FrameId == accepted.FrameId);
        Need(start.Parent > 0 && start.Turn == firstTurn && start.Source == accepted.Source && start.Hash == grant.GameplayHash &&
            Facts<ProgramBindingStartedEvent>().Count(fact => fact.FrameId == accepted.FrameId && fact.OwnerSeat == owner &&
                fact.SkillId == Powei && fact.Window == SkillProgramTriggerWindow.AfterTurnEnded) == 1 &&
            Facts<SkillsAcquiredEvent>().Count(fact => fact.PlayerSeat == owner && fact.SourceSkillId == Powei && fact.SkillIds.SequenceEqual([Shenzhu])) == 1,
            "The original foreign AfterTurnEnded binding issues Shenzhu exactly once through the actual awakening frame.");

        PendingDecision? Prompt() => game.CreateSnapshot(source).PendingDecision;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        bool HasSkill(string skillId) => game.CreateSnapshot(owner).Players[owner].Skills?.Any(skill => skill.ContentId == skillId) == true;
        bool SourcePlay() => Prompt() is { PlayerSeat: source, Kind: DecisionKind.PlayCard } && game.ResolutionStack.Count == 0;
        int[] WeiBySeat() => game.CreateSnapshot(source).Players.Select(player =>
            player.Markers?.FirstOrDefault(marker => marker.Kind == PlayerMarkerKind.ShenTaishiCiWei)?.Count ?? 0).ToArray();

        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Need(Prompt() is null && game.State.Winner == Winner.None, "The fixture reached an unexpected public command boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The original Play or actual turn-end boundary did not arrive.");
        }

        void Submit(GameCommand command)
        {
            var accepted = game.Submit(command);
            Need(accepted.Accepted, accepted.Error?.Message ?? "The public Powei fixture command was rejected.");
        }

        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Grant: {grantAwakening}, turn {game.State.TurnNumber}/{game.State.Phase}, prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-happy-powei-huishi", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, GuoJia, TaishiCi);
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 0, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "辉逝与破围的实际结束", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [GuoJia, TaishiCi, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
