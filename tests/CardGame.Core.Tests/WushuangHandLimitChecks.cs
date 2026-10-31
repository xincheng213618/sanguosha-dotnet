using CardGame.Core;
using System.Reflection;
using System.Text.Json;

internal static partial class WushuangModeChecks
{
    public static void MaxHpBaseYieldsToRealJigongTurnDamageAndExpiresCold()
    {
        foreach (var tactic in new[] { 2166, 2211 })
        {
            var (game, registry) = Start(true, 26, skills: [$"wushuang:tactic-{tactic}", "classic:jigong"],
                initialHp: 20, aiQuota: 0);
            Drive(game, () => Shop(game) is { CanShop: true, Phase: WushuangShopPhase.OpeningTactic });
            Buy(game, Shop(game).Offers.Single(offer => offer.TacticId == 2002).Id);
            Drive(game, () => P(game)?.SkillPrompt?.SkillId == "classic:jigong");
            Require(game.CreateSnapshot(0).Players[0] is { Hp: 20, MaxHp: 30, HandLimit: 30 },
                "The tactic replaces the wounded owner's HP base with MaxHp before the real optional Jigong activation.");
            Cold(game, registry);
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            var prompt = P(game)!;
            Accept(game, new AnswerPromptCommand(0, prompt.PromptId,
                prompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate").Id, game.Revision));
            Play(game);
            Require(game.CreateSnapshot(0).Players[0].HandLimit == 0 &&
                E<ProgramDamageHandLimitChangedEvent>(game).Single() is { OwnerSeat: 0, DamageCount: 0 },
                "Jigong's actual current-turn Set overrides the MaxHp base with zero, without conflict or an extra MaxHp addition.");
            Cold(game, registry);
            game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            var action = game.GetHumanLegalActions().First(option => option.Kind == LegalActionKind.Slash &&
                option.ConversionSource is null && option.TargetSeat == 1);
            Accept(game, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, game.Revision, P(game)!.PromptId,
                action.PlayedCardKind)); Play(game);
            Require(E<DamageAppliedEvent>(game).Count(damage => damage.SourceSeat == 0 && damage.TargetSeat == 1 && damage.Amount == 1) == 1 &&
                game.CreateSnapshot(0).Players[0].HandLimit == 1 &&
                E<ProgramDamageHandLimitChangedEvent>(game).Last() is { OwnerSeat: 0, DamageCount: 1 },
                "A real native one-damage Slash changes the turn-specific hand limit to exactly one, preserving ordinary damage accounting.");
            Cold(game, registry);
            var ownerTurn = game.State.TurnNumber;
            End(game); Shopping(game);
            Accept(game, new FinishWushuangShopCommand(0, game.Revision));
            Drive(game, () => game.State.CurrentSeat == 1 && E<TurnStartedEvent>(game)
                .Any(turn => turn.ActorSeat == 1 && turn.TurnNumber > ownerTurn));
            var owner = ((IEnumerable<CharacterState>)global::FixtureRuntimeAccess.Engine.GetField("_players", BindingFlags.Instance | BindingFlags.NonPublic)!
                .GetValue(game)!).Single(player => player.Seat == 0);
            var actualLimit = (int)global::FixtureRuntimeAccess.Engine.GetMethod("GetHandLimit", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(game, [owner])!;
            var evaluation = (RuleQueryEvaluation)global::FixtureRuntimeAccess.Engine.GetMethod("EvaluateHandLimit", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(game, [owner, false])!;
            Require(E<TurnEndedEvent>(game).Count(turn => turn.ActorSeat == 0 && turn.TurnNumber == ownerTurn) == 1 &&
                game.CreateSnapshot(0).Players[0] is { HandLimit: null, MaxHp: 30 } && actualLimit == 30,
                "After the actual turn ends the temporary Jigong Set expires and the original MaxHp base remains; the off-turn public HandLimit is absent. " +
                JsonSerializer.Serialize(new { game.State.CurrentSeat, game.State.TurnNumber, OwnerTurn = ownerTurn,
                    owner.Hp, owner.MaxHp, ActualLimit = actualLimit, evaluation.BaseTerms, evaluation.Stages }));
            Cold(game, registry);
        }
    }
}
