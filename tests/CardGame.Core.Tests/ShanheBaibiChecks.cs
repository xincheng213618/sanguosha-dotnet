using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBaibiChecks
{
    private const string Weapon = "equipment:baibi-dao", Driver = "fixture:shanhe-baibi-driver";
    private const string Actor = "fixture:shanhe-baibi-actor", Peer = "fixture:shanhe-baibi-peer";
    private const string Deck = "fixture:shanhe-baibi-deck";
    private static readonly Lazy<ContentRegistry> Registry = new(() =>
        ComposedSkillContentRegistry.CreateShowcase(additionalPackages: [new Fixture()]));

    public static void ActualEquippedWeaponObtainsRealHandCardAfterAllFourSlashKinds()
    {
        var kinds = new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash };
        var s = Scene.Start(kinds, 4); s.Equip();
        foreach (var kind in kinds)
        {
            var targetBefore = s.V(1).Hand.Select(c => c.Id).ToArray();
            var hpBefore = s.V(1).Hp;
            var slash = s.Card(kind, [1]); s.Offer();
            var use = s.E<CardUseDeclaredEvent>().Single(e => e.CardId == slash).ResolutionId;
            Need(s.V(1).Hp == hpBefore - 1 && s.E<DamageAppliedEvent>().Count(e =>
                    e.SourceCardUseResolutionId == use && e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1 &&
                s.E<MoCaoCaoBaibiIssuedEvent>().Last().Claim is { PhysicalWeaponId: > 0, VirtualIssueId: 0 } claim &&
                claim.PhysicalWeaponId == s.WeaponId && claim.UseId == use,
                "The actual equipped weapon offers only after this paid Slash has applied its real damage, without a virtual-equipment grant.");
            s.Invoke(true); s.Selection();
            var choice = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("branch") == "card");
            var slot = int.Parse(choice.Parameters["slot-index"], System.Globalization.CultureInfo.InvariantCulture);
            var taken = targetBefore[slot];
            Need(s.P is { IsPrivate: true, PlayerSeat: 0 } &&
                s.P.Choices.Where(c => c.Parameters.GetValueOrDefault("branch") == "card").All(c => c.Cards.Count == 0),
                "The holder selects a hidden native hand slot; the target's physical card faces are not offered to the holder.");
            s.Answer(c => c.Id == choice.Id); s.Play();
            Need(s.V().Hand.Any(c => c.Id == taken) && s.V(1).Hand.All(c => c.Id != taken) &&
                s.G.CardMovements.Count(m => m.CardId == taken && m.From == CardLocation.Hand(1) &&
                    m.To == CardLocation.Hand(0) && m.Reason.Value == "equipment.baibi-dao.obtain") == 1 &&
                s.E<MoCaoCaoBaibiObtainedEvent>().Last().Arrived &&
                s.G.CardMovements.Count(m => m.CardId == slash && m.To == CardLocation.DiscardPile) == 1,
                "Each Slash kind obtains one existing hand entity through the native movement and retires its own paid Slash once.");
        }
        Need(s.V(1).HandCount == 0 && s.E<MoCaoCaoBaibiIssuedEvent>().Length == 4 &&
            s.E<MoCaoCaoBaibiObtainedEvent>().Length == 4,
            "The four real Slash families each get one optional native obtain, without a duplicate supplemental skill.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public int WeaponId { get; private set; }
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        private void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual Baibi command."); }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Offer() => Until(() => P?.SkillPrompt?.SkillId == Weapon &&
            P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Selection() => Until(() => P?.SkillPrompt?.SkillId == Weapon &&
            P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "card"));
        public void Invoke(bool yes) => Answer(c => c.Parameters.GetValueOrDefault("program-action") == (yes ? "activate" : "skip"));
        public void Answer(Func<PromptChoice, bool> select)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(select);
            Need(choice is not null, "Missing actual Baibi choice: " + JsonSerializer.Serialize(p));
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Equip()
        {
            WeaponId = Card(CardKind.BaiBiDao, []); Play();
            Need(V().Equipment.Any(c => c.Id == WeaponId && c.Kind == CardKind.BaiBiDao) &&
                G.CreateCardZoneDiagnostics().Single(c => c.CardId == WeaponId).Location == CardLocation.Equipment(0) &&
                G.CardMovements.Count(m => m.CardId == WeaponId && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Processing && m.Reason == CardMoveReasons.EquipmentUse) == 1 &&
                G.CardMovements.Count(m => m.CardId == WeaponId && m.From == CardLocation.Processing &&
                    m.To == CardLocation.Equipment(0) && m.Reason == CardMoveReasons.EquipmentEnter) == 1,
                "Native equipment use pays the genuine BaiBiDao from hand through processing once, then installs that same entity in the actual weapon slot.");
        }
        public int Card(CardKind kind, int[] targets)
        {
            var card = V().Hand.First(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card.Id, targets, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            return card.Id;
        }
        public void Use(string id, int[]? targets = null)
        {
            Accept(new UseProgramSkillCommand(0, Driver, id, [], targets ?? [], G.Revision, P!.PromptId)); Play();
        }
        private void Until(Func<bool> done)
        {
            for (var step = 0; step < 150; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded actual Baibi fixture must retain its combatants.");
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "cold-damage"))
                    { Answer(c => c.Parameters.GetValueOrDefault("action") == "cold-damage"); continue; }
                    throw new InvalidOperationException("Unexpected actual Baibi boundary: " + JsonSerializer.Serialize(p));
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Actual Baibi did not reach its bounded decision.");
        }
        public static Scene Start(CardKind[] slashes, int targetCards)
        {
            var setup = new ShanheBattleSetup(61, 1, "fixture:shanhe-baibi", Actor, 8, 8, 0, 0, 0, [],
                new[] { new ShanheStartingCard(CardKind.BaiBiDao, Suit.Spade, 2) }
                    .Concat(slashes.Select((kind, index) => new ShanheStartingCard(kind, Suit.Spade, index + 1))).ToArray(), [],
                Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer, Hp: 8, MaxHpBonus: 4)
                { StartingHand = Enumerable.Range(0, targetCards).Select(index => new ShanheStartingCard(CardKind.Peach, Suit.Heart, index + 1)).ToArray() }).ToArray(),
                [], new string('b', 64));
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, Registry.Value));
            scene.Accept(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shanhe-baibi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            foreach (var kind in new[] { CardKind.BaiBiDao, CardKind.ColdSlash })
            {
                var card = CardCatalog.Get(kind);
                b.AddCard(new("fixture:shanhe-baibi-card-" + kind, card.DisplayName, card.CategoryName, card.Description, kind));
            }
            var catalog = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"slash-quota","query":"slashLimit","operation":"unlimited","value":0,"priority":0}],"activations":[
                {"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
                {"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"selectedTarget","chained":true}]},
                {"id":"unequip","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["equipment"]}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "百辟刀真实驱动", description = "真实伤害、连环和卸下装备" } } }));
            b.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            b.AddGeneral(new(Actor, "真实百辟刀使用者", "supporter", Driver, "qun", 4));
            b.AddGeneral(new(Peer, "实际杀伤目标", "supporter", "standard:none", "shu", 4));
            b.AddDeck(new(Deck, "固定真实摸牌", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "山河百辟刀真实装备", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Actor, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
