using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature
{
    public string StartLog(ModePresentationContext context) =>
        _mode.Id == WushuangMode.ModeId ? $"单骑无双开始，本局随机先手为 {context.CurrentPlayerName}（{context.CurrentSeat + 1:00} 号位）。" :
        _mode.ModeKind == ContentModeKind.Survival ? $"{context.PlayerCount}人文和乱武开始，随机{context.CurrentSeat + 1}号位先行动，独自存活者获胜。" :
        _mode.ModeKind == ContentModeKind.NationalWarLite ? $"{context.PlayerCount}人国战简化局开始，1号位先行动。" :
        _mode.ModeKind == ContentModeKind.Team ? $"{context.PlayerCount}人公开阵营局开始，{context.FirstTeamName}先行动。" :
        $"{context.PlayerCount}人身份局开始，主公先行动。";

    public string RulesText(ModePresentationContext context) =>
        _mode.Id == WushuangMode.ModeId
            ? "单骑无双本地版：单机1v1对战AI，先手4张后手5张初牌；开局战法三选一，每个自然回合开始双方各得1虎符，新一轮另得1虎符；回合结束进入虎符商店，刷新花1虎符，支持锁定；额外武将技能最多两个，战法不占技能槽；15分钟到时按本地计分结算"
            : _mode.ModeKind == ContentModeKind.Survival
                ? "八名角色独立求生；按死亡顺序排名，最后存活者获胜，击杀获得体力上限与摸牌奖励"
                : _mode.ModeKind == ContentModeKind.NationalWarLite
                    ? $"{context.FactionSummary}；每名角色选择两名同势力武将，势力和武将默认暗置，明置后公开；消灭其他势力获胜"
                    : _mode.ModeKind == ContentModeKind.Team
                        ? $"{context.TeamSummary}；阵营身份和队友关系公开，击败另一阵营即获胜，{context.FirstTeamName}的首位座位先行动"
                        : context.RoleSummary;

    public string WinnerLog(ModeWinnerPresentationContext context)
    {
        var winnerName = _mode.GuanduActivity is not null && context.WinningTeamName is { } army ? army :
            _mode.DragonBoatActivity is not null && context.WinningTeamId is { Length: >= 5 } boat
                ? $"{context.FactionNames.FirstOrDefault(faction => faction.Id == boat[5..])?.Name ?? boat[5..]}队" :
            _mode.ModeKind == ContentModeKind.Survival && context.WinningPlayerName is { } champion ? champion :
            _mode.ModeKind == ContentModeKind.NationalWarLite
                ? context.WinningFactionName is { } faction ? $"{faction}势力" : "未知势力"
                : context.DefaultWinnerName;
        return context.Winner == Winner.Draw ? "游戏结束：本局平局。" : $"游戏结束：{winnerName}获胜。 ";
    }
}
