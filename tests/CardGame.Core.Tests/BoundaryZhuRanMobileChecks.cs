using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryZhuRanMobileChecks
{
    private const string Hero = "boundary:zhu-ran-mobile", Skill = "boundary:danshou-mobile", Source = "fixture:boundary-zhu-ran-source", Peer = "fixture:boundary-zhu-ran-peer",
        Quiet = "fixture:boundary-zhu-ran-quiet", Skip = "fixture:boundary-zhu-ran-skip", Driver = "fixture:boundary-zhu-ran-driver",
        CostHold = "fixture:boundary-zhu-ran-cost", GainHold = "fixture:boundary-zhu-ran-gain", RoleChange = "fixture:boundary-zhu-ran-role";

    public static void MandatoryDrawAndActualActorInsteadOfProvider()
    {
        var s = Scene.Start(); s.Play(); var before = s.V(1).Hand.Count; s.End(); s.Gain();
        s.Need(s.Root?.BoundaryDanshou is { X: 0, Actor: 0, Stage: BoundaryDanshouStage.Complete } && s.V(1).Hand.Count == before + 1 &&
            s.Facts<BoundaryDanshouPaidEvent>().Length == 0 && s.P!.SkillPrompt!.SkillId == GainHold,
            "A genuinely untargeted other role's Ending forces one native draw without offering activation/decline or any discard branch.");
        s.Release(); s.Play(ambient: true);
        s.Need(!s.Facts<BoundaryDanshouStartedEvent>().Any(e => e.Actor == 1), "Zhu Ran's own actual Ending never starts either Danshou branch.");

        s = Scene.Start(roleChange: true); s.Play(); var slash = s.V().Hand.First(c => c.Kind == CardKind.Slash).Id;
        s.Native(slash, 1); s.Play();
        var use = s.Facts<CardUseDeclaredEvent>().Single(e => e.CardId == slash);
        s.Need(s.Facts<BoundaryDanshouUseActorEvent>().Last(e => e.UseId == use.ResolutionId) is { Actor: 3, Provider: 0 } &&
            s.Facts<BoundaryDanshouUseTargetEvent>().Count(e => e.UseId == use.ResolutionId && e.Target == 1) == 1,
            "The original physical provider remains seat0, while the native actor replacement belongs to seat3 and the whole Slash has only one target occurrence.");
        s.End(); s.Gain();
        s.Need(s.Root?.BoundaryDanshou is { X: 0, Actor: 0 } && s.Facts<BoundaryDanshouPaidEvent>().Length == 0,
            "The ending material provider did not actually use that replaced Slash, so its Ending takes the mandatory draw branch.");
    }

    public static void WholeUseHistoryExactMixedHePaymentAndPaidCold()
    {
        var s = Scene.Start(heroHp: 6); s.Play(); s.EquipHero(); s.Play();
        var armor = s.V(1).Equipment.Single(c => c.Kind == CardKind.RenwangShield).Id;
        var duel = s.V().Hand.Single(c => c.Kind == CardKind.Duel).Id;
        s.Native(duel, 1); s.Play();
        s.Need(s.Facts<CardActionAcceptedEvent>().Count(e => e.Action.Type == CardActionType.Response && e.Action.EffectiveKind == CardKind.Slash) >= 4,
            "The fixed real Duel exchanges several actual played Slash responses before completing its one native use.");
        var hp = s.V(1).Hp; var slash = s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id;
        s.Native(slash, 1); s.Play();
        var slashUse = s.Facts<CardUseDeclaredEvent>().Single(e => e.CardId == slash).ResolutionId;
        s.Need(s.V(1).Hp == hp && s.Facts<BoundaryDanshouUseTargetEvent>().Count(e => e.UseId == slashUse && e.Target == 1) == 1,
            "A real black Slash made ineffective by native Renwang armor remains a card use that designated Zhu Ran once.");
        s.End(); s.Offer(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.CostMenu();
        s.Need(s.Root?.BoundaryDanshou is { X: 2, Actor: 0 } && s.P!.Choices.Any(c => c.Cards.SequenceEqual([armor])) &&
            !s.Facts<BoundaryDanshouStartedEvent>().Any(e => e.X > 2),
            "One Duel and one ineffective Slash cost exactly two HE cards; played responses and repeated event/target-effect visits do not inflate the whole-use count.");
        var hand = s.V(1).Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Cards.SequenceEqual([armor])); s.Cold();
        s.Need(s.Zone(armor) == CardLocation.Equipment(1) && s.Zone(hand) == CardLocation.Hand(1) && s.Facts<BoundaryDanshouPaidEvent>().Length == 0,
            "The first selected real equipment entity is staged only; cold cannot pay an incomplete mixed-zone batch.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "card" && c.Cards.SequenceEqual([hand]));
        var targetHp = s.V().Hp; s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "pay"); s.Held();
        s.Need(s.Zone(armor) == CardLocation.DiscardPile && s.Zone(hand) == CardLocation.DiscardPile && s.V().Hp == targetHp &&
            s.Root?.BoundaryDanshou is { Stage: BoundaryDanshouStage.PaidChildren, Actual: 2 } &&
            s.Facts<BoundaryDanshouPaidEvent>() is [{ X: 2, Actual: 2 }],
            "Both original HE entities pay in the same native logical-discard request before its actual movement child returns and before damage is authorized.");
        s.Cold(); s.Release(); s.Play(ambient: true);
        s.Need(s.V().Hp == targetHp - 1 && s.Facts<DamageAppliedEvent>().Count(e => e.SourceSeat == 1 && e.TargetSeat == 0 && e.Amount == 1) == 1 &&
            s.Facts<BoundaryDanshouCompletedEvent>() is [{ Actual: 2, DamageAuthorized: true }] &&
            s.G.CardMovements.Count(m => (m.CardId == armor || m.CardId == hand) && m.Reason.Value == "skill-program.boundary-zhu-ran.danshou.discard") == 2,
            "Cold drains the original paid child once and then the same skill frame deals exactly one native damage without repaying either source entity.");
        s.End(); s.Gain();
        s.Need(s.Root?.BoundaryDanshou is { X: 0, Actor: 0, Turn: > 1 }, "A later actual turn starts a fresh target history rather than carrying the preceding two targeted uses forward.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play(bool ambient = false) => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, ambient);
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Gain() => Reach(() => P?.SkillPrompt?.SkillId == GainHold);
        public void Held() => Reach(() => P?.SkillPrompt?.SkillId == CostHold);
        public void Offer() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void CostMenu() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-danshou"));
        public void Release() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void EquipHero()
        {
            Accept(new UseProgramSkillCommand(0, Driver, "equip-target", [], [1], G.Revision, P!.PromptId));
            Reach(() => P?.SkillPrompt?.SkillId == Driver); Answer(c => c.Cards.Count == 1);
        }
        public void Native(int id, params int[] targets)
        {
            var a = G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == id && a.Kind != LegalActionKind.Recast && a.TargetSeats.SequenceEqual(targets));
            Need(a is not null, "The fixed scene retains its exact legal native card use.");
            Accept(new PlayCardCommand(0, id, a!.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
            { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.Single(predicate); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision)); }
        public void Reach(Func<bool> done, bool ambient = false)
        {
            for (var i = 0; i < 300; i++)
            {
                if (done()) return;
                if (P is { Kind: DecisionKind.RespondSlash or DecisionKind.RespondDodge } response)
                {
                    if (response.PlayerSeat != 0) { Accept(new AdvanceOneStepCommand(G.Revision)); continue; }
                    var c = response.Choices.FirstOrDefault(c => c.Cards.Count == 1) ?? response.Choices.Last();
                    Accept(new AnswerPromptCommand(response.PlayerSeat, response.PromptId, c.Id, G.Revision)); continue;
                }
                if (P?.SkillPrompt?.SkillId == RoleChange) { Answer(c => c.Targets.SequenceEqual([3])); continue; }
                if (ambient && P?.SkillPrompt?.SkillId == GainHold) { Release(); continue; }
                Need(P is null, "The native Danshou scene reached an unexpected decision."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The bounded native Danshou scene missed its next original boundary.");
        }
        public void Cold()
        {
            var state = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == state, "Cold preserves the original actual-target history, private staged HE set and once-paid original Ending child.");
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements });
        public void Need(bool ok, string why)
        {
            if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, P,
                Root = Root?.BoundaryDanshou, Actors = Facts<BoundaryDanshouUseActorEvent>(), Targets = Facts<BoundaryDanshouUseTargetEvent>(), Last = G.Events.TakeLast(10).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) }));
        }
        private void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Danshou command."); }
        public static Scene Start(bool roleChange = false, int heroHp = 8)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] hand = [new(CardKind.RenwangShield, Suit.Club, 2), new(CardKind.Duel, Suit.Heart, 1), new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Spade, 8), new(CardKind.Slash, Suit.Spade, 9), new(CardKind.IronChain, Suit.Club, 9)];
            ShanheStartingCard[] heroHand = [new(CardKind.Slash, Suit.Club, 3), new(CardKind.Slash, Suit.Heart, 4), new(CardKind.Dodge, Suit.Club, 5), new(CardKind.Peach, Suit.Heart, 6)];
            var setup = new ShanheBattleSetup(29, 1, "boundary-zhu-ran", Source, 8, 0, 0, 0, 0, roleChange ? [RoleChange] : [], hand, [],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }],
                [new(Hero, MaxHpBonus: 4, Hp: heroHp, ExtraSkillIds: [Quiet, Skip, CostHold, GainHold]) { StartingHand = heroHand }], new string('d', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var s = new Scene(g, registry); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-zhu-ran-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
             {"id":"{{Skip}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
             {"id":"{{Driver}}","revision":1,"activations":[{"id":"equip-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"zones":["hand"],"cardCategories":["equipment"],"count":1,"destination":"selectedTargetEquipment"}]}]},
             {"id":"{{CostHold}}","revision":1,"triggers":[{"id":"paid","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill-program.boundary-zhu-ran.danshou.discard"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
             {"id":"{{GainHold}}","revision":1,"triggers":[{"id":"paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:danshou-mobile.Draw"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
             {"id":"{{RoleChange}}","revision":1,"triggers":[{"id":"replace","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["slash"],"priority":1000,"optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLegalCurrentCardTarget"},{"op":"replaceCurrentCardUseActor","target":"selectedTarget"}]}]}
            ]}
            """;
            var names = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Skip, Driver, RoleChange }) names[id] = new { name = id, description = "固定真实阶段及原生角色变更。" };
            foreach (var id in new[] { CostHold, GainHold }) names[id] = new { name = id, description = "原生移动付款后的真实子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Source, "胆守固定原生使用者", "", Quiet, "wei", 8, [Driver])); b.AddGeneral(new(Peer, "胆守安静同桌", "", Quiet, "wei", 8, [Skip]));
            const string deck = "fixture:boundary-zhu-ran-deck";
            b.AddDeck(new(deck, "胆守固定库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "界朱然实际原生用牌", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Source, Hero, Peer],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
