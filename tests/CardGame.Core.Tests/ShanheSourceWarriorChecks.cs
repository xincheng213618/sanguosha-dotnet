using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceWarriorChecks
{
    private const string Luoyi = StandardShanheSourceWarriorPackage.LuoyiSkillId;
    private const string Deck = "fixture:source-warrior-deck", Enemy = "fixture:source-warrior-target";
    private const string MoveReason = "skill-program." + Luoyi + ".MoveBoundCards";

    public static void LuoyiAcceptsMixedEntitiesAndItsSourceBonusExpiresAtTheNextOwnerTurn()
    {
        var s = Scene.Start();
        s.Reveal();
        var shown = s.Revealed;
        var gained = shown.Where(c => c.Kind is CardKind.Slash or CardKind.Crossbow).Select(c => c.Id).ToArray();
        var rest = shown.Single(c => c.Kind == CardKind.BaguaFormation).Id;
        s.Option("skip-draw"); s.Play();
        var grant = s.Events<CardDamageModifierGrantedEvent>().Single(e => e.Modifier.Source.SkillId == Luoyi).Modifier;
        Need(gained.Length == 2 && gained.All(id => s.V.Hand.Any(c => c.Id == id) &&
                s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing &&
                    m.To == CardLocation.Hand(0) && m.Reason.Value == MoveReason) == 1) &&
            s.CleanupCount(rest) == 1 && s.Zone(rest) == CardLocation.DiscardPile && s.NormalDraws == 0 && s.V.HandCount == 4 &&
            s.Events<ProgramTurnPhaseSubstitutedEvent>().Count(e => e.SkillId == Luoyi && e.SkippedPhase == SkillProgramTurnPhase.Draw) == 1 &&
            grant.Amount == 1 && grant.Expiration == SkillProgramDamageModifierExpiration.NextOwnerTurnStart &&
            grant.SourceScope == SkillProgramDamageModifierSourceScope.DamageSource,
            "The actual second acceptance gives all eligible original cards, discards the remainder and commits real Draw substitution before its native +1 source modifier. " + s.Diagnostic());
        s.Slash(); s.Play();
        Need(s.Damages is [{ Amount: 2 }], "The accepted source723 benefit adds exactly one to the real Slash damage.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.G.State.TurnNumber == 3 && s.IsInitialChoice);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play();
        Need(s.Events<ProgramCardsRevealedEvent>().Count(e => e.SkillId == Luoyi) == 1 && s.NormalDraws == 2,
            "On the next real owner turn the first choice can decline without revealing again and normal Draw2 remains available.");
        s.Slash(); s.Play();
        Need(s.Damages.Select(e => e.Amount).SequenceEqual([2, 1]) &&
            s.Events<CardDamageModifierGrantedEvent>().Count(e => e.Modifier.Source.SkillId == Luoyi) == 1,
            "The original +1 expires at next owner turn start; a later physical Slash deals one without a second grant. " + s.Diagnostic());
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public bool IsInitialChoice => P?.SkillPrompt?.SkillId == Luoyi &&
            P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public CardSnapshot[] Revealed => Events<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Luoyi).Cards.ToArray();
        public DamageAppliedEvent[] Damages => Events<DamageAppliedEvent>().Where(e => e.SourceSeat == 0 && e.TargetSeat == 1).ToArray();
        public int NormalDraws => G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Draw);
        public int CleanupCount(int id) => G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing &&
            m.To == CardLocation.DiscardPile && m.Reason.Value == MoveReason);
        public CardLocation Zone(int id) => G.CardMovements.Last(m => m.CardId == id).To;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, (result.Error?.Message ?? "Rejected source warrior command.") + " " + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Need(p.PlayerSeat == 0, "Only the real human answers this source723 prompt."); Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(predicate).Id, G.Revision)); }
        public void Option(string id) => Answer(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public void Reveal()
        {
            Need(IsInitialChoice && Events<ProgramCardsRevealedEvent>().All(e => e.SkillId != Luoyi) && NormalDraws == 0,
                "Source723 offers its first optional invocation before revealing or performing normal Draw.");
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Until(() => P?.SkillPrompt?.SkillId == Luoyi && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "normal-draw"));
            Need(Revealed.Length == 3 && Revealed.All(c => Zone(c.Id) == CardLocation.Processing) && V.HandCount == 2 &&
                NormalDraws == 0 && !Events<ProgramTurnPhaseSubstitutedEvent>().Any(e => e.SkillId == Luoyi) &&
                !Events<CardDamageModifierGrantedEvent>().Any(e => e.Modifier.Source.SkillId == Luoyi),
                "Three original deck entities are public and remain in Processing at the actual second decision; skip, gain and bonus have not happened.");
        }
        public void Slash()
        {
            var card = V.Hand.First(c => c.Kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash);
            var action = G.GetHumanLegalActions().First(a => a.CardId == card.Id && a.TargetSeats.SequenceEqual([1]) && a.ConversionSource is null);
            Accept(new PlayCardCommand(0, card.Id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The bounded source723 flow must not finish a battle.");
                if (P is { PlayerSeat: 0 } p)
                {
                    var pass = p.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                    Need(pass is not null, "Unexpected source723 human boundary. " + Diagnostic());
                    Accept(new AnswerPromptCommand(0, p.PromptId, pass!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded source723 flow missed its native boundary. " + Diagnostic());
        }
        public string Diagnostic() => JsonSerializer.Serialize(new
        {
            G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Pending = P, V.HandCount, NormalDraws,
            Moves = G.CardMovements.TakeLast(8), Stack = G.ResolutionStack.Select(f => new { f.Id, Type = f.GetType().Name })
        });
        public static Scene Start(bool armorOnly = false, CardKind initialSlashKind = CardKind.Slash)
        {
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-warrior", StandardShanheSourceWarriorPackage.HellId,
                4, 0, 0, 2, 0, [], [new(initialSlashKind, Suit.Heart, 7), new(initialSlashKind, Suit.Club, 8)], [],
                [new(Enemy) { StartingHand = [] }], [], new string('a', 64));
            var production = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { EnemyRoster = [new("classic:guan-yu")] });
            var registry = production.WithExtension(new Fixture(armorOnly, initialSlashKind == CardKind.ColdSlash), mode => mode.Id == ShanheBattleSetup.BattleModeId
                ? mode with { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Append(Enemy).Distinct(StringComparer.Ordinal).ToArray() } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry));
            scene.Accept(new StartGameCommand()); scene.Until(() => scene.IsInitialChoice); return scene;
        }
    }

    private sealed class Fixture(bool armorOnly, bool registerColdSlash) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-warrior", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            if (registerColdSlash)
            {
                var cold = CardCatalog.Get(CardKind.ColdSlash);
                builder.AddCard(new("fixture:source-warrior-cold-slash", cold.DisplayName, cold.CategoryName,
                    cold.Description, CardKind.ColdSlash));
            }
            builder.AddGeneral(new(Enemy, "原生空手目标", "supporter", null, "wei", 12));
            builder.AddDeck(new(Deck, "固定三张实体，正常摸牌由己方真实加值提供", 0, 0, [])
            {
                PhysicalCards = armorOnly
                    ? Enumerable.Range(0, 3).Select(i => new ContentDeckPhysicalCard("standard:bagua", Suit.Diamond, 6 + i)).ToArray()
                    : [new("standard:slash", Suit.Spade, 3), new("standard:crossbow", Suit.Club, 5), new("standard:bagua", Suit.Diamond, 6)]
            });
        }
    }
}
