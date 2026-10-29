namespace CardGame.Core;

public sealed partial class GameEngine
{
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
                "hp" => player.HasHpValue ? player.Hp : null,
                "lostHp" => player.HasHpValue ? player.MaxHp - player.Hp : null,
                "maxHp" => player.MaxHp,
                "handCount" => engine.GetHand(player).Count,
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
