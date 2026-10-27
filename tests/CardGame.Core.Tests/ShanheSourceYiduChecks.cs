using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceYiduChecks
{
    private const string Source = StandardShanheSourceWuqueAssassinPackage.YiduSkillId;
    private const string Owner = "fixture:source-yidu-owner", Target = "fixture:source-yidu-target";
    private const string Watch = "fixture:source-yidu-discard-child", Deck = "fixture:source-yidu-deck";
    private const string Ol = "ol:yidu";

    public static void SoleTargetCompletesNativeRevealWhileOlRetainsMultipleTargets()
    {
        foreach (var (source, multiple) in new[] { (true, false), (true, true), (false, true) })
        {
            var scene = Scene.Start(source);
            var skill = source ? Source : Ol;
            var kind = multiple ? CardKind.ArrowBarrage : CardKind.Slash;
            var card = scene.View(0).Hand.Single(c => c.Kind == kind);
            var originals = scene.View(1).Hand.Select(c => c.Id).ToArray();
            var targets = multiple ? new[] { 1, 2, 3 } : [1];
            var legal = scene.Game.GetHumanLegalActions().Single(action => action.CardId == card.Id &&
                (multiple || action.TargetSeats.SequenceEqual(targets)));
            scene.Submit(new PlayCardCommand(0, card.Id, legal.TargetSeats, scene.Game.Revision, scene.P!.PromptId, kind));

            if (source && multiple)
            {
                scene.Play();
                var use = scene.Events<CardUseDeclaredEvent>().Single(e => e.CardId == card.Id);
                Need(scene.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.ResolutionId) == 1 &&
                    scene.Events<CardResponseCompletedEvent>().Count(e => e.ParentFrameId == use.ResolutionId &&
                        e.EffectiveKind == CardKind.Dodge) == 3 &&
                    scene.Events<DamageAppliedEvent>().Length == 0 &&
                    scene.Events<ProgramBindingStartedEvent>().All(e => e.SkillId != Source) &&
                    scene.Events<CompletedUndamagedTargetRevealStartedEvent>().Length == 0 &&
                    Enumerable.Range(1, 3).All(seat => scene.View(seat).HandCount == 3),
                    "Source3113 cannot borrow an undamaged participant of a real three-target Arrow Barrage; all three native Dodges and the original Use finish normally.");
                continue;
            }

            scene.Until(() => scene.P?.SkillPrompt?.SkillId == skill &&
                scene.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            var completedUse = scene.Game.ResolutionStack.OfType<CardUseFrame>().Single(use => use.CardId == card.Id);
            Need(completedUse.Step == ResolutionFrameStep.Completed && completedUse.TargetSeats.SequenceEqual(targets) &&
                scene.Events<DamageAppliedEvent>().Length == 0 &&
                scene.Events<CardResponseCompletedEvent>().Count(e => e.ParentFrameId == completedUse.Id &&
                    e.EffectiveKind == CardKind.Dodge) == targets.Length,
                "The real physical damage card completes its own native response tree before either version may reveal an undamaged target.");
            scene.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            scene.Until(() => scene.P?.SkillPrompt?.SkillId == skill &&
                scene.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "target"));
            Need(scene.P!.Choices.SelectMany(c => c.Targets).SequenceEqual(targets),
                "Source3113 exposes its sole actual target; unchanged OL Yidu still offers all three undamaged actual targets.");
            scene.Answer(c => c.Targets.SequenceEqual([1]));
            for (var slot = 0; slot < 3; slot++)
            {
                scene.Until(() => scene.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "slot") == true);
                var chosenSlot = slot.ToString(System.Globalization.CultureInfo.InvariantCulture);
                scene.Answer(c => c.Parameters.GetValueOrDefault("branch") == "slot" &&
                    c.Parameters.GetValueOrDefault("slot-index") == chosenSlot);
            }
            scene.Until(() => scene.P?.SkillPrompt?.SkillId == Watch);
            var start = scene.Events<CompletedUndamagedTargetRevealStartedEvent>().Single();
            var reveal = scene.Events<ProgramCardsRevealedEvent>().Single(e => e.FrameId == start.FrameId);
            var expectedReason = $"skill-program.{skill}.completed-undamaged-target.discard";
            var moved = scene.Game.CardMovements.Where(m => m.From == CardLocation.Hand(1) &&
                m.To == CardLocation.DiscardPile && m.Reason.Value == expectedReason).ToArray();
            Need(start.Source.SkillId == skill && start.Source.OwnerSeat == 0 &&
                start.CardUseFrameId == completedUse.Id && start.CardActionId == completedUse.Action!.ActionId &&
                reveal.Cards.Count == 3 && reveal.Cards.All(c => originals.Contains(c.Id) && c.Suit == Suit.Heart) &&
                scene.Events<NativeLogicalDiscardInvocationRecordedEvent>().Count(e => e.ParentFrameId == start.FrameId &&
                    e.InitiatorSeat == 0 && e.ExecutorSeat == (source ? 1 : 0) && e.SubjectSeat == 1 &&
                    e.ActualCount == 3 && e.HandCount == 3 && e.Reason.Value == expectedReason) == 1 &&
                moved.Length == 3 && moved.Select(m => m.CardId).Distinct().Count() == 3 &&
                moved.Select(m => m.CardId).SequenceEqual(reveal.Cards.Select(c => c.Id)) &&
                scene.View(1).HandCount == 0 && !scene.View(1).IsHuman &&
                scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == start.FrameId &&
                    f.CompletedUndamagedTargetReveal is { MovementIssued: true } && f.PendingMovementContinuation is not null) &&
                !scene.Events<CompletedUndamagedTargetRevealCompletedEvent>().Any(e => e.FrameId == start.FrameId),
                "The original observer publicly reveals three original same-color entities, discards each once and waits on the actual target's discard child before completing.");
            var childId = scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Watch).Id;
            scene.Submit(new AdvanceOneStepCommand(scene.Game.Revision));
            scene.Play();
            Need(scene.Events<CompletedUndamagedTargetRevealCompletedEvent>() is
                    [{ FrameId: var finished, SelectedCount: 3, ActualDiscardCount: 3 }] && finished == start.FrameId &&
                scene.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == start.FrameId && e.Completed) == 1 &&
                scene.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == childId && e.Completed) == 1 &&
                scene.Events<ProgramOptionChosenEvent>().Count(e => e.FrameId == childId && e.SkillId == Watch &&
                    e.OwnerSeat == 1 && e.ChooserSeat == 1 && e.OptionId == "continue") == 1 &&
                scene.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == completedUse.Id) == 1 &&
                scene.Game.CardMovements.Count(m => m.CardId == card.Id && m.From == CardLocation.Hand(0) &&
                    m.To == CardLocation.Processing) == 1 &&
                scene.Game.CardMovements.Count(m => m.CardId == card.Id && m.From == CardLocation.Processing &&
                    m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.UseFinished) == 1 &&
                !scene.Game.ResolutionStack.Any(f => f.Id == start.FrameId || f.Id == completedUse.Id),
                "The paid discard child returns to the same original completed Use, without repeating its materials, reveal, discard or completion.");
        }
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => Game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected source3113 native command."); }
        public void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = P!;
            Need(prompt.PlayerSeat == 0, "Only the actual human submits external source3113 choices.");
            Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(pick).Id, Game.Revision));
        }
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 160; step++)
            {
                if (done()) return;
                Need(P is null || !View(P.PlayerSeat).IsHuman,
                    "Unexpected source3113 native boundary: " + JsonSerializer.Serialize(P));
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Source3113 did not reach its bounded native boundary.");
        }
        public static Scene Start(bool source)
        {
            var opponents = Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition(Target)
            { StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 2), 4).ToArray() }).ToArray();
            var setup = new ShanheBattleSetup(83, 1, "source-yidu-native", Owner, 8, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.ArrowBarrage, Suit.Spade, 1)],
                [], opponents, [], new string('a', 64));
            var production = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            {
                PlayerGeneralId = StandardShanheSourceWuqueAssassinPackage.HardId, Hp = 3,
                EnemyRoster = Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition("classic:guan-yu")).ToArray()
            });
            var registry = production.WithExtension(new Fixture(source), mode => mode.Id == ShanheBattleSetup.BattleModeId
                ? mode with { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Owner, Target]).Distinct(StringComparer.Ordinal).ToArray() }
                : mode);
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2
            }, registry);
            var scene = new Scene(game); scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }

    private sealed class Fixture(bool source) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-yidu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var reason = $"skill-program.{(source ? Source : Ol)}.completed-undamaged-target.discard";
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Watch}}}","revision":1,"triggers":[
                    {"id":"discard-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],
                     "movementOccurrence":"perOwnerBatch","movementReasons":["{{{reason}}}"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "真实遗毒弃牌孩子", description = "完成后返回原遗毒",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Owner, "真实遗毒持有者", "supporter", source ? Source : Ol, "qun", 8));
            builder.AddGeneral(new(Target, "同色实体手牌目标", "supporter", Watch, "wei", 8));
            builder.AddCard(new("fixture:source-yidu-arrows", "万箭齐发", "锦囊牌", "真实多目标响应", LegacyKind: CardKind.ArrowBarrage));
            builder.AddDeck(new(Deck, "零随机起手和摸牌", 0, 0, [new("standard:slash", 20), new("standard:dodge", 20),
                new("fixture:source-yidu-arrows", 4)]));
        }
    }
}
