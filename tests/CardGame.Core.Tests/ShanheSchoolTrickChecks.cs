using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSchoolTrickChecks
{
    private const string Owner = "fixture:school-trick-owner", PeerA = "fixture:school-trick-peer-a", PeerB = "fixture:school-trick-peer-b";
    private const string Deck = "fixture:school-trick-deck", Fengzi = "ol:fengzi";

    public static void OnePaidUseRepeatsNativeDamageEffectsAndCoexistsWithFengzi()
    {
        foreach (var scenario in new[] { (Complete: false, Fengzi: false, Effects: 1),
                     (Complete: true, Fengzi: false, Effects: 2), (Complete: true, Fengzi: true, Effects: 3) })
        {
            var s = Scene.Start(scenario.Complete, scenario.Fengzi);
            var before = s.Player(1).Hp;
            var card = s.Use(CardKind.Duel, 1); s.Play(payFengzi: scenario.Fengzi);
            var declared = s.Events<CardUseDeclaredEvent>().Where(e => e.CardId == card).ToArray();
            Need(declared is [var use] && s.Player(1).Hp == before - scenario.Effects &&
                s.Events<DamageAppliedEvent>().Count(e => e.SourceCardUseResolutionId == use.ResolutionId) == scenario.Effects &&
                s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
                "One physical black Duel is accepted and paid once; each extra native effect really damages without another use or material payment.");
            Need(s.Events<ShanheSchoolTrickIssuedEvent>().Count() == (scenario.Complete ? 1 : 0) &&
                s.Events<ShanheSchoolTrickStartedEvent>().Count() == (scenario.Complete ? 1 : 0),
                "A real completed school issues one extra effect; an unearned copy of the skill cannot issue an effect.");
            if (scenario.Fengzi)
                Need(s.Events<ExtraCardEffectIssuedEvent>().Count() == 1 &&
                    s.Events<ShanheSchoolTrickCompletedEvent>().Count() == 1,
                    "Fengzi's actual separate discard and the school receipt coexist; neither overwrites the other's extra native round.");
            else
            {
                var count = s.Events<ShanheSchoolTrickIssuedEvent>().Count();
                var drawCard = s.Use(CardKind.DrawTwo); s.Play();
                Need(s.Events<ShanheSchoolTrickIssuedEvent>().Count() == count &&
                    s.Events<CardUseDeclaredEvent>().Count(e => e.CardId == drawCard) == 1,
                    "A nondamage trick retains its ordinary single effect and never receives a school grant.");
            }
        }
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PlayerSnapshot Player(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> Events<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The school-trick native command failed."); }
        private void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = G.PendingDecision!;
            var choice = prompt.Choices.FirstOrDefault(predicate) ?? throw new InvalidOperationException(JsonSerializer.Serialize(prompt));
            Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
        }
        public void Play(bool payFengzi = false)
        {
            for (var step = 0; step < 160; step++)
            {
                if (G.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                if (G.PendingDecision is { PlayerSeat: 0 } prompt)
                {
                    if (payFengzi && prompt.SkillPrompt?.SkillId == Fengzi &&
                        prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"))
                        Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
                    else if (payFengzi && prompt.SkillPrompt?.SkillId == Fengzi &&
                             prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "cost"))
                        Answer(c => c.Parameters.GetValueOrDefault("branch") == "cost" &&
                            c.Cards.Any(id => Player(0).Hand.Any(card => card.Id == id && card.Kind == CardKind.DrawTwo)));
                    else if (prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip"))
                        Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                    else if (prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die"))
                        Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                    else throw new InvalidOperationException("Unexpected school-trick prompt: " + JsonSerializer.Serialize(prompt));
                }
                else
                {
                    Need(G.State.Winner == Winner.None, "The bounded school-trick fixture must retain its second enemy.");
                    Accept(new AdvanceOneStepCommand(G.Revision));
                }
            }
            throw new InvalidOperationException("The native school-trick effects failed to return to the original Play frame.");
        }
        public int Use(CardKind kind, int? target = null)
        {
            var action = G.GetHumanLegalActions().First(a => a.CardId is { } card &&
                Player(0).Hand.Any(c => c.Id == card && c.Kind == kind) &&
                (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            var id = action.CardId!.Value;
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, G.PendingDecision!.PromptId,
                action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
            return id;
        }
        public static Scene Start(bool complete, bool fengzi, bool firstDodge = false, int? firstHp = null)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheSchoolTrickPackage(), new Fixture());
            var member = new ShanheRewardDefinition("fixture:trick-member", "锦囊实际组件")
                { CampaignEconomy = new(ReplacedEquipmentGold: 1), Rarity = 1, SchoolMembership = new(2025, 9, 1) };
            var definition = new ShanheCampaignDefinition("fixture:trick-school", "原生锦囊套装", [Owner], ["battle"],
                [new("battle", "守军", ShanheNodeKind.Battle, [],
                    [new("enemy", "两名守军", [new(PeerA, Hp: firstHp) { StartingHand = firstDodge ? [new(CardKind.Dodge)] : [] },
                        new(PeerB) { StartingHand = [] }])])], [member])
                { Schools = [new(9, "锦囊流", 2363, "伤害类锦囊结算两次", [1, 0, 0, 0])] };
            var campaign = ShanheCampaignSession.Create(definition, new(13, Owner)
                { EquippedTacticIds = complete ? [member.Id] : [] }, _ => registry);
            var moved = campaign.Submit(new MoveShanheNodeCommand("battle", campaign.Revision));
            Need(moved.Accepted, moved.Error ?? "The actual school ownership fixture could not enter battle.");
            var setup = campaign.Snapshot.PendingBattleSetup!;
            Need(setup.ActiveSchoolIds.Contains(9) == complete, "Only an owned real component completes the fixture's small school recipe.");
            setup = setup with
            {
                ExtraSkillIds = fengzi ? [Fengzi] : [],
                // Present even in the negative scene: a standalone skill grant
                // cannot substitute for actual campaign school ownership.
                TacticSkillIds = [ShanheSchoolEffects.TrickRepeatSkillId],
                StartingCards = [new(CardKind.Duel, Suit.Spade, 2), new(CardKind.ArrowBarrage, Suit.Heart, 3),
                    new(CardKind.DrawTwo, Suit.Heart, 4), new(CardKind.Slash, Suit.Spade, 5)]
            };
            var s = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-school-trick", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:wang-rong");
            builder.AddGeneral(new(Owner, "锦囊使用者", "supporter", "standard:none", "wu", 20));
            builder.AddGeneral(new(PeerA, "第一守军", "supporter", "standard:none", "wei", 20));
            builder.AddGeneral(new(PeerB, "第二守军", "supporter", "standard:none", "wei", 20));
            builder.AddDeck(new(Deck, "固定无普通摸牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "锦囊套装小局", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, PeerA, PeerB], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
