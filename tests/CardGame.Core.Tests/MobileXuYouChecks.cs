using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileXuYouChecks
{
    private const string Mobile = "mobile:chenglue", Classic = "classic:chenglue", Owner = "fixture:mobile-xu-you-owner",
        Driver = "fixture:mobile-xu-you-driver", Hold = "fixture:mobile-xu-you-paid-child",
        Requester = "fixture:mobile-xu-you-ending-requester",
        Mode = "identity:classic-mobile-xu-you-fixture", Deck = "fixture:mobile-xu-you-deck";

    public static void ActualTurnSuitsSurviveExtraPlayAndSourceLossAfterPaidCold()
    {
        foreach (var mobile in new[] { false, true })
        {
            var skill = mobile ? Mobile : Classic; var s = Scene.Start(mobile); s.Play(0);
            s.Need(!s.CanSlash(2) && s.CanSlash(1), "Before payment a native range-one Slash reaches its neighbor and cannot reach the opposite real seat.");
            s.Use(skill, "alternate"); s.DiscardChoice();
            s.Answer(c => c.Cards.Count == 1); s.DiscardChoice();
            s.Need(!s.Facts<PlayPhaseSuitAllowanceGrantedEvent>().Any() && s.Costs() == 0,
                "An incomplete real two-card Yang payment grants no suit allowance and moves no partial material.");
            s.Answer(c => c.Cards.Count == 1); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
            var allowance = s.Facts<PlayPhaseSuitAllowanceGrantedEvent>().Single();
            s.Need(allowance.SkillId == skill && allowance.DiscardedCardIds.Count == 2 && allowance.Suits.SequenceEqual([Suit.Spade]) && s.Costs() == 2 &&
                (mobile ? allowance.Expiry == SkillProgramTurnSkillExpiry.ActualTurnEnd && allowance.ActualTurnOwnerSeat == 0 :
                    allowance.Expiry is null && allowance.ActualTurnOwnerSeat is null && !JsonSerializer.Serialize(allowance).Contains("Expiry", StringComparison.Ordinal)),
                "Only the exact paid original hand set issues one allowance; mobile freezes its actual-turn expiry and classic retains its omitted default fields.");
            s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play(0);
            s.Use(Driver, "lose-source"); s.Play(0);
            s.Need(s.CanSlash(2) && s.V(0).Skills!.All(k => k.ContentId != skill) && !s.G.GetLegalActions(0).Any(a => a.ProgramSkillId == skill),
                "Losing the source after the paid native child does not revoke an already issued permission in its original Play.");
            s.End(0); s.Play(0);
            s.Need(s.G.State.TurnNumber == allowance.TurnNumber && s.Facts<ProgramPhaseScheduledEvent>().Any(e => !e.Started) && s.Costs() == 2,
                "The inserted first Play really returns into a separate normal Play in the same actual turn without repeating either original discard.");
            if (!mobile)
            {
                s.Need(!s.CanSlash(2) && s.CanSlash(1) && s.V(0).IssuedPlayPhaseSuitUseAllowances is null,
                    "Classic's omitted policy expires at the original Play boundary and does not extend to the next Play.");
                continue;
            }
            s.Need(s.CanSlash(2) && s.V(0).IssuedPlayPhaseSuitUseAllowances is [{ Expiry: SkillProgramTurnSkillExpiry.ActualTurnEnd } projected] &&
                projected.PhaseInstanceId == allowance.PhaseInstanceId && projected.ActualTurnOwnerSeat == allowance.ActualTurnOwnerSeat,
                "The new Play exposes the original issued mobile allowance with its original phase identity and actual-turn deadline.");
            var hp = s.V(2).Hp; s.Slash(2); s.Play(0); s.Slash(2); s.Play(0);
            s.Need(s.V(2).Hp == hp - 2 && s.Facts<PlayPhaseSuitAllowanceGrantedEvent>().Length == 1 && s.Costs() == 2,
                "The actual same-suit physical Slashes cross the normal distance and use-count limits in the later Play, with no new permission or material payment.");
            s.End(0); s.Play(1);
            for (var seat = 1; seat < 4; seat++) { s.End(seat); s.Play(seat == 3 ? 0 : seat + 1); }
            s.Need(s.G.State.TurnNumber > allowance.TurnNumber && s.V(0).IssuedPlayPhaseSuitUseAllowances is null && !s.CanSlash(2) && s.CanSlash(1),
                "Returning to the owner's next actual turn cannot reuse the old discarded-suit permission.");
        }
    }

    public static void LoaderRejectsUnsupportedAllowanceDeadlinesAndUnrelatedNode()
    {
        foreach (var onTop in new[] { false, true })
        {
            var bundle = onTop ? "classic-xu-you" : "mobile-xu-you";
            var rules = JsonNode.Parse(Embedded(bundle, "rules.json"))!;
            var effect = onTop ? rules["skills"]![1]!["triggers"]![0]!["effects"]![0]! : rules["skills"]![0]!["activations"]![0]!["effects"]![0]!;
            effect["allowanceExpiry"] = onTop ? "actualTurnEnd" : "recipientNextActualTurnStart";
            try { SkillProgramCatalog.Load(rules.ToJsonString(), Embedded(bundle, "presentation.json")); }
            catch (InvalidOperationException error) when (error.Message.Contains("allowanceExpiry", StringComparison.Ordinal)) { continue; }
            throw new InvalidOperationException("Suit allowance expiry must reject unsupported deadlines and must not attach to the unrelated completed-top operation.");
        }
    }

    public static void IssuedActualTurnAllowanceSupportsEndingUseUntilCursorReturns()
    {
        foreach (var mobile in new[] { false, true })
        {
            var s = Scene.Start(mobile, endingUse: true); s.Play(0);
            s.Use(mobile ? Mobile : Classic, "alternate"); s.DiscardChoice(); s.Answer(c => c.Cards.Count == 1);
            s.DiscardChoice(); s.Answer(c => c.Cards.Count == 1); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hold);
            var allowance = s.Facts<PlayPhaseSuitAllowanceGrantedEvent>().Single();
            s.Need(!mobile || allowance.ActualTurnOwnerSeat == 0,
                "The inserted own Play freezes the real actual turn owner.");
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play(0); s.End(0); s.Play(0); s.End(0);
            s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "ending-continue") == true);
            s.Need(s.G.State.Phase != TurnPhase.Play && s.G.State.CurrentSeat == 0 && s.G.GetSeatDistance(0, 2) > s.G.GetAttackRange(0) &&
                (s.V(0).IssuedPlayPhaseSuitUseAllowances is not null) == mobile,
                "Only mobile's issued actual-turn allowance remains visible in its real ending phase, outside Play, while the native recipient is genuinely beyond attack range.");
            s.Cold();
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "ending-continue");
            var hp = s.V(2).Hp; var usesBefore = s.Facts<CardActionAcceptedEvent>().Length;
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Requester && s.P.PlayerSeat == 2);
            s.Answer(c => c.Targets.SequenceEqual([0]));
            if (mobile)
            {
                s.Reach(() => s.P?.SkillPrompt?.SkillId == Requester && s.P.PlayerSeat == 0);
                var card = s.P!.Choices.First(c => c.Cards.Count == 1 && c.Targets.SequenceEqual([2])).Cards.Single();
                s.Need(s.G.State.Phase != TurnPhase.Play && s.V(0).Hand.Any(c => c.Id == card && c.Kind == CardKind.Slash),
                    "The mobile ending request must expose a genuine currently held physical Slash before it is actually used.");
                s.Answer(c => c.Cards.SequenceEqual([card]) && c.Targets.SequenceEqual([2]));
                s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 2 } or { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
                if (s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 2 })
                    s.Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                s.Need(s.Facts<CardActionAcceptedEvent>().Any(e => e.Action is { ActorSeat: 0, Type: CardActionType.Use,
                    EffectiveKind: CardKind.Slash, EffectiveSuit: Suit.Spade } a && a.PhysicalCards.Select(c => c.CardId).SequenceEqual([card])),
                    "After the ending cold return the exact offered distant Slash entity is really used, including the native no-Dodge fast path.");
            }
            else
            {
                s.Reach(() => s.P?.SkillPrompt?.SkillId == Requester && s.P.PlayerSeat == 0 ||
                    s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
                if (s.P?.SkillPrompt?.SkillId == Requester)
                    s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "request-slash-decline");
                s.Need(s.Facts<CardActionAcceptedEvent>().Length == usesBefore && s.V(2).Hp == hp,
                    "Classic's native decline or no-offer completion performs no real card use and no damage.");
            }
            s.Play(1);
            s.Need(s.V(2).Hp == hp - (mobile ? 1 : 0) && s.V(0).IssuedPlayPhaseSuitUseAllowances is null &&
                s.Facts<ActualTurnCursorReturnedEvent>().Any(e => e.ActualTurn == allowance.TurnNumber && e.OwnerSeat == 0) && s.Costs() == 2,
                "The actual turn cursor returns after the native ending use; both policies expose no expired permission and neither re-pays the original discard.");
        }
    }
    private static string Embedded(string bundle, string suffix)
    {
        using var stream = typeof(StandardClassicGeneralPackage).Assembly.GetManifestResourceStream("CardGame.Content.Standard.SkillPrograms." + bundle + "." + suffix)!;
        using var reader = new StreamReader(stream); return reader.ReadToEnd();
    }
    private static string Peer(int seat) => "fixture:mobile-xu-you-peer-" + seat;
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int Costs() => G.CardMovements.Count(m => m.Reason.Value == "skill-program.alternating-suit.discard");
        public bool CanSlash(int target) => G.GetLegalActions(0).Any(a => a.Kind == LegalActionKind.Slash && a.CardId is not null &&
            a.TargetSeats.Contains(target) && V(0).Hand.Any(c => c.Id == a.CardId && c.Kind == CardKind.Slash));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat, seat);
        public void DiscardChoice() => Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "alternating-suit-top") == true);
        public void Use(string skill, string activation) => Accept(new UseProgramSkillCommand(0, skill, activation, [], [], G.Revision, P!.PromptId));
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Slash(int target)
        { var card = V(0).Hand.First(c => c.Kind == CardKind.Slash); Accept(new PlayCardCommand(0, card.Id, [target], G.Revision, P!.PromptId)); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done, int? playSeat = null)
        {
            for (var i = 0; i < 180 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p)
                { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer(p.PlayerSeat), G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification })
                { var response = P!; Accept(new AnswerPromptCommand(response.PlayerSeat, response.PromptId, response.Choices.First(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage").Id, G.Revision)); continue; }
                if (playSeat is { } target && P is { Kind: DecisionKind.PlayCard } otherPlay && otherPlay.PlayerSeat != target)
                { End(otherPlay.PlayerSeat); continue; }
                Need(P is null, "Unexpected native mobile Xu You boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The fixed actual-Play allowance scene missed its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the original paid discard, private native child and nullable issued expiry without repeating the cost.");
        }
        public void Need(bool condition, string message)
        { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(P)); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Xu You command."); }
        public static Scene Start(bool mobile, bool endingUse = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mobile, endingUse));
            var g = GameEngine.CreateStandard(new() { Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r);
            var scene = new Scene(g, r); scene.Accept(new StartGameCommand()); return scene;
        }
    }
    private sealed class Fixture(bool mobile, bool endingUse) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-xu-you", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var skill = mobile ? Mobile : Classic; CharacterFixtureContent.Register(b, "classic:xu-you");
            if (mobile) CharacterFixtureContent.Register(b, "mobile:xu-you");
            var endingTriggers = endingUse ? """
              ,{"id":"ending-hold","window":"turnEnding","subject":"owner","optional":false,"isSkillInvocation":false,"priority":100,
                "effects":[{"op":"chooseOption","target":"owner","resultBind":"ending","options":[{"id":"ending-continue"}]}]}
              """ : "";
            // Only Chenglue is held here; the exact shared Shicai/Cunmu flows already have focused coverage.
            var c = SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"triggers":[{"id":"first-inserted-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,
                "effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}{{endingTriggers}}],"activations":[
                {"id":"lose-source","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                 "effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{skill}}"],"sourceBind":"standard:none"}]}]},
              {"id":"{{Hold}}","revision":1,"triggers":[{"id":"real-discard-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perOwnerBatch",
                "movementReasons":["skill-program.alternating-suit.discard"],"optional":false,"isSkillInvocation":false,
                "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
              {"id":"{{Requester}}","revision":1,"triggers":[{"id":"ending-request","window":"turnEnding","subject":"owner","turnOwnerScope":"otherLiving","optional":false,"isSkillInvocation":false,"usageScope":"game","usageLimit":1,
                "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"ending-slash"}]}]}
            ]}
            """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> {
                    [Driver] = new { name = "原生阶段与失源", description = "真实插入Play及失去原成略来源。",
                        optionLabels = endingUse ? new Dictionary<string, string> { ["ending-continue"] = "继续结束阶段用牌" } : new Dictionary<string, string>() },
                    [Requester] = new { name = "结束阶段原生用杀请求", description = "对真实远距离角色使用实体杀。" },
                    [Hold] = new { name = "原生成略弃牌子窗", description = "两张材料已经弃置。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description)
                { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "真实成略许可持有者", "supporter", skill, "qun", 20, [Driver, Hold]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "真实原生杀目标", "supporter", "standard:none", "wei", 20,
                endingUse && seat == 2 ? [Requester] : []));
            b.AddDeck(new(Deck, "固定真实花色", 12, 0, []) { PhysicalCards = Enumerable.Range(0, 120)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "原阶段及实际回合花色权限", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1,
                [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
