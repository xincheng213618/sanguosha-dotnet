using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlZhongyunRequiredTargetChecks
{
    private const string Hero = "ol:wei-guan", Zhongyun = "ol:zhongyun";
    private const string Peer = "fixture:zhongyun-peer", Deck = "fixture:zhongyun-deck";

    internal static void HandLossKeepsEquipmentOnlyDiscardOption()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        foreach (var hasEquipment in new[] { true, false })
        {
            var peer = new ShanheCombatantDefinition(Peer)
            {
                StartingHand = [],
                StartingEquipment = hasEquipment ? [new(CardKind.QinggangSword, Suit.Spade, 5)] : []
            };
            var setup = new ShanheBattleSetup(83, 1, "zhongyun-hand-loss", Hero, 2, 0, 0, 0, 0, [],
                [new(CardKind.Crossbow, Suit.Club, 1), new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Heart, 3)],
                [], [peer], [], new string('b', 64));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
            }, registry);

            PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
            void Submit(GameCommand command)
            {
                var result = game.Submit(command);
                Need(result.Accepted, result.Error?.Message ?? "The Zhongyun fixture command was rejected.");
            }
            PendingDecision Reach(Func<PendingDecision, bool> ready)
            {
                for (var step = 0; step < 80 && game.State.Winner == Winner.None; step++)
                {
                    if (Prompt() is { } prompt)
                    {
                        Need(prompt.PlayerSeat == 0 && ready(prompt), "Zhongyun reached an unexpected human choice.");
                        return prompt;
                    }
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
                throw new InvalidOperationException("Zhongyun did not reach its expected choice.");
            }

            Submit(new StartGameCommand());
            var play = Reach(prompt => prompt.Kind == DecisionKind.PlayCard);
            var players = game.CreateSnapshot(0).Players;
            Need(players[0] is { Hp: 2, HandCount: 3 } && players[1] is { IsAlive: true, HandCount: 0 } &&
                players[1].Equipment.Count == (hasEquipment ? 1 : 0),
                "Both actual Play scenes differ only in the empty-handed opponent's starting equipment.");
            var weapon = players[0].Hand.Single(card => card.Kind == CardKind.Crossbow);
            var equip = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Equip && action.CardId == weapon.Id);
            Submit(new PlayCardCommand(0, weapon.Id, equip.TargetSeats, game.Revision, play.PromptId));

            var menu = Reach(prompt => prompt.SkillPrompt?.SkillId == Zhongyun &&
                prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "choose-option"));
            var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Zhongyun);
            var paid = game.CardMovements.Single(move => move.CardId == weapon.Id && move.Reason == CardMoveReasons.EquipmentUse);
            var expectedOptions = hasEquipment ? new[] { "draw", "discard" } : new[] { "draw" };
            Need(frame.TriggerId == "equal-hand-hp-after-hand-loss" &&
                frame.WindowContext is { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } batch } &&
                paid.From == CardLocation.Hand(0) && paid.To == CardLocation.Processing && batch.Movements.Contains(paid) &&
                game.CreateSnapshot(0).Players[0] is { Hp: 2, HandCount: 2 } && menu.IsPrivate &&
                menu.Choices.Select(choice => choice.Parameters.GetValueOrDefault("option-id")).SequenceEqual(expectedOptions),
                "The real equipment payment makes hand count equal HP. Discard is offered only when the empty-handed peer has equipment.");

            if (hasEquipment)
            {
                var discard = menu.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "discard");
                Submit(new AnswerPromptCommand(0, menu.PromptId, discard.Id, game.Revision));
                var target = Reach(prompt => prompt.SkillPrompt?.SkillId == Zhongyun &&
                    prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-target"));
                Need(target.IsPrivate && target.ValidTargetSeats.SequenceEqual([1]) &&
                    target.Choices is [var only] && only.Targets.SequenceEqual([1]) &&
                    game.ResolutionStack.OfType<ProgramSkillFrame>().Single(item => item.SkillId == Zhongyun).Id == frame.Id,
                    "Choosing discard offers the original equipment-only opponent on the same Zhongyun frame.");
            }
        }
    }

    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-zhongyun-required-target", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            builder.AddGeneral(new(Peer, "固定弃牌目标", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "忠允固定牌堆", 0, 0, [new("standard:dodge", 30)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "忠允实际手牌移动", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
