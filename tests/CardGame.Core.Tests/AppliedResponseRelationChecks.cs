using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class AppliedResponseRelationChecks
{
    private const string Hold = "fixture:response-relation-hold";
    private const string Wenji = "dragonboat:wenji";
    private const BindingFlags Hidden = BindingFlags.Instance | BindingFlags.NonPublic;

    public static void ActualNamesFreezeNativeUsesAndCurrentCounterspellNodes()
    {
        var game = Start(Deck.DuelAndSlash);
        var use = PlayDuel(game);
        var capabilities = Capabilities(game);
        var rules = capabilities.Get<IAppliedResponseRules>();
        var frames = capabilities.Get<IProgramRuntimeFrames>();
        var store = capabilities.Get<IAppliedResponseRelationStore>();
        var relation = store.Relations.Single(item => item.RelationId == "ol:wenji" && item.ActorSeat == 0);
        Need(E<WenjiGiftPaidEvent>(game).Any(item => item.Source.OwnerSeat == 0 && item.Name == CardKind.Duel) &&
            use.AppliedResponseRulesCaptured && rules.IsResponseForbidden(use.Id, 1, CardKind.Slash, 0) &&
            rules.IsResponseForbidden(use.Id, 2, CardKind.Slash, 0) && !rules.IsResponseForbidden(use.Id, 0, CardKind.Slash, 1),
            "A real paid Wenji gift freezes the native Duel's all-other-responder audience, including a nontarget, while retaining self response.");
        Need(capabilities.Get<ICardZoneQueries>().CardsAt(CardLocation.Hand(1)).Any(card => card.Kind == CardKind.Slash),
            "The native prohibition fixture gives the actual Duel opponent a physical Slash.");
        var owner = capabilities.Get<IMatchParticipantStateAccess>().At(0);
        var grant = owner.SkillGrants.Grants.First(item => item.SkillId == Wenji);
        Need(owner.SkillGrants.RemoveGrant(grant.GrantId), "The fixture removes the actual Wenji source skill.");
        var end = relation.Lifetime!.Until.Single(item => item.Event == SkillStateBoundaryEvent.TurnEnded);
        capabilities.Get<IAppliedResponseRelationBoundaries>().ProcessBoundary(
            new(end.Event, end.Seat, end.Timing, end.IssuedOccurrence + 1, end.Phase));
        owner.IsAlive = false;
        Need(!store.Relations.Any(item => item.Sequence == relation.Sequence) &&
            ReferenceEquals(use, frames.Find(use.Id)) && rules.IsResponseForbidden(use.Id, 1, CardKind.Slash, 0),
            "Clearing future grants, losing the source skill and source death do not revoke the original native use's issued response rule.");
        owner.IsAlive = true;
        ContinueHold(game);
        Reach(game, () => !game.ResolutionStack.Any(frame => frame.Id == use.Id));
        Need(E<DuelResponseEvent>(game).Any(item => item.ResolutionId == use.Id && item.ResponderSeat == 1 && !item.UsedSlash) &&
            !E<DuelResponseEvent>(game).Any(item => item.ResolutionId == use.Id && item.ResponderSeat == 1 && item.UsedSlash),
            "The actual native Duel consumes the frozen prohibition and prevents an available physical Slash after future-grant cleanup.");

        var separate = Start(Deck.DuelGrantWithCounterspells);
        var parent = PlayDuel(separate);
        var separateRules = Capabilities(separate).Get<IAppliedResponseRules>();
        Need(separateRules.IsResponseForbidden(parent.Id, 1, CardKind.Nullification),
            "A Duel-name grant prohibits the original trick's initial other-player counterspell response.");
        ContinueHold(separate);
        Reach(separate, () => P(separate) is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        Need(separate.ResolutionStack.OfType<NullificationWindowFrame>().Last().ChainDepth == 0,
            "The original Duel restriction leaves the actual declarer as the first available counterspell responder.");
        RespondNullification(separate);
        var current = separate.ResolutionStack.OfType<NullificationWindowFrame>().Last();
        Need(current.ResponseRuleNode is { Action.ActorSeat: 0 } && current.ChainDepth > 0 &&
            !separateRules.IsCounterspellResponseForbidden(current, 1),
            "A real accepted Nullification owns its current node and cannot inherit the parent Duel's name prohibition.");
        Need(P(separate) is { Kind: DecisionKind.Nullification, PlayerSeat: 1 },
            "The native scheduler offers the next genuine opponent a counterspell after the current Nullification escapes its parent Duel restriction.");

        var counterGame = Start(Deck.NullificationGrant);
        PlayDuel(counterGame);
        ContinueHold(counterGame);
        Reach(counterGame, () => P(counterGame) is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
        RespondNullification(counterGame);
        var window = counterGame.ResolutionStack.OfType<NullificationWindowFrame>().Last();
        var counterCapabilities = Capabilities(counterGame);
        var counterRules = counterCapabilities.Get<IAppliedResponseRules>();
        var counterFrames = counterCapabilities.Get<IProgramRuntimeFrames>();
        Need(window.ResponseRuleNode is { Action: { ActorSeat: 0, ProviderSeat: 0, ResponderSeat: 0 } } &&
            counterRules.IsCounterspellResponseForbidden(window, 1) &&
            counterRules.IsCounterspellResponseForbidden(window, 1, providerSeat: 0) &&
            !counterRules.IsCounterspellResponseForbidden(window, 0, providerSeat: 1) &&
            !counterRules.IsResponseForbidden(window.ParentFrameId, 1, CardKind.Slash),
            "A genuine self-provided Nullification freezes its own name rule; logical responder identity stays separate from provider and its unmatched parent trick.");
        var query = counterRules.CreateCounterspellQuery(window, window.SourceSeat, 1)!.Value;
        counterFrames.Replace(window.Id, window with { ChainDepth = window.ChainDepth + 1 });
        Need(!counterRules.GetResponseDecision(query).Forbidden &&
            !counterRules.IsCounterspellResponseForbidden((NullificationWindowFrame)counterFrames.Find(window.Id)!, 1),
            "A later counterspell depth cannot reuse an earlier node's action or issued response rules.");
        var restored = JsonSerializer.Deserialize<NullificationWindowFrame>(JsonSerializer.Serialize(window))!;
        counterFrames.Replace(window.Id, restored);
        Need(counterRules.IsCounterspellResponseForbidden(restored, 1),
            "The actual counterspell node and its frozen audience survive ordinary original-frame JSON recovery.");
        GenericJsonDeclaresMatchingAndLifetimes();
    }

    private static void GenericJsonDeclaresMatchingAndLifetimes()
    {
        var game = Start(Deck.Generic);
        var selected = Capabilities(game).Get<ICardZoneQueries>().CardsAt(CardLocation.Hand(0)).First(card => card.Kind == CardKind.Slash);
        Need(selected.Suit == Suit.None, "The generic source-bind fixture starts with a real unknown-color card.");
        Accept(game, new UseProgramSkillCommand(0, Hold, "apply", [selected.Id], [], game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game)?.SkillPrompt?.SkillId == Hold);
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Last();
        var capabilities = Capabilities(game);
        var rules = capabilities.Get<IAppliedResponseRules>();
        var store = capabilities.Get<IAppliedResponseRelationStore>();
        var any = store.Relations.Single(item => item.RelationId == "fixture:any");
        var color = store.Relations.Single(item => item.RelationId == "fixture:unknown");
        Need(any.ProducerFrameId == frame.Id && any.Source.SkillId == Hold && any.Lifetime is not null &&
            capabilities.Get<ICardZoneQueries>().CardsAt(CardLocation.DiscardPile).Any(card => card.Id == selected.Id) &&
            color.Spec.Match is { Kind: ResponseRelationMatchKind.EffectiveColor, IsRed: null } &&
            rules.GetMatchingRelation("fixture:unknown", 0, 1, CardKind.Slash, true) is null &&
            rules.GetMatchingRelation("fixture:unknown", 0, 1, CardKind.Slash, false) is null &&
            rules.GetMatchingRelation("fixture:unknown", 0, 1, CardKind.Slash, null) is null,
            "Executed JSON freezes the actual card binding into the shared store; an unknown frozen color is never a wildcard.");
        Need(rules.GetMatchingRelation("fixture:slash", 0, 1, CardKind.Slash, null) is not null &&
            rules.GetMatchingRelation("fixture:slash", 0, 1, CardKind.FireSlash, null) is null &&
            rules.GetMatchingRelation("fixture:slash", 0, 1, CardKind.ThunderSlash, null) is null,
            "The generic exact-card-name matcher preserves Slash, Fire Slash and Thunder Slash as distinct actual names.");
        ContinueHold(game);
        Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard });
        var use = PlayDuel(game);
        Need(rules.IsResponseForbidden(use.Id, 1, CardKind.Slash, 0),
            "A real subsequent native use captures the generic JSON Any relationship.");
        var end = any.Lifetime!.Until.Single();
        capabilities.Get<IAppliedResponseRelationBoundaries>().ProcessBoundary(new(end.Event, end.Seat, end.Timing,
            end.IssuedOccurrence + 1, end.Phase));
        Need(store.Relations.Count == 0 && rules.IsResponseForbidden(use.Id, 1, CardKind.Slash, 0),
            "The declared JSON lifetime clears future relationships without changing the already issued native use.");

        foreach (var invalid in new[] { "matcher", "scope", "binding" })
        {
            var node = JsonNode.Parse(HoldRules)!;
            var effects = node["skills"]![0]!["activations"]![0]!["effects"]!;
            if (invalid == "matcher") effects[2]!["match"] = "any"; // An Any matcher cannot declare an exact name.
            else if (invalid == "scope") effects[3]!["captureKinds"] = new JsonArray("all");
            else effects[1]!["sourceBind"] = "missing";
            var rejected = false;
            try { _ = SkillProgramCatalog.LoadCurrent(node.ToJsonString(), HoldPresentation()); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "The shared loader rejects malformed response matchers/scopes and undeclared source-card resources: " + invalid);
        }
    }

    private static GameEngine Start(Deck deck)
    {
        var type = typeof(DragonBoatGeneralChecks).GetNestedType("Fixture", BindingFlags.NonPublic)!;
        var original = (IGameContentPackage)Activator.CreateInstance(type,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, [deck == Deck.Generic ? "rules" : "wenji"], null)!;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(original, deck));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 8, HumanSeat = 0, HumanRole = null,
            HumanTeamId = "team:wei", ModeId = "fixture:dragonboat-generals", UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 24 }, registry);
        Accept(game, new StartGameCommand());
        Reach(game, () => P(game)?.Kind == DecisionKind.SelectGeneral);
        var prompt = P(game)!;
        var general = prompt.Choices.First(choice => !choice.Parameters.GetValueOrDefault("general-id", "").StartsWith("dragonboat:", StringComparison.Ordinal));
        Accept(game, new SelectGeneralCommand(0, general.Parameters.GetValueOrDefault("general-id") ?? general.Id.Value,
            game.Revision, prompt.PromptId));
        Reach(game, () => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        return game;
    }

    private static CardUseFrame PlayDuel(GameEngine game)
    {
        var action = game.GetHumanLegalActions().First(item => item.Kind == LegalActionKind.Duel && item.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game)?.SkillPrompt?.SkillId == Hold);
        return game.ResolutionStack.OfType<CardUseFrame>().Last();
    }
    private static void ContinueHold(GameEngine game)
    {
        var prompt = P(game)!;
        Need(prompt.SkillPrompt?.SkillId == Hold, "The fixture returns the actual target-finalization child.");
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First().Id, game.Revision));
    }
    private static void RespondNullification(GameEngine game)
    {
        var prompt = P(game)!;
        var choice = prompt.Choices.First(item => item.Parameters.GetValueOrDefault("response") == "nullification");
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
    }
    private static ProgramCapabilityRegistry Capabilities(GameEngine game) => (ProgramCapabilityRegistry)typeof(GameEngine)
        .GetProperty("ProgramCapabilities", Hidden)!.GetValue(game)!;
    private static PendingDecision? P(GameEngine game) => Enumerable.Range(0, 8).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(value => value is not null);
    private static IEnumerable<T> E<T>(GameEngine game) => game.Events.Select(item => item.Payload).OfType<T>();
    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The actual response-relation fixture command was rejected.");
    }
    private static void Reach(GameEngine game, Func<bool> done)
    {
        for (var index = 0; index < 4000; index++)
        {
            if (done()) return;
            var prompt = P(game);
            if (prompt is { PlayerSeat: 0 } && prompt.Kind is not (DecisionKind.PlayCard or DecisionKind.SelectGeneral))
            {
                var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") == "wenji" && item.Targets.SequenceEqual([4])) ??
                    prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") == "activate") ??
                    prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") == "skip") ??
                    prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") ??
                    prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") != "activate");
                Need(choice is not null, "The response-relation fixture needs a real published choice.");
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice!.Id, game.Revision));
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The fixed-seed response-relation fixture missed its native boundary.");
    }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
    private enum Deck { DuelAndSlash, DuelGrantWithCounterspells, NullificationGrant, Generic }

    private const string HoldRules = """
        {"skills":[{"id":"fixture:response-relation-hold","revision":1,"activations":[
          {"id":"apply","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[
            {"op":"captureSelectedCards","target":"owner","resultBind":"source"},
            {"op":"applyResponseRelation","target":"owner","relationId":"fixture:unknown","sourceBind":"source","match":"effectiveColor","captureKinds":["nativeUse"],"until":[{"event":"turnEnded","participant":{"kind":"owner"},"timing":"before"}],"forbid":true,"audience":"all","excludeActor":true},
            {"op":"applyResponseRelation","target":"owner","relationId":"fixture:slash","match":"exactCardName","cardKind":"slash","captureKinds":["nativeUse"],"until":[{"event":"turnEnded","participant":{"kind":"owner"},"timing":"before"}],"forbid":true,"audience":"all","excludeActor":true},
            {"op":"applyResponseRelation","target":"owner","relationId":"fixture:any","match":"any","captureKinds":["nativeUse"],"until":[{"event":"turnEnded","participant":{"kind":"owner"},"timing":"before"}],"forbid":true,"audience":"all","excludeActor":true},
            {"op":"moveBoundCards","target":"owner","sourceBind":"source","destination":"discardPile","awaitMovementTriggers":true},
            {"op":"chooseOption","target":"owner","resultBind":"applied","options":[{"id":"continue"}]}]}],"triggers":[
          {"id":"hold","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["duel"],
           "optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"held","options":[{"id":"continue"}]}]}]}]}
        """;
    private static string HoldPresentation() => JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
        skills = new Dictionary<string, object> { [Hold] = new { name = "Hold actual use", description = "Read the original accepted native use.",
            optionLabels = new Dictionary<string, string> { ["continue"] = "Continue" } } } });

    private sealed class Fixture(IGameContentPackage original, Deck deck) : IGameContentPackage
    {
        public PackageManifest Manifest => original.Manifest;
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = JsonNode.Parse(HoldRules)!;
            if (deck != Deck.Generic) rules["skills"]![0]!.AsObject().Remove("activations");
            var catalog = SkillProgramCatalog.LoadCurrent(rules.ToJsonString(), HoldPresentation());
            builder.AddSkill(new(Hold, catalog.Presentations[Hold].Name, catalog.Presentations[Hold].Description)
            { Program = catalog.Programs[Hold], ProgramPresentation = catalog.Presentations[Hold] });
            original.Register(new Builder(builder, deck));
        }
        private sealed class Builder(IContentRegistryBuilder target, Deck deck) : IContentRegistryBuilder
        {
            public void AddCard(ContentCardDefinition value) => target.AddCard(value);
            public void AddSkill(ContentSkillDefinition value) => target.AddSkill(value);
            public void AddGeneral(ContentGeneralDefinition value) => target.AddGeneral(value.SkillId == "fixture:dragonboat-general-driver"
                ? value with { AdditionalSkillIds = (value.AdditionalSkillIds ?? []).Append(Hold).ToArray() } : value);
            public void AddDeck(ContentDeckRecipe value) => target.AddDeck(value with
            {
                PhysicalCards = value.PhysicalCards!.Select((card, index) => card with
                {
                    CardDefinitionId = index + 1 == 46 || deck != Deck.NullificationGrant && index + 1 == 142 ? "standard:duel" :
                        deck is Deck.DuelAndSlash or Deck.Generic ? "standard:slash" : "standard:nullification",
                    Suit = deck == Deck.Generic ? Suit.None : card.Suit
                }).ToArray()
            });
            public void AddMode(ContentModeDefinition value) => target.AddMode(value);
            public void AddEquipmentContent(EquipmentContentPackage value) => target.AddEquipmentContent(value);
            public void AddModeFeatureFactory(IGameModeFeatureFactory value) => target.AddModeFeatureFactory(value);
            public bool HasModeFeatureFactory(string id) => target.HasModeFeatureFactory(id);
        }
    }
}
