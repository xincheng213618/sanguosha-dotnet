using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
internal sealed record ChainedStateBasicNeed(ChainedStateBasicIntent Intent, long? ParentFrameId,
        long? RequestFrameId, long? ParentActionId, int Cursor, int? FixedTargetSeat);
}

public sealed partial class SimpleAiBrain
{
    private (double Score, string Reason) ScoreChainedStateBasicAction(GameSnapshot view, PlayerSnapshot self, Role role, LegalAction action)
    {
        if (action.CardId != 0 || action.ChainedStateBasicUse != false || action.ConversionSource is null || action.PlayedCardKind is not { } kind ||
            !IsSlashCard(kind) || action.TargetSeats.Count == 0) return (-1000, "连环费用转换缺少真实合法用牌方向。");
        var pressure = action.TargetSeats.Select(s => view.Players.Single(p => p.Seat == s)).Sum(t => GetHostility(view, role, t) + (t.Hp <= 1 ? 28 : 0));
        return (CardCatalog.Get(kind).AiPlayValue + pressure + 2, "按公开目标与体力评估实际解除连环后的杀，不读取他人暗牌。");
    }
    internal (bool UseSlash, AiThoughtRecord Thought) ChooseChainedStateBasicTargetUse(GameSnapshot view, int targetSeat, int sequence)
    {
        var self = view.Players.Single(p => p.Seat == Seat); var target = view.Players.Single(p => p.Seat == targetSeat);
        var score = GetHostility(view, self.Role ?? Role.Renegade, target) * 1.2 + (target.Hp <= 1 ? 45 : 0) + 2;
        var use = score > 0; var text = use ? "解除连环状态继续使用杀。" : "保留当前连环状态，放弃追杀。";
        return (use, new(sequence, view.TurnNumber, Seat, text,
            [new(new LegalAction(LegalActionKind.Slash, 0, targetSeat, text, CardKind.Slash), score, "只依据公开关系与目标体力。")], text));
    }
}
