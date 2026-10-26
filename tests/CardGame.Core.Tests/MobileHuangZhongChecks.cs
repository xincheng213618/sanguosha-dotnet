using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileHuangZhongChecks
{
    private const string Skill = "mobile:liegong", Legacy = "classic:liegong", Watch = "fixture:ordinary-mobile-huang-zhong-watch",
        Deck = "fixture:ordinary-mobile-huang-zhong-deck", Mode = "identity:classic-ordinary-mobile-huang-zhong";

    public static void DesignatedTargetQualificationSurvivesLaterHandGainCold()
    {
        foreach (var legacy in new[] { false, true })
        {
            var s = Scene.Start(legacy); var slash = s.V(0).Hand.Single().Id; var hp = s.V(1).Hp;
            s.Need(s.V(0).Hand.Single().Kind == CardKind.FireSlash && s.V(1).Hand is [{ Kind: CardKind.Dodge }] &&
                s.V(1).HandCount <= s.G.GetAttackRange(0) && s.V(1).HandCount < s.V(0).Hp,
                "Real opening entities establish eligibility only through the attack-range branch, with an actual target Dodge still available.");
            s.Accept(new PlayCardCommand(0, slash, [1], s.G.Revision, s.P!.PromptId));
            if (!legacy)
            {
                s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill);
                var window = s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last();
                var candidate = window.Candidates[window.CandidateIndex];
                s.Need(candidate.SkillId == Skill && candidate.FrozenContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: 1,
                    Facts: { EventTargetHandCount: 1 } facts, CardUse: { ActorSeat: 0 } action } &&
                    facts.CurrentAttackRange == 1 && facts.CurrentHp == s.V(0).Hp &&
                    s.G.ResolutionStack.OfType<CardUseFrame>().Any(u => u.Id == action.ParentCardUseFrameId && u.CardId == slash) &&
                    s.V(1).HandCount == 1 && s.E<ResponseRequestedEvent>().Length == 0,
                    "Mobile Liegong is offered on the original physical Slash's actual designated-target window before target effects can change the qualification.");
                s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            }
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
            var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(u => u.CardId == slash);
            s.Need(s.V(1).HandCount == 2 && s.V(1).HandCount > s.G.GetAttackRange(0) && s.V(1).HandCount < s.V(0).Hp &&
                use.CardAttack is { TargetSeat: 1 } attack && attack.ProhibitsDodge == !legacy &&
                use.Action is { ActorSeat: 0, Type: CardActionType.Use, EffectiveKind: CardKind.FireSlash, PhysicalCards: [var material] } && material.CardId == slash &&
                s.E<ResponseRequestedEvent>().Length == 0 && s.E<DamageAppliedEvent>().Length == 0 &&
                s.G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(1) && m.Reason.Value == $"skill-program.{Watch}.Draw") == 1,
                "A genuine target-effect Draw makes both live comparisons false; only the mobile original use already owns its exact target's prohibition, before any response or damage.");
            if (!legacy) s.Cold();
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            if (legacy)
            {
                s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 });
                s.Answer(c => c.Cards.Count == 1 && s.V(1).Hand.Any(h => h.Id == c.Cards[0] && h.Kind == CardKind.Dodge));
            }
            s.Play();
            s.Need(s.V(1).Hp == hp - (legacy ? 0 : 1) && s.V(1).HandCount == (legacy ? 1 : 2),
                "The mobile ban retains both target cards and deals one damage; classic spends one target Dodge and loses no HP.");
            s.Need(s.E<ProgramCardTriggerResolvedEvent>().Count(e => e.SkillId == Skill && e.Activated) == (legacy ? 0 : 1) &&
                s.E<ProgramCardTriggerResolvedEvent>().All(e => e.SkillId != Legacy || !e.Activated),
                "Only mobile issues its original qualified benefit; classic's later ineligible comparison does not activate.");
            var responses = s.E<CardRespondedEvent>().Where(e => e.ResponderSeat == 1 && e.EffectiveCardKind == CardKind.Dodge).ToArray();
            s.Need(responses.Length == (legacy ? 1 : 0),
                "Actual Dodge response fact count must agree with the legal human response: " + responses.Length);
            var damage = s.E<DamageAppliedEvent>().Where(e => e.SourceCardUseResolutionId == use.Id).ToArray();
            s.Need(damage.Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1 && e.Nature == DamageNature.Fire) == (legacy ? 0 : 1),
                "The original physical FireSlash keeps its exact damage attribution: " + JsonSerializer.Serialize(damage));
            var paid = s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing);
            var cleaned = s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile);
            var finished = s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id);
            s.Need(paid == 1 && cleaned == 1 && finished == 1,
                $"The original physical Slash pays, cleans and finishes once: paid={paid}, cleaned={cleaned}, finished={finished}.");
        }
    }

    private static string General(int seat) => "fixture:ordinary-mobile-huang-zhong-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry, bool legacy)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command) { var r = G.Submit(command); Need(r.Accepted, r.Error?.Message ?? "Rejected real mobile Huang Zhong command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.FirstOrDefault(predicate); Need(c is not null, "Missing actual native Huang Zhong choice."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 150; step++)
            {
                if (ready()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } choose)
                { Accept(new SelectGeneralCommand(choose.PlayerSeat, General(choose.PlayerSeat), G.Revision, choose.PromptId)); continue; }
                Need(P is null, "Unexpected native Huang Zhong boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded native Huang Zhong scene did not reach its next boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the original physical Slash, frozen target prohibition, actual hand gain and native parent/child cursor.");
        }
        public void Need(bool ok, string why)
        { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { Legacy = legacy, G.State.CurrentSeat, G.State.Phase,
            Hands = Enumerable.Range(0, 2).Select(i => V(i).HandCount).ToArray(), Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null })); }
        public static Scene Start(bool legacy)
        {
            // Reuse one fixed native deal to identify the exact owner slot, then
            // rebuild that physical recipe. There is no seed search or state patch.
            var stock = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray();
            var probe = Boot(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock, legacy)), legacy);
            stock[probe.V(0).Hand.Single().Id - 1] = new("standard:fire_slash", Suit.Diamond, 7);
            return Boot(ContentRegistry.Build(new StandardContentPackage(), new Fixture(stock, legacy)), legacy);
        }
        private static Scene Boot(ContentRegistry r, bool legacy)
        {
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, r), r, legacy);
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture(IReadOnlyList<ContentDeckPhysicalCard> stock, bool legacy) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ordinary-mobile-huang-zhong", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:huang-zhong", "classic:huang-zhong");
            var program = new { id = Watch, revision = 1, triggers = new[] { new { id = "actual-later-target-effect", window = "cardUseBeforeTargetEffects", ownerRelation = "target",
                cardKinds = new[] { "slash", "fireSlash", "thunderSlash", "coldSlash" }, optional = false, isSkillInvocation = false,
                effects = new object[] { new { op = "draw", target = "owner", amount = 1 },
                    new { op = "chooseOption", target = "owner", resultBind = "after-gain", options = new[] { new { id = "continue" } } } } } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { program } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> {
                    [Watch] = new { name = "真实指定后获得牌", description = "在目标效果窗让原目标实际摸一张，再暂停原父帧。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续原杀" } } } }));
            b.AddSkill(new(Watch, c.Presentations[Watch].Name, c.Presentations[Watch].Description) { Program = c.Programs[Watch], ProgramPresentation = c.Presentations[Watch] });
            for (var seat = 0; seat < 4; seat++) b.AddGeneral(new(General(seat), "真实固定参与者", "supporter", seat == 0 ? legacy ? Legacy : Skill : "standard:none", "shu",
                seat == 0 ? 3 : 8, AdditionalSkillIds: seat == 1 ? [Watch] : []));
            b.AddDeck(new(Deck, "固定真实杀闪实体", 1, 0, []) { PhysicalCards = stock });
            b.AddMode(new(Mode, "普通移动黄忠指定时点", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
