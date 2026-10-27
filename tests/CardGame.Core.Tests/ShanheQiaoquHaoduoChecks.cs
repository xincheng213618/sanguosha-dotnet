using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheQiaoquHaoduoChecks
{
    private const string Owner = "fixture:qiaoqu-owner", Slasher = "fixture:qiaoqu-slasher";
    private const string Peer = "fixture:qiaoqu-peer", Deck = "fixture:qiaoqu-deck";

    public static void BorrowedSwordKeepsTrickOwnerBonusOnPhysicalSlashAndOneChainPropagation()
    {
        foreach (var borrowerHasTactic in new[] { true, false })
        {
            var scene = Scene.Start(borrowerHasTactic);
            scene.Play(CardKind.IronChain, [2, 3]);
            var fireId = scene.Game.CreateSnapshot(1).Players[1].Hand.Single().Id;
            scene.Play(CardKind.BorrowedSword, [1, 2]);

            var parent = scene.Events<CardUseDeclaredEvent>().Single(fact => fact.CardKind == CardKind.BorrowedSword);
            var child = scene.Events<CardUseDeclaredEvent>().Single(fact => fact.CardKind == CardKind.FireSlash);
            var parentAction = scene.Events<CardUseAppearanceCapturedEvent>().Select(fact => fact.Action)
                .Single(action => action.Type == CardActionType.Use && action.EffectiveKind == CardKind.BorrowedSword);
            var childAction = scene.Events<CardUseAppearanceCapturedEvent>().Select(fact => fact.Action)
                .Single(action => action.Type == CardActionType.Use && action.EffectiveKind == CardKind.FireSlash);
            var damage = borrowerHasTactic ? 2 : 1;
            Need(parent.SourceSeat == 0 && child.SourceSeat == 1 && child.CardId == fireId &&
                parentAction.ActorSeat == 0 && childAction.ActorSeat == 1 && childAction.ProviderSeat == 1 &&
                childAction.RequesterSeat is null && childAction.ParentActionId == parentAction.ActionId &&
                childAction.PhysicalCards is [{ CardId: var paid, From: var from }] && paid == fireId && from == CardLocation.Hand(1),
                "The original trick user stays distinct from its real forced Slash actor/provider; direct card supply has no requester and retains the original parent action.");
            Need(scene.Events<DamageAppliedEvent>().Select(fact =>
                    (fact.SourceSeat, fact.TargetSeat, fact.Amount, fact.Nature, fact.SourceCardUseResolutionId))
                    .SequenceEqual([(1, 2, damage, DamageNature.Fire, (long?)child.ResolutionId),
                                    (1, 3, damage, DamageNature.Fire, (long?)child.ResolutionId)]) &&
                scene.Events<ChainedDamagePropagatedEvent>() is [{ SourceSeat: 1, FromSeat: 2, TargetSeat: 3 } chain] && chain.Amount == damage,
                "Only the original borrower's tactic increases the physical Fire Slash; its actual source remains the slasher and chain inherits the final amount exactly once.");
            Need(scene.Events<BorrowedSwordResolvedEvent>() is
                    [{ ResolutionId: var resolved, SourceSeat: 0, WeaponOwnerSeat: 1, SlashTargetSeat: 2, UsedSlash: true, SlashCardId: var used }] &&
                resolved == parent.ResolutionId && used == fireId &&
                scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == child.ResolutionId) == 1 &&
                scene.Events<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == parent.ResolutionId) == 1 &&
                scene.Game.CardMovements.Count(move => move.CardId == fireId && move.From == CardLocation.Hand(1) &&
                    move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1,
                "The exact physical Slash is paid and completed once before its own Borrowed Sword parent finishes.");
            CheckModifierOwner(scene, borrowerHasTactic, child.ResolutionId);

            scene.Play(CardKind.Slash, [3]);
            var independent = scene.Events<CardUseDeclaredEvent>().Last();
            Need(independent is { CardKind: CardKind.Slash, SourceSeat: 0 } &&
                scene.Events<DamageAppliedEvent>().Last() is { SourceSeat: 0, TargetSeat: 3, Amount: 1, Nature: DamageNature.Normal } hit &&
                hit.SourceCardUseResolutionId == independent.ResolutionId && scene.Events<DamageAppliedEvent>().Length == 3 &&
                scene.Events<CardUseAppearanceCapturedEvent>().Last(fact => fact.Action.Type == CardActionType.Use).Action.ParentActionId is null,
                "A later independent physical Slash receives no leftover bonus, even when its user owns Qiaoqu Haoduo.");
            CheckModifierOwner(scene, borrowerHasTactic, child.ResolutionId);
        }
    }

    private static void CheckModifierOwner(Scene scene, bool hasTactic, long childUseId)
    {
        var modifiers = scene.Events<ProgramCardDamageModifiedEvent>()
            .Where(fact => fact.Source.SkillId == StandardShanheQiaoquHaoduoPackage.SkillId).ToArray();
        Need(modifiers.Length == (hasTactic ? 1 : 0) && modifiers.All(fact =>
                fact.ResolutionId == childUseId && fact.Source.OwnerSeat == 0 && fact.TargetSeat == 2 &&
                fact.BaseAmount == 1 && fact.ModifiedAmount == 2),
            "One native card-damage fact belongs to the original trick owner's exact forced Slash; neither chain nor unrelated uses issue it again.");
    }

    private static void Need(bool value, string message)
    { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        private void Submit(GameCommand command)
        {
            var result = Game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "Qiaoqu Haoduo native command rejected.");
        }
        private void ReachPlay()
        {
            for (var step = 0; step < 160; step++)
            {
                if (Game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && Game.ResolutionStack.Count == 0) return;
                Need(Game.State.Winner == Winner.None, "The bounded borrowed-Sword fixture must keep every participant alive.");
                if (Game.PendingDecision is { PlayerSeat: 0 } prompt && prompt.Kind != DecisionKind.PlayCard)
                {
                    var choice = prompt.Choices.FirstOrDefault(candidate => candidate.Parameters.GetValueOrDefault("response") is
                        "pass" or "take-damage" or "let-die") ?? prompt.Choices[0];
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Qiaoqu Haoduo did not return from its bounded native card-use tree.");
        }
        public void Play(CardKind kind, int[] targets)
        {
            var card = Game.CreateSnapshot(0).Players[0].Hand.Single(item => item.Kind == kind);
            var action = Game.GetHumanLegalActions().Single(candidate => candidate.CardId == card.Id &&
                candidate.ConversionSource is null && candidate.TargetSeats.SequenceEqual(targets));
            Submit(new PlayCardCommand(0, card.Id, targets, Game.Revision, Game.PendingDecision!.PromptId,
                action.PlayedCardKind, action.TargetCardId));
            ReachPlay();
        }
        public static Scene Start(bool borrowerHasTactic)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheQiaoquHaoduoPackage(), new Fixture());
            var setup = new ShanheBattleSetup(79, 1, "fixture:qiaoqu", Owner, 30, 0, 0, 0, 0, [],
                [new(CardKind.IronChain, Suit.Club, 6), new(CardKind.BorrowedSword, Suit.Club, 12), new(CardKind.Slash, Suit.Spade, 7)], [],
                [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
                [new(Slasher)
                {
                    StartingHand = [new(CardKind.FireSlash, Suit.Heart, 10)],
                    StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 1)],
                    ExtraSkillIds = borrowerHasTactic ? [] : [StandardShanheQiaoquHaoduoPackage.SkillId]
                }], new string('e', 64))
            { TacticSkillIds = borrowerHasTactic ? [StandardShanheQiaoquHaoduoPackage.SkillId] : [] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry);
            var scene = new Scene(game); scene.Submit(new StartGameCommand()); scene.ReachPlay(); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:qiaoqu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            // An ungranted observer enables the existing whole-catalog native
            // appearance facts without changing any participant's behavior.
            const string appearanceObserver = "fixture:qiaoqu-appearance-observer";
            var observations = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{appearanceObserver}}","revision":1,"triggers":[
                    {"id":"appearance","window":"cardUseCompleted","ownerRelation":"actor","optional":false,
                     "condition":{"kind":"cardActionSuitIs","suits":["heart"]},
                     "effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{
                  "{{appearanceObserver}}":{"name":"原生外观记录","description":"记录真实用牌外观。"} } }
                """);
            builder.AddSkill(new(appearanceObserver, "原生外观记录", "记录真实用牌外观。")
            {
                Program = observations.Programs[appearanceObserver],
                ProgramPresentation = observations.Presentations[appearanceObserver]
            });
            var borrowedSword = CardCatalog.Get(CardKind.BorrowedSword);
            builder.AddCard(new("fixture:qiaoqu-borrowed-sword", borrowedSword.DisplayName,
                borrowedSword.CategoryName, borrowedSword.Description, borrowedSword.Kind));
            builder.AddGeneral(new(Owner, "借刀者", "supporter", "standard:none", "shu", 30));
            builder.AddGeneral(new(Slasher, "持刀队友", "supporter", "standard:none", "shu", 30));
            builder.AddGeneral(new(Peer, "敌军", "supporter", "standard:none", "wei", 30));
            builder.AddDeck(new(Deck, "固定零摸牌", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "巧取豪夺实战", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Slasher, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
