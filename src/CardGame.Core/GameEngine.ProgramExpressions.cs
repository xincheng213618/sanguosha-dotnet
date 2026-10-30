namespace CardGame.Core;

public sealed record ProgramAmountEvaluatedEvent(long FrameId, string SkillId, string GameplayHash,
    int InstructionIndex, int Amount) : IGameEvent;
public sealed record ProgramExpressionNumberCapturedEvent(long FrameId, string SkillId, string GameplayHash,
    int InstructionIndex, string Name, int Value) : IGameEvent;

internal static class ProgramExpressionAmounts
{
    internal const string Prefix = "amount-expression-";
    internal static string Key(int index) => Prefix + index.ToString(System.Globalization.CultureInfo.InvariantCulture);
    internal static int IssuedAmount(ProgramSkillFrame frame) =>
        frame.NumberBindings.Where(binding => binding.Name == Key(frame.InstructionIndex)).ToArray() is [var issued] && issued.Value >= 0
            ? issued.Value : -1;
}

public sealed partial class GameEngine
{
    private int? EvaluateProgramAmountExpression(long frameId, ProgramExpression expression,
        SkillProgramUnknownAmountBehavior onUnknown = SkillProgramUnknownAmountBehavior.Skip)
    {
        var frame = GetActiveProgramFrame(frameId);
        var key = ProgramExpressionAmounts.Key(frame.InstructionIndex);
        if (frame.NumberBindings.Any(binding => binding.Name == key))
            throw new InvalidOperationException("An amount expression cannot be issued twice on the same instruction.");
        var amount = ProgramExpressions.EvaluateInt(expression, new ProgramExpressionWorld(this, frame));
        if (amount is null)
        {
            if (onUnknown == SkillProgramUnknownAmountBehavior.CancelBinding)
                CancelProgramBindingAndCleanup(frame, "体力值未定义，无法计算此效果。");
            return null;
        }
        amount = Math.Max(0, amount.Value);
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
                plan.Instructions[index - 1].AmountExpression is null || binding.Value < 0 ||
                ProgramEventHistory<ProgramAmountEvaluatedEvent>().Count(issued => issued.FrameId == frame.Id &&
                    issued.SkillId == frame.SkillId && issued.GameplayHash == frame.GameplayHash &&
                    issued.InstructionIndex == index && issued.Amount == binding.Value) != 1)
                throw new InvalidOperationException("An amount expression lost its exact issued quantity.");
        }
    }

    private const string ExpressionNumberPrefix = "expression-number-";
    private int? ReadProgramExpressionNumber(ProgramSkillFrame frame, string name) =>
        frame.NumberBindings.Where(binding => binding.Name == ExpressionNumberPrefix + name).ToArray() is [var value] ? value.Value : null;

    private void CaptureProgramExpressionNumber(long frameId, string name, int value)
    {
        var frame = GetActiveProgramFrame(frameId);
        if (frame.NumberBindings.Any(binding => binding.Name == ExpressionNumberPrefix + name))
            throw new InvalidOperationException("A captured expression number cannot be overwritten.");
        ReplaceRuntimeTop(frame with { NumberBindings = Array.AsReadOnly(frame.NumberBindings
            .Append(new ProgramSkillNumberBinding(ExpressionNumberPrefix + name, value)).ToArray()) });
        AdvanceEventRulesAndQueueFact(new ProgramExpressionNumberCapturedEvent(frame.Id, frame.SkillId,
            frame.GameplayHash, frame.InstructionIndex, name, value));
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 93003)]
    private void AssertCapturedExpressionNumbers(ProgramSkillFrame frame, ProgramExecutionPlan plan)
    {
        var bindings = frame.NumberBindings.Where(binding => binding.Name.StartsWith(ExpressionNumberPrefix, StringComparison.Ordinal)).ToArray();
        if (bindings.Select(binding => binding.Name).Distinct(StringComparer.Ordinal).Count() != bindings.Length)
            throw new InvalidOperationException("A captured expression number lost its unique name.");
        foreach (var binding in bindings)
        {
            var name = binding.Name[ExpressionNumberPrefix.Length..];
            var issued = ProgramEventHistory<ProgramExpressionNumberCapturedEvent>().Where(record => record.FrameId == frame.Id && record.Name == name).ToArray();
            if (issued is not [var record] || record.SkillId != frame.SkillId || record.GameplayHash != frame.GameplayHash ||
                record.Value != binding.Value || record.InstructionIndex < 1 || record.InstructionIndex > frame.InstructionIndex ||
                record.InstructionIndex > plan.Instructions.Count ||
                !ProgramOperationCatalog.Default.Resolve(plan.Instructions[record.InstructionIndex - 1].Op)
                    .Resources(plan.Instructions[record.InstructionIndex - 1]).OfType<CreateNumberBinding>().Any(produced => produced.Name == name))
                throw new InvalidOperationException("A captured expression number lost its exact producer receipt.");
        }
    }

    /// <summary>
    /// Current public seat facts for expression conditions, read at the moment the
    /// instruction runs. Participants come from the running frame only.
    /// </summary>
    private sealed class ProgramExpressionWorld(GameEngine engine, ProgramSkillFrame frame) : IProgramExpressionWorld
    {
        public int? Number(string name) => engine.ReadProgramExpressionNumber(frame, name);
        public int? BindingStat(string stat, string name)
        {
            if (frame.CardSetBindings.SingleOrDefault(binding => binding.Name == name) is not { } binding) return null;
            if (stat == "boundCount") return binding.CardIds.Count;
            if (binding.CardIds.Count == 0 && stat is "boundSuitCount" or "boundDistinctSuitCount") return 0;
            if (binding.Visibility != SkillProgramCardSetVisibility.Public && binding.SourceLocations.Any(location =>
                location.OwnerSeat != frame.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))) return null;
            var cards = binding.CardIds.Select(id => engine._cardZones.CardsAt(engine._cardZones.GetLocation(id)).Single(card => card.Id == id)).ToArray();
            return stat switch
            {
                "boundSuitCount" => (binding.FrozenSelectedSuits ?? cards.Select(card => card.Suit).ToArray()).Where(suit => suit != Suit.None).Distinct().Count(),
                "boundDistinctSuitCount" => (binding.FrozenSelectedSuits ?? cards.Select(card => card.Suit).ToArray()).Distinct().Count(),
                "boundRank" => cards is [var card] ? card.Rank : null,
                "boundIsEquipment" => cards is [var card] ? EquipmentCatalog.IsEquipment(card.Kind) ? 1 : 0 : null,
                _ => null
            };
        }
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
