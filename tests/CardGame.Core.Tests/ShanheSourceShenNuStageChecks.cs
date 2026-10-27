using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceShenNuStageChecks
{
    private const string Driver = "fixture:source-shennu-stage-driver", Deck = "fixture:source-shennu-stage-deck";
    private const string PriorChange = "fixture:source-shennu-prior-change";
    private const int BossSeat = 1;

    public static void ActualSource2687EntersItsTwentyHpShenNuStageWithCumulativeGear()
    {
        foreach (var difficulty in new[] { 16, 21 })
        {
            var changedFirstForm = difficulty == 21;
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var region = StandardShanheSourceRouteCatalog.Season5.ByRegionId[14];
            var node = source.Nodes.Single(candidate => candidate.Id == "source-city-5148");
            var encounter = node.Encounters!.Single(candidate => candidate.SourceBattleId == 2687);
            var stage = encounter.BossTransition!;
            Need(region.MinimumSourceDifficultyId == 16 && region.CityIds.Contains(5148) && node.ChapterNumber == 3 &&
                source.SourceDifficultyId == difficulty && source.SourceEnemyGrowthHash == StandardShanheSourceEnemyGrowth.Season5.ContentHash &&
                encounter.SourceCityId == 5148 && encounter.Enemies is [{ GeneralId: StandardShanheS5GaichaoPackage.DongZhuoGaichaoId }] &&
                stage is { GeneralId: StandardShanheSourceShenNuStagePackage.GeneralId, Hp: 20, MaxHp: 20, DrawCards: 4 } &&
                stage.Combatant is { Hp: 20, MaxHpBonus: 0, DrawBonus: 0, InitialHandBonus: 0,
                    SourceDifficultyAttributes: [{ DifficultyGroupId: 5, Attributes.ExtraSlash: 1, Attributes.ArmorGain: 1 }] } &&
                (stage.StartingEquipment ?? []).Select(Face).SequenceEqual(new[]
                    { (CardKind.ShuraInfernoHalberd, Suit.Heart, 12), (CardKind.LionBelt, Suit.Club, 2) }),
                "The actually available high-tier source city offers2687→268701 with its own20/20 row, retained source-growth metadata and cumulative Shura/Lion faces.");

            // This small raw battle preserves source rows but deliberately
            // does not install the campaign's day/chapter/count growth layer.
            // Its actual entry values must remain10/15 and20/20 respectively.
            var setup = new ShanheBattleSetup(71, 1, "fixture:source-shennu-stage-" + difficulty,
                "classic:guan-yu", 34, 30, 0, 0, 0, [Driver], [new(CardKind.Dodge, Suit.Club, 2)], [],
                encounter.Enemies.Select(member => member with
                {
                    StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7), 4).ToArray(),
                    ExtraSkillIds = changedFirstForm ? [.. (member.ExtraSkillIds ?? []), PriorChange] : member.ExtraSkillIds
                }).ToArray(), [],
                ShanheCampaignContent.GetContentHash(source))
            { SourceDifficultyId = difficulty, BossTransition = stage };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            {
                ExtraSkillIds = [],
                EnemyRoster = setup.EnemyRoster.Select(member => member with
                { ExtraSkillIds = (member.ExtraSkillIds ?? []).Where(skill => skill != PriorChange).ToArray() }).ToArray()
            });
            var phaseBody = registry.Generals[stage.GeneralId];
            Need(phaseBody is { BaseHp: 20, InitialHp: 20 } &&
                new[] { phaseBody.SkillId }.OfType<string>().Concat(phaseBody.AdditionalSkillIds ?? []).Order()
                    .SequenceEqual(new[] { StandardShanheSourceShenNuStagePackage.SkillId, "boundary:benghuai-current" }.Order()) &&
                registry.Skills[StandardShanheSourceShenNuStagePackage.SkillId].Program is not null &&
                stage.StartingEquipment!.All(card => registry.Cards.Values.Any(definition => definition.LegacyKind == card.Kind)),
                "The production factory registers the exact two-skill source body, its real Shennu program and both physical equipment kinds.");

            var scene = new Scene(setup, registry.WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode));
            scene.Submit(new StartGameCommand());
            scene.Reach(() => scene.Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && scene.Game.ResolutionStack.Count == 0);
            Need(scene.Boss is { GeneralId: StandardShanheS5GaichaoPackage.DongZhuoGaichaoId, Hp: 10, MaxHp: 15, HandCount: 4 } &&
                new[] { StandardShanheS5BossPackage.JiuchiId, StandardShanheS5BossPackage.RoulinId,
                    StandardShanheS5BossPackage.DaoshuId }
                    .All(skillId => scene.Boss.Skills!.Any(skill => skill.ContentId == skillId)) &&
                scene.Boss.Skills!.Any(skill => skill.ContentId == "boundary:benghuai-current") &&
                scene.Boss.Skills!.All(skill => skill.ContentId != StandardShanheCampaignCatalog.S5ZaiqiSkillId) &&
                scene.Boss.Skills!.All(skill => skill.ContentId != StandardShanheSourceShenNuStagePackage.SkillId),
                "The real raw first form keeps10/15, Benghuai and four physical opening cards before either case acts, without premature Zaiqi or Shennu.");
            var oldBenghuai = scene.Grants.Single(grant => grant.SkillId == "boundary:benghuai-current" &&
                grant.SourceId == CharacterState.PrimarySkillSource);
            SkillGrant? acquiredZaiqi = null;
            if (changedFirstForm)
            {
                var firstTurn = scene.Game.State.TurnNumber;
                scene.Submit(new EndPlayPhaseCommand(0, scene.Game.Revision, scene.Prompt!.PromptId));
                scene.Reach(() => scene.Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                    scene.Game.ResolutionStack.Count == 0 && scene.Game.State.TurnNumber > firstTurn);
                Need(scene.Boss.Skills!.All(skill => skill.ContentId != "boundary:benghuai-current" && skill.ContentId != PriorChange) &&
                    scene.Boss.Skills!.Any(skill => skill.ContentId == StandardShanheCampaignCatalog.S5ZaiqiSkillId) &&
                    scene.Grants.All(grant => grant.GrantId != oldBenghuai.GrantId),
                    "The NPC must actually execute its one legal prior activation before the human's next Play; its self-removal prevents a second stage-time execution.");
                acquiredZaiqi = scene.Grants.Single(grant => grant.SkillId == StandardShanheCampaignCatalog.S5ZaiqiSkillId &&
                    grant.SourceId == "acquired:" + PriorChange);
                Need(acquiredZaiqi.IsEnabled && acquiredZaiqi.SkillInstanceId == acquiredZaiqi.GrantId &&
                    acquiredZaiqi.TurnExpiry is null && acquiredZaiqi.PhaseExpiry is null && acquiredZaiqi.NextActualTurnStartExpiry is null,
                    "The real pre-stage acquisition owns an enabled independent source and persistent instance rather than a printed or temporary skill.");
            }
            Need(scene.Events<ProgramOwnerSkillsReplacedEvent>().Length == (changedFirstForm ? 1 : 0) &&
                (!changedFirstForm || scene.Events<ProgramOwnerSkillsReplacedEvent>() is
                    [{ SkillId: PriorChange, OwnerSeat: BossSeat, GrantedSkillId: StandardShanheCampaignCatalog.S5ZaiqiSkillId,
                        LostSkillIds: ["boundary:benghuai-current", PriorChange] }]),
                "The prior-source case really executes the existing shared skill mutation on its owner; it never edits grants or masquerades as an unexecuted wine Slash.");

            var before = scene.Game.CardMovements.Select(move => move.Sequence).DefaultIfEmpty(0).Max();
            var turn = scene.Game.State.TurnNumber;
            var round = scene.Events<RoundStartedEvent>().Last().RoundNumber;
            var earlierSlashes = scene.Slashes().Length;
            var carriedZaiqi = scene.Grants.Where(grant => grant.SkillId == StandardShanheCampaignCatalog.S5ZaiqiSkillId).ToArray();
            scene.Submit(new UseProgramSkillCommand(0, Driver, "stage", [], [BossSeat], scene.Game.Revision, scene.Prompt!.PromptId));
            scene.Reach(() => scene.Events<ShanheBossStageStartedEvent>().Length == 1 && scene.Game.State.CurrentSeat == BossSeat &&
                scene.Game.State.Phase == TurnPhase.Play && scene.Game.ResolutionStack.Count == 0);
            var stageGrants = scene.Grants;
            var stageDiagnostic = $"tier={difficulty}; changed={changedFirstForm}; " +
                $"boss={scene.Boss.GeneralId}/{scene.Boss.Hp}/{scene.Boss.MaxHp}/{scene.Boss.IsAlive}; " +
                $"skills=[{string.Join(',', scene.Boss.Skills!.Select(skill => skill.ContentId))}]; " +
                $"turnBefore={turn}; turnAfter={scene.Game.State.TurnNumber}; roundBefore={round}; " +
                $"roundAfter={scene.Events<RoundStartedEvent>().Last().RoundNumber}; " +
                $"stages={JsonSerializer.Serialize(scene.Events<ShanheBossStageStartedEvent>())}; " +
                $"interrupts={JsonSerializer.Serialize(scene.Events<ShanheBossTurnInterruptedEvent>())}; " +
                $"turns={JsonSerializer.Serialize(scene.Events<TurnStartedEvent>())}; " +
                $"replacements={JsonSerializer.Serialize(scene.Events<ProgramOwnerSkillsReplacedEvent>())}; " +
                $"grants={JsonSerializer.Serialize(stageGrants)}";
            Need(scene.Events<DamageAppliedEvent>().Any(fact => fact.SourceSeat == 0 && fact.TargetSeat == BossSeat && fact.Amount == 20) &&
                scene.Events<ShanheBossStageStartedEvent>() is [{ PreviousGeneralId: StandardShanheS5GaichaoPackage.DongZhuoGaichaoId,
                    GeneralId: StandardShanheSourceShenNuStagePackage.GeneralId, Hp: 20, MaxHp: 20 }] &&
                scene.Boss is { GeneralId: StandardShanheSourceShenNuStagePackage.GeneralId, Hp: 20, MaxHp: 20, IsAlive: true },
                "The real lethal attempt must enter the exact living20/20 phase once: " + stageDiagnostic);
            Need(
                scene.Boss.Skills!.Any(skill => skill.ContentId == StandardShanheSourceShenNuStagePackage.SkillId) &&
                scene.Boss.Skills!.All(skill => skill.ContentId != StandardShanheS5BossPackage.JiuchiId &&
                    skill.ContentId != StandardShanheS5BossPackage.RoulinId && skill.ContentId != StandardShanheS5BossPackage.DaoshuId),
                "The phase must enable its own Shennu and remove the obsolete first-form printed skills: " + stageDiagnostic);
            Need(scene.Boss.Skills!.Any(skill => skill.ContentId == "boundary:benghuai-current") &&
                stageGrants.Count(grant => grant.IsEnabled && grant.SkillId == "boundary:benghuai-current" &&
                    grant.SourceId == CharacterState.PrimarySkillSource && grant.GrantId == oldBenghuai.GrantId &&
                    grant.SkillInstanceId == oldBenghuai.SkillInstanceId) == 1 &&
                stageGrants.Count(grant => grant.IsEnabled && grant.SkillId == StandardShanheSourceShenNuStagePackage.SkillId &&
                    grant.SourceId == CharacterState.PrimarySkillSource) == 1 &&
                scene.Boss.Skills!.Any(skill => skill.ContentId == StandardShanheCampaignCatalog.S5ZaiqiSkillId) == changedFirstForm &&
                RetainsZaiqiSources(stageGrants, carriedZaiqi) &&
                (acquiredZaiqi is null || stageGrants.Any(grant => grant.GrantId == acquiredZaiqi.GrantId &&
                    grant.SourceId == acquiredZaiqi.SourceId && grant.SkillInstanceId == acquiredZaiqi.SkillInstanceId && grant == acquiredZaiqi)),
                "The new template binds its own Shennu+Benghuai while preserving the exact independently acquired Zaiqi source/instance without duplicating it: " + stageDiagnostic);
            Need(scene.Events<ShanheBossTurnInterruptedEvent>() is [{ PlayerSeat: 0, BossSeat: BossSeat }] &&
                scene.Events<TurnStartedEvent>().Last().ActorSeat == BossSeat && scene.Game.State.TurnNumber > turn &&
                scene.Events<RoundStartedEvent>().Last().RoundNumber == round,
                "The phase must retire the human turn and resume the boss natural turn in the same round: " + stageDiagnostic);
            Need(scene.Boss.Equipment.Select(card => (card.Kind, card.Suit, card.Rank)).Order()
                    .SequenceEqual(stage.StartingEquipment!.Select(Face).Order()) &&
                scene.Boss.Equipment.All(card => scene.Game.CardMovements.Count(move => move.Sequence > before && move.CardId == card.Id &&
                    move.From == CardLocation.OutsideGame && move.To == CardLocation.Equipment(BossSeat)) == 1),
                "The real phase installs both cumulative equipment entities into native slots exactly once.");

            scene.Reach(() => scene.Slashes().Length > earlierSlashes && scene.Slashes().Skip(earlierSlashes).All(use =>
                scene.Events<CardUseFinishedEvent>().Any(fact => fact.ResolutionId == use.ResolutionId)) && scene.Game.ResolutionStack.Count == 0);
            var slash = scene.Slashes()[earlierSlashes];
            Need(scene.Game.CardMovements.Count(move => move.CardId == slash.CardId && move.From == CardLocation.Hand(BossSeat) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
                scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == slash.ResolutionId) == 1 &&
                scene.Events<ProgramOwnerSkillsReplacedEvent>().Length == (changedFirstForm ? 1 : 0) &&
                RetainsZaiqiSources(scene.Grants, carriedZaiqi) &&
                scene.Events<ShanheBossStageStartedEvent>().Length == 1 && scene.Game.State.Winner == Winner.None,
                "The actual high-tier source body uses a paid physical Slash and completes it once without another transformation or a completed-battle shortcut.");
        }
    }

    private static bool RetainsZaiqiSources(IReadOnlyList<SkillGrant> actual, IReadOnlyList<SkillGrant> expected) =>
        actual.Count(grant => grant.SkillId == StandardShanheCampaignCatalog.S5ZaiqiSkillId) == expected.Count &&
        expected.All(previous => actual.Any(grant => grant == previous && grant.GrantId == previous.GrantId &&
            grant.SourceId == previous.SourceId && grant.SkillInstanceId == previous.SkillInstanceId && grant.IsEnabled));
    private static (CardKind Kind, Suit Suit, int Rank) Face(ShanheStartingCard card) => (card.Kind, card.Suit, card.Rank);
    private sealed class Scene(ShanheBattleSetup setup, ContentRegistry registry)
    {
        private static readonly FieldInfo PlayersField = typeof(GameEngine).GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!;
        public GameEngine Game { get; } = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 6
        }, registry);
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot Boss => Game.CreateSnapshot(BossSeat).Players[BossSeat];
        // Trusted read-only host audit: capture the real immutable grant records,
        // without changing CharacterState, issuing grants or exposing a new view.
        public SkillGrant[] Grants => ((IReadOnlyList<CharacterState>)PlayersField.GetValue(Game)!)[BossSeat].SkillGrants.Grants.ToArray();
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public CardUseDeclaredEvent[] Slashes() => Events<CardUseDeclaredEvent>()
            .Where(fact => fact.SourceSeat == BossSeat && fact.CardKind == CardKind.Slash).ToArray();
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 480; step++)
            {
                if (done()) return;
                Need(Game.State.Winner == Winner.None, "The bounded Shennu stage fixture ended before its native assertion boundary.");
                if (Prompt is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard && Game.ResolutionStack.Count != 0)
                    { Submit(new AdvanceOneStepCommand(Game.Revision)); continue; }
                    var pass = prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0 &&
                        choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(pass is not null, $"Unexpected Shennu stage decision: {prompt.Kind}/{prompt.SkillPrompt?.SkillId}.");
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, pass!.Id, Game.Revision));
                    continue;
                }
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException($"Shennu stage exceeded its bound: {Prompt?.Kind}/{Game.State.CurrentSeat}/{Game.State.Phase}.");
        }
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Shennu-stage command."); }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-shennu-stage", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var programs = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[
                    {"id":"stage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                     "effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]},
                  {"id":"{{PriorChange}}","revision":1,"activations":[
                    {"id":"prior-shared-replacement","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,
                     "targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"draw","target":"owner","amount":1},
                       {"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["boundary:benghuai-current","{{PriorChange}}"],
                        "sourceBind":"{{StandardShanheCampaignCatalog.S5ZaiqiSkillId}}"}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Driver] = new { name = "阶段攻击", description = "真实致死伤害触发篡天恶魂。" },
                        [PriorChange] = new { name = "既有换技", description = "以原生共享技能变更建立阶段前的再起来源。" }
                    } }));
            builder.AddSkill(new(Driver, "阶段攻击", "真实致死伤害触发篡天恶魂。")
            { Program = programs.Programs[Driver], ProgramPresentation = programs.Presentations[Driver] });
            builder.AddSkill(new(PriorChange, "既有换技", "以原生共享技能变更建立阶段前的再起来源。")
            { Program = programs.Programs[PriorChange], ProgramPresentation = programs.Presentations[PriorChange] });
            builder.AddDeck(new(Deck, "固定实体杀", 0, 2, [new("standard:slash", 120)]));
        }
    }
    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
