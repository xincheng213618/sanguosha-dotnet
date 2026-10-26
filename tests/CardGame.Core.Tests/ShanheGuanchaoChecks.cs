using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGuanchaoChecks
{
    private const string Source = StandardShanheGuanchaoPackage.SkillId;
    private const string Owner = "fixture:shanhe-guanchao-owner", Peer = "fixture:shanhe-guanchao-peer";
    private const string Ending = "fixture:shanhe-guanchao-ending", Attack = "fixture:shanhe-guanchao-peer-use";
    private const string Cardless = "fixture:shanhe-guanchao-cardless";
    private const string Deck = "fixture:shanhe-guanchao-deck";

    public static void TrueResponseUseKeepsThePhaseRankLedgerAndWholeOwnTurnScope()
    {
        var zero = Scene.Start(Source);
        zero.CardlessSlash(); zero.Play();
        Need(zero.Facts() is [{ Rank: 0, Drew: false }],
            "A real cardless virtual Slash has no effective rank and cannot earn a fabricated comparison draw.");
        foreach (var skill in new[] { Source, "ol:guanchao" })
        {
            var s = Scene.Start(skill);
            s.Use(CardKind.Duel, 1);
            s.Reach(p => p.Kind == DecisionKind.Nullification && p.PlayerSeat == 0 && p.Choices.Any(c => c.Cards.Count == 1));
            s.Answer(c => c.Cards.Count == 1); s.Play();
            var response = s.G.Events.Select(e => e.Payload).OfType<CardActionAcceptedEvent>()
                .Single(e => e.Action.Type == CardActionType.Response).Action;
            Need(response.EffectiveKind == CardKind.Nullification && response.PhysicalCards.Count == 1 &&
                s.G.CardMovements.Count(move => move.CardId == response.PhysicalCards[0].CardId &&
                    move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1,
                "A real owned counterspell remains one paid native response entity, rather than a fixture Use event.");
            Need(s.Facts().Select(e => (e.Rank, e.Drew)).SequenceEqual(skill == Source
                    ? new[] { (2, true), (9, true) } : new[] { (2, true) }),
                "The Shanhe source observes the real Nullification use without requiring any Wushuang content; the OL variant keeps its Play-use scope.");
            s.Use(CardKind.Alcohol); s.Play();
            s.Use(CardKind.Slash, 1); s.Play();
            Need(s.Facts().Select(e => (e.Rank, e.Drew)).SequenceEqual(skill == Source
                    ? new[] { (2, true), (9, true), (5, false), (12, false) }
                    : new[] { (2, true), (5, true), (12, true) }),
                "The prior true response's rank stays in the original phase ledger: 2,9,5 breaks ascending and a later 12 cannot repair it; OL remains 2,5,12. Actual: " +
                JsonSerializer.Serialize(new
                {
                    Skill = skill, Facts = s.Facts(),
                    Actions = s.G.Events.Select(e => e.Payload).OfType<CardActionAcceptedEvent>().Select(e => new
                    { e.Action.ActionId, e.Action.ActorSeat, e.Action.Type, e.Action.EffectiveKind, e.Action.EffectiveRank, e.Action.ParentActionId }),
                    PhaseHistory = s.G.Events.Select(e => e.Payload).Where(e => e is PhaseChangedEvent or TurnStartedEvent or ProgramGuanchaoPatternChosenEvent)
                        .Select(e => new { Kind = e.GetType().Name, Value = JsonSerializer.Serialize(e, e.GetType()) })
                }));

            var beforeEnding = s.Facts().Length;
            s.End();
            s.Reach(p => p.Kind == DecisionKind.RespondDodge && p.PlayerSeat == 0 &&
                s.G.State.CurrentSeat == 0 && p.Choices.Any(c => c.Cards.Count == 1));
            Need(s.G.State.CurrentSeat == 0 && s.G.State.Phase != TurnPhase.Play,
                "The opponent's native ending conversion asks the owner to use Dodge during the owner's real ending phase.");
            s.Answer(c => c.Cards.Count == 1);
            s.Reach(p => p.Kind == DecisionKind.RespondDodge && p.PlayerSeat == 0 &&
                s.G.State.CurrentSeat == 1 && p.Choices.Any(c => c.Cards.Count == 1));
            Need(s.Facts().Length == beforeEnding + (skill == Source ? 1 : 0),
                "The source rewards the owner's true ending-phase Dodge use while the ordinary OL trigger stays confined to Play.");
            if (skill == Source) Need(s.Facts().Last() is { Rank: 7, Drew: true },
                "The ending is a new actual phase, so its first ranked own use matches the retained own-turn pattern.");
            var beforeForeign = s.Facts().Length;
            s.Answer(c => c.Cards.Count == 1);
            Need(s.G.State.CurrentSeat == 1 && s.Facts().Length == beforeForeign,
                "An owned true response during somebody else's turn earns no Guanchao event or draw.");
        }
    }

    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, string skill)
    {
        public GameEngine G { get; } = game;
        public ProgramGuanchaoRankDrawEvent[] Facts() => G.Events.Select(e => e.Payload)
            .OfType<ProgramGuanchaoRankDrawEvent>().Where(e => e.SkillId == skill).ToArray();
        private void Accept(GameCommand command)
        { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "The native Guanchao command was rejected."); }
        public void Answer(Func<PromptChoice, bool> select)
        {
            var p = G.PendingDecision ?? throw new InvalidOperationException("Missing Guanchao prompt.");
            var c = p.Choices.FirstOrDefault(select) ?? throw new InvalidOperationException(JsonSerializer.Serialize(p));
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision));
        }
        public void Reach(Func<PendingDecision, bool> done)
        {
            for (var step = 0; step < 150; step++)
            {
                if (G.PendingDecision is { } p && done(p)) return;
                if (G.PendingDecision is { PlayerSeat: 0 } own)
                {
                    var pass = own.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(pass is not null, "Unexpected Guanchao decision: " + JsonSerializer.Serialize(own));
                    Accept(new AnswerPromptCommand(0, own.PromptId, pass!.Id, G.Revision));
                }
                else
                {
                    Need(G.State.Winner == Winner.None, "Both bounded Guanchao participants must remain alive.");
                    Accept(new AdvanceOneStepCommand(G.Revision));
                }
            }
            throw new InvalidOperationException("The native Guanchao scene lost its bounded continuation.");
        }
        public void Play() => Reach(p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(CardKind kind, int? target = null)
        {
            var action = G.GetHumanLegalActions().First(a => a.CardId is { } cardId &&
                G.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == cardId && card.Kind == kind) &&
                (target is null || a.TargetSeats.SequenceEqual([target.Value])));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision,
                G.PendingDecision!.PromptId, action.PlayedCardKind)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, G.PendingDecision!.PromptId));
        public void CardlessSlash() => Accept(new UseProgramSkillCommand(0, Cardless, "slash", [], [1], G.Revision, G.PendingDecision!.PromptId));
        public static Scene Start(string skill)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheGuanchaoPackage(), new Fixture());
            Need(!registry.Skills.ContainsKey("wushuang:skill-3290"), "The source fixture must not borrow Wushuang's use-observer registration.");
            ShanheStartingCard[] cards = [new(CardKind.Duel, Suit.Heart, 2),
                new(CardKind.Nullification, Suit.Spade, 9), new(CardKind.Nullification, Suit.Club, 9),
                new(CardKind.Nullification, Suit.Spade, 9), new(CardKind.Alcohol, Suit.Heart, 5),
                new(CardKind.Slash, Suit.Spade, 12)];
            var setup = new ShanheBattleSetup(83, 1, "fixture:shanhe-guanchao", Owner, 20, 0, 0, 0, 0,
                [skill], cards, [], [new(Peer) { StartingHand = [new(CardKind.Slash, Suit.Spade, 3)] }], [], new string('c', 64));
            var s = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 6, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry), skill);
            s.Accept(new StartGameCommand());
            s.Reach(p => p.SkillPrompt?.SkillId == skill && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.Reach(p => p.Choices.Any(c => c.Parameters.ContainsKey("ascending")));
            s.Answer(c => c.Parameters.GetValueOrDefault("ascending") == "true"); s.Play();
            return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-guanchao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "ol:yan-jun");
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Ending}}","revision":1,"triggers":[{"id":"other-ending-use","window":"turnEnding","subject":"owner","turnOwnerScope":"otherLiving","optional":false,
                    "effects":[{"op":"selectOwnedCards","target":"owner","minimumCards":1,"maximumCards":1,"zones":["hand"],"resultBind":"ending-material"},
                      {"op":"useBoundCardsAsSlash","target":"owner","sourceBind":"ending-material"}]}]},
                  {"id":"{{Cardless}}","revision":1,"activations":[{"id":"slash","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLivingVirtualSlashTarget","usesPerTurn":null,
                    "effects":[{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}]}]},
                  {"id":"{{Attack}}","revision":1,"triggers":[{"id":"foreign-use","window":"playPhaseStarting","subject":"owner","optional":false,
                    "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
                      {"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget","useCardActionWindows":true}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Ending, Attack, Cardless }.ToDictionary(id => id,
                        _ => new { name = "真实阶段用牌", description = "固定小局的原生结束阶段及他人回合用牌" }) }));
            foreach (var (id, program) in programs.Programs)
                b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = programs.Presentations[id] });
            b.AddGeneral(new(Owner, "观潮使用者", "supporter", Cardless, "wu", 20));
            b.AddGeneral(new(Peer, "固定守军", "supporter", Attack, "wei", 20, [Ending]));
            b.AddDeck(new(Deck, "固定原生闪牌", 0, 0, [])
                { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "观潮原生小局", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
