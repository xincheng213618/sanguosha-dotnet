using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static class WuGuanYuWuyouChecks
{
    private const string WySkill = "wumiao:wuyou", WyDriver = "fixture:wy-driver", WyWatch = "fixture:wy-watch";
    private const string WyOwner = "fixture:wy-owner", WyQuiet = "fixture:wy-quiet", WyNeutral = "fixture:wy-neutral";
    private const string WyConditional = "fixture:wy-conditional", WyInsert = "fixture:wy-insert", WyCounter = "fixture:wy-counter", WyCaller = "fixture:wy-caller", WyMode = "identity:classic-wumiao-wuyou";
    private const string WyContribution = "skill-program.wumiao.wuyou.contribute", WyReturn = "skill-program.wumiao.wuyou.return";

    public static void WuyouLaterPhaseNamesNativeResponsesAndProvidedUseCold()
    {
        var counter = WyScenario.Start("later-null"); var id = counter.NameAtPlay(CardKind.Nullification, 2);
        counter.Use("incoming-duel", targets: [1]); counter.Reach(() => counter.P is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        counter.Private(0); counter.Cold();
        counter.Answer(c => c.Cards.SequenceEqual([id]) && c.Parameters.GetValueOrDefault("conversion-skill-id") == WySkill); counter.Play();
        counter.Need(counter.E<NullificationRespondedEvent>().Count(e => e.NullificationCardId == id && e.ResponderSeat == 0 && e.EffectCardKind == CardKind.Duel && e.EffectNullified) == 1 &&
            counter.E<CardRespondedEvent>().Count(e => e.CardId == id && e.ResponderSeat == 0 && e.EffectiveCardKind == CardKind.Nullification) == 1 &&
            counter.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Nullification) == 1 &&
            counter.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
            !counter.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0),
            "The second real Play's fixed Nullification name pays the original Slash once in the genuine foreign Duel counterspell window and returns to that original native effect."); counter.Cold();

        var rescue = WyScenario.Start("later-peach"); id = rescue.NameAfterOtherIssuers(CardKind.Peach); rescue.Use("peer-dying", targets: [1]);
        rescue.Reach(() => rescue.P is { PlayerSeat: 0, Kind: DecisionKind.RescueDying } && rescue.P.Choices.Any(c => c.Cards.SequenceEqual([id])));
        rescue.Private(0); rescue.Cold(); rescue.Answer(c => c.Cards.SequenceEqual([id]) && c.Parameters.GetValueOrDefault("conversion-skill-id") == WySkill);
        rescue.Play(); var peach = rescue.E<WuyouUseAcceptedEvent>().Single().Receipt;
        rescue.Need(peach is { ActorSeat: 0, ProviderSeat: 0, EffectiveKind: CardKind.Peach } && peach.Material.CardId == id && peach.Material.CardKind == CardKind.Slash &&
            rescue.V(1).IsAlive && rescue.V(1).Hp == 1 && rescue.E<RecoveryAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1 &&
            rescue.E<DyingResponseEvent>().Count(e => e.ResponderSeat == 0 && e.UsedPeach && e.PeachCardId == id) == 1 &&
            rescue.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == peach.UseId && e.CardId == id && e.CardKind == CardKind.Peach) == 1 &&
            rescue.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "The third real invocation's fixed Peach is a once-paid rescue of another dying player, with its original printed Slash and exact native recovery return."); rescue.Cold();

        var provider = WyScenario.Start("later-provider"); id = provider.NameAfterOtherIssuers(CardKind.Slash);
        var caller = provider.G.CreateSnapshot(0).Players.Single(p => p.Role == Role.Lord).Seat;
        provider.Need(caller != 0 && provider.V().Role == Role.Loyalist && provider.V(caller).GeneralId == "fixture:wy-peer-1" &&
            provider.G.GetAttackRange(caller) == 1 && Enumerable.Range(0, 4).Where(seat => seat != caller).All(seat => provider.G.GetCombatDistance(caller, seat) > 1),
            "The true lord requester is fixed by its actual role and every target is genuinely beyond its ordinary range; the human provider retains several ordinary physical Slashes.");
        provider.End(); provider.Reach(() => provider.P is { PlayerSeat: 0, Kind: DecisionKind.RespondSlash } &&
            provider.P.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "faction-slash-slash"));
        var offered = provider.P!.Choices.Where(c => c.Cards.Count != 0).ToArray();
        var ordinary = provider.V().Hand.First(card => !provider.E<WuyouLeaseIssuedEvent>().Any(e => e.Lease.CardId == card.Id)).Id;
        provider.Need(offered.Length == 1 && offered[0].Cards.SequenceEqual([id]) && offered[0].Parameters.GetValueOrDefault("conversion-skill-id") == WySkill &&
            provider.V().Hand.Count >= 2 && provider.V().Hand.All(card => card.Kind == CardKind.Slash),
            "Only the exact live named entity can pay this distant true requester Use; other real Slashes cannot borrow its material-specific range permission.");
        provider.RejectChoice(new ChoiceId(offered[0].Id.Value.Replace("card-" + id, "card-" + ordinary, StringComparison.Ordinal))); provider.Private(0); provider.Cold();
        provider.Answer(c => c.Id == offered[0].Id); provider.Reach(() => provider.Hold("supplied")); provider.Cold();
        var supplied = provider.E<WuyouUseAcceptedEvent>().Single().Receipt;
        var use = provider.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == supplied.UseId);
        var target = use.TargetSeats.Single();
        provider.Need(supplied.ActorSeat == caller && supplied.ProviderSeat == 0 && supplied.Lease.HolderSeat == 0 && supplied.Material.CardId == id &&
            supplied.Material.From == CardLocation.Hand(0) && supplied.Material.CardKind == CardKind.Slash &&
            use.Action is { Type: CardActionType.Use, RequesterSeat: { } requester } && requester == caller &&
            use.Action.PhysicalCards.SequenceEqual([supplied.Material]) && provider.G.GetCombatDistance(caller, target) > provider.G.GetAttackRange(caller),
            "The accepted supplied Slash belongs to the real requester while preserving the original provider Hand entity and its exact naming lease; the provider receives no fake Use.");
        provider.Continue(); provider.Reach(() => provider.E<CardUseFinishedEvent>().Any(e => e.ResolutionId == supplied.UseId));
        provider.Need(provider.E<CardSupplyCompletionStartedEvent>().Count(e => e.ActionId == supplied.ActionId && e.RequesterSeat == caller && e.ProviderSeat == 0) == 1 &&
            provider.E<CardSupplyCompletedEvent>().Count(e => e.ActionId == supplied.ActionId && e.RequesterSeat == caller && e.ProviderSeat == 0) == 1 &&
            provider.E<DamageAppliedEvent>().Any(e => e.SourceSeat == caller && e.TargetSeat == target && e.Amount == 1) &&
            provider.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
            provider.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == supplied.UseId && e.CardId == id) == 1,
            "The exact provider material crosses the true supply-completion observer and native damage/whole-Use return once, including cold restoration inside the supplied child."); provider.Cold();

        var combo = WyScenario.Start("later-combo"); id = combo.Name(CardKind.Indulgence);
        combo.Native(combo.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.CardId != id && a.ConversionSource is null && a.TargetSeats.SequenceEqual([1]))); combo.Play();
        var fire = combo.G.GetHumanLegalActions().Single(a => a.CardId == id && a.PlayedCardKind == CardKind.FireSlash &&
            a.ConversionSource?.SkillId == "wumiao:juewu" && a.TargetSeats.SequenceEqual([2]));
        combo.Need(combo.G.GetCombatDistance(0, 2) > combo.G.GetAttackRange(0) &&
            !combo.G.GetHumanLegalActions().Any(a => a.CardId != id && a.PlayedCardKind == CardKind.FireSlash && a.ConversionSource?.SkillId == "wumiao:juewu"),
            "After the ordinary Slash allowance is spent, only the exact rank-two named entity can combine Wuyou permissions with the genuine Juewu output.");
        combo.Native(fire); combo.Reach(() => combo.Hold("combo")); combo.Cold();
        var named = combo.E<WuyouUseAcceptedEvent>().Single().Receipt; var converted = combo.E<JuewuUseIssuedEvent>().Single();
        combo.Need(named.UseId == converted.UseId && named.ActionId == converted.ActionId && named.Lease.EffectiveKind == CardKind.Indulgence &&
            named.EffectiveKind == CardKind.FireSlash && converted.OutputKind == CardKind.FireSlash && converted.CardId == id &&
            converted.PrintedKind == CardKind.Slash && converted.PrintedRank == 2 && converted.From == CardLocation.Hand(0),
            "One accepted native Fire Slash carries both independent exact source receipts without changing the physical Slash or fabricating a second payment.");
        combo.Continue(); combo.Play();
        combo.Need(combo.E<JuewuUseFinishedEvent>().Count(e => e.UseId == converted.UseId) == 1 &&
            combo.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == converted.UseId && e.CardId == id && e.CardKind == CardKind.FireSlash) == 1 &&
            combo.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
            combo.E<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == 2 && e.Amount == 1 && e.Nature == DamageNature.Fire) &&
            !combo.G.GetHumanLegalActions().Any(a => a.Kind == LegalActionKind.Slash || a.ConversionSource is { SkillId: "wumiao:juewu", BindingId: "rank-two-fireSlash" }),
            "The combined native Use finishes and deals real fire damage once; Juewu's name quota and ordinary Slash debits remain spent after the physical naming lease leaves Hand."); combo.Cold();
    }

    private sealed class WyScenario(string scene, GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public LegalAction[] Contributions() => G.GetHumanLegalActions().Where(a => a.ProgramSkillId == WySkill).ToArray();
        public LegalAction Contribution(int owner) => Contributions().Single(a => a.ProgramSkillOwnerSeat == owner);
        public bool Hold(string option) => P?.SkillPrompt?.SkillId == WyWatch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == option);
        public void Contribute(LegalAction action, int id) => Accept(new UseProgramSkillCommand(0, action.ProgramSkillId!, action.ProgramActivationId!, [id], action.SelectableTargetSeats, G.Revision, P!.PromptId) { SkillOwnerSeat = action.ProgramSkillOwnerSeat });
        public void ReturnPrompt(int owner) { Reach(() => P?.PlayerSeat == owner && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "wuyou-return")); var r = G.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.Wuyou is not null).Wuyou!; Console.WriteLine("Wuyou actual sample " + scene + ": " + JsonSerializer.Serialize(new { r.Names, r.RandomBefore, r.RandomAfter })); }
        public int Name(CardKind kind) { var id = V().Hand[0].Id; Contribute(Contribution(0), id); ReturnPrompt(0); Cold(); Answer(c => c.Parameters.GetValueOrDefault("branch") == "give" && c.Cards.SequenceEqual([id]) && c.Parameters.GetValueOrDefault("card-kind") == kind.ToString()); Play(); return id; }
        public int NameAtPlay(CardKind kind, int ordinal)
        {
            var turn = G.State.TurnNumber; var phases = new List<long>();
            for (var i = 1; i < ordinal; i++)
            {
                Contribute(Contribution(0), V().Hand[0].Id); ReturnPrompt(0); Cold();
                phases.Add(G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Wuyou is not null).Wuyou!.Phase.Token);
                Answer(c => c.Parameters.GetValueOrDefault("branch") == "decline"); Play(); End(); Play();
            }
            var id = Name(kind); var started = E<WuyouStartedEvent>().Last(); phases.Add(started.Phase.Token);
            Need(E<WuyouStartedEvent>().Length == ordinal && phases.Distinct().Count() == ordinal && G.State.TurnNumber == turn &&
                E<WuyouCompletedEvent>().Count(e => e.Returned) == 1,
                "The fixed sampled name comes from successive genuine Play phases in one actual turn, never a searched seed, fake phase or skipped missing name."); return id;
        }
        public int NameAfterOtherIssuers(CardKind kind)
        {
            var issuers = Contributions().Where(a => a.ProgramSkillOwnerSeat != 0).Select(a => a.ProgramSkillOwnerSeat!.Value).Order().ToArray();
            Need(issuers.Length == 2, "Two distinct real foreign Wuyou issuers are available for this donor's same actual Play.");
            foreach (var issuer in issuers)
            {
                var card = V().Hand.First(c => !E<WuyouLeaseIssuedEvent>().Any(e => e.Lease.CardId == c.Id)).Id;
                Contribute(Contribution(issuer), card); ReturnPrompt(issuer); Private(issuer); Cold();
                var frameId = E<WuyouStartedEvent>().Last().FrameId;
                Reach(() => E<WuyouCompletedEvent>().Any(e => e.FrameId == frameId)); Play();
            }
            var id = Name(kind); var starts = E<WuyouStartedEvent>();
            Need(starts.Length == 3 && starts.Select(e => e.Source.OwnerSeat).Distinct().Count() == 3 &&
                starts.All(e => e.DonorSeat == 0 && e.Phase == starts[0].Phase) && starts[^1].Source.OwnerSeat == 0 &&
                E<WuyouCompletedEvent>().Length == 3 && E<WuyouCompletedEvent>().All(e => e.Returned),
                "Three genuine distinct issuer contributions consume the fixed first three samples in one actual Play, without inserting a phase inside an extra phase or changing random state."); return id;
        }
        public void Use(string id, int[]? cards = null, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, WyDriver, id, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
        public void Take(int owner, int id) { var index = V(owner).Hand.Select((c, i) => (c, i)).Single(x => x.c.Id == id).i; Use("take", targets: [owner]); Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("slot-index") == index.ToString()) == true); Answer(c => c.Parameters.GetValueOrDefault("slot-index") == index.ToString()); }
        public PlayCardCommand Command(LegalAction a, CardConversionSource? source = null) => new(0, a.CardId!.Value, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = source ?? a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources };
        public void Native(LegalAction a) => Accept(Command(a));
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; var c = p.Choices.FirstOrDefault(match); Need(p.PlayerSeat == 0 && c is not null, "The human answers its actual canonical private prompt."); Accept(new AnswerPromptCommand(0, p.PromptId, c!.Id, G.Revision)); }
        public void Continue() => Answer(c => c.Parameters.ContainsKey("option-id"));
        public void Private(int owner) => Need(P is { IsPrivate: true } p && p.PlayerSeat == owner && Enumerable.Range(0, 4).Where(i => i != owner).All(i => G.CreateSnapshot(i).PendingDecision is null), "Only the actual chooser can inspect its five random names and current Hand return cards.");
        public void RejectOld(PendingDecision p) { var before = State(); Need(!G.Submit(new AnswerPromptCommand(0, p.PromptId, p.Choices[0].Id, G.Revision)).Accepted && before == State(), "An old return token cannot repeat the paid selection."); }
        public void RejectChoice(ChoiceId id) { var before = State(); Need(!G.Submit(new AnswerPromptCommand(0, P!.PromptId, id, G.Revision)).Accepted && before == State(), "A forged ordinary material cannot enter an exact named provider choice or change any committed state."); }
        public void RejectSource(LegalAction a) { var before = State(); Need(!G.Submit(Command(a, a.ConversionSource! with { SkillInstanceId = "forged-wuyou" })).Accepted && before == State(), "An unrelated instance cannot borrow this physical entity's original identity or permissions."); }
        public void RejectContribution(LegalAction a) { var before = State(); Need(!G.Submit(new UseProgramSkillCommand(0, WySkill, a.ProgramActivationId!, [V(1).Hand[0].Id], a.SelectableTargetSeats, G.Revision, P!.PromptId) { SkillOwnerSeat = a.ProgramSkillOwnerSeat }).Accepted && before == State(), "A peer's private entity cannot pay the donor's published Hand contribution."); }
        public void Reach(Func<bool> done, [CallerLineNumber] int line = 0)
        {
            for (var i = 0; i < 180; i++)
            {
                if (done()) return; Need(G.State.Winner == Winner.None, "Fixture terminated before boundary " + line); var p = P;
                if (p is null || p.PlayerSeat != 0) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (p.Kind == DecisionKind.PlayCard) End();
                else if (p.SkillPrompt?.SkillId == WyWatch) Continue();
                else if (p.SkillPrompt?.SkillId == WyInsert && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target")) Answer(c => c.Targets.SequenceEqual([0]));
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else if (p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying) Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die");
                else if (p.Kind == DecisionKind.FireAttackDiscard) Answer(c => c.Cards.Count == 1);
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "target-card-slot")) Answer(c => c.Parameters.GetValueOrDefault("action") == "target-card-slot");
                else if (p.Kind == DecisionKind.SelectHarvestCard) Answer(c => c.Cards.Count == 1);
                else throw new InvalidOperationException("Unexpected native boundary " + line + " " + Diagnostic());
            }
            throw new InvalidOperationException("Missing bounded native boundary " + line + " " + Diagnostic());
        }
        public void Accept(GameCommand command) { try { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, "Rejected " + command.GetType().Name + ": " + JsonSerializer.Serialize(result)); } catch (Exception e) { throw new InvalidOperationException("scene=" + scene + " command=" + command.GetType().Name + " cause=" + e.Message + " " + Diagnostic(), e); } }
        public void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message + " " + Diagnostic()); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements, Zones = G.CreateCardZoneDiagnostics(), Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())), Commands = CommandJson.Serialize(G.AcceptedCommands) });
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == State(), "Cold replay preserves original entities, private RNG choices, phase entitlements, paid child cursors and native returns."); }
        private string Diagnostic() => JsonSerializer.Serialize(new { Scene = scene, G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner, Prompt = P is not { } p ? null : new { p.Kind, p.PlayerSeat, Count = p.Choices.Count, Choices = p.Choices.Take(3).Select(c => new { c.Id, c.Cards, c.Targets, c.Parameters }) }, Players = Enumerable.Range(0, 4).Select(i => new { Seat = i, V(i).Hp, V(i).MaxHp, V(i).IsAlive, V(i).HandCount }), Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind, f.Step, Stage = (f as ProgramSkillFrame)?.Wuyou?.Stage }), Started = E<WuyouStartedEvent>().Length, Names = E<WuyouNameSampledEvent>().TakeLast(5).Select(e => e.Kind), Leases = E<WuyouLeaseIssuedEvent>().Length, Uses = E<WuyouUseAcceptedEvent>().Select(e => new { e.Receipt.UseId, e.Receipt.EffectiveKind, e.Receipt.Material.CardId }) });
        public static WyScenario Start(string scene)
        {
            ContentRegistry r; try { r = ContentRegistry.Build(new StandardContentPackage(), new WyFixture(scene)); } catch (Exception e) { throw new InvalidOperationException("scene=" + scene + " fixture=" + e.Message, e); }
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = scene == "later-provider" ? Role.Loyalist : Role.Lord, ModeId = WyMode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var s = new WyScenario(scene, g, r); s.Accept(new StartGameCommand()); s.Reach(() => s.P is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }); s.Accept(new SelectGeneralCommand(0, WyOwner, g.Revision, s.P!.PromptId)); s.Play(); return s;
        }
    }

    private sealed class WyFixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wumiao-wuyou", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            string Read(string suffix) { using var stream = typeof(StandardContentPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms.wumiao-guan-yu." + suffix + ".json") ?? throw new InvalidOperationException("Missing real Wuyou bundle"); using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
            var production = SkillProgramCatalog.Load(Read("rules"), Read("presentation")); b.AddSkill(new(WySkill, production.Presentations[WySkill].Name, production.Presentations[WySkill].Description) { Program = production.Programs[WySkill], ProgramPresentation = production.Presentations[WySkill] });
            if (scene == "later-combo") b.AddSkill(new("wumiao:juewu", production.Presentations["wumiao:juewu"].Name, production.Presentations["wumiao:juewu"].Description) { Program = production.Programs["wumiao:juewu"], ProgramPresentation = production.Presentations["wumiao:juewu"] });
            static JsonNode N(string text) => JsonNode.Parse(text)!;
            static JsonObject A(string id, string effects, int cards = 0, int targets = 0) => N("{\"id\":\"" + id + "\",\"minCards\":" + cards + ",\"maxCards\":" + cards + ",\"minTargets\":" + targets + ",\"maxTargets\":" + targets + ",\"targetKind\":\"anyLiving\",\"usesPerTurn\":null,\"effects\":[" + effects + "]}").AsObject();
            static string Choice(string id) => "{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"seen\",\"options\":[{\"id\":\"" + id + "\"}]}";
            var defs = new JsonArray(new JsonObject { ["id"] = WyDriver, ["revision"] = 1, ["activations"] = new JsonArray(
                A("give", "{\"op\":\"giveSelected\",\"target\":\"selectedTarget\"}", 1, 1),
                A("take", "{\"op\":\"selectAndMoveOwnedCard\",\"target\":\"owner\",\"chooserRef\":{\"kind\":\"owner\"},\"cardOwnerRef\":{\"kind\":\"selectedTarget\"},\"zones\":[\"hand\"],\"count\":1,\"destination\":\"ownerHand\",\"awaitMovementTriggers\":true}", targets: 1),
                A("lose-wuyou", "{\"op\":\"loseOwnerSkillsAndGrant\",\"target\":\"owner\",\"skillIds\":[\"wumiao:wuyou\"],\"sourceBind\":\"" + WyNeutral + "\"}"),
                A("grant-wuyou", "{\"op\":\"grantSkills\",\"target\":\"owner\",\"skillIds\":[\"wumiao:wuyou\"]}")),
                ["modifiers"] = N("[{\"id\":\"keep\",\"query\":\"handLimit\",\"operation\":\"add\",\"value\":40,\"priority\":0}]") },
                N("{\"id\":\"" + WyQuiet + "\",\"revision\":1,\"triggers\":[{\"id\":\"quiet\",\"window\":\"afterNormalDraw\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"skipTurnPhases\",\"target\":\"owner\",\"phases\":[\"play\"]}]}]}"),
                N("{\"id\":\"" + WyNeutral + "\",\"revision\":1,\"modifiers\":[{\"id\":\"neutral\",\"query\":\"handLimit\",\"operation\":\"add\",\"value\":1,\"priority\":0}]}"));
            var driver = defs[0]!.AsObject(); var acts = driver["activations"]!.AsArray();
            acts.Add(A("self-dying", "{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":14}"));
            if (scene == "later-null")
            {
                var duel = A("incoming-duel", "{\"op\":\"useSelectedActorDuel\",\"target\":\"owner\"}", targets: 1);
                duel["targetKind"] = "otherLiving"; duel["usesPerPhase"] = 2; acts.Add(duel);
            }
            if (scene == "later-peach") acts.Add(A("peer-dying", "{\"op\":\"loseHp\",\"target\":\"selectedTarget\",\"amount\":13}", targets: 1));
            if (scene == "named-alcohol") driver["viewAs"] = N("[{\"id\":\"native-wine-to-slash\",\"inputKinds\":[\"alcohol\"],\"inputSuits\":[],\"outputKind\":\"slash\",\"forPlay\":true,\"forResponse\":false}]");
            var own = new List<string> { scene == "use-loss" ? WyConditional : WySkill, WyDriver };
            if (scene is "extra" or "named-dodge" or "later-null") { defs.Add(N("{\"id\":\"" + WyInsert + "\",\"revision\":1,\"triggers\":[{\"id\":\"new-real-play\",\"window\":\"playEnding\",\"subject\":\"owner\",\"usageScope\":\"game\",\"usageLimit\":1,\"optional\":false,\"effects\":[{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"currentTurnPlayer\"},{\"op\":\"insertPhase\",\"target\":\"selectedTarget\",\"phase\":\"play\",\"phaseContinuation\":\"beforeNormalPreparation\"}]}]}")); own.Add(WyInsert); }
            if (scene == "named-dodge") { defs.Add(N("{\"id\":\"" + WyCounter + "\",\"revision\":1,\"triggers\":[{\"id\":\"real-counter-use\",\"window\":\"playPhaseStarting\",\"subject\":\"owner\",\"usageScope\":\"game\",\"usageLimit\":1,\"optional\":true,\"effects\":[{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"otherLivingVirtualSlashTarget\"},{\"op\":\"selectedTargetVirtualSlashAgainstOwner\",\"target\":\"owner\"}]}]}")); own.Add(WyCounter); }
            if (scene == "later-provider")
            {
                defs.Add(N("{\"id\":\"" + WyCaller + "\",\"revision\":1,\"modifiers\":[{\"id\":\"real-far-requester\",\"query\":\"outgoingDistance\",\"operation\":\"add\",\"value\":2,\"priority\":0}],\"cardPolicies\":[{\"id\":\"real-lord-request\",\"kind\":\"factionResponseRequest\",\"requiredCardKinds\":[\"slash\"],\"factionId\":\"shu\",\"ownerRole\":\"lord\"}],\"triggers\":[{\"id\":\"real-lord-use\",\"window\":\"turnStartBeforeNormalFlow\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"otherLivingSlashable\"},{\"op\":\"requestFactionCard\",\"target\":\"selectedTarget\",\"providerFactionId\":\"shu\",\"requiredKind\":\"slash\"}]}]}"));
                defs.Add(N("{\"id\":\"" + WyWatch + "\",\"revision\":1,\"triggers\":[{\"id\":\"true-supply\",\"window\":\"cardSupplyCompleted\",\"ownerRelation\":\"actor\",\"cardKinds\":[\"slash\",\"fireSlash\",\"thunderSlash\"],\"optional\":false,\"effects\":[" + Choice("supplied") + "]}]}")); own.Add(WyWatch);
            }
            if (scene == "later-combo")
            {
                own.Add("wumiao:juewu"); own.Add(WyWatch);
                defs.Add(N("{\"id\":\"" + WyWatch + "\",\"revision\":1,\"triggers\":[{\"id\":\"real-combined-use\",\"window\":\"cardUseCommitted\",\"ownerRelation\":\"actor\",\"cardKinds\":[\"fireSlash\"],\"optional\":false,\"effects\":[" + Choice("combo") + "]}]}"));
            }
            if (scene is "use-loss" or "foreign-loss")
                defs.Add(N("{\"id\":\"" + WyConditional + "\",\"revision\":1,\"hpSkillGrants\":[{\"id\":\"healthy-issuer\",\"skillId\":\"wumiao:wuyou\",\"minimumHp\":12}]}"));
            if (scene == "use-loss") { defs.Add(N("{\"id\":\"" + WyWatch + "\",\"revision\":1,\"triggers\":[{\"id\":\"cost\",\"window\":\"cardsMoved\",\"subject\":\"owner\",\"sourceZones\":[\"hand\"],\"movementOccurrence\":\"perBatch\",\"movementReasons\":[\"" + CardMoveReasons.Use.Value + "\"],\"usageScope\":\"game\",\"usageLimit\":1,\"optional\":false,\"effects\":[" + Choice("cost") + ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":3}]}]}")); own.Add(WyWatch); }
            if (scene == "foreign-loss")
            {
                defs.Add(N("{\"id\":\"" + WyWatch + "\",\"revision\":1,\"triggers\":[{\"id\":\"contributed\",\"window\":\"cardsGained\",\"subject\":\"owner\",\"destinationZones\":[\"hand\"],\"movementOccurrence\":\"perBatch\",\"movementReasons\":[\"" + WyContribution + "\"],\"optional\":false,\"effects\":[" + Choice("contributed") + ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":2}]}]}"));
            }
            var labels = defs.ToDictionary(n => n!["id"]!.GetValue<string>(), n => new { name = n!["id"]!.GetValue<string>(), description = "真实武佑贡献、原牌租约与原生子流程", optionLabels = new[] { "activations", "triggers" }.SelectMany(k => n[k]?.AsArray() ?? new JsonArray()).SelectMany(t => t!["effects"]!.AsArray()).SelectMany(e => e!["options"]?.AsArray() ?? new JsonArray()).Select(o => o!["id"]!.GetValue<string>()).Distinct().ToDictionary(x => x, x => x) });
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = defs }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels })); foreach (var (id, p) in c.Programs) b.AddSkill(new(id, id, "真实武佑夹具") { Program = p, ProgramPresentation = c.Presentations[id] });
            const string pick = "fixture:wy-pick"; b.AddSkill(new(pick, "本人", "固定真人候选") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, _ => -100000000d) }); b.AddGeneral(new(WyOwner, "真实武佑者", "supporter", pick, "shu", 13, own));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:wy-peer-" + i).ToArray();
            for (var i = 1; i <= 3; i++)
            {
                var pp = "fixture:wy-peer-pick-" + i;
                b.AddSkill(new(pp, "其他角色", "固定公开候选") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role,
                    role => scene == "later-provider" && i == 1 ? role == Role.Lord ? 100000000d : -100000000d : 1000000d * (4 - i)) });
                var skills = new List<string> { WyQuiet }; if (i <= 2) skills.Add(scene == "foreign-loss" && i == 1 ? WyConditional : WySkill);
                if (scene == "foreign-loss" && i == 1) skills.Add(WyWatch); if (scene == "later-provider" && i == 1) skills.Add(WyCaller);
                b.AddGeneral(new(peers[i - 1], "真实受贡献者", "supporter", pp, "shu", 13, skills));
            }
            b.AddDeck(new("fixture:wy-deck", "真实原实体牌堆", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard(scene is "named-alcohol" or "named-rescue-alcohol" ? "standard:alcohol" : "standard:slash", Suit.Heart, scene == "later-combo" ? 2 : 7)).ToArray() });
            var roles = scene == "later-provider" ? new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 } : new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 };
            b.AddMode(new(WyMode, "真实武佑原生边界", 4, 4, roles, "fixture:wy-deck", GeneralCandidateCount: 4, GeneralPoolIds: [WyOwner, .. peers]));
        }
    }
}
