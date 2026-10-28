using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceTiepaoNativeChecks
{
    private const string Skill = StandardShanheSourceWuCongPackage.TiepaoSkillId;
    private const string Actor = "fixture:source-tiepao-owner", Peer = "fixture:source-tiepao-peer";
    private const string Driver = "fixture:source-tiepao-driver", Watch = "fixture:source-tiepao-payment-child";
    private const string Quiet = "fixture:source-tiepao-quiet", Deck = "fixture:source-tiepao-deck";

    public static void ActualMaterialRetaliationReturnsAndDirectionalDistanceExpiresAtOwnerStart()
    {
        var s = Scene.Start(dodge: false);
        var first = s.V().Hand.Single(c => c.Kind == CardKind.Duel).Id;
        var initialHp = s.V(1).Hp;
        s.Incoming(1); s.Offer(); s.Answer(c => c.Cards.SequenceEqual([first]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        var r = parent.ShanheSourceTiepao!;
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == r.UseId);
        Need(r is { Returned: false, MaterialCardId: not null } && r.MaterialCardId == first &&
            use.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.Slash } action &&
            action.ActorSeat == 0 && action.ProviderSeat == 0 && action.TargetSeats.SequenceEqual([1]) &&
            action.PhysicalCards is [{ CardId: var material, From: var from, CardKind: CardKind.Duel }] && material == first && from == CardLocation.Hand(0) &&
            action.ConversionChain.Single().SkillId == Skill &&
            s.G.CardMovements.Count(m => m.CardId == first && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.E<ProgramBindingResolvedEvent>().All(e => e.FrameId != parent.Id) && s.E<ShanheSourceTiepaoDistanceGrantedEvent>().Length == 0,
            "A real non-Slash material pays once into Processing and its movement child pauses under the same original damage/Tiepao/native-use frames before any counter damage or distance grant.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        Need(s.V(1).Hp == initialHp - 1 && s.E<ShanheSourceTiepaoReturnedEvent>() is [{ CausedDamage: true }] &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == parent.Id && e.Completed) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.UseId) == 1 &&
            s.G.GetCombatDistance(1, 0) == 2 && s.G.GetCombatDistance(0, 1) == 1,
            "The original material, Dodge/damage children and whole native Use return once; only the original damage source's distance toward the owner gains one.");

        var second = s.V().Hand.First(c => c.Kind == CardKind.Peach).Id;
        s.Incoming(1); s.Offer(); s.Answer(c => c.Cards.SequenceEqual([second])); s.Play();
        Need(s.E<ShanheSourceTiepaoDistanceGrantedEvent>().Length == 2 && s.G.GetCombatDistance(1, 0) == 3 &&
            s.G.GetCombatDistance(0, 1) == 1 && s.E<CardUseFinishedEvent>().Count(e => e.CardId == second) == 1,
            "A second real received-damage occurrence in the same turn can consume another arbitrary material and stacks one directed distance contribution without a turn quota.");
        var offers = s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill);
        s.Incoming(2); s.Play();
        Need(s.G.GetCombatDistance(0, 2) == 2 && s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Skill) == offers,
            "A distant real damage source does not receive a range-free counter Slash when the source text provides no distance exemption.");
        var turn = s.G.State.TurnNumber;
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.G.State.TurnNumber > turn && s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && s.G.ResolutionStack.Count == 0);
        Need(s.G.GetCombatDistance(1, 0) == 1 && s.G.GetCombatDistance(0, 1) == 1 &&
            s.E<ShanheSourceTiepaoDistanceGrantedEvent>().Length == 2 && s.G.State.Winner == Winner.None,
            "Both paid contributions expire at the owner's next genuine TurnStarted, without removing the original facts or completing a battle.");

        var blocked = Scene.Start(dodge: true);
        var material2 = blocked.V().Hand.Single(c => c.Kind == CardKind.Duel).Id;
        blocked.Incoming(1); blocked.Offer(); blocked.Answer(c => c.Cards.SequenceEqual([material2])); blocked.Play();
        Need(blocked.E<CardRespondedEvent>().Any(e => e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) &&
            blocked.E<ShanheSourceTiepaoReturnedEvent>() is [{ CausedDamage: false }] &&
            blocked.E<ShanheSourceTiepaoDistanceGrantedEvent>().Length == 0 && blocked.G.GetCombatDistance(1, 0) == 1 &&
            blocked.E<CardUseFinishedEvent>().Count(e => e.CardId == material2) == 1,
            "A real Dodge consumes the actual response and completes the original counter Use without granting distance merely for declaration.");
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(0).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, (result.Error?.Message ?? "Rejected source14075 command.") + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Incoming(int seat) => Accept(new UseProgramSkillCommand(0, Driver, "incoming", [], [seat], G.Revision, P!.PromptId));
        public void Offer()
        {
            Until(() => P?.SkillPrompt?.SkillId == Skill);
            if (P!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"))
                Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Need(P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "source-tiepao") == true,
                "The accepted original damage trigger offers its actual owned material selection." + Diagnostic());
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 260; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The fresh bounded source14075 fixture remains live." + Diagnostic());
                if (P is { } p)
                {
                    if (p.PlayerSeat != 0) Accept(new AdvanceOneStepCommand(G.Revision));
                    else if (p.SkillPrompt?.SkillId == Watch) Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
                    else if (p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage") is { } pass)
                        Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, pass.Id, G.Revision));
                    else throw new InvalidOperationException("Unexpected source14075 native boundary." + Diagnostic());
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source14075 exceeded bounded fresh progression." + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, P,
            Frames = G.ResolutionStack.Select(f => new { f.Id, Type = f.GetType().Name }),
            Recent = G.Events.TakeLast(5).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(bool dodge)
        {
            var setup = new ShanheBattleSetup(109, 1, "source-tiepao-native", Actor, 9, 0, 0, 0, 0, [],
                [new(CardKind.Duel, Suit.Club, 7), new(CardKind.Peach, Suit.Heart, 6), new(CardKind.Peach, Suit.Diamond, 8)], [],
                [new(Peer) { StartingHand = dodge ? [new(CardKind.Dodge, Suit.Heart, 2)] : [] },
                 new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }], [], new string('b', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            { PlayerGeneralId = StandardShanheSourceWuCongPackage.BaseId, StartingCards = [],
                EnemyRoster = setup.EnemyRoster.Select(r => r with { GeneralId = "classic:guan-yu" }).ToArray() })
                .WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with
                    { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Actor, Peer]).Distinct().ToArray() } : mode);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-tiepao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"native-material-child","window":"cardsMoved","subject":"owner",
                    "sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{CardMoveReasons.Use.Value}}"],"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Driver, Watch, Quiet }.ToDictionary(id => id, id => new { name = id, description = "铁炮原生伤害、用牌与付款返回",
                        optionLabels = id == Watch ? new Dictionary<string, string> { ["continue"] = "继续" } : new Dictionary<string, string>() }) }));
            foreach (var (id, program) in catalog.Programs)
                b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Actor, "原铁炮持有者", "supporter", Skill, "qun", 9, [Driver, Watch]));
            b.AddGeneral(new(Peer, "实际伤害来源", "supporter", Quiet, "wei", 12));
            b.AddDeck(new(Deck, "安静的实际正常摸牌", 0, 2, [new("standard:dodge", 48)]));
        }
    }
}
