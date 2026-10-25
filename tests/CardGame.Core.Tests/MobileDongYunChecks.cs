using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileDongYunChecks
{
    private const string Mobile = "mobile:sheyan", Ol = "ol:sheyan", Driver = "fixture:mobile-dong-yun-driver",
        Watch = "fixture:mobile-dong-yun-completed", Owner = "fixture:mobile-dong-yun-owner", Mode = "identity:classic-mobile-dong-yun";

    public static void RealRemovalChangesNativeTargetsAndPreservesOlDefaultCold()
    {
        foreach (var mobile in new[] { true, false })
        {
            var s = Scene.Start(CardKind.ArrowBarrage, mobile); var hp = Enumerable.Range(0, 4).Select(i => s.V(i).Hp).ToArray();
            s.Use(CardKind.ArrowBarrage, [1, 2, 3]); s.Adjust(1);
            var useId = s.UseFrame.Id; var actionId = s.UseFrame.Action!.ActionId;
            s.Need(s.UseFrame.TargetSeats.SequenceEqual([1, 2, 3]) && s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == (mobile ? "remove" : "ineffective") && c.Targets.SequenceEqual([2])),
                "The genuine multi-target native trick offers actual removal only under the mobile policy and keeps the old OL ineffective branch.");
            s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == (mobile ? "remove" : "ineffective") && c.Targets.SequenceEqual([2]));
            s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 });
            var use = s.Native(useId); var expected = mobile ? new[] { 1, 3 } : new[] { 1, 2, 3 };
            s.Need(use.Action!.ActionId == actionId && use.TargetSeats.SequenceEqual(expected) && use.Action.TargetSeats.SequenceEqual(expected) && use.Action.EffectiveDesignatedTargetSeats.SequenceEqual(expected) &&
                (mobile ? use.IneffectiveTargetSeats?.Contains(2) != true : use.IneffectiveTargetSeats?.Contains(2) == true),
                "Mobile updates both native target lists and the current Action, while OL retains its original target and uses its existing ineffective record.");
            s.Cold(); s.Answer(c => c.Cards.Count == 0);
            s.Reach(() => s.P is { Kind: DecisionKind.RespondDodge } p && p.PlayerSeat is 2 or 3);
            s.Need(s.P!.PlayerSeat == 3, "The removed or ineffective seat2 receives no actual Dodge demand; the native trick continues to seat3.");
            s.Answer(c => c.Cards.Count == 0); s.Completed();
            var fact = s.E<TargetedOrdinaryTrickResolvedEvent>().Single(); var completed = s.Native(useId);
            s.Need(s.V(1).Hp == hp[1] - 1 && s.V(2).Hp == hp[2] && s.V(3).Hp == hp[3] - 1 && completed.TargetSeats.SequenceEqual(expected) &&
                completed.Action!.ActionId == actionId && completed.Action.TargetSeats.SequenceEqual(expected) &&
                fact.ResultTargets.SequenceEqual(expected) && fact.RemovedTarget == (mobile ? 2 : null) && fact.IneffectiveTarget == (mobile ? null : 2) &&
                (!mobile || !s.E<CardEffectSkippedEvent>().Any(e => e.ResolutionId == useId && e.TargetSeat == 2)),
                "The entire native multi-target effect follows the real reduced population, and its scalar formal fact distinguishes removal from old OL invalidation after cold return.");
            s.Cold();
        }
    }

    public static void SingleTargetCannotBeRemovedAndMobileAdditionIgnoresDistanceCold()
    {
        foreach (var mobile in new[] { true, false })
        {
            var s = Scene.Start(CardKind.Snatch, mobile); var hand1 = s.V(1).HandCount; var hand2 = s.V(2).HandCount;
            s.Need(s.G.GetCombatDistance(0, 2) == 2, "The fixed native source and opposite seat2 are genuinely outside normal Snatch distance.");
            s.Use(CardKind.Snatch, [1]); s.Adjust(1); var useId = s.UseFrame.Id;
            s.Need(s.P!.Choices.All(c => c.Parameters.GetValueOrDefault("branch") is not ("remove" or "ineffective")) &&
                s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "add" && c.Targets.SequenceEqual([2])) == mobile,
                "Neither policy can remove its sole target; only current mobile Sheyan may add the legal opposite Snatch target without applying distance1.");
            s.Cold(); s.Answer(c => mobile ? c.Parameters.GetValueOrDefault("branch") == "add" && c.Targets.SequenceEqual([2]) : c.Parameters.GetValueOrDefault("branch") == "decline");
            s.Completed(); var expected = mobile ? new[] { 1, 2 } : new[] { 1 };
            s.Need(s.Native(useId).TargetSeats.SequenceEqual(expected) && s.Native(useId).Action!.TargetSeats.SequenceEqual(expected) &&
                s.V(1).HandCount == hand1 - 1 && s.V(2).HandCount == hand2 - (mobile ? 1 : 0),
                "The distance-free mobile added target undergoes its own real Snatch effect; the legacy OL default leaves that remote participant untouched.");
            s.Cold();
        }
    }

    public static void BorrowedSwordRemovesOnlyTheChosenPrimaryPairCold()
    {
        var s = Scene.Start(CardKind.BorrowedSword, mobile: true, secondOwner: true);
        s.Equip(1); s.Equip(2); var weapon1 = s.V(1).Equipment.Single().Id; var weapon2 = s.V(2).Equipment.Single().Id;
        s.Use(CardKind.BorrowedSword, [1, 2]); s.Adjust(1); var useId = s.UseFrame.Id;
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "add" && c.Targets.SequenceEqual([2, 3]));
        s.Adjust(2);
        s.Need(s.UseFrame.TargetSeats.SequenceEqual([1, 2, 2, 3]) && s.UseFrame.Action!.EffectiveDesignatedTargetSeats.SequenceEqual([1, 2]) &&
            s.P!.Choices.Count(c => c.Parameters.GetValueOrDefault("branch") == "remove") == 2,
            "The actual Borrowed Sword contains two ordered primary/victim pairs, including seat2 as both an earlier victim and a later primary.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "remove" && c.Targets.SequenceEqual([2]));
        var reduced = s.Native(useId);
        s.Need(reduced.TargetSeats.SequenceEqual([1, 2]) && reduced.Action!.TargetSeats.SequenceEqual([1, 2]) && reduced.Action.EffectiveDesignatedTargetSeats.SequenceEqual([1]) &&
            s.E<TargetedOrdinaryTrickResolvedEvent>().Last() is { RemovedTarget: 2 } removal && removal.BeforeTargets.SequenceEqual([1, 2, 2, 3]) && removal.ResultTargets.SequenceEqual([1, 2]),
            "Removing primary2 removes only its own pair [2,3], preserving its independent role as primary1's Slash victim and keeping one complete primary target.");
        s.Cold(); s.Completed();
        s.Need(s.V(1).Equipment.All(c => c.Id != weapon1) && s.V(0).Hand.Any(c => c.Id == weapon1) && s.V(2).Equipment.Any(c => c.Id == weapon2) &&
            s.Native(useId).TargetSeats.SequenceEqual([1, 2]),
            "Only the retained native Borrowed Sword pair resolves its actual no-Slash weapon transfer; the removed pair never requests a Slash or takes seat2's weapon.");
        s.Cold();
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry, CardKind kind)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardUseFrame UseFrame => G.ResolutionStack.OfType<CardUseFrame>().Last(f => f.CardKind == kind);
        public CardUseFrame Native(long id) => G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == id);
        public void Adjust(int seat)
        {
            Reach(() => P?.SkillPrompt?.SkillId is Mobile or Ol && P.PlayerSeat == seat && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P?.PlayerSeat == seat && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "targeted-ordinary-trick"));
        }
        public void Completed() => Reach(() => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        public void Use(CardKind played, IReadOnlyList<int> targets)
        {
            Play(); var action = G.GetLegalActions(0).First(a => a.PlayedCardKind == played && a.TargetSeats.SequenceEqual(targets) && a.ConversionSource?.SkillId == Driver);
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void Equip(int seat) { Play(); Accept(new UseProgramSkillCommand(0, Driver, "equip", [], [seat], G.Revision, P!.PromptId)); Play(); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual mobile Dong Yun command."); }
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 240 && !ready(); step++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } discard) { Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision)); continue; }
                if (P is { Kind: DecisionKind.PlayCard } playing) throw new InvalidOperationException("The native Dong Yun trick returned to Play before its requested boundary: " + playing.PlayerSeat);
                if (P is { } prompt)
                {
                    var choice = prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        prompt.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "take-damage" or "pass" or "decline") ??
                        prompt.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? prompt.Choices.First();
                    Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded mobile Dong Yun native boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))) && G.CardMovements.SequenceEqual(copy.CardMovements),
                "Cold replay preserves the original actual trick, current Action, exact ordered targets and original target-adjustment receipt."); G = copy;
        }
        public void Need(bool ok, string text) { if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { P, Turn = G.State.TurnNumber, Current = G.State.CurrentSeat })); }
        public static Scene Start(CardKind kind, bool mobile, bool secondOwner = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(kind, mobile, secondOwner));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry), registry, kind);
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-dong-yun-peer-" + seat;
    private sealed class Fixture(CardKind kind, bool mobile, bool secondOwner) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-dong-yun", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:dong-yun", "ol:dong-yun");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"viewAs":[{"id":"native-trick","inputKinds":[],"inputSuits":[],"outputKind":"{{JsonNamingPolicy.CamelCase.ConvertName(kind.ToString())}}","forPlay":true,"forResponse":false,"singleCardTrickUse":true}],
                "activations":[{"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"weapon"}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-trick-completed","window":"cardUseCompleted","ownerRelation":"actor","cardCategories":["instantTrick"],"optional":false,
                "effects":[{"op":"chooseOption","target":"owner","resultBind":"native-completed-pause","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var names = new Dictionary<string, object> { [Driver] = new { name = "原生锦囊实体", description = "真实实体转换与装备付款。" },
                [Watch] = new { name = "原生锦囊完成", description = "完整原生效果后的实际完成窗口。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "原生锦囊使用者", "supporter", Driver, "wei", 6, [Watch]));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "实际舍宴目标", "supporter", seat == 1 || seat == 2 && secondOwner ? mobile ? Mobile : Ol : "standard:none", "shu", 6));
            const string deck = "fixture:mobile-dong-yun-deck";
            var cards = Enumerable.Range(0, 100).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2))
                .Concat(kind == CardKind.BorrowedSword ? Enumerable.Range(0, 12).Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Spade, 1)) : []);
            b.AddDeck(new(deck, "固定真实锦囊材料与武器", 4, 0, []) { PhysicalCards = cards.ToArray() });
            b.AddMode(new(Mode, "移动董允真实目标", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
