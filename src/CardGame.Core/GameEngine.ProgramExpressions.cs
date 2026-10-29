namespace CardGame.Core;

public sealed record ProgramAmountEvaluatedEvent(long FrameId, string SkillId, string GameplayHash,
    int InstructionIndex, int Amount) : IGameEvent;

internal static class ProgramExpressionAmounts
{
    internal const string Prefix = "amount-expression-";
    internal static string Key(int index) => Prefix + index.ToString(System.Globalization.CultureInfo.InvariantCulture);
    internal static int IssuedAmount(ProgramSkillFrame frame) =>
        frame.NumberBindings.Where(binding => binding.Name == Key(frame.InstructionIndex)).ToArray() is [var issued] && issued.Value > 0
            ? issued.Value : -1;
}

public sealed partial class GameEngine
{
    private int? EvaluateProgramAmountExpression(long frameId, ProgramExpression expression)
    {
        var frame = GetActiveProgramFrame(frameId);
        var key = ProgramExpressionAmounts.Key(frame.InstructionIndex);
        if (frame.NumberBindings.Any(binding => binding.Name == key))
            throw new InvalidOperationException("An amount expression cannot be issued twice on the same instruction.");
        var amount = ProgramExpressions.EvaluateInt(expression, new ProgramExpressionWorld(this, frame));
        if (amount is null or <= 0) return amount is null ? null : 0;
        ReplaceRuntimeTop(frame with { NumberBindings = frame.NumberBindings.Append(new ProgramSkillNumberBinding(key, amount.Value)).ToArray() });
        AdvanceEventRulesAndQueueFact(new ProgramAmountEvaluatedEvent(frame.Id, frame.SkillId, frame.GameplayHash,
            frame.InstructionIndex, amount.Value));
        return amount;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 93002)]
    private void AssertProgramAmountExpressions(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        var bindings = frame.NumberBindings.Where(binding => binding.Name.StartsWith(ProgramExpressionAmounts.Prefix, StringComparison.Ordinal)).ToArray();
        if (bindings.Select(binding => binding.Name).Distinct(StringComparer.Ordinal).Count() != bindings.Length)
            throw new InvalidOperationException("An amount expression lost its unique instruction receipt.");
        foreach (var binding in bindings)
        {
            if (!int.TryParse(binding.Name.AsSpan(ProgramExpressionAmounts.Prefix.Length),
                    System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var index) ||
                binding.Name != ProgramExpressionAmounts.Key(index) || index < 1 || index > frame.InstructionIndex || index > plan.Instructions.Count ||
                plan.Instructions[index - 1].AmountExpression is null || binding.Value <= 0 ||
                ProgramEventHistory<ProgramAmountEvaluatedEvent>().Count(issued => issued.FrameId == frame.Id &&
                    issued.SkillId == frame.SkillId && issued.GameplayHash == frame.GameplayHash &&
                    issued.InstructionIndex == index && issued.Amount == binding.Value) != 1)
                throw new InvalidOperationException("An amount expression lost its exact issued quantity.");
        }
    }

    /// <summary>
    /// Current public seat facts for expression conditions, read at the moment the
    /// instruction runs. Participants come from the running frame only.
    /// </summary>
    private sealed class ProgramExpressionWorld(GameEngine engine, ProgramSkillFrame frame) : IProgramExpressionWorld
    {
        public int? Participant(string name) => Valid(name switch
        {
            "owner" => frame.OwnerSeat,
            "selectedTarget" => frame.SelectedTargetSeats is [var seat] ? seat : null,
            "eventSource" => frame.WindowContext?.SourceSeat,
            "eventTarget" => frame.WindowContext?.TargetSeat,
            _ => null
        });

        public IReadOnlyList<int>? ParticipantSet(string name) => name switch
        {
            "living" => engine._players.Where(player => player.IsAlive).Select(player => player.Seat).ToArray(),
            "livingOthers" => engine._players.Where(player => player.IsAlive && player.Seat != frame.OwnerSeat)
                .Select(player => player.Seat).ToArray(),
            _ => null
        };

        public int? Stat(string stat, int seat)
        {
            var player = engine._players[seat];
            return stat switch
            {
                "hp" => engine.UnambiguousHp(player),
                "lostHp" => engine.UnambiguousHp(player) is { } hp ? player.MaxHp - hp : null,
                "maxHp" => player.MaxHp,
                "handCount" => engine.GetHand(player).Count,
                "handLimit" => engine.GetHandLimit(player),
                "equipmentCount" => engine.GetEquipment(player).Count,
                _ => null
            };
        }

        public bool? Flag(string flag, int seat)
        {
            var player = engine._players[seat];
            return flag switch
            {
                "alive" => player.IsAlive,
                "hasHp" => player.HasHpValue,
                "faceDown" => player.IsFaceDown,
                "chained" => player.IsChained,
                _ => null
            };
        }

        private int? Valid(int? seat) => seat is { } value && engine.IsValidPlayerSeat(value) ? value : null;
    }
}
