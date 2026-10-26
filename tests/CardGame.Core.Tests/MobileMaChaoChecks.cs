using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileMaChaoChecks
{
    private const string Tieqi = "mobile:tieqi", Legacy = "classic:tieqi", Driver = "fixture:mobile-ma-chao-driver", Watch = "fixture:mobile-ma-chao-watch";
    private const string Mode = "identity:classic-mobile-ma-chao", Weapon = "fixture:mobile-ma-chao-halberd";

    public static void ActualThreeTargetSlashJudgesRedBlackAndDeclineBeforeEffectsCold()
    {
        var s = Scene.Start(); s.Prepare();
        s.Need(s.V(0).Hand is [{ Kind: CardKind.FireSlash }] && s.G.GetCombatDistance(0, 2) == 1 && s.G.GetCombatDistance(2, 0) == 2,
            "A real last-hand FireSlash and equipped Fangtian authorize the native multi-target Use, while reused Mashu reduces only outgoing distance.");
        var material = s.V(0).Hand.Single().Id; var red = s.Red; var black = s.Black;
        s.Native(material, [1, 2, 3]); s.TieqiPrompt(1);
        var original = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material); var actionId = original.Action!.ActionId;
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        s.TieqiPrompt(2);
        s.Activate(); s.Judgment(2, red, Suit.Heart, original.Id, actionId); s.Continue(); s.TieqiPrompt(3);
        s.Need(s.Use(original.Id).CardAttack is { TargetSeat: 1, ProhibitsDodge: false } &&
            s.Use(original.Id).PreparedTargetAttacks!.Select(a => (a.TargetSeat, a.ProhibitsDodge)).SequenceEqual([(1, false), (2, true), (3, false)]),
            "The second target's red judgment marks its later prepared native attack, while the still-current first target remains independently Dodgeable.");
        s.Activate(); s.Judgment(3, black, Suit.Spade, original.Id, actionId);
        s.Cold(); s.Continue(); s.BeforeEffects();
        var ready = s.Use(original.Id);
        s.Need(ready.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.FireSlash, ActorSeat: 0, ProviderSeat: 0 } action &&
            action.ActionId == actionId && action.TargetSeats.SequenceEqual([1, 2, 3]) && action.PhysicalCards is [var cost] && cost.CardId == material &&
            ready.CardAttack is { TargetSeat: 1, ProhibitsDodge: false } &&
            ready.PreparedTargetAttacks!.Select(a => (a.TargetSeat, a.ProhibitsDodge)).SequenceEqual([(1, false), (2, true), (3, false)]) &&
            s.E<ProgramCardTriggerResolvedEvent>().Where(e => e.SkillId == Tieqi).Select(e => (e.OpponentSeat, e.Activated)).SequenceEqual([(1, false), (2, true), (3, true)]) &&
            s.E<JudgmentRequestedEvent>().Count(e => e.Reason == "skill.mobile.tieqi") == 2 &&
            s.E<ResponseRequestedEvent>().Length == 0 && s.E<DamageAppliedEvent>().Length == 0 &&
            Enumerable.Range(1, 3).All(i => s.V(i).HandCount == 4 && s.V(i).Hp == 8),
            "All three actual designated targets choose separately before the first native target effect: decline leaves target one Dodgeable, red prohibits only prepared target two, black leaves target three Dodgeable, and the original physical action remains unchanged.");
        s.Continue(); s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 }); s.Dodge();
        s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 3 });
        s.Need(s.V(2).Hp == 7 && s.V(2).HandCount == 4 &&
            !s.E<ResponseRequestedEvent>().Any(e => e.TargetSeat == 2 && e.RequiredCardKind == CardKind.Dodge),
            "Fangtian restores the red second target's prepared prohibition: that target cannot offer or pay Dodge and takes its original FireSlash damage with its Hand untouched.");
        s.Dodge(); s.Play();
        s.Need(s.V(1).Hp == 8 && s.V(3).Hp == 8 && s.V(1).HandCount == 3 && s.V(3).HandCount == 3 &&
            s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            s.E<CardRespondedEvent>().Count(e => e.ResponderSeat == 3 && e.EffectiveCardKind == CardKind.Dodge) == 1 &&
            s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).Select(e => (e.TargetSeat, e.Amount)).SequenceEqual([(2, 1)]) &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == original.Id && e.CardId == material && e.CardKind == CardKind.FireSlash) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
            new[] { red, black }.All(id => s.Zone(id) == CardLocation.DiscardPile && s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing &&
                m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.mobile:tieqi.MoveBoundCards") == 1),
            "Cold final-target judgment returns to the same original multi-target Slash: both unbanned targets pay Dodge once, only the red target is damaged, and the two real judgment entities are cleaned once.");
    }

    public static void ClassicTieqiRetainsActualResponseStageDefault()
    {
        var s = Scene.Start(legacy: true); s.Prepare(); var material = s.V(0).Hand.Single().Id;
        s.Native(material, [1]); s.BeforeEffects();
        s.Need(s.E<JudgmentRequestedEvent>().Length == 0 && !s.E<ProgramBindingStartedEvent>().Any(e => e.SkillId == Legacy),
            "The unchanged classic Tieqi does not move to designated-target timing: its original native target effect starts before any Tieqi judgment.");
        s.Continue(); s.TieqiPrompt(1); s.Activate();
        s.Reach(() => s.IsWatch("judgment-pause"));
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Legacy);
        var native = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
        s.Need(root.WindowContext is { Window: SkillProgramTriggerWindow.SlashBeforeResponse, TargetSeat: 1 } &&
            s.G.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Single().Judgment is { CardId: var id, Suit: Suit.Heart } && id == s.Red,
            "The original classic descriptor still executes its exact SlashBeforeResponse red judgment against the current target.");
        s.Continue(); s.Play();
        s.Need(s.V(1).Hp == 7 && s.V(1).HandCount == 4 && s.E<CardRespondedEvent>().Length == 0 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == native.Id && e.CardId == material) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Use) == 1 &&
            s.Zone(s.Red) == CardLocation.DiscardPile,
            "Classic response-stage red Tieqi still prohibits the real Dodge and returns once to its original native Slash without inheriting the new finalized-target policy.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry content, bool legacy)
    {
        public GameEngine G = game;
        public int Red, Black;
        private string Skill => legacy ? Legacy : Tieqi;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public CardUseFrame Use(long id) => G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == id);
        public void Accept(GameCommand command) { var r = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(r.Accepted, r.Error?.Message ?? "Rejected real mobile Ma Chao command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing actual Ma Chao native choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Dodge() => Answer(c => c.Parameters.GetValueOrDefault("response") == "dodge" && c.Cards.Count == 1 && V(P!.PlayerSeat).Hand.Any(h => h.Id == c.Cards[0] && h.Kind == CardKind.Dodge));
        public bool IsWatch(string bind) => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == bind);
        public void BeforeEffects() => Reach(() => IsWatch("before-effects-pause"));
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Native(int card, int[] targets)
        {
            var action = G.GetLegalActions(0).FirstOrDefault(a => a.CardId == card && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets));
            Need(action is not null, "The real physical Ma Chao card must expose its native target group.");
            Accept(new PlayCardCommand(0, card, targets, G.Revision, P!.PromptId, action!.PlayedCardKind, action.TargetCardId));
        }
        public void Prepare()
        {
            Red = V(0).Hand.Single(c => c.Kind == CardKind.Peach && c.Suit == Suit.Heart).Id;
            Black = V(0).Hand.Single(c => c.Kind == CardKind.Peach && c.Suit == Suit.Spade).Id;
            Native(V(0).Hand.Single(c => c.Kind == CardKind.FangtianHalberd).Id, []); Play();
            foreach (var id in new[] { Black, Red })
            { Accept(new UseProgramSkillCommand(0, Driver, "top", [id], [], G.Revision, P!.PromptId)); Play(); }
        }
        public void TieqiPrompt(int target)
        {
            Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            var window = G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last();
            Need(window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] is { } c && c.SkillId == Skill && c.OpponentSeat == target,
                "The actual native candidate must retain this exact designated target.");
        }
        public void Judgment(int target, int card, Suit suit, long useId, long actionId)
        {
            Reach(() => IsWatch("judgment-pause"));
            var root = G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Tieqi);
            Need(root.WindowContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized } c && c.TargetSeat == target &&
                c.CardUse is { } native && native.ParentCardUseFrameId == useId && native.CardActionId == actionId &&
                G.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().Single().Judgment is { } judgment && judgment.CardId == card && judgment.Suit == suit &&
                Use(useId).Action!.ActionId == actionId && E<ResponseRequestedEvent>().Length == 0 && E<DamageAppliedEvent>().Length == 0,
                "The original target-specific finalized Slash frame owns this actual frozen judgment before every response or damage.");
        }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 180 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, General(p.PlayerSeat), G.Revision, p.PromptId)); continue; }
                Need(P is null, "Unexpected native Ma Chao boundary while seeking a specific original continuation.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded native Ma Chao boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), content);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.CardMovements.SequenceEqual(copy.CardMovements) && G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))
                    .SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))),
                "Cold preserves the already issued later-target ban, current native judgment child, original action nonce and all prepared target attack states."); G = copy;
        }
        public void Need(bool ok, string text)
        {
            if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { Legacy = legacy, G.State.TurnNumber, G.State.CurrentSeat,
                Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId, Choices = p.Choices.Take(6).Select(c => new { c.Cards, c.Targets, c.Parameters }).ToArray() } : null,
                Attacks = G.ResolutionStack.OfType<CardUseFrame>().Select(u => new { u.Id, u.CardId, u.CardKind, ActionId = u.Action?.ActionId, u.TargetSeats,
                    u.CardAttack, u.PreparedTargetAttacks }).ToArray(),
                Facts = G.Events.Select(e => e.Payload).Where(e => e is JudgmentRequestedEvent or JudgmentResolvedEvent or ProgramCardTriggerResolvedEvent or ResponseRequestedEvent or DamageAppliedEvent)
                    .TakeLast(8).Select(e => JsonSerializer.Serialize(e, e.GetType())).ToArray() }));
        }
        public static Scene Start(bool legacy = false)
        {
            // One fixed-seed opening locates the native deal slots; the second
            // uses those same physical IDs with explicit materials. No mutable
            // game state, draw order or production pending object is patched.
            var stock = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7)).ToArray();
            var probe = Boot(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock, legacy)), legacy);
            for (var seat = 0; seat < 4; seat++)
            {
                var ids = probe.V(seat).Hand.Select(c => c.Id).ToArray(); probe.Need(ids.Length == 4, "The fixed native deal provides exactly four actual opening entities.");
                for (var i = 0; i < 4; i++) stock[ids[i] - 1] = seat == 0 ? i switch
                { 0 => new(Weapon, Suit.Spade, 7), 1 => new("standard:fire_slash", Suit.Spade, 7), 2 => new("standard:peach", Suit.Heart, 7), _ => new("standard:peach", Suit.Spade, 7) }
                    : new("standard:dodge", Suit.Club, 2);
            }
            return Boot(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock, legacy)), legacy);
        }
        private static Scene Boot(ContentRegistry registry, bool legacy)
        {
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, registry), registry, legacy);
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private static string General(int seat) => "fixture:mobile-ma-chao-general-" + seat;
    private sealed class Fixture(IReadOnlyList<ContentDeckPhysicalCard> stock, bool legacy) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-ma-chao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:ma-chao", "classic:ma-chao");
            var reason = legacy ? "skill.slash-response-judgment" : "skill.mobile.tieqi";
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"top","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"top"},{"op":"moveBoundCards","target":"owner","sourceBind":"top","destination":"drawPileTop","awaitMovementTriggers":true}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[
                {"id":"actual-final-judgment","window":"judgmentFinalized","subject":"owner","judgmentReasons":["{{reason}}"],"minimumRank":1,"maximumRank":13,"suits":["spade","club","heart","diamond","none"],"excludedReasons":[],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"judgment-pause","options":[{"id":"continue"}]}]},
                {"id":"first-native-target-effect","window":"cardUseBeforeTargetEffects","ownerRelation":"actor","cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"before-effects-pause","options":[{"id":"continue"}]}]}
              ]}
            ]}
            """;
            var labels = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实判定置顶", description = "实际手牌进入原生牌堆顶。" },
                    [Watch] = new { name = "原生指定后判定与效果", description = "在原生帧上观察准确时序。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, labels);
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            for (var seat = 0; seat < 4; seat++) b.AddGeneral(new(General(seat), "真实固定参与者", "supporter", seat == 0 ? legacy ? Legacy : Tieqi : "standard:none", "shu", seat == 0 ? 4 : 8,
                seat == 0 ? ["classic:mashu", Driver, Watch] : []));
            var weapon = CardCatalog.Get(CardKind.FangtianHalberd); b.AddCard(new(Weapon, weapon.DisplayName, weapon.CategoryName, weapon.Description, CardKind.FangtianHalberd));
            const string deck = "fixture:mobile-ma-chao-deck"; b.AddDeck(new(deck, "真实方天实体", 4, 0, []) { PhysicalCards = stock });
            b.AddMode(new(Mode, "移动普通马超原生目标", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 },
                deck, GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
