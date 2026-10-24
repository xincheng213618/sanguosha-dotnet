using System.Collections.ObjectModel;
using System.Globalization;
using CardGame.Core;

namespace CardGame.Modes.Standard;

internal sealed partial class StandardModeSetupFeature : IMatchModeSetupPresentation
{
    public PromptChoice GeneralChoice(ModeGeneralSelectionFacts player, ModeGeneralChoiceFacts general, bool fullRoster = false)
    {
        var factionSuffix = Rules.Visibility.UsesHiddenFaction ? $" · {general.FactionName}" : string.Empty;
        var description = $"选择 {general.Name}（{general.SkillSummary}{factionSuffix}）";
        var parameters = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["action"] = "select-general", ["general-id"] = general.Id,
            ["faction-id"] = general.FactionId ?? string.Empty
        };
        if (fullRoster) parameters["full-roster"] = "true";
        if (_mode.GeneralVariantGroups is not null) parameters["candidate-general-id"] = general.CandidateId;
        if (!Rules.Visibility.UsesSecondaryGeneral && Rules.UsesGeneralBaseHp)
        {
            var lordBonus = player.Role == Role.Lord ? 1 : 0;
            var maximum = general.BaseHp + lordBonus;
            parameters["base-hp"] = general.BaseHp.ToString(CultureInfo.InvariantCulture);
            parameters["identity-max-hp"] = maximum.ToString(CultureInfo.InvariantCulture);
            parameters["health-preview"] = (player.Role == Role.Lord
                ? $"主公体力上限为基础 {general.BaseHp} + 1 = {maximum}。"
                : $"身份模式体力上限为 {maximum}。") +
                (general.InitialHp is { } initialHp ? $"初始体力为 {initialHp + lordBonus}/{maximum}。" : string.Empty);
        }
        if (Rules.Visibility.UsesSecondaryGeneral)
        {
            parameters["base-hp"] = general.BaseHp.ToString(CultureInfo.InvariantCulture);
            var health = $"基础体力 {general.BaseHp}";
            if (player.GeneralSelected)
            {
                var lordBonus = _mode.DualGenerals && player.Role == Role.Lord ? 1 : 0;
                var maximum = (player.PrimaryBaseHp + general.BaseHp) / 2 + lordBonus;
                parameters["primary-base-hp"] = player.PrimaryBaseHp.ToString(CultureInfo.InvariantCulture);
                parameters["combined-max-hp"] = maximum.ToString(CultureInfo.InvariantCulture);
                health += $" · 组合上限 {maximum}（{player.PrimaryBaseHp}+{general.BaseHp} 平均向下取整）";
                if (lordBonus > 0) health += " · 含主公 +1";
            }
            parameters["health-preview"] = health;
            description += " · " + health;
        }
        if (_mode.ShenwuActivity is { } activity)
        {
            var god = activity.GodPoolIds.Contains(general.Id, StringComparer.Ordinal);
            var healthBonus = (activity.PlayerTier >= (activity.Edition == 0 ? 2 : 3) ? 1 : 0) + (activity.PlayerTier >= 5 ? 1 : 0);
            parameters["candidate-general-id"] = general.CandidateId;
            parameters["shenwu-shared-god"] = god ? "true" : "false";
            parameters["health-preview"] = $"本地{(activity.PlayerTier == 5 ? "五" : activity.PlayerTier.ToString(CultureInfo.InvariantCulture))}阶：初始体力 {(general.InitialHp ?? general.BaseHp) + healthBonus}/{general.BaseHp + healthBonus}。" +
                (god ? "此神将来自三人共享候选；选择后其他队友不能再选。" : "普通候选仅对你可见。");
        }
        return new(new ChoiceId($"setup.general.{general.Id}"), description, Array.Empty<int>(), Array.Empty<int>(),
            new ReadOnlyDictionary<string, string>(parameters)) { ContentIds = Array.AsReadOnly(new[] { general.Id }) };
    }

    public string SelectionPrompt(ModeGeneralSelectionFacts player) =>
        Rules.Visibility.UsesHiddenFaction
            ? player.GeneralSelected
                ? $"主将 {player.PrimaryGeneralName}（基础体力 {player.PrimaryBaseHp}）· 请选择副将，卡片显示组合后的体力上限。"
                : "请选择你的第 一 名同势力武将。候选仅对你可见，明置前势力不会公开。"
            : _mode.DualGenerals
                ? player.GeneralSelected
                    ? $"主将 {player.PrimaryGeneralName} · 请选择副将。体力为两将基础体力平均向下取整，主公额外 +1；势力与性别沿用主将。"
                    : "请选择主将，再选择副将。两将技能同时生效，势力与性别沿用主将。"
                : Rules.Visibility.ShowsPublicTeams
                    ? _mode.DragonBoatActivity is not null
                        ? "请选择本势力六名候选中的武将。你与同船队友互见候选，其他队伍不可见。"
                        : "请选择你的武将。候选仅对你可见，所有人完成后才会公开结果。"
                    : "主公先选并公开，本地玩家接着选，其余玩家随后暗选。候选仅对你可见，其余武将在开局确认后公开。";

    public string SetupCompletedText(ModeSetupCompletionFacts facts) => Rules.Visibility.UsesHiddenFaction
        ? "国战选将完成，双将与势力保持暗置；出牌阶段或杀响应时可按需明置一名武将。"
        : $"选将完成并公开：{string.Join("、", facts.PublicGeneralNames)}。";

    public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string generalId) => !UsesFamilyDraft ? [] :
        Array.AsReadOnly((_mode.GeneralVariantGroups?.Values.FirstOrDefault(ids => ids.Contains(generalId, StringComparer.Ordinal)) ?? []).ToArray());
}
