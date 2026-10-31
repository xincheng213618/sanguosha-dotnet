using System.Text.Json;
using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class SkillStateChecks
{
    private const string Driver = "fixture:skill-state-driver", Counter = "fixture:skill-state-counter";
    private const string Locked = "fixture:skill-state-locked", Owner = "fixture:skill-state-owner";
    private const string Mode = "identity:classic-shared-skill-state";

    public static void StateChangesPreserveOwnershipChoicesInstancesAndExpiryCold()
    {
        foreach (var (name, check) in new (string, Action)[]
        {
            ("suppression", SuppressionKeepsOwnershipAndExemptsLockedSkills),
            ("chosen return", ChosenLossReturnsToOrdinaryDrawCold),
            ("overlapping expiry", OverlappingExpiryPreservesTheExistingInstance),
            ("existing transfer", TransferReusesAnAlreadyOwnedInstance),
            ("named future source", NamedSuppressionCoversAnIndependentlyAcquiredSource),
            ("filter resources", FiltersShareExpressionResourcesAndReadSkillFacts),
            ("permanent death expiry", PermanentRestrictionExpiresOnItsTargetDeath),
            ("consumed Alcohol Slash", ConsumedAlcoholSlashQualifiesOnlyItsOriginalDamage)
        })
            try { check(); }
            catch (Exception error) { throw new InvalidOperationException($"Shared skill state {name}: {error.Message}", error); }
    }

    private static void SuppressionKeepsOwnershipAndExemptsLockedSkills()
    {
        var s = Scene.Start();
        var instance = s.CounterState(0).SkillInstanceId;
        Require(s.CanUse(0, Counter, "pulse") && s.CanUse(0, Locked, "pulse"), "Both original skills start effective.");
        s.Use(0, Driver, "suppress-self"); s.Play(0);
        Require(s.Owns(0, Counter) && s.Owns(0, Driver) && s.Owns(0, Locked) &&
            !s.CanUse(0, Counter, "pulse") && !s.CanUse(0, Driver, "choose") && s.CanUse(0, Locked, "pulse"),
            "Non-locked suppression retains owned printed skills while removing their activations; a locked skill remains effective.");
        s.Cold();
        Require(s.Owns(0, Counter) && !s.CanUse(0, Counter, "pulse") && s.CanUse(0, Locked, "pulse"),
            "Cold replay retains the ownership/effectiveness distinction.");
        s.Play(1); s.Play(0);
        Require(s.CanUse(0, Counter, "pulse") && s.CanUse(0, Driver, "choose") && s.CounterState(0).SkillInstanceId == instance,
            "Actual turn-end expiry restores the existing skill instance and its activations.");
    }

    private static void ChosenLossReturnsToOrdinaryDrawCold()
    {
        var s = Scene.Start();
        var hand = s.View(0).HandCount;
        s.Use(0, Driver, "choose", 1);
        s.Reach(() => s.Pending is { } p && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skill-state"));
        var pending = s.Pending!;
        Require(pending.PlayerSeat == 0 && pending.Choices.Count == 1 &&
            pending.Choices[0].Parameters.GetValueOrDefault("skill") == Counter &&
            s.G.CreateSnapshot(0).Players[1].Skills!.Any(skill => skill.ContentId == Counter) &&
            s.G.CreateSnapshot(1).PendingDecision is null && s.View(0).HandCount == hand,
            "The owner privately chooses a publicly owned non-locked target skill before the following draw runs.");
        var frameId = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Driver).Id;
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("skill") == Counter); s.Play(0);
        Require(!s.Owns(1, Counter) && s.Owns(1, Locked) && s.View(0).HandCount == hand + 1 &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == frameId && e.Completed) == 1 &&
            s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) &&
                m.Reason.Value == $"skill-program.{Driver}.Draw") == 1,
            "The chosen loss completes on its original frame, then executes one ordinary native draw.");
        s.Cold(); s.Play(1);
        Require(s.Owns(1, Counter) && s.CanUse(1, Counter, "pulse"),
            "Temporary loss expires at the actual current turn end and restores the target's original skill.");
    }

    private static void OverlappingExpiryPreservesTheExistingInstance()
    {
        var s = Scene.Start(); s.Play(1);
        s.Use(1, Counter, "mark"); s.Play(1);
        var original = s.CounterState(1);
        Require(original.Value, "The target has a genuinely changed, persistent skill state before suppression.");
        s.Play(0);
        s.Use(0, Driver, "overlap-now", 1); s.Play(0);
        s.Use(0, Driver, "overlap-through-target", 1); s.Play(0);
        s.Play(1);
        Require(s.Owns(1, Counter) && !s.CanUse(1, Counter, "pulse") && s.CanUse(1, Locked, "pulse"),
            "Expiring one current-turn restriction must retain the independent restriction through the target's current turn.");
        s.Cold();
        Require(!s.CanUse(1, Counter, "pulse"), "Cold replay preserves the remaining overlapping restriction.");
        s.Play(2); s.Play(1);
        var restored = s.CounterState(1);
        Require(s.CanUse(1, Counter, "pulse") && restored.SkillInstanceId == original.SkillInstanceId && restored.Value &&
            !s.CanUse(1, Counter, "mark"),
            "The nearest matching target turn end restores the same changed instance without resetting its game quota.");
        s.Cold();
        Require(s.CounterState(1) == restored && s.CanUse(1, Counter, "pulse"), "Expired state and original instance cold-replay together.");
    }

    private static void TransferReusesAnAlreadyOwnedInstance()
    {
        var s = Scene.Start();
        s.Use(0, Counter, "mark"); s.Play(0);
        s.Use(0, Counter, "pulse"); s.Play(0);
        var original = s.CounterState(0);
        var acquired = s.E<SkillsAcquiredEvent>().Count(e => e.PlayerSeat == 0 && e.SkillIds.Contains(Counter));
        s.Use(0, Driver, "transfer", 1); s.Play(0);
        Require(s.Owns(0, Counter) && s.CounterState(0) == original &&
            !s.CanUse(0, Counter, "mark") && !s.CanUse(0, Counter, "pulse") &&
            s.E<SkillsAcquiredEvent>().Count(e => e.PlayerSeat == 0 && e.SkillIds.Contains(Counter)) == acquired,
            "Transferring an already owned skill retains its original state and paid quota without announcing a second acquisition.");
        s.Cold(); s.Play(1); s.Play(0);
        Require(s.Owns(0, Counter) && s.CounterState(0) == original && !s.CanUse(0, Counter, "pulse"),
            "Expiring the transferred source keeps the recipient's permanent source, state and game quota.");
    }

    private static void NamedSuppressionCoversAnIndependentlyAcquiredSource()
    {
        var s = Scene.Start();
        s.Use(0, Driver, "lose-own-counter"); s.Play(0);
        Require(!s.Owns(0, Counter) && !s.Effective(0, Counter), "The original Counter source was genuinely lost first.");
        s.Use(0, Driver, "suppress-absent-and-grant"); s.Play(0);
        Require(s.Owns(0, Counter) && !s.Effective(0, Counter) && !s.CanUse(0, Counter, "pulse") &&
            s.E<SkillsAcquiredEvent>().Count(e => e.PlayerSeat == 0 && e.SkillIds.Contains(Counter)) == 1,
            "Named suppression issued while Counter is absent also covers its later independent acquisition source.");
        s.Cold();
        Require(s.Owns(0, Counter) && !s.Effective(0, Counter), "Cold replay preserves a named restriction over a later skill source.");
        s.Play(1); s.Play(0);
        Require(s.Owns(0, Counter) && s.Effective(0, Counter) && s.CanUse(0, Counter, "pulse") &&
            s.CounterGrants(0).Any(g => g.SourceId.StartsWith("acquired:", StringComparison.Ordinal) && g.IsEnabled) &&
            s.CounterGrants(0).Where(SkillStateQuery.IsPrinted).All(g => !g.IsEnabled),
            "Expiry restores only the later acquired source; the permanently lost printed source stays disabled.");
    }

    private static void FiltersShareExpressionResourcesAndReadSkillFacts()
    {
        foreach (var fact in new[] { "skill.locked", "skill.limited", "skill.awakening", "skill.lord", "skill.printed", "skill.ordinal" })
            LoadFilter(JsonSerializer.Serialize(new object[] { "==", new object[] { "number", fact }, 0 }));
        LoadFilter("[\"==\",[\"number\",\"threshold\"],0]", captureThreshold: true);
        foreach (var filter in new[] { "[\"==\",[\"number\",\"missing-number\"],0]", "[\"==\",[\"boundCount\",\"missing-cards\"],0]" })
        {
            var rejected = false;
            try { LoadFilter(filter); }
            catch (InvalidOperationException) { rejected = true; }
            Require(rejected, "Skill filters must use the existing number/card resource validation for missing bindings.");
        }
        var s = Scene.Start();
        s.Use(0, Driver, "filter", 1); s.Play(0);
        Require(s.Owns(1, Counter) && !s.Effective(1, Counter) && s.Owns(1, Locked) && s.Effective(1, Locked),
            "The expression reads the candidate's printed ordinal and locked tag, suppressing Counter while leaving Locked effective.");
        s.Cold();
        Require(s.Owns(1, Counter) && !s.Effective(1, Counter) && s.Effective(1, Locked),
            "Skill-fact expression selection cold-replays without converting possession into effectiveness.");
    }

    private static void LoadFilter(string filter, bool captureThreshold = false)
    {
        var capture = captureThreshold ? "{\"op\":\"captureNumber\",\"target\":\"owner\",\"resultBind\":\"threshold\",\"valueExpr\":0}," : "";
        _ = SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Driver}}}","revision":1,
              "activations":[{"id":"filter","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                "effects":[{{{capture}}}{"op":"skillState","target":"owner","action":"suppress","scope":"expression","duration":"currentTurn","filter":{{{filter}}}}]}]}]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "Filter", description = "Shared expression resources" } } }));
    }

    private static void PermanentRestrictionExpiresOnItsTargetDeath()
    {
        var s = Scene.Start();
        s.Use(0, Driver, "permanent-until-death", 1); s.Play(0);
        var issued = s.E<SkillStateChangedEvent>().Single(e => e.Seat == 1 && e.SkillId == Counter && e.Action == SkillStateAction.Lose);
        Require(!s.Owns(1, Counter) && s.StateLeases(1) == 1 && s.Effective(0, Driver),
            "Permanent loss with death expiry retains a target-owned lease without losing the issuing skill.");
        s.Cold();
        s.Use(0, Driver, "kill-target", 1);
        s.Reach(() => s.E<PlayerDiedEvent>().Any(e => e.VictimSeat == 1));
        Require(!s.View(1).IsAlive && s.View(0).IsAlive && s.StateLeases(1) == 0 &&
            s.E<SkillStateExpiredEvent>().Count(e => e.Seat == 1 && e.SourceId == issued.SourceId) == 1,
            "The affected target's native death expires a permanent restriction independently of the living turn owner.");
        s.Cold();
        Require(s.StateLeases(1) == 0 && s.E<SkillStateExpiredEvent>().Count(e => e.SourceId == issued.SourceId) == 1,
            "Cold replay does not restore the expired lease or publish a second expiry.");
    }

    private static void ConsumedAlcoholSlashQualifiesOnlyItsOriginalDamage()
    {
        foreach (var (drank, chained) in new[] { (false, false), (true, false), (true, true) })
        {
            var s = Scene.Start();
            if (chained)
                foreach (var seat in new[] { 1, 2 }) { s.Use(0, Driver, "chain", seat); s.Play(0); }
            if (drank) { s.PlayAs("wine"); s.Play(0); s.Cold(); }
            var hp = s.View(1).Hp;
            s.PlayAs("fire-slash", 1); s.Play(0);
            var damage = s.E<DamageAppliedEvent>().Where(e => !e.SourceLess && e.SourceSeat == 0).ToArray();
            var states = s.E<SkillStateChangedEvent>().Where(e => e.Action == SkillStateAction.Suppress && e.SkillId == Counter).ToArray();
            Require(s.View(1).Hp == hp - (drank ? 2 : 1) && damage.Length == (chained ? 2 : 1) &&
                damage.All(e => e.Amount == (drank ? 2 : 1) && e.Nature == DamageNature.Fire) &&
                s.E<CardUseAlcoholConsumptionCapturedEvent>().Length == (drank ? 1 : 0),
                "Real converted Slash consumes Alcohol once; linked fire damage uses the original enhanced amount.");
            Require(states.Length == (drank ? 1 : 0) && states.All(e => e.Seat == 0 && e.Origin?.Source.SkillId == Driver) &&
                s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Driver && e.BindingId == "alcohol-state") == (drank ? 1 : 0) &&
                s.Owns(0, Counter) && s.Effective(0, Counter) == !drank,
                "Only the owner's original Alcohol-consuming Slash damage offers the binding and issues one named suppression; ordinary and chain-propagated damage do not.");
            s.Cold();
            Require(s.Effective(0, Counter) == !drank, "The shared Alcohol qualifier and resulting named state cold-replay together.");
        }
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? Pending => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot View(int seat) => G.CreateSnapshot(seat).Players[seat];
        private CharacterState Player(int seat) => ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine
            .GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(G)!)[seat];
        public bool Owns(int seat, string skill) => G.NationalSkillsEntries.HasOwnedRuntimeSkill(Player(seat), skill);
        public bool Effective(int seat, string skill) => G.NationalSkillsEntries.HasRuntimeSkill(Player(seat), skill);
        public int StateLeases(int seat) => Player(seat).SkillGrants.Grants.Count(g => g.SkillState is not null);
        public IReadOnlyList<SkillGrant> CounterGrants(int seat) => Player(seat).SkillGrants.Grants.Where(g => g.SkillId == Counter && g.SkillState is null).ToArray();
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool CanUse(int seat, string skill, string activation) => G.GetLegalActions(seat).Any(a => a.ProgramSkillId == skill && a.ProgramActivationId == activation);
        public ProgramBooleanStateSnapshot CounterState(int seat) => View(seat).SkillRuntimeStates!.Single(s => s.SkillId == Counter).BooleanStates!.Single(s => s.StateId == "marked");
        public void Use(int seat, string skill, string activation, params int[] targets)
        {
            Require(Pending is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat && CanUse(seat, skill, activation),
                $"Missing genuine activation {seat}/{skill}/{activation}.");
            Submit(new UseProgramSkillCommand(seat, skill, activation, [], targets, G.Revision, Pending!.PromptId));
        }
        public void Answer(Func<PromptChoice, bool> match)
        { var p = Pending!; Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(match).Id, G.Revision)); }
        public void PlayAs(string binding, params int[] targets)
        {
            var action = G.GetLegalActions(0).First(a => a.ConversionSource is { SkillId: Driver } source &&
                source.BindingId == binding && a.TargetSeats.SequenceEqual(targets));
            Submit(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, Pending!.PromptId, action.PlayedCardKind)
            { ConversionSource = action.ConversionSource });
        }
        public void Play(int seat) => Reach(() => Pending is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void Reach(Func<bool> stop)
        {
            for (var step = 0; step < 500; step++)
            {
                if (stop()) return;
                if (Pending is not { } p) { Submit(new AdvanceOneStepCommand(G.Revision)); continue; }
                switch (p.Kind)
                {
                    case DecisionKind.SelectGeneral:
                        var general = p.PlayerSeat == 0 ? Owner : p.ValidContentIds[0];
                        Submit(new SelectGeneralCommand(p.PlayerSeat, general, G.Revision, p.PromptId)); break;
                    case DecisionKind.PlayCard:
                        Submit(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); break;
                    case DecisionKind.DiscardCards:
                        Submit(new DiscardCardsCommand(p.PlayerSeat, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, G.Revision)); break;
                    case DecisionKind.RescueDying:
                        Answer(choice => choice.Cards.Count == 0); break;
                    case DecisionKind.RespondDodge:
                        Answer(choice => choice.Parameters.GetValueOrDefault("response") == "take-damage"); break;
                    default: throw new InvalidOperationException($"Unexpected shared skill-state prompt {p.Kind}, seat {p.PlayerSeat}.");
                }
            }
            throw new InvalidOperationException("Shared skill-state fixture missed its bounded native boundary.");
        }
        private void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Require(result.Accepted, result.Error?.Message ?? "A genuine skill-state command was rejected.");
        }
        public void Cold()
        {
            var views = Enumerable.Range(0, 4).Select(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat))).ToArray();
            var commands = G.AcceptedCommands.Count;
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Require(G.AcceptedCommands.Count == commands && Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(G.CreateSnapshot(seat)) == views[seat]),
                "Cold command replay preserves choices, every viewer's privacy, skill state and accepted command prefix.");
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var scene = new Scene(GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4,
                HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 16 }, registry), registry);
            scene.Submit(new StartGameCommand()); scene.Play(0);
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shared-skill-state", "1.0.0", "Shared skill-state command checks");
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"viewAs":[
                    {"id":"wine","inputKinds":["dodge"],"inputSuits":[],"sourceZones":["hand"],"outputKind":"alcohol","forPlay":true,"forResponse":false,"useOnly":true},
                    {"id":"fire-slash","inputKinds":["dodge"],"inputSuits":[],"sourceZones":["hand"],"outputKind":"fireSlash","forPlay":true,"forResponse":false,"useOnly":true}
                  ],"activations":[
                    {"id":"suppress-self","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"owner","action":"suppress","scope":"nonLocked","duration":"currentTurn"}]},
                    {"id":"choose","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"lose","scope":"chosenFromTarget","duration":"currentTurn","excludeLocked":true},
                      {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true}]},
                    {"id":"overlap-now","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","scope":"named","skillIds":["{{{Counter}}}"],"duration":"untilTurnEndOf","untilTurnEndOf":{"kind":"owner"}}]},
                    {"id":"overlap-through-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","scope":"named","skillIds":["{{{Counter}}}"],"duration":"untilTurnEndOf","untilTurnEndOf":{"kind":"selectedTarget"}}]},
                    {"id":"transfer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"transfer","scope":"named","skillIds":["{{{Counter}}}"],"duration":"currentTurn"}]},
                    {"id":"lose-own-counter","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"owner","action":"lose","scope":"named","skillIds":["{{{Counter}}}"],"duration":"permanent"}]},
                    {"id":"suppress-absent-and-grant","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"owner","action":"suppress","scope":"named","skillIds":["{{{Counter}}}"],"duration":"untilTurnEndOf","untilTurnEndOf":{"kind":"owner"}},
                      {"op":"grantSkills","target":"owner","skillIds":["{{{Counter}}}"]}]},
                    {"id":"filter","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"suppress","scope":"expression","duration":"currentTurn",
                       "filter":["and",["==",["number","skill.locked"],0],["==",["number","skill.printed"],1],["==",["number","skill.ordinal"],1]]}]},
                    {"id":"permanent-until-death","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"skillState","target":"selectedTarget","action":"lose","scope":"named","skillIds":["{{{Counter}}}"],"duration":"permanent","expireOnDeath":true}]},
                    {"id":"kill-target","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"loseHp","target":"selectedTarget","amount":20}]},
                    {"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[
                      {"op":"setChainedState","target":"selectedTarget","chained":true}]}
                  ],"triggers":[{"id":"alcohol-state","window":"afterDamageApplied","subject":"damageSource","optional":false,
                    "damageOccurrence":"perDamage","requiresConsumedAlcoholSlash":true,"effects":[
                      {"op":"skillState","target":"owner","action":"suppress","scope":"named","skillIds":["{{{Counter}}}"],"duration":"currentTurn","characterOnly":false}]}]},
                  {"id":"{{{Counter}}}","revision":1,"states":[{"id":"marked","initialValue":false,"visibility":"public","resetScope":"game","reacquirePolicy":"preserveUntilGameEnd"}],"activations":[
                    {"id":"mark","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"usesPerGame":1,"effects":[{"op":"setBooleanState","target":"owner","stateId":"marked","value":true}]},
                    {"id":"pulse","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"usesPerGame":1,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                  {"id":"{{{Locked}}}","revision":1,"activations":[{"id":"pulse","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"usesPerGame":1,"effects":[{"op":"draw","target":"owner","amount":1}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "State driver", description = "Shared state effects" },
                        [Counter] = new { name = "Counter", description = "Persistent state and quota" }, [Locked] = new { name = "Locked", description = "Locked exemption" } } }));
            foreach (var (id, program) in rules.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, Tags = id == Locked ? SkillTag.Locked : SkillTag.None });
            builder.AddGeneral(new(Owner, "State owner", "supporter", Driver, "wei", 8, [Counter, Locked]));
            for (var i = 1; i < 4; i++) builder.AddGeneral(new($"fixture:skill-state-other-{i}", "Public target", "supporter", Counter, "qun", 8, [Locked]));
            builder.AddDeck(new("fixture:skill-state-deck", "Quiet fixed deck", 4, 2, [])
            { PhysicalCards = Enumerable.Range(0, 160).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, i % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "Shared skill state", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 }, "fixture:skill-state-deck",
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, "fixture:skill-state-other-1", "fixture:skill-state-other-2", "fixture:skill-state-other-3"]));
        }
    }
}
