using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class ConditionalSkillGrantChecks
{
    private const string Driver = "fixture:conditional-driver", HpSource = "fixture:conditional-hp",
        HpLeaf = "fixture:conditional-once", SuitSource = "fixture:conditional-suits", SuitLeaf = "fixture:conditional-suit-leaf",
        PileSource = "fixture:conditional-pile", PileLeaf = "fixture:conditional-pile-leaf", Rewrite = "fixture:conditional-rewrite",
        Owner = "fixture:conditional-owner", Mode = "identity:classic-conditional-grants", Deck = "fixture:conditional-deck";

    public static void ConditionalGrantsKeepInstancesAndIndependentSourcesThroughCommandBoundaries()
    {
        DefinitionsKeepConditionalBoundaries();
        var s = Scene.Start();
        var parent = s.Player.SkillGrants.Grants.Single(grant => grant.SkillId == HpSource);
        var issued = s.Derived(HpLeaf)!;
        Need(issued.ConditionalSource is { } source && source.CapabilityGrantId == parent.GrantId &&
            source.CapabilitySkillInstanceId == parent.SkillInstanceId, "A conditional grant names its exact actual parent source.");
        s.Use(HpLeaf, "once"); s.Play();
        s.Use(Driver, "hurt"); s.Play();
        Need(!s.Effective(HpLeaf) && s.Derived(HpLeaf) is null, "Real HP loss removes only the condition's child grant.");
        s.Cold(); s.Use(Driver, "heal"); s.Play();
        Need(s.Derived(HpLeaf)?.SkillInstanceId == issued.SkillInstanceId,
            "Returning through the same HP condition restores the originally issued skill instance.");
        s.Reject(new UseProgramSkillCommand(0, HpLeaf, "once", [], [], s.Game.Revision, s.Pending!.PromptId));
        s.Use(Driver, "independent"); s.Play();
        var independent = s.Player.SkillGrants.Grants.Single(grant => grant.SkillId == HpLeaf && grant.ConditionalSource is null);
        Need(independent.SkillInstanceId == independent.GrantId && independent.SkillInstanceId != issued.SkillInstanceId &&
            s.Derived(HpLeaf)?.SkillInstanceId == issued.SkillInstanceId,
            "An independent acquisition owns its own instance while the existing conditional source retains its original instance.");
        s.Use(Driver, "hurt"); s.Play();
        Need(s.Derived(HpLeaf) is null && s.Effective(HpLeaf) && s.Player.SkillGrants.Grants.Contains(independent),
            "Conditional removal cannot delete an independent same-name source.");
        s.Use(Driver, "heal"); s.Play(); s.Use(Driver, "suppress-parent"); s.Play();
        Need(s.Player.SkillGrants.Grants.Contains(parent) && s.Game.NationalSkillsEntries.HasOwnedRuntimeSkill(s.Player, HpSource) &&
            !s.Effective(HpSource) && s.Derived(HpLeaf) is null && s.Effective(HpLeaf),
            "Suppressing the exact parent retains its real ownership while removing only that parent's dependent child.");
        s.Cold(); s.NextOwnPlay();
        Need(s.Effective(HpSource) && s.Derived(HpLeaf)?.SkillInstanceId == issued.SkillInstanceId,
            "Native turn cleanup restores the same parent and dependent instance after suppression expires.");
        s.Use(Driver, "lose-parent"); s.Play(); s.Cold();
        Need(!s.Game.NationalSkillsEntries.HasOwnedRuntimeSkill(s.Player, HpSource) && s.Derived(HpLeaf) is null &&
            s.Player.SkillGrants.Grants.Contains(independent) && s.Effective(HpLeaf),
            "Permanent source loss does not revive like an expired suppression and cannot revoke independent ownership.");
    }

    private static void DefinitionsKeepConditionalBoundaries()
    {
        void Load(string member) => SkillProgramCatalog.Load(
            $$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{HpSource}}","revision":1,{{member}}},{"id":"{{HpLeaf}}","revision":1,"modifiers":[{"id":"leaf","query":"handLimit","operation":"add","value":1,"priority":0}]}]}""",
            JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new[] { HpSource, HpLeaf }.ToDictionary(id => id, id => new { name = id, description = "Conditional definition" }) }));
        string Rule(string expression) => "\"skillGrants\":[{\"id\":\"rule\",\"skillId\":\"" + HpLeaf + "\",\"while\":" + expression + "}]";
        Load(Rule("""[">=",["count","livingOthers",["ruleWounded","it"]],0]"""));
        foreach (var member in new[] { "\"hpSkillGrants\":[]", "\"equipmentSuitSkillGrants\":[]", "\"skillGrants\":[]",
                     "\"skillGrants\":[{\"id\":\"rule\",\"skillId\":\"" + HpLeaf + "\"}]",
                     Rule("""["alive","selectedTarget"]"""), Rule("""["alive","eventTarget"]"""),
                     Rule("""[">",["number","captured"],0]"""), Rule("""["inIssuedTurn","owner"]""") })
        {
            var rejected = false;
            var field = member.StartsWith("\"hpSkillGrants\"", StringComparison.Ordinal) ? "hpSkillGrants" :
                member.StartsWith("\"equipmentSuitSkillGrants\"", StringComparison.Ordinal) ? "equipmentSuitSkillGrants" : ".skillGrants";
            try { Load(member); }
            catch (InvalidOperationException error) when (error.Message.Contains(field, StringComparison.Ordinal)) { rejected = true; }
            Need(rejected, "Conditional definitions reject legacy fields, empty/missing conditions and frame-bound facts: " + member);
        }
    }

    public static void ConditionalGrantFactsReadEquipmentPilesAndHpAuthority()
    {
        var s = Scene.Start();
        var undefined = new CharacterState { Seat = 0, Name = "Undefined HP", IsHuman = true, Role = Role.Rebel, RoleRevealed = true,
            General = s.Player.General, GeneralSelected = true, GeneralRevealed = true, Hp = 9, MaxHp = 9,
            IdentityConcealment = new(s.Player.General.Id, 9) };
        var world = new SkillStateConditionWorld(new(0, 0), undefined, health: s.Game.HealthQueriesEntries);
        using (var expression = JsonDocument.Parse("""[">=",["hp","owner"],9]"""))
            Need(ProgramExpressions.EvaluateBool(ProgramExpressions.Parse(expression.RootElement, "authority"), world) is null,
                "The existing HP authority keeps concealed HP unknown instead of using the stored scalar.");
        s.Use(Driver, "stock"); s.Play();
        s.PlayPhysical(card => card.Kind == CardKind.QinggangSword && card.Suit == Suit.Spade);
        s.PlayPhysical(card => card.Kind == CardKind.BaguaFormation && card.Suit == Suit.Heart);
        Need(s.Game.PhysicalOwnedCardQueriesEntries.GetEquipment(s.Player).Count == 2 && !s.Effective(SuitLeaf) &&
            s.Derived(PileLeaf) is not null && s.PileCount() == 2,
            "Two true equipment uses collapse to one rewritten suit and put actual top cards into the named public pile.");
        var pileInstance = s.Derived(PileLeaf)!.SkillInstanceId;
        s.Use(Driver, "suppress-rewrite"); s.Play();
        Need(s.Effective(SuitLeaf), "Disabling the independent suit-rewrite root exposes two equipment suits without recursing into a final shard.");
        var suitInstance = s.Derived(SuitLeaf)!.SkillInstanceId;
        s.Cold(); s.NextOwnPlay();
        Need(!s.Effective(SuitLeaf) && s.PileCount() == 2,
            "Native cleanup restores the independent rewrite without consuming the bounded source's actual pile.");
        s.TakePileCard();
        Need(s.PileCount() == 1 && s.Derived(PileLeaf)?.SkillInstanceId == pileInstance,
            "Taking one actual pile card leaves the nonempty condition and its same instance active.");
        s.TakePileCard();
        Need(s.PileCount() == 0 && s.Derived(PileLeaf) is null,
            "Taking the final card through the published native pile choice removes its dependent grant.");
        s.Cold();
        s.Use(Driver, "suppress-rewrite"); s.Play();
        Need(s.Derived(SuitLeaf)?.SkillInstanceId == suitInstance,
            "The same equipment condition can exit and return without resetting its remembered instance.");
        s.PlayPhysical(_ => true);
        Need(s.PileCount() == 1 && s.Derived(PileLeaf)?.SkillInstanceId == pileInstance,
            "Refilling the same source's real public pile restores its original conditional instance.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry content)
    {
        public GameEngine Game { get; private set; } = game;
        public PendingDecision? Pending => Game.CreateSnapshot(0).PendingDecision;
        public CharacterState Player => ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine
            .GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(Game)!)[0];
        public SkillGrant? Derived(string skill) => Player.SkillGrants.Grants.SingleOrDefault(grant => grant.SkillId == skill && grant.ConditionalSource is not null);
        public bool Effective(string skill) => Game.NationalSkillsEntries.HasRuntimeSkill(Player, skill);
        public int PileCount() => Game.PublicPileSourcesEntries.PublicPileSources(0).Where(source => source.SkillId == PileSource)
            .Sum(source => Game.PublicPileSourcesEntries.PublicPileCards(source).Count);
        public void Use(string skill, string activation) => Accept(new UseProgramSkillCommand(0, skill, activation, [], [], Game.Revision, Pending!.PromptId));
        public void Play() => Reach(() => Pending is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void PlayPhysical(Func<CardSnapshot, bool> choose)
        {
            var ids = Game.CreateSnapshot(0).Players[0].Hand.Where(choose).Select(card => card.Id).ToHashSet();
            var action = Game.GetHumanLegalActions().First(action => action.CardId is { } id && ids.Contains(id) &&
                action.ConversionSource is null && action.Kind is LegalActionKind.Equip or LegalActionKind.IronChain);
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, Game.Revision, Pending!.PromptId,
                action.PlayedCardKind, action.TargetCardId)); Play();
        }
        public void TakePileCard()
        {
            Use(Driver, "take-pile");
            Reach(() => Pending?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "public-pile-flow") == true);
            var prompt = Pending!;
            var choice = prompt.Choices.First(choice => choice.Parameters.GetValueOrDefault("program-action") == "public-pile-flow");
            Accept(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision)); Play();
        }
        public void NextOwnPlay()
        {
            var turns = Game.Events.Count(envelope => envelope.Payload is TurnStartedEvent);
            Accept(new EndPlayPhaseCommand(0, Game.Revision, Pending!.PromptId));
            Reach(() => Game.Events.Count(envelope => envelope.Payload is TurnStartedEvent) > turns &&
                Pending is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 600; step++)
            {
                if (done()) return;
                if (Pending is { PlayerSeat: 0 } prompt)
                {
                    var skip = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
                    if (skip is null) throw new InvalidOperationException("Unexpected conditional-grant prompt: " + JsonSerializer.Serialize(prompt));
                    Accept(new AnswerPromptCommand(0, prompt.PromptId, skip.Id, Game.Revision));
                }
                else Accept(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Conditional-grant native boundary did not arrive.");
        }
        public void Accept(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected conditional-grant command."); }
        public void Reject(GameCommand command)
        {
            var before = SnapshotJson.Serialize(Game.CreateSnapshot(0));
            Need(!Game.Submit(command).Accepted && SnapshotJson.Serialize(Game.CreateSnapshot(0)) == before,
                "A condition exit/re-entry cannot reset the same instance's already consumed current-turn usage.");
        }
        public void Cold()
        {
            var before = SnapshotJson.Serialize(Game.CreateSnapshot(0));
            Game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(Game.CreateCheckpoint())), content);
            Need(SnapshotJson.Serialize(Game.CreateSnapshot(0)) == before, "Cold recovery preserves the condition, real grants, instances and native continuation.");
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var s = new Scene(GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0,
                HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 12, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry), registry);
            s.Accept(new StartGameCommand()); s.Reach(() => s.Pending is { Kind: DecisionKind.SelectGeneral });
            s.Accept(new SelectGeneralCommand(0, Owner, s.Game.Revision, s.Pending!.PromptId)); s.Play(); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-conditional-grants", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            string Activation(string id, string effects) => $$"""{"id":"{{id}}","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{{effects}}]}""";
            string Suppress(string skill) => $$"""{"op":"skillState","target":"owner","action":"suppress","match":{"skillIds":["{{skill}}"]},"until":[{"event":"turnCleanup","participant":{"kind":"turnOwner"},"timing":"before"}]}""";
            var activations = new[] { Activation("hurt", """{"op":"loseHp","target":"owner","amount":1}"""),
                Activation("heal", """{"op":"recover","target":"owner","amount":1}"""),
                Activation("stock", string.Join(',', Enumerable.Repeat("""{"op":"draw","target":"owner","amount":20}""", 3))),
                Activation("independent", $$"""{"op":"grantSkills","target":"owner","skillIds":["{{HpLeaf}}"]}"""),
                Activation("take-pile", $$"""{"op":"obtainPublicPileCard","target":"owner","skillIds":["{{PileSource}}"]}"""),
                Activation("suppress-parent", Suppress(HpSource)), Activation("suppress-rewrite", Suppress(Rewrite)),
                Activation("lose-parent", $$"""{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{HpSource}}"]},"until":[]}""") };
            var ids = new[] { Driver, HpSource, HpLeaf, SuitSource, SuitLeaf, PileSource, PileLeaf, Rewrite };
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                {"id":"{{{Driver}}}","revision":1,"activations":[{{{string.Join(',', activations)}}}]},
                {"id":"{{{HpSource}}}","revision":1,"skillGrants":[{"id":"healthy","skillId":"{{{HpLeaf}}}","while":[">=",["hp","owner"],9],"onSourceLoss":"finishInvocation"}]},
                {"id":"{{{HpLeaf}}}","revision":1,"activations":[{"id":"once","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"draw","target":"owner","amount":1}]}]},
                {"id":"{{{SuitSource}}}","revision":1,"skillGrants":[{"id":"two-suits","skillId":"{{{SuitLeaf}}}","while":[">=",["equipmentSuitCount","owner"],2]}]},
                {"id":"{{{SuitLeaf}}}","revision":1,"modifiers":[{"id":"limit","query":"handLimit","operation":"add","value":1,"priority":0}]},
                {"id":"{{{PileSource}}}","revision":1,"skillGrants":[{"id":"filled","skillId":"{{{PileLeaf}}}","while":[">",["publicPileCount","owner","{{{PileSource}}}"],0]}],"triggers":[
                {"id":"store","window":"cardUseCompleted","ownerRelation":"actor","optional":false,"singleActionInstance":true,"effects":[{"op":"storeTopCardInPublicPile","target":"owner","amount":4}]}]},
                {"id":"{{{PileLeaf}}}","revision":1,"modifiers":[{"id":"limit","query":"handLimit","operation":"add","value":1,"priority":0}]},
                {"id":"{{{Rewrite}}}","revision":1,"cardPolicies":[{"id":"spade-heart","kind":"rewriteSuit","inputSuit":"spade","outputSuit":"heart"}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = ids.ToDictionary(id => id, id => new { name = id, description = "Shared conditional grant mechanism" }) }));
            foreach (var id in ids) builder.AddSkill(new(id, id, "Shared conditional grant mechanism")
            { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id], ExecutionForms = SkillExecutionForm.State | SkillExecutionForm.Trigger,
                ActionForms = id is Driver or HpLeaf ? SkillActionForm.Active : SkillActionForm.None });
            builder.AddGeneral(new(Owner, "Conditional owner", "supporter", HpSource, "qun", 8, [Driver, SuitSource, PileSource, Rewrite]));
            var peers = Enumerable.Range(1, 3).Select(seat => "fixture:conditional-peer-" + seat).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "Peer", "supporter", SuitLeaf, "shu", 8));
            var physical = new List<ContentDeckPhysicalCard>();
            foreach (var (kind, suit) in new[] { (CardKind.QinggangSword, Suit.Spade), (CardKind.QinggangSword, Suit.Heart),
                         (CardKind.BaguaFormation, Suit.Spade), (CardKind.BaguaFormation, Suit.Heart), (CardKind.IronChain, Suit.Diamond) })
            {
                var id = "fixture:conditional-card-" + kind.ToString().ToLowerInvariant() + "-" + suit.ToString().ToLowerInvariant();
                var card = CardCatalog.Get(kind); builder.AddCard(new(id, card.DisplayName, card.CategoryName, "Native fixture card", kind));
                physical.AddRange(Enumerable.Repeat(new ContentDeckPhysicalCard(id, suit, 7), 16));
            }
            builder.AddDeck(new(Deck, "Fixed shared mechanism deck", 4, 0, []) { PhysicalCards = physical.ToArray() });
            builder.AddMode(new(Mode, "Conditional grants", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
