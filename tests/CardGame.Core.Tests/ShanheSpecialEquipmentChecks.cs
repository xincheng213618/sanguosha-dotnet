using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSpecialEquipmentChecks
{
    private const string Halberd = "hulao:wushuang-halberd", Robe = "hulao:red-cotton-robe";
    private const string Shura = "shenwu:shura-inferno-halberd", Yaodao = "shanhe:yaodao-chitong";
    private const string Driver = "fixture:special-equipment-driver", Actor = "fixture:special-equipment-actor";
    private const string Peer = "fixture:special-equipment-peer", Deck = "fixture:special-equipment-deck";
    private static readonly Lazy<ContentRegistry> Production = new(() =>
        StandardShanheCampaignCatalog.CreateBattleRegistry(new ShanheBattleSetup(73, 1,
            "fixture:special-equipment-production", "classic:cao-cao", 4, 4, 0, 0, 0, [],
            new[] { CardKind.WushuangHalberd, CardKind.RedCottonRobe, CardKind.ShuraInfernoHalberd,
                    CardKind.QiBaoDao, CardKind.BaiBiDao, CardKind.YaodaoChitong }
                .Select(kind => new ShanheStartingCard(kind, Suit.Spade, 2)).ToArray(), [],
            [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }], [], "fixture:special-equipment")));

    public static void YaodaoColdSlashUsesPaidNativeJudgmentAndActualRedirect()
    {
        CheckYaodaoDefinition();
        var s = Scene.Start(CardKind.YaodaoChitong, rejudge: true);
        var weapon = s.Card(CardKind.YaodaoChitong, []); s.Play();
        var replacement = s.V().Hand.Single(c => c.Kind == CardKind.Dodge && c.Suit == Suit.Club).Id;
        var slash = s.Card(CardKind.ColdSlash, [1]);
        s.Until(() => s.P?.Kind == DecisionKind.ProgramJudgmentReplacement);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Yaodao);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == slash);
        var judgment = s.G.ResolutionStack.OfType<JudgmentFrame>().Single();
        Need(root.YaodaoChitong is { WeaponCardId: var id, OriginalTargetSeat: 1, Resolved: false } && id == weapon &&
            judgment.ParentFrameId == root.Id && judgment.TargetSeat == 0 && judgment.Reason == "shanhe.yaodao-chitong" &&
            use.CardKind == CardKind.ColdSlash && use.Action!.PhysicalCards.Select(c => c.CardId).SequenceEqual([slash]) &&
            s.Zone(slash) == CardLocation.Processing && s.Zone(replacement) == CardLocation.Hand(0) &&
            s.E<YaodaoChitongResolvedEvent>().Length == 0,
            "The paid real Cold Slash suspends on the weapon's own native judgment before a replacement or random redirect is paid.");
        s.Answer(c => c.Cards.SequenceEqual([replacement])); s.Play();
        var resolved = s.E<YaodaoChitongResolvedEvent>().Single();
        var redirected = resolved.NewTargetSeat;
        Need(resolved is { OriginalTargetSeat: 1, FinalSuit: Suit.Club, CandidateCount: 2 } && redirected is 2 or 3 &&
            s.E<ProgramJudgmentReplacementResolvedEvent>().Single() is
                { Activated: true, OwnerSeat: 0, ReplacementCardId: var paid } && paid == replacement &&
            s.E<JudgmentRequestedEvent>().Count(e => e.Reason == "shanhe.yaodao-chitong") == 1 &&
            s.E<DamageAppliedEvent>() is [{ SourceSeat: 0, Amount: 2, Nature: DamageNature.Cold }] &&
            s.E<DamageAppliedEvent>().Single().TargetSeat == redirected && s.V(1).Hp == 8 && s.V(redirected!.Value).Hp == 6 &&
            s.Zone(replacement) == CardLocation.DiscardPile &&
            s.G.CardMovements.Count(m => m.CardId == replacement && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == replacement && m.From == CardLocation.Processing && m.To.Zone == CardZoneKind.Judgment) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == replacement && m.To == CardLocation.DiscardPile) == 1 &&
            s.V().Equipment.Any(c => c.Id == weapon),
            "The real Club retrial entity is spent once; the final black result redirects the original Cold Slash to one legal other target and applies its black-Slash bonus once.");
        s.PaidOnce(slash);
    }

    private static void CheckYaodaoDefinition()
    {
        var rules = $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Yaodao}}}","revision":1,"triggers":[
         {"id":"judge-and-redirect","window":"slashTargetRedirecting","ownerRelation":"actor",
          "cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"optional":false,"effects":[
           {"op":"beginYaodaoChitong","target":"owner"},
           {"op":"startJudgment","target":"owner","judgmentReason":"shanhe.yaodao-chitong","resultBind":"cards","outcomeBind":"outcome","visibility":"public"},
           {"op":"resolveYaodaoChitong","target":"owner","sourceBind":"outcome"},
           {"op":"moveBoundCards","target":"owner","sourceBind":"cards","destination":"discardPile","awaitMovementTriggers":true}]}]}]}
        """;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [Yaodao] = new { name = "妖刀赤瞳", description = "原生四种杀的固定判定合同" } } });
        Need(SkillProgramCatalog.Load(rules, presentation).Programs[Yaodao].Triggers.Single().CardKinds.Contains(CardKind.ColdSlash),
            "The native Slash-redirecting loader admits an explicit Cold Slash member of the complete weapon family.");
        foreach (var invalid in new[]
        {
            rules.Replace("\"ownerRelation\":\"actor\"", "\"ownerRelation\":\"target\"", StringComparison.Ordinal),
            rules.Replace("\"coldSlash\"", "\"duel\"", StringComparison.Ordinal),
            rules.Replace("\"sourceBind\":\"cards\"", "\"sourceBind\":\"outcome\"", StringComparison.Ordinal)
        })
        {
            var rejected = false;
            try { _ = SkillProgramCatalog.Load(invalid, presentation); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "Explicit Cold Slash support preserves the weapon's fixed actor, Slash-only filter and exact physical judgment cleanup contract.");
        }
    }

    public static void OtherLivingTargetSetUsesCapturedCountAndRejectsWrongDefinitions()
    {
        const string id = "fixture:other-living-targets";
        var rules = $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{id}}}","revision":1,"modifiers":[
         {"id":"all-others","query":"cardTargetCount","operation":"set","valueExpression":"otherLivingPlayerCount",
          "cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"priority":0}]}]}
        """;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = "实际其他存活人数", description = "原生杀目标数" } } });
        var modifier = SkillProgramCatalog.Load(rules, presentation).Programs[id].Modifiers.Single();
        var context = new SkillProgramRuleContext(new(0, 4, 4, 0, TurnPhase.Play), 1);
        Need(new[] { 0, 2, 7 }.All(count => modifier.EvaluateValue(context with { OtherLivingPlayerCount = count }) == count),
            "The shared modifier uses its captured actual other-living count, including zero, rather than a local mode maximum.");
        foreach (var invalid in new[]
        {
            rules.Replace("\"operation\":\"set\"", "\"operation\":\"add\"", StringComparison.Ordinal),
            rules.Replace("\"query\":\"cardTargetCount\"", "\"query\":\"handLimit\"", StringComparison.Ordinal),
            rules.Replace("\"coldSlash\"", "\"duel\"", StringComparison.Ordinal),
            rules.Replace(",\"coldSlash\"", "", StringComparison.Ordinal)
        })
        {
            var rejected = false;
            try { _ = SkillProgramCatalog.Load(invalid, presentation); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "The loader limits dynamic all-other targets to an owner CardTargetCount Set over the complete four-Slash family.");
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.PlayerCount).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        private void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.ToString() ?? "Rejected native special-equipment command."); }
        public void Answer(Func<PromptChoice, bool> choose)
        {
            var p = P!; var c = p.Choices.FirstOrDefault(choose);
            Need(c is not null, "Missing native special-equipment choice: " + JsonSerializer.Serialize(p));
            Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision));
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, int[] targets)
        { Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId)); Play(); }
        public int Card(CardKind kind, int[] targets)
        {
            var card = V().Hand.Single(c => c.Kind == kind);
            var action = G.GetHumanLegalActions().Single(a => a.CardId == card.Id && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, card.Id, targets, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
            return card.Id;
        }
        public void PaidOnce(int card)
        {
            Need(G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
                G.CardMovements.Count(m => m.CardId == card && m.To == CardLocation.DiscardPile) == 1 &&
                E<CardUseFinishedEvent>().Count(e => e.CardId == card) == 1 && Zone(card) == CardLocation.DiscardPile,
                "The native Slash keeps one physical payment, one completion and one final discard through equipment continuations.");
        }
        public void Until(Func<bool> ready)
        {
            for (var step = 0; step < 180; step++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None, "The bounded equipment fixture ended before its real boundary.");
                if (P is { PlayerSeat: 0 } p)
                {
                    if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "cold-damage"))
                        Answer(c => c.Parameters.GetValueOrDefault("action") == "cold-damage");
                    else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "pass"))
                        Answer(c => c.Parameters.GetValueOrDefault("response") == "pass");
                    else throw new InvalidOperationException("Unexpected special-equipment boundary: " + JsonSerializer.Serialize(p));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded native special-equipment continuation: " + JsonSerializer.Serialize(P));
        }
        public static Scene Start(CardKind equipment, int playerCount = 4, bool robeDeck = false, bool targetHand = false, bool rejudge = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(playerCount, robeDeck));
            var cards = new List<ShanheStartingCard> { new(equipment, Suit.Spade, 6), new(CardKind.ColdSlash, Suit.Spade, 7) };
            if (rejudge) cards.Add(new(CardKind.Dodge, Suit.Club, 8));
            var enemies = Enumerable.Range(1, playerCount - 1).Select(_ => new ShanheCombatantDefinition(Peer)
                { StartingHand = targetHand ? [new(CardKind.Peach, Suit.Heart, 3)] : [] }).ToArray();
            var setup = new ShanheBattleSetup(73, 1, "native-special-equipment", Actor, 8, 0, 0, 0, 0,
                rejudge ? ["boundary:guidao"] : [], cards, [], enemies, [], new string('c', 64));
            var g = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = playerCount, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, registry);
            var s = new Scene(g); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(int playerCount, bool robeDeck) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-special-equipment", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            foreach (var id in new[] { Halberd, Robe, Shura, Yaodao, "boundary:guidao" }) b.AddSkill(Production.Value.GetSkill(id));
            foreach (var kind in new[] { CardKind.WushuangHalberd, CardKind.RedCottonRobe, CardKind.ShuraInfernoHalberd, CardKind.YaodaoChitong, CardKind.ColdSlash })
                b.AddCard(new("fixture:special-" + kind.ToString().ToLowerInvariant(), CardCatalog.Get(kind).DisplayName, CardCatalog.Get(kind).CategoryName, CardCatalog.Get(kind).Description, kind));
            var rules = $$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[
             {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipment"}]},
             {"id":"cold","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1,"nature":"cold"}]},
             {"id":"normal","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]},
             {"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"selectedTarget","chained":true}]}]}]}
            """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "实际装备与属性伤害", description = "只驱动原生装备和伤害" } } }));
            b.AddSkill(new(Driver, "实际装备与属性伤害", "小固定实体牌堆") { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            b.AddGeneral(new(Actor, "特殊装备实际使用者", "supporter", Driver, "qun", 8));
            b.AddGeneral(new(Peer, "特殊装备实际目标", "supporter", "standard:none", "wei", 8));
            b.AddDeck(new(Deck, "小固定实体库存", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 24).Select(_ =>
                new ContentDeckPhysicalCard(robeDeck ? "fixture:special-redcottonrobe" : "standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "真实特殊装备", playerCount, playerCount, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Actor, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = playerCount - 1 }));
        }
    }
}
