using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhangXingCaiChecks
{
    private const string Hero = "mobile:zhang-xing-cai", Shen = "mobile:shenxian", Qiang = "mobile:qiangwu";
    private const string Owner = "fixture:mobile-zhang-xing-cai-owner", Peer = "fixture:mobile-zhang-xing-cai-peer-";
    private const string Watch = "fixture:mobile-zhang-xing-cai-watch", Extra = "fixture:mobile-zhang-xing-cai-extra";
    private const string Discards = "fixture:mobile-zhang-xing-cai-discards", Mode = "identity:classic-mobile-zhang-xing-cai";
    private const string DrawReason = "skill-program.mobile:shenxian.DrawFromOtherActualBasicDiscard";

    public static void UnlimitedSeparateActualBasicDiscardBatchesAndPaidDrawCold()
    {
        var s = Scene.Start(shen: true);
        var before = s.V.HandCount;
        s.End();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Shen);
        s.Activate();
        s.Reach(() => s.Watching("draw"));
        var first = s.E<OtherActualBasicDiscardDrawIssuedEvent>().Single();
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Shen);
        s.Need(first.FrameId == parent.Id && parent.InstructionIndex == 1 && s.V.HandCount == before + 1 &&
            parent.WindowContext?.MovementBatch is { } batch && batch.Id == first.MovementBatchId &&
            batch.Movements.Count > 1 && batch.Movements.All(m => m.CardKind == CardKind.Dodge &&
                m.From.OwnerSeat != 0 && m.To == CardLocation.DiscardPile),
            "A whole real multi-card Basic discard batch draws once, on the original already-paid observer frame.");
        s.Cold();
        s.Continue();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Shen);
        s.Need(s.E<OtherActualBasicDiscardDrawIssuedEvent>().Length == 1,
            "A second independent discard opportunity remains available during the same other actual turn.");
        s.Activate();
        s.Reach(() => s.Watching("draw"));
        var draws = s.E<OtherActualBasicDiscardDrawIssuedEvent>();
        s.Need(draws.Length == 2 && draws[0].ActualTurnNumber == draws[1].ActualTurnNumber &&
            draws[0].ActualTurnOwnerSeat == draws[1].ActualTurnOwnerSeat && draws[0].MovementBatchId != draws[1].MovementBatchId &&
            draws.All(e => e.Source.OwnerSeat == 0 && e.DiscardOwnerSeat != 0) && s.V.HandCount == before + 2,
            "Mobile Shenxian has no turn quota: two real discard batches each issue their own single native draw.");
        s.Continue();
        s.Reach(() => s.G.State.Phase == TurnPhase.Play && s.G.State.CurrentSeat != 0 && s.P?.SkillPrompt?.SkillId != Watch);
        s.Need(s.E<OtherActualBasicDiscardDrawIssuedEvent>().Length == 2 &&
            s.G.CardMovements.Count(m => m.Reason.Value == DrawReason) == 2,
            "Returning the paid gain child never repeats the first draw or treats multiple discarded Basics as separate batches.");
    }

    public static void QiangwuFrozenJudgmentNativeSlashAndActualPhaseExpiryCold()
    {
        var s = Scene.Start();
        var turn = s.G.State.TurnNumber;
        s.UseQiangwu();
        s.Reach(() => s.Watching("judgment"));
        var final = s.E<JudgmentResolvedEvent>().Single(e => e.Reason == Qiang);
        s.Need(final.Rank == 7 && s.E<TurnJudgedRankSlashPolicyGrantedEvent>().Length == 0,
            "The original real judgment observer finishes before a rank permission can be issued.");
        s.Cold(); s.Continue(); s.Play();
        var policy = s.E<TurnJudgedRankSlashPolicyGrantedEvent>().Single().Policy;
        s.Need(policy.PhaseInstanceId is > 0 && policy.JudgmentFrameId == final.ResolutionId && policy.Rank == 7 &&
            Enumerable.Range(0, 4).All(v => s.G.CreateSnapshot(v).Players[0].JudgedRankSlashThreshold is { Rank: 7 }) &&
            Enumerable.Range(0, 4).All(v => s.G.CreateSnapshot(v).Players[0].ActiveEffects?.Any(e =>
                e.SkillId == Qiang && e.Expiry == ActivePlayerEffectExpiry.PlayPhaseEnd) == true) &&
            !s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Qiang),
            "Mobile Qiangwu freezes the exact public judgment and issuing actual Play identity once per phase.");
        var low = s.V.Hand.First(c => c.Rank == 5).Id;
        var equal = s.V.Hand.Single(c => c.Rank == 7).Id;
        var high = s.V.Hand.First(c => c.Rank == 9).Id;
        s.Need(s.Slash(low, 2) is not null && s.Slash(equal, 2) is null && s.Slash(high, 2) is null,
            "Only rank below the real judgment bypasses distance; equality and above keep normal range.");
        s.PlaySlash(high, 1); s.Reach(() => s.Watching("slash"));
        var accepted = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.JudgedRankSlashUse is not null);
        s.Need(accepted.JudgedRankSlashUse is { IgnoresQuota: true, IgnoresDistance: false } receipt && receipt.Policy == policy &&
            s.E<CardUseDebitRecordedEvent>().Length == 0,
            "A genuine committed high-rank Slash owns its frozen quota receipt rather than a debit/refund approximation.");
        s.Cold(); s.Continue(); s.Play();
        s.PlaySlash(low, 2); s.Play();
        s.Need(s.E<CardUseDebitRecordedEvent>().Length == 1 && s.Slash(equal, 1) is null &&
            s.Slash(s.V.Hand.Single(c => c.Rank == 9).Id, 1) is not null &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardKind == CardKind.Slash) == 2,
            "The low-rank genuine use consumes the normal quota, while another above-threshold entity remains legal.");
        s.End(); s.Play();
        s.Need(s.G.State.TurnNumber == turn && s.E<TurnJudgedRankSlashPolicyGrantedEvent>().Length == 1 &&
            Enumerable.Range(0, 4).All(v => s.G.CreateSnapshot(v).Players[0].JudgedRankSlashThreshold is null) &&
            Enumerable.Range(0, 4).All(v => s.G.CreateSnapshot(v).Players[0].ActiveEffects?.Any(e => e.SkillId == Qiang) != true) &&
            s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Qiang) &&
            s.Slash(s.V.Hand.Single(c => c.Rank == 5).Id, 2) is null,
            "Returning from the real inserted Play to normal Play in the same turn expires distance and renews the activation allowance.");
        s.PlaySlash(equal, 1); s.Play();
        s.Need(s.Slash(s.V.Hand.Single(c => c.Rank == 9).Id, 1) is null &&
            s.E<JudgedRankSlashUsePolicyAppliedEvent>().Length == 2,
            "The next actual Play also loses the old high-rank quota exemption, without manufacturing another permission.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public T[] E<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Watching(string id) => P?.SkillPrompt?.SkillId == Watch && G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.TriggerId == id;
        public LegalAction? Slash(int card, int target) => G.GetHumanLegalActions().FirstOrDefault(a => a.Kind == LegalActionKind.Slash && a.CardId == card && a.TargetSeats.SequenceEqual([target]));
        public void PlaySlash(int card, int target)
        {
            var action = Slash(card, target); Need(action is not null, $"Missing genuine Slash entity {card} to {target}.");
            Submit(new PlayCardCommand(0, card, action!.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind));
        }
        public void UseQiangwu() => Submit(new UseProgramSkillCommand(0, Qiang, "judge-rank-slash-phase", [], [], G.Revision, P!.PromptId));
        public void End() => Submit(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        private void Answer(Func<PromptChoice, bool> pick)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(pick); Need(choice is not null, "Missing native prompt choice.");
            Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Mobile Zhang Xing Cai command rejected.");
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Reach(Func<bool> done)
        {
            for (var n = 0; n < 160; n++)
            {
                if (done()) return;
                var p = P;
                if (p is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral }) Submit(new SelectGeneralCommand(0, Owner, G.Revision, p.PromptId));
                else if (p is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards }) Submit(new DiscardCardsCommand(0, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, G.Revision));
                else if (p is { PlayerSeat: 0 } && p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue")) Continue();
                else if (p is { PlayerSeat: 0 } && p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip")) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The bounded native mobile Zhang Xing Cai scene did not reach its named boundary.");
        }
        private string Dump() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves all prepared views, original paid observer/judgment/use frames and issued scalar receipts.");
        }
        public void Need(bool ok, string reason)
        {
            if (!ok) throw new InvalidOperationException(reason + " " + JsonSerializer.Serialize(new
            {
                G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase,
                Pending = P is { } p ? new { p.Kind, p.PlayerSeat, SkillId = p.SkillPrompt?.SkillId } : null, Hand = V.Hand,
                Frames = G.ResolutionStack.Select(f => new
                {
                    f.Id, Kind = f.GetType().Name, Instruction = (f as ProgramSkillFrame)?.InstructionIndex,
                    SourceBatch = (f as ProgramSkillFrame)?.WindowContext?.MovementBatch?.Id,
                    SourceBatchCount = (f as ProgramSkillFrame)?.WindowContext?.MovementBatch?.Movements.Count
                }),
                Draws = E<OtherActualBasicDiscardDrawIssuedEvent>(), Policies = E<TurnJudgedRankSlashPolicyGrantedEvent>(),
                Uses = E<JudgedRankSlashUsePolicyAppliedEvent>(), Movements = G.CardMovements.TakeLast(16)
            }));
        }
        public static Scene Start(bool shen = false)
        {
            Scene Boot(IReadOnlyDictionary<int, int>? ranks)
            {
                var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(shen, ranks));
                var game = GameEngine.CreateStandard(new GameOptions
                {
                    Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
                    UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 6
                }, registry);
                var s = new Scene(registry, game); s.Submit(new StartGameCommand()); s.Play(); return s;
            }
            var probe = Boot(null);
            if (shen) return probe;
            // One fixed-seed deal determines recipe IDs; no seed search, mutations or filesystem fixture.
            probe.Need(probe.V.Hand.Count == 5, "The fixed recipe begins at the inserted Play before normal draw.");
            var ranks = new[] { 5, 7, 9, 9, 5 };
            var recipe = probe.V.Hand.Select((card, i) => (card.Id, Rank: ranks[i])).ToDictionary(p => p.Id, p => p.Rank);
            return Boot(recipe);
        }
    }

    private sealed class Fixture(bool shen, IReadOnlyDictionary<int, int>? ranks) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-zhang-xing-cai", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var definitions = new List<JsonNode>
            {
                JsonNode.Parse("""
                {"id":"fixture:mobile-zhang-xing-cai-watch","revision":1,"triggers":[
                  {"id":"draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.mobile:shenxian.DrawFromOtherActualBasicDiscard"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"draw-paid","options":[{"id":"continue"}]}]},
                  {"id":"judgment","window":"judgmentFinalized","subject":"owner","judgmentReasons":["mobile:qiangwu"],"suits":["heart"],"minimumRank":1,"maximumRank":13,"excludedReasons":[],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"judgment-seen","options":[{"id":"continue"}]}]},
                  {"id":"slash","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"slash-paid","options":[{"id":"continue"}]}]}
                ]}
                """)!,
                JsonNode.Parse("""
                {"id":"fixture:mobile-zhang-xing-cai-extra","revision":1,"triggers":[{"id":"one-real-extra-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]}
                """)!,
                JsonNode.Parse("""
                {"id":"fixture:mobile-zhang-xing-cai-discards","revision":1,"triggers":[{"id":"two-real-basic-discards","window":"playPhaseStarting","subject":"owner","optional":false,"effects":[
                  {"op":"selectOwnedCards","target":"owner","minimumCards":2,"maximumCards":2,"zones":["hand"],"resultBind":"first-two"},
                  {"op":"moveBoundCards","target":"owner","sourceBind":"first-two","destination":"discardPile","awaitMovementTriggers":true},
                  {"op":"selectOwnedCards","target":"owner","minimumCards":2,"maximumCards":2,"zones":["hand"],"resultBind":"second-two"},
                  {"op":"moveBoundCards","target":"owner","sourceBind":"second-two","destination":"discardPile","awaitMovementTriggers":true}
                ]}]}
                """)!
            };
            var labels = definitions.ToDictionary(n => n["id"]!.GetValue<string>(), n => (object)(n["id"]!.GetValue<string>() == Watch
                ? new Dictionary<string, object> { ["name"] = "真实子窗", ["description"] = "实际已支付的摸牌、判定与用牌", ["optionLabels"] = new Dictionary<string, string> { ["continue"] = "继续" } }
                : new Dictionary<string, object> { ["name"] = "实际阶段和弃置", ["description"] = "合法原生阶段与实体弃牌" }));
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddSkill(new("fixture:mobile-zhang-xing-cai-idle", "固定其他角色", "无额外行为"));
            builder.AddGeneral(new(Owner, "移动张星彩原生边界", "supporter", Shen, "shu", 12, shen ? [Qiang, Watch] : [Qiang, Watch, Extra], GeneralGender.Female));
            for (var i = 1; i < 4; i++) builder.AddGeneral(new(Peer + i, "固定真实对手", "supporter", shen ? Discards : "fixture:mobile-zhang-xing-cai-idle", "wei", 12, Gender: GeneralGender.Male));
            const string deck = "fixture:mobile-zhang-xing-cai-deck";
            builder.AddDeck(new(deck, "固定实体与真实判定", 5, 0, [])
            {
                PhysicalCards = Enumerable.Range(1, 64).Select(id => new ContentDeckPhysicalCard(shen ? "standard:dodge" : "standard:slash", Suit.Heart, ranks?.GetValueOrDefault(id, 7) ?? 7)).ToArray()
            });
            builder.AddMode(new(Mode, "移动张星彩实际边界", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Renegade)] = 3 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer + 1, Peer + 2, Peer + 3]));
        }
    }
}
