using System.Windows.Input;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private bool _shanheResumeFailed;
    private ShanheCampaignNode? _shanhePreviewNode;
    private ShanheEncounterDefinition? _shanhePreviewEncounter;
    private string _shanheEncounterEnemyName = string.Empty;
    private string _shanheEncounterEnemyDetail = string.Empty;
    private string _shanheEncounterRewardText = string.Empty;
    private Brush _shanheEncounterEnemyPortrait = Brushes.Transparent;
    private IReadOnlyList<ShanheEncounterEnemyCard> _shanheEncounterEnemyCards = Array.Empty<ShanheEncounterEnemyCard>();
    private string _shanheEncounterInitiativeText = string.Empty;
    private int _shanheChapter = 1;
    private string _shanheChapterHeading = "第一章 · 关隘争锋";
    private int _shanheChapterCompleted;

    public bool IsShanheRunInProgress => _shanheSession?.Snapshot.Phase is ShanheCampaignPhase.Map
        or ShanheCampaignPhase.NodeChoice or ShanheCampaignPhase.Battle or ShanheCampaignPhase.Reward or ShanheCampaignPhase.OpeningSkillDraft;
    public bool IsShanhePreparation => !IsShanheRunInProgress;
    // Do not replace a saved expedition before the user has tried to resume it.
    public bool CanPrepareShanheExpedition => !IsTutorialActive && !IsShanheRunInProgress &&
        (!HasShanheProgress || _shanheSession is not null || _shanheResumeFailed);
    public string ShanhePreparationHint => IsShanheRunInProgress
        ? "征程中，武将与初始战法固定。"
        : HasShanheProgress && _shanheSession is null && !_shanheResumeFailed
            ? "本机有未读取的征程，请先继续征程。"
            : "选择武将与初始战法，开始征程。";
    public string ShanheCodexPhaseText => IsShanheRunInProgress
        ? "征程中仅可查看，结束后可编辑初始战法。"
        : CanPrepareShanheExpedition ? "出征前可编辑初始战法" : ShanhePreparationHint;
    public string ShanheChapterTitle => _shanheChapterHeading;
    public string ShanheChapterProgressText => $"本章已过 {_shanheChapterCompleted} 处 · 难度 {_shanheRunDifficulty}";
    public string ShanheChapterObjectiveText => _shanheSession?.Snapshot.Phase switch
    {
        ShanheCampaignPhase.Battle => "返回牌桌，击败本场全部敌军",
        ShanheCampaignPhase.OpeningSkillDraft => "传承触发，先选择一项稀有技能",
        ShanheCampaignPhase.Reward => _shanheSession?.Snapshot.BossRewardSummary is { IsBonusOnly: true }
            ? "领取追加奖励，确认后继续行军" : _shanheSession?.Snapshot.BossRewardSummary is not null
            ? "分别领取红色战法和技能，确认后进入下一章" : "领取一项战利品，或放弃并领取铜币",
        ShanheCampaignPhase.NodeChoice => "完成当前地点的选择，继续行军",
        ShanheCampaignPhase.Won => "此番征程已完成，可重新整备出征",
        ShanheCampaignPhase.Lost => "此番征程已结束，可重新整备出征",
        _ => _shanheChapter == 3
            ? _shanheSession?.Snapshot.AvailableNodes.Any(node => node.Kind == ShanheNodeKind.EliteBattle) == true
                ? "选择精英挑战或最终路线，继续进军" : "选择最终路线，迎接最后一战"
            : "选择亮起的地点，击败本章首领"
    };
    public Brush CurrentShanheGeneralPortrait => _shanheSession?.Snapshot.PlayerGeneralId is { } id
        ? GeneralArt.GetPortrait(id, decodeWidth: 300) : SelectedShanhePortrait;
    public string CurrentShanheGeneralName => _shanheSession?.Snapshot.PlayerGeneralId is { } id
        ? ShanheGeneralName(id) : SelectedShanheGeneral?.Name ?? "待选武将";
    public string CurrentShanheGeneralSkillsText => _shanheSession?.Snapshot.PlayerGeneralId is { } id
        ? ShanheGenerals.FirstOrDefault(general => general.Id == id)?.SkillSummary ?? string.Empty : SelectedShanheSkillText;

    public bool IsShanheEncounterPreviewOpen => _shanhePreviewNode is not null;
    public string ShanheEncounterTitle => _shanhePreviewEncounter?.Name ?? _shanhePreviewNode?.Name ?? string.Empty;
    public string ShanheEncounterDescription => _shanhePreviewNode is { } preview
        ? !string.IsNullOrWhiteSpace(_shanhePreviewEncounter?.StoryText) ? _shanhePreviewEncounter.StoryText!
            : _shanheSession?.Definition.Nodes.SingleOrDefault(node => node.Id == preview.Id)?.StoryText ?? preview.Description : string.Empty;
    public string ShanheEncounterEnemyName => _shanheEncounterEnemyName;
    public string ShanheEncounterEnemyPortraitName { get; private set; } = string.Empty;
    public Brush ShanheEncounterEnemyPortrait => _shanheEncounterEnemyPortrait;
    public string ShanheEncounterEnemyDetail => _shanheEncounterEnemyDetail;
    public IReadOnlyList<ShanheEncounterEnemyCard> ShanheEncounterEnemyCards => _shanheEncounterEnemyCards;
    public IReadOnlyList<ShanheEncounterSkill> ShanheEncounterEnemySkills => _shanheEncounterEnemyCards.FirstOrDefault()?.Skills
        ?? Array.Empty<ShanheEncounterSkill>();
    public string ShanheEncounterEnemyStats => _shanheEncounterEnemyCards.FirstOrDefault()?.Stats ?? string.Empty;
    public string ShanheEncounterInitiativeText => _shanheEncounterInitiativeText;
    public string ShanheEncounterRewardText => _shanheEncounterRewardText;
    public ICommand PreviewShanheNodeCommand { get; private set; } = null!;
    public ICommand ChallengeShanheEncounterCommand { get; private set; } = null!;
    public ICommand CloseShanheEncounterPreviewCommand { get; private set; } = null!;

    private void InitializeShanheJourney()
    {
        PreviewShanheNodeCommand = new RelayCommand<ShanheCampaignNode>(PreviewShanheNode, IsCurrentShanheNode);
        ChallengeShanheEncounterCommand = new RelayCommand(() =>
        {
            var node = _shanhePreviewNode;
            if (node is null || IsTutorialActive || !IsCurrentShanheNode(node)) return;
            CloseShanheEncounterPreview();
            SelectShanheNode(node);
        }, () => !IsTutorialActive && _shanhePreviewNode is { } node && IsCurrentShanheNode(node));
        CloseShanheEncounterPreviewCommand = new RelayCommand(CloseShanheEncounterPreview);
    }

    private void PreviewShanheNode(ShanheCampaignNode node)
    {
        if (IsTutorialActive || !IsCurrentShanheNode(node) || _shanheSession is not { } session) return;
        var definition = session.Definition.Nodes.Single(item => item.Id == node.Id);
        if (definition.Kind is not (ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle))
        {
            SelectShanheNode(node);
            return;
        }
        IReadOnlyList<ShanheEncounterDefinition> encounters = [session.PreviewEncounter(node.Id)];
        var enemies = encounters.SelectMany(encounter => encounter.Enemies).Distinct().ToArray();
        var first = encounters[0];
        var requiredEnemies = enemies.Concat(encounters.Where(encounter => encounter.BossTransition is not null)
            .Select(encounter => new ShanheCombatantDefinition(encounter.BossTransition!.GeneralId))).Distinct().ToArray();
        var state = session.Snapshot;
        // A registry probe reads only published content. No campaign command, battle engine,
        // random-state change or save is produced until the player confirms the challenge.
        var probe = new ShanheBattleSetup(1, state.EncounterOrdinal + 1, definition.Id, state.PlayerGeneralId,
            state.Hp, state.Bonuses.MaxHp, state.Bonuses.InitialHand, state.Bonuses.Draw, state.Bonuses.Slash,
            state.ExtraSkillIds, state.StartingCards, state.StartingEquipment, requiredEnemies,
            encounters.SelectMany(encounter => encounter.Allies ?? []).Distinct().ToArray(), session.ContentHash)
        { BossTransition = first.BossTransition };
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(probe);
        _shanhePreviewNode = node;
        _shanhePreviewEncounter = first;
        _shanheEncounterEnemyName = string.Join(" · ", enemies.Select(enemy => registry.Generals[enemy.GeneralId].Name));
        ShanheEncounterEnemyPortraitName = registry.Generals[enemies[0].GeneralId].Name;
        _shanheEncounterEnemyPortrait = GeneralArt.GetPortrait(enemies[0].GeneralId, decodeWidth: 360);
        _shanheEncounterEnemyCards = Array.AsReadOnly(enemies.Select(enemy =>
        {
            var general = registry.Generals[enemy.GeneralId];
            var maximum = general.BaseHp + enemy.MaxHpBonus;
            var hand = (enemy.StartingHand?.Count ?? 4) + enemy.InitialHandBonus;
            var skills = general.SkillIds.Concat(enemy.ExtraSkillIds ?? []).Distinct(StringComparer.Ordinal)
                .Select(registry.GetSkill).Select(skill => new ShanheEncounterSkill(skill.Name, GetVisibleSkillDescription(skill))).ToArray();
            return new ShanheEncounterEnemyCard(general.Name, GeneralArt.GetPortrait(enemy.GeneralId, decodeWidth: 360),
                $"体力 {enemy.Hp ?? general.InitialHp ?? maximum}/{maximum} · 起手 {hand}\n摸牌阶段 {2 + enemy.DrawBonus} · 出杀次数 {1 + enemy.SlashBonus}" + ShanheSourceCombatantStats(enemy),
                Array.AsReadOnly(skills));
        }).ToArray());
        // Ask the same immutable opening rule used by the actual mode. A future
        // random opening is reported as undecided without consuming campaign RNG.
        var initiativeSetup = probe with { EnemyRoster = first.Enemies, AllyRoster = first.Allies ?? [] };
        var initiativeOptions = new GameOptions { ShanheBattle = initiativeSetup,
            PlayerCount = 1 + initiativeSetup.EnemyRoster.Count + initiativeSetup.AllyRoster.Count,
            HumanRole = null, ModeId = ShanheBattleSetup.BattleModeId };
        var opening = StandardModeRuleProfiles.Create(registry.Modes[ShanheBattleSetup.BattleModeId], initiativeOptions).Opening;
        var life = Enumerable.Range(0, initiativeOptions.PlayerCount).Select(seat =>
            new ModePlayerLife(seat, Role.Loyalist, true, seat <= initiativeSetup.AllyRoster.Count
                ? ShanheBattleSetup.PlayerTeamId : ShanheBattleSetup.EnemyTeamId, null)).ToArray();
        var needsRandom = false;
        var firstSeat = opening.GetStartingSeat(life, _ => { needsRandom = true; return 0; });
        _shanheEncounterInitiativeText = needsRandom ? "先手角色：开战时确定" : firstSeat == 0 ? "先手角色：玩家"
            : firstSeat <= initiativeSetup.AllyRoster.Count ? "先手角色：同行"
            : "先手角色：" + registry.Generals[initiativeSetup.EnemyRoster[firstSeat - initiativeSetup.AllyRoster.Count - 1].GeneralId].Name;
        _shanheEncounterEnemyDetail = (encounters.Count > 1 ? "可能出现的完整阵容：\n" : string.Empty) +
            string.Join("\n\n", encounters.Select(encounter =>
            {
                var totalSeats = 1 + encounter.Enemies.Count + (encounter.Allies?.Count ?? 0);
                var roster = string.Join("\n", encounter.Enemies.Select(enemy =>
                {
                    var general = registry.Generals[enemy.GeneralId];
                    var maxHp = general.BaseHp + enemy.MaxHpBonus;
                    var skillIds = general.SkillIds.Concat(enemy.ExtraSkillIds ?? []).Distinct(StringComparer.Ordinal);
                    var hand = (enemy.StartingHand?.Count ?? 4) + enemy.InitialHandBonus;
                    var fixedCards = enemy.StartingCards is { Count: > 0 } cards
                        ? "\n额外起手牌（随机起手之外）：\n" + string.Join("\n", cards.Select(card =>
                        {
                            var face = CardCatalog.Get(card.Kind);
                            return $"【{face.DisplayName}】{face.Description}";
                        })) : string.Empty;
                    return $"{general.Name}  体力 {enemy.Hp ?? general.InitialHp ?? maxHp}/{maxHp}\n" +
                        $"起手 {hand} · 摸牌阶段基础 {2 + enemy.DrawBonus} · 出杀基础 {1 + enemy.SlashBonus}\n" +
                        string.Join(" · ", skillIds.Select(id => registry.GetSkill(id).Name)) + ShanheSourceCombatantStats(enemy) + fixedCards;
                }));
                var stage = string.Empty;
                if (encounter.BossTransition is { } transition)
                {
                    var secondForm = registry.Generals[transition.GeneralId];
                    var secondSkills = string.Join(" · ", secondForm.SkillIds.Select(id => registry.GetSkill(id).Name));
                    var equipment = transition.StartingEquipment is { Count: > 0 } fixedEquipment
                        ? "\n阶段转换时自动装备：" + string.Join(" · ", fixedEquipment.Select(card =>
                            $"【{CardCatalog.Get(card.Kind).DisplayName}】"))
                        : string.Empty;
                    stage = $"\n二阶段：{secondForm.Name}  体力 {transition.Hp}/{transition.MaxHp}" +
                        $" · 补牌 {transition.DrawCards}\n二阶段技能：{secondSkills}" + equipment +
                        "\n第一阶段血量打空后同席转换，不结算胜利；中断当前回合，由首领开始新的自然回合。";
                    if (transition.Combatant is { } second)
                        stage += $"\n二阶段摸牌 {2 + second.DrawBonus} · 出杀 {1 + second.SlashBonus}" + ShanheSourceCombatantStats(second);
                }
                return $"{encounter.Name}\n{totalSeats}人阵容：敌军 {encounter.Enemies.Count} · 同行 {encounter.Allies?.Count ?? 0} · 玩家 1\n" + roster + stage;
            }));
        var gold = encounters.Select(encounter => encounter.VictoryGold).Distinct().Order().ToArray();
        var goldText = gold.Length == 1 ? gold[0].ToString() : $"{gold[0]}–{gold[^1]}";
        var finalEncounter = definition.ChapterNumber >= 3 && definition.NextNodeIds.Count == 0;
        _shanheEncounterRewardText = finalEncounter
            ? $"基础铜币 × {goldText}\n第三章通关直接进入最终结算，发放图册并保存征程记录，不再选择战法或技能奖励。"
            : (definition.BossRewards is { } boss
                ? $"基础铜币 × {boss.FixedGold}    技能槽 +{boss.SkillSlots}\n" +
                    string.Join("    ", boss.Groups.Select(group => $"{group.Name}（{group.ChoiceCount}选一）"))
                : $"基础铜币 × {goldText}" +
                    (definition.RewardIds?.Count > 0 ? "\n" + ShanheEncounterRewardPoolText(definition, session.Definition) : string.Empty)) +
                (first.HealAfterVictory > 0 && encounters.All(encounter => encounter.HealAfterVictory == first.HealAfterVictory)
                    ? $"\n胜利后恢复 {first.HealAfterVictory} 点体力，剩余体力带入后续战斗" : string.Empty);
        NotifyShanheEncounterPreview();
    }

    private static string ShanheEncounterRewardPoolText(ShanheNodeDefinition node, ShanheCampaignDefinition definition)
    {
        static ShanheRewardCategory CategoryOf(ShanheRewardDefinition reward) => IsShanheTactic(reward)
            ? ShanheRewardCategory.Tactic : reward.SkillId is not null ? ShanheRewardCategory.Skill
            : reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0 ? ShanheRewardCategory.Card : ShanheRewardCategory.Supply;
        bool IsBaseline(ShanheRewardDefinition reward)
        {
            var rarity = CategoryOf(reward) switch
            {
                ShanheRewardCategory.Tactic => node.PrimaryTacticRarity,
                ShanheRewardCategory.Skill => node.PrimarySkillRarity,
                _ => 0
            };
            return (rarity == 0 || reward.Rarity == rarity) && reward.OpeningCompanionPool is null &&
                reward.CampaignEconomy?.StartingGold is not > 0;
        }
        var pool = (node.RewardIds ?? []).Select(id => definition.Rewards.Single(reward => reward.Id == id))
            .Where(IsBaseline).DistinctBy(reward => reward.Id).ToArray();
        if (pool.Length == 0) return string.Empty;
        var groups = pool.GroupBy(CategoryOf).OrderBy(group => group.Key).ToArray();
        var perCategory = node.CategorizedRewards || groups.Length == 1;
        var labels = groups.Select(group =>
        {
            var quality = string.Join(" / ", group.Select(reward => reward.Rarity).Distinct().Order().Select(rarity => rarity switch
                { 1 => "普通", 2 => "稀有", 3 => "史诗", 4 => "传说", _ => "" }).Where(text => text.Length > 0));
            var kind = group.Key switch
            {
                ShanheRewardCategory.Tactic => "战法",
                ShanheRewardCategory.Skill => "技能",
                ShanheRewardCategory.Card => group.All(reward => reward.StartingEquipment?.Count > 0) ? "装备"
                    : group.All(reward => reward.StartingCards?.Count > 0) ? "手牌" : "卡牌",
                _ => "补给"
            };
            return quality + kind + (perCategory ? $"（{Math.Min(node.RewardChoiceCount, group.Count())}选一）" : "");
        });
        return "基础奖励：" + string.Join(" · ", labels) +
            (!perCategory ? $"（合计{Math.Min(node.RewardChoiceCount, pool.Length)}选一）" : "") +
            (node.CategorizedRewards ? "\n先选择类别，再查看候选。" : "") +
            "\n候选数量与品质提升按所持战法结算。" +
            (pool.All(IsShanheTactic) ? "\n战法必须领取，不可放弃换取铜币。" :
                node.RewardSkipGold > 0 ? "\n装备、手牌与技能可按本次候选标明的金额放弃换取铜币；战法必须领取。" : "");
    }

    private void CloseShanheEncounterPreview()
    {
        _shanhePreviewNode = null;
        _shanhePreviewEncounter = null;
        NotifyShanheEncounterPreview();
    }

    private void NotifyShanheEncounterPreview()
    {
        foreach (var property in new[] { nameof(IsShanheEncounterPreviewOpen), nameof(ShanheEncounterTitle),
            nameof(ShanheEncounterDescription), nameof(ShanheEncounterEnemyName), nameof(ShanheEncounterEnemyPortraitName), nameof(ShanheEncounterEnemyPortrait),
            nameof(ShanheEncounterEnemyDetail), nameof(ShanheEncounterRewardText), nameof(ShanheEncounterEnemyCards),
            nameof(ShanheEncounterEnemySkills), nameof(ShanheEncounterEnemyStats), nameof(ShanheEncounterInitiativeText) }) RaisePropertyChanged(property);
        (ChallengeShanheEncounterCommand as RelayCommand)?.NotifyCanExecuteChanged();
    }

    private void RefreshShanheJourney()
    {
        if (_shanhePreviewNode is { } node && !IsCurrentShanheNode(node)) CloseShanheEncounterPreview();
        foreach (var property in new[] { nameof(IsShanheRunInProgress), nameof(IsShanhePreparation), nameof(CanPrepareShanheExpedition),
            nameof(ShanhePreparationHint), nameof(ShanheCodexPhaseText), nameof(ShanheChapterTitle), nameof(ShanheChapterProgressText),
            nameof(ShanheChapterObjectiveText), nameof(CurrentShanheGeneralPortrait), nameof(CurrentShanheGeneralName),
            nameof(CurrentShanheGeneralSkillsText) }) RaisePropertyChanged(property);
        foreach (var command in new[] { StartShanheCommand, UnlockCodexNodeCommand, EquipCodexTacticCommand, RemoveCodexTacticCommand })
            (command as RelayCommand)?.NotifyCanExecuteChanged();
    }

    private static string ShanheSourceCombatantStats(ShanheCombatantDefinition combatant)
    {
        var values = new List<string>();
        if (combatant.InitialArmor > 0) values.Add($"初始护甲 {combatant.InitialArmor}");
        if (combatant.ArmorRecoveryPerRound > 0)
            values.Add($"每轮回甲 {combatant.ArmorRecoveryPerRound}（至多 {combatant.ArmorRecoveryCap}）");
        if (combatant.ExtraDamage > 0) values.Add($"伤害 +{combatant.ExtraDamage}");
        return values.Count > 0 ? "\n" + string.Join(" · ", values) : string.Empty;
    }

    private IEnumerable<ShanheNodeDefinition> CurrentShanheChapterNodes(ShanheCampaignDefinition definition,
        IReadOnlyDictionary<string, int> depths, string? currentNodeId, IReadOnlySet<string> completed)
    {
        var state = _shanheSession?.Snapshot;
        var focusNode = state?.Phase == ShanheCampaignPhase.Map && state.AvailableNodes.Count > 0
            ? state.AvailableNodes.OrderBy(node => depths.GetValueOrDefault(node.Id)).First()
            : definition.Nodes.FirstOrDefault(node => node.Id == currentNodeId);
        int ChapterOf(ShanheNodeDefinition node) => node.ChapterNumber > 0 ? node.ChapterNumber
            : depths.GetValueOrDefault(node.Id) <= 4 ? 1 : depths.GetValueOrDefault(node.Id) <= 8 ? 2 : 3;
        // New scenarios publish explicit chapter metadata. The depth fallback preserves the
        // older saved local definitions without pretending their map has new chapter content.
        _shanheChapter = focusNode is null ? 1 : ChapterOf(focusNode);
        var available = state?.AvailableNodes.Select(node => node.Id).ToHashSet(StringComparer.Ordinal) ?? [];
        var north = completed.Contains("hulao-start") || available.Contains("hulao-start") || currentNodeId is "hulao-start" or "hulao";
        var south = completed.Contains("guangcheng-start") || available.Contains("guangcheng-start") || currentNodeId is "guangcheng-start" or "guangcheng";
        _shanheChapterHeading = _shanheChapter switch
        {
            1 => north ? "第一章 · 虎牢关" : south ? "第一章 · 广成关" : "第一章 · 关隘争锋",
            2 => completed.Contains("dagu") || currentNodeId is "dagu" or "caravan" ? "第二章 · 大谷关"
                : completed.Contains("huanyuan") || currentNodeId is "huanyuan" or "rumor" ? "第二章 · 轘辕关" : "第二章 · 西凉烽烟",
            _ => currentNodeId is "luoyang" or "suanzao" or "henei"
                ? "第三章 · " + definition.Nodes.Single(node => node.Id == currentNodeId).Name : "第三章 · 最后的营火"
        };
        var sourceEdition = definition.SourceDifficultyId is not null;
        var nodes = definition.Nodes.Where(node => !node.IsStoryBattle && ((!IsShanheRunInProgress && !sourceEdition) || _shanheChapter == ChapterOf(node))).ToArray();
        nodes = FilterShanheSourceRegionNodes(nodes, sourceEdition, focusNode, currentNodeId, available, completed);
        var chapterTitle = sourceEdition ? SelectedShanheSourceRegion?.Key ?? focusNode?.ChapterTitle
            : focusNode?.ChapterTitle ?? nodes.FirstOrDefault(node => node.ChapterNumber == _shanheChapter)?.ChapterTitle;
        if (!string.IsNullOrWhiteSpace(chapterTitle)) _shanheChapterHeading = chapterTitle;
        _shanheChapterCompleted = nodes.Count(node => completed.Contains(node.Id));
        return nodes;
    }
}

public sealed record ShanheEncounterSkill(string Name, string Description);
public sealed record ShanheEncounterEnemyCard(string Name, Brush Portrait, string Stats, IReadOnlyList<ShanheEncounterSkill> Skills);
