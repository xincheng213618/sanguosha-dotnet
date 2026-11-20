using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheHuxinFieldEquipmentChecks
{
    private const string Mirror = StandardShanheHuxinMirrorPackage.SkillId;
    private const string Owner = "fixture:huxin-field-owner", Peer = "fixture:huxin-field-peer";
    private const string Driver = "fixture:huxin-field-driver", Watch = "fixture:huxin-field-movement";
    private const string Deck = "fixture:huxin-field-deck", PaymentReason = "equipment.huxin-mirror.prevent";

    internal static void OriginalEquippedMirrorPaysOnceOrDeclinesCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheHuxinMirrorPackage(), new Fixture());
        var setup = new ShanheBattleSetup(63, 1, "huxin-field", Owner, 4, 0, 0, 0, 0, [], [],
            [new(CardKind.HuxinMirror, Suit.Diamond, 2)], [new ShanheCombatantDefinition(Peer, Hp: 6)],
            [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 2
        }, content);
        Submit(game, new StartGameCommand());
        var play = Reach(game, IsOpenPlay);
        var armor = View(game).Equipment.Single(card => card.Kind == CardKind.HuxinMirror);
        var installed = game.CardMovements.Single(move => move.CardId == armor.Id);
        Need(HasMirror(game) && installed.From == CardLocation.OutsideGame && installed.To == CardLocation.Equipment(0),
            "The official mirror starts as a physical equipped card with its active skill grant.");

        Submit(game, new UseProgramSkillCommand(0, Driver, "hurt-two", [], [], game.Revision, play.PromptId));
        var offer = Reach(game, (_, prompt) => prompt.SkillPrompt?.SkillId == Mirror);
        var damage = game.ResolutionStack.OfType<BeforeDamageProgramWindowFrame>().Single();
        Need(offer.PlayerSeat == 0 && damage is { Amount: 2, TargetSeat: 0, Prevented: false } &&
             View(game).Hp == 4 && HasMirror(game) && game.CardMovements.Last(move => move.CardId == armor.Id) == installed &&
             !Events<HuxinMirrorPaidEvent>(game).Any(),
            "The original finalized two-point damage offers its physical mirror before any payment.");

        var declined = Cold(game, content);
        var declineOffer = Reach(declined, (_, prompt) => prompt.SkillPrompt?.SkillId == Mirror);
        Need(declineOffer.PromptId == offer.PromptId, "Cold replay restores the same unpaid mirror offer.");
        Answer(declined, declineOffer, "program-action", "skip");
        Reach(declined, IsOpenPlay);
        var applied = Events<DamageAppliedEvent>(declined).Single();
        Need(applied is { TargetSeat: 0, Amount: 2 } && applied.SourceSeat == damage.SourceSeat &&
             View(declined).Hp == 2 && View(declined).Equipment.Single().Id == armor.Id && HasMirror(declined) &&
             declined.CardMovements.Last(move => move.CardId == armor.Id) == installed &&
             !Events<HuxinMirrorPaidEvent>(declined).Any() && !Events<ProgramDamagePreventedEvent>(declined).Any(),
            "Declining applies the original two damage and keeps the mirror and its grant.");

        Answer(game, offer, "program-action", "activate");
        var child = Reach(game, (_, prompt) => prompt.SkillPrompt?.SkillId == Watch);
        var paid = Events<HuxinMirrorPaidEvent>(game).Single();
        var move = game.CardMovements.Single(move => move.Sequence == paid.Receipt.MovementSequence);
        Need(paid.Receipt is { Source.OwnerSeat: 0, Source.SkillId: Mirror, Amount: 2 } &&
             paid.Receipt.Source.SkillInstanceId == "equipment:shanhe-huxin-mirror:" + armor.Id &&
             paid.Receipt.CardId == armor.Id && paid.Receipt.WindowFrameId == damage.Id &&
             paid.Receipt.AttackFrameId == (damage.ContinuationAttackResolutionId ?? damage.ParentFrameId) &&
             move.CardId == armor.Id && move.CardKind == CardKind.HuxinMirror &&
             move.From == CardLocation.Equipment(0) && move.To == CardLocation.DiscardPile && move.Reason.Value == PaymentReason,
            "Acceptance pays the exact equipped entity into discard on the original damage window and attack frame.");
        Need(View(game).Equipment.Count == 0 && !HasMirror(game) && View(game).Hp == 4 &&
             game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == paid.ProgramFrameId && frame.HuxinMirrorPayment == paid.Receipt),
            "The original paid frame suspends in its actual movement child after losing the equipment grant.");

        game = Cold(game, content);
        var restoredChild = Reach(game, (_, prompt) => prompt.SkillPrompt?.SkillId == Watch);
        Need(restoredChild.PromptId == child.PromptId && Events<HuxinMirrorPaidEvent>(game).Single() == paid &&
             game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == paid.ProgramFrameId && frame.HuxinMirrorPayment == paid.Receipt) &&
             View(game).Equipment.Count == 0 && !HasMirror(game),
            "Cold replay returns to the same already-paid child with the original receipt and grant revoked.");
        Answer(game, restoredChild, "option-id", "continue");
        Reach(game, IsOpenPlay);
        var prevented = Events<ProgramDamagePreventedEvent>(game).Single();
        Need(prevented is { SkillId: Mirror, TargetSeat: 0, Amount: 2 } && prevented.FrameId == damage.Id &&
             prevented.SourceSeat == (applied.SourceLess ? null : applied.SourceSeat) &&
             !Events<DamageAppliedEvent>(game).Any() && View(game).Hp == 4 &&
             Events<HuxinMirrorPaidEvent>(game).Single() == paid && View(game).HandCount == 1 &&
             game.CardMovements.Where(entry => entry.CardId == armor.Id && entry.Sequence > installed.Sequence).SequenceEqual([move]),
            "Returning from the movement child draws once and prevents the same two damage without repaying the mirror.");
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content) =>
        GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);

    private static PlayerSnapshot View(GameEngine game) => game.CreateSnapshot(0).Players.Single(player => player.Seat == 0);
    private static bool HasMirror(GameEngine game) => View(game).Skills!.Any(skill => skill.ContentId == Mirror);
    private static IEnumerable<T> Events<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static bool IsOpenPlay(GameEngine game, PendingDecision prompt) =>
        prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0;

    private static PendingDecision Reach(GameEngine game, Func<GameEngine, PendingDecision, bool> ready)
    {
        for (var step = 0; step < 40; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
            {
                if (ready(game, prompt))
                    return prompt;
                Need(prompt.Kind == DecisionKind.PlayCard && game.ResolutionStack.Count != 0,
                    $"Unexpected mirror fixture prompt {prompt.Kind} for seat {prompt.PlayerSeat} and skill {prompt.SkillPrompt?.SkillId}.");
            }
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The mirror fixture did not reach the expected native prompt.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, string key, string value)
    {
        var choice = prompt.Choices.Single(candidate => candidate.Parameters.GetValueOrDefault(key) == value);
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, game.Revision));
    }

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The mirror fixture command was rejected.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:huxin-field", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var skills = new object[]
            {
                new { id = Driver, revision = 1, activations = new[] { new { id = "hurt-two", minCards = 0, maxCards = 0,
                    minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "damage", target = "owner", amount = 2 } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "original-movement", window = "cardsMoved",
                    subject = "owner", sourceZones = new[] { "equipment" }, movementOccurrence = "perOwnerBatch",
                    movementReasons = new[] { PaymentReason }, optional = false, effects = new object[]
                    {
                        new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } },
                        new { op = "draw", target = "owner", amount = 1 }
                    } } } }
            };
            var labels = new Dictionary<string, object>
            {
                [Driver] = new { name = "两点伤害", description = "对自己造成两点伤害。" },
                [Watch] = new { name = "弃镜后的移动窗口", description = "原护心镜入堆后摸一张牌。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var programs = SkillProgramCatalog.Load(
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs)
                builder.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
                    { Program = program, ProgramPresentation = programs.Presentations[id] });
            builder.AddGeneral(new(Owner, "护心镜持有者", "supporter", Driver, "shu", 4, [Watch]));
            builder.AddGeneral(new(Peer, "对手", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "无自然起手的牌堆", 0, 0, [new("standard:dodge", 32)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "护心镜双人夹具", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
