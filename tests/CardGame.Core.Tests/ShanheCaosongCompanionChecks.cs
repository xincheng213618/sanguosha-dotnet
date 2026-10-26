using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheCaosongCompanionChecks
{
    private const string Huanghan = StandardShanheCaosongCompanionPackage.HuanghanSkillId;
    private const string Driver = "fixture:huanghan-driver", Owner = "fixture:huanghan-owner", Enemy = "fixture:huanghan-enemy";
    private const string Deck = "fixture:huanghan-deck";

    public static void NativeCardDamageDrawsByNameAndDiscardsLostHpCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
            new StandardShanheCaosongCompanionPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "huanghan", Owner, 4, 0, 0, 0, 0,
            [Huanghan], [], [], [new(Enemy) { StartingHand = [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.FireSlash, Suit.Diamond, 4)] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        Submit(new StartGameCommand()); Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        Use("hurt"); Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
        Need(game.State.Players[0].Hp == 3 && !Facts<ProgramBindingResolvedEvent>().Any(item => item.SkillId == Huanghan),
            "Direct native skill damage has no originating card and does not offer Huanghan.");
        var startHand = game.CreateSnapshot(0).Players[0].HandCount;
        foreach (var lostHp in new[] { 2, 3 })
        {
            var drawsBefore = game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program." + Huanghan + ".Draw");
            var discardBefore = Discards().Length;
            Use("request");
            Reach(() => Prompt() is { Kind: DecisionKind.RespondDodge } || AtHuanghan("activate"));
            if (Prompt() is { Kind: DecisionKind.RespondDodge })
            { Answer(choice => choice.Parameters.GetValueOrDefault("response") == "take-damage"); Reach(() => AtHuanghan("activate")); }
            var receipt = Facts<DamageAppliedEvent>().Last();
            var kind = Facts<CardUseDeclaredEvent>().Last(item => item.SourceSeat == 1).CardKind;
            Need(receipt.TargetSeat == 0 && receipt.Amount == 1 && game.State.Players[0].Hp == 4 - lostHp,
                "A real requested physical card injures the owner before the optional skill.");
            Cold(); Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => AtHuanghan("select-owned-cards"));
            var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Huanghan);
            Need(frame.OwnedCardSelection is { } selection && selection.RequiredCount == lostHp &&
                game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program." + Huanghan + ".Draw") - drawsBefore == CardCatalog.Get(kind).DisplayName.Length,
                "Huanghan draws the effective Chinese card-name length once, then requests the actual lost-HP count.");
            var frameId = frame.Id;
            Cold();
            while (game.ResolutionStack.OfType<ProgramSkillFrame>().Any(item => item.Id == frameId && item.OwnedCardSelection is not null))
            {
                Need(AtHuanghan("select-owned-cards"), "The original owner frame controls each real discard choice.");
                Answer(choice => choice.Cards.Count == 1);
                Cold();
                if (!AtHuanghan("select-owned-cards")) break;
            }
            Reach(() => Prompt() is { Kind: DecisionKind.PlayCard });
            Need(Discards().Length - discardBefore == lostHp &&
                Facts<ProgramBindingResolvedEvent>().Count(item => item.SkillId == Huanghan && item.Activated && item.Completed) == lostHp - 1,
                "Each damage occurrence owns one original draw/discard sequence; cold resumes never draw or pay twice.");
        }
        Need(game.CreateSnapshot(0).Players[0].HandCount == startHand - 2 &&
            Facts<CardUseDeclaredEvent>().Where(item => item.SourceSeat == 1).Select(item => item.CardKind).ToHashSet().SetEquals([CardKind.Slash, CardKind.FireSlash]),
            "Two same-turn activations apply the distinct one/two name lengths and two/three lost-HP discard counts.");

        var elite = registry.Generals[StandardShanheCaosongCompanionPackage.EliteId];
        Need(elite.BaseHp == 4 && elite.SkillIds.ToHashSet().SetEquals(["ol:lilu", "ol:yizheng-cao-song", Huanghan]) &&
            registry.Generals["ol:cao-song"].SkillIds.ToHashSet().SetEquals(["ol:lilu", "ol:yizheng-cao-song"]),
            "The elite companion adds Huanghan while the free original keeps only its two complete support skills.");
        return;

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        bool AtHuanghan(string action) => Prompt()?.SkillPrompt?.SkillId == Huanghan &&
            Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == action);
        T[] Facts<T>() => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        CardMovementRecord[] Discards() => game.CardMovements.Where(move => move.From.OwnerSeat == 0 && move.To == CardLocation.DiscardPile &&
            move.Reason.Value == "skill-program." + Huanghan + ".MoveBoundCards").ToArray();
        void Submit(GameCommand command)
        { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The native Huanghan command was rejected."); }
        void Answer(Func<PromptChoice, bool> predicate)
        { var prompt = Prompt()!; Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
        void Use(string activation) => Submit(new UseProgramSkillCommand(0, Driver, activation, [], activation == "request" ? [1] : [], game.Revision, Prompt()!.PromptId));
        void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 180; step++)
            {
                if (condition()) return;
                Need(Prompt()?.PlayerSeat != 0, "An unexpected human prompt interrupted the Huanghan fixture: " + JsonSerializer.Serialize(Prompt()));
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The expected Huanghan boundary was not reached.");
        }
        void Cold()
        {
            var before = SnapshotJson.Serialize(game.CreateSnapshot(0));
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == before, "Native cold recovery preserves the pending frame and private selection exactly.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-huanghan", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,"activations":[
                 {"id":"request","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                  "effects":[{"op":"requestSlashByTarget","target":"selectedTarget","resultBind":"requested"}]},
                 {"id":"hurt","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,
                  "effects":[{"op":"damage","target":"owner","amount":1}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "真实伤害驱动", description = "直接伤害及请求实体杀" } } }));
            builder.AddSkill(new(Driver, "真实伤害驱动", "实体伤害") { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "惶汗使用者", "", Driver, "wei", 4));
            builder.AddGeneral(new(Enemy, "持杀敌方", "", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "固定闪", 4, 2, [new("standard:dodge", 60)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "惶汗实战", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
