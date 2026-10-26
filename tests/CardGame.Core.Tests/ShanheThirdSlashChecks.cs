using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheThirdSlashChecks
{
    private const string Owner = "fixture:third-slash-owner", Peer = "fixture:third-slash-peer";
    private const string Driver = "fixture:third-slash-driver", Quiet = "fixture:third-slash-quiet", Deck = "fixture:third-slash-deck";

    public static void NativeThirdSlashCountsDodgedVirtualAndCrossRoundDeclarations()
    {
        foreach (var (skill, bonus) in new[] { (StandardShanheCampaignCatalog.CurrentSanbanfuSkillId, 1),
                     (StandardShanheCampaignCatalog.CurrentSanbanfu2SkillId, 2) })
        {
            var s = Scene.Start(skill);
            s.Card(CardKind.Slash, [1], 1);
            Need(s.Events<DamageAppliedEvent>().Length == 0 && s.Events<CardUseDeclaredEvent>().Count(e => e.SourceSeat == 0 && e.CardKind == CardKind.Slash) == 1,
                "The first real Slash must actually be dodged while retaining its declared-use position.");
            s.VirtualSlash();
            Need(s.Events<DamageAppliedEvent>() is [{ TargetSeat: 1, Amount: 1 }],
                "The second Slash is a true zero-material native use and still receives no third-use damage bonus.");
            s.Card(CardKind.ColdSlash, [1], 1 + bonus);
            Need(s.Events<DamageAppliedEvent>().Last().Amount == 1 + bonus,
                "The third declared Slash is Cold Slash; the earlier miss and virtual use both count toward its native damage.");

            s.NextOwnPlay();
            s.Card(CardKind.Slash, [1], 1); s.VirtualSlash();
            Need(s.Events<DamageAppliedEvent>().TakeLast(2).All(e => e.Amount == 1),
                "The fourth and fifth declarations retain the whole-battle count across a genuine turn and round boundary.");
            s.Card(CardKind.IronChain, [1, 2], null); s.Card(CardKind.FireSlash, [1], 1 + bonus);
            var modifiers = s.Events<ProgramCardDamageModifiedEvent>().Where(e => e.Source.SkillId == skill).ToArray();
            Need(s.Events<DamageAppliedEvent>().TakeLast(2).Select(e => (e.TargetSeat, e.Amount, e.Nature))
                    .SequenceEqual([(1, 1 + bonus, DamageNature.Fire), (2, 1 + bonus, DamageNature.Fire)]) &&
                s.Events<ChainedDamagePropagatedEvent>() is [{ FromSeat: 1, TargetSeat: 2 } propagated] && propagated.Amount == 1 + bonus &&
                modifiers.Length == 2 && modifiers.All(e => e.BaseAmount == 1 && e.ModifiedAmount == 1 + bonus) &&
                s.Events<CurrentTurnCardUseKindsRecordedEvent>().Count(e => e.ActorSeat == 0 && e.RoundActionType == CardActionType.Use &&
                    e.EffectiveCardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash) == 6 &&
                s.Events<WushuangCombatUseFactEvent>().Length == 0,
                "The sixth declaration receives one card-damage bonus, propagates that final card amount without another bonus, and uses no Wushuang ownership or ledger. " +
                JsonSerializer.Serialize(new { hits = s.Events<DamageAppliedEvent>(), modifiers, chain = s.Events<ChainedDamagePropagatedEvent>() }));
        }
    }

    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        private GameEngine G { get; } = game;
        private PendingDecision? Pending => G.PendingDecision;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private PlayerSnapshot View() => G.CreateSnapshot(0).Players[0];
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native third-Slash command."); }
        private void ReachPlay()
        {
            for (var step = 0; step < 180; step++)
            {
                if (Pending is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                Need(G.State.Winner == Winner.None, "The bounded third-Slash fixture ended before its expected boundary.");
                if (Pending is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
                {
                    var choice = prompt.Kind is DecisionKind.RescueDying or DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification
                        ? prompt.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") : prompt.Choices[0];
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded third-Slash native Play boundary.");
        }
        public void Card(CardKind kind, int[] targets, int? expectedDamage)
        {
            var card = View().Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            if (expectedDamage is { } expected)
            {
                var revision = G.Revision;
                for (var query = 0; query < 3; query++)
                    Need(G.PredictPublicActionDamage(0, action, targets[0]).DamageAmount == expected,
                        "Public prediction must read the same next declaration ordinal without consuming it.");
                Need(G.Revision == revision, "Repeated third-Slash predictions must leave the command revision unchanged.");
            }
            Accept(new PlayCardCommand(0, card.Id, targets, G.Revision, Pending!.PromptId, action.PlayedCardKind, action.TargetCardId)); ReachPlay();
        }
        public void VirtualSlash()
        {
            Accept(new UseProgramSkillCommand(0, Driver, "virtual", [], [1], G.Revision, Pending!.PromptId)); ReachPlay();
        }
        public void NextOwnPlay()
        {
            var round = Events<RoundStartedEvent>().Last().RoundNumber;
            Accept(new EndPlayPhaseCommand(0, G.Revision, Pending!.PromptId)); ReachPlay();
            Need(Events<RoundStartedEvent>().Last().RoundNumber > round,
                "The fourth declaration must occur in a genuinely later native round.");
        }
        public static Scene Start(string skill)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheThirdSlashPackage(), new Fixture());
            var setup = new ShanheBattleSetup(87, 1, "fixture:third-slash", Owner, 4, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.ColdSlash, Suit.Spade, 9), new(CardKind.Slash, Suit.Club, 8),
                 new(CardKind.FireSlash, Suit.Heart, 10), new(CardKind.IronChain, Suit.Club, 11)], [],
                Enumerable.Range(1, 3).Select(seat => new ShanheCombatantDefinition(Peer)
                { StartingHand = seat == 1 ? [new(CardKind.Dodge, Suit.Diamond, 2)] : [] }).ToArray(), [], new string('c', 64))
                { TacticSkillIds = [skill] };
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var s = new Scene(game); s.Accept(new StartGameCommand()); s.ReachPlay(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:third-slash", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddCard(new("fixture:third-cold-slash", "冰杀", "基本牌", CardCatalog.Get(CardKind.ColdSlash).Description, CardKind.ColdSlash));
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"modifiers":[
                    {"id":"range","query":"attackRange","operation":"add","value":10,"priority":0},
                    {"id":"quota","query":"slashLimit","operation":"add","value":10,"priority":0},
                    {"id":"hand","query":"handLimit","operation":"add","value":20,"priority":0}],
                   "activations":[{"id":"virtual","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                     "effects":[{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"usageScope":"round","usageLimit":1,
                    "effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]},{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Quiet }.ToDictionary(id => id, _ => new { name = "三板斧真实用杀驱动", description = "实体、虚拟、闪避与原生回合" }) }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Owner, "三板斧使用者", "supporter", Driver, "qun", 4));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", Quiet, "wei", 30));
            builder.AddDeck(new(Deck, "实体闪", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "山河图三板斧", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
