using System.Windows.Input;
using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private bool _shanheResumeFailed;
    private ShanheCampaignNode? _shanhePreviewNode;
    private ShanheNodeDefinition? _shanhePreviewBattleNode;
    private ShanheEncounterDefinition? _shanhePreviewEncounter;
    private string _shanheEncounterEnemyName = string.Empty;
    private string _shanheEncounterEnemyDetail = string.Empty;
    private string _shanheEncounterRewardText = string.Empty;
    private IReadOnlyList<ShanheEncounterRewardSummaryItem> _shanheEncounterRewardSummary = [];
    private Brush _shanheEncounterEnemyPortrait = Brushes.Transparent;
    private IReadOnlyList<ShanheEncounterEnemyCard> _shanheEncounterEnemyCards = Array.Empty<ShanheEncounterEnemyCard>();
    private ShanheEncounterEnemyCard? _selectedShanheEncounterEnemy;
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
    public string ShanheChapterProgressText => _shanheUsesChapterProgress
        ? $"主线进度 {_shanheChapterCompleted} / 6 · 难度 {_shanheRunDifficulty}"
        : $"本章已过 {_shanheChapterCompleted} / {ShanheChapterStepCount} 程 · 难度 {_shanheRunDifficulty}";
    public string ShanheChapterObjectiveText => _shanheSession?.Snapshot.Phase switch
    {
        ShanheCampaignPhase.Battle => IsShanheBattleRetryReady ? "关卡进度保留，可重新挑战本关" : "返回牌桌，击败本场全部敌军",
        ShanheCampaignPhase.OpeningSkillDraft => ShanheRewardNotice,
        ShanheCampaignPhase.Reward => IsShanheRewardOpen ? ShanheRewardNotice : ShanheBossRewardNotice,
        ShanheCampaignPhase.NodeChoice => IsShanheRestChoiceOpen
            ? "营地：摸牌数／体力上限二选一，确认后继续行军" : "完成当前地点的选择，继续行军",
        ShanheCampaignPhase.Won => "此番征程已完成，可重新整备出征",
        ShanheCampaignPhase.Lost => "此番征程已结束，可重新整备出征",
        _ => _shanheUsesChapterProgress ? _shanheChapterCompleted >= 6
            ? ShanheCompletedChapterObjectiveText
            : "完成亮起的主线地点推进；可选战斗与擂台不占主线进度"
            : _shanheChapter == 3
            ? _shanheSession?.Snapshot.AvailableNodes.Any(node => node.Kind == ShanheNodeKind.EliteBattle) == true
                ? "选择精英挑战或最终路线，继续进军" : "选择最终路线，迎接最后一战"
            : _shanheSession?.Definition.AllowChapterDetours == true
                ? "亮起的地点都可挑战，也可返回补打本章分支"
                : "选择亮起的地点，击败本章首领"
    };
    private string ShanheCompletedChapterObjectiveText
    {
        get
        {
            var available = _shanheSession?.Snapshot.AvailableNodes ?? [];
            var remaining = new List<string>();
            var optional = available.Count(node => node.ChapterRole is ShanheChapterNodeRole.OpeningBattle or ShanheChapterNodeRole.OptionalBattle);
            var arenas = available.Count(node => node.ChapterRole == ShanheChapterNodeRole.Arena);
            var pursuits = available.Count(node => node.ChapterRole == ShanheChapterNodeRole.ExtraBattle);
            if (optional > 0) remaining.Add($"{optional}场可选战斗");
            if (arenas > 0) remaining.Add($"{arenas}场擂台");
            if (pursuits > 0) remaining.Add($"{pursuits}场追击");
            return remaining.Count == 0 ? "主线已满，可直接挑战首领"
                : "主线已满，可挑战首领；剩余" + string.Join("、", remaining);
        }
    }
    public Brush CurrentShanheGeneralPortrait => _shanheSession?.Snapshot.PlayerGeneralId is { } id
        ? GeneralArt.GetPortrait(id, decodeWidth: 300) : SelectedShanhePortrait;
    public string CurrentShanheGeneralName => _shanheSession?.Snapshot.PlayerGeneralId is { } id
        ? ShanheGeneralName(id) : SelectedShanheGeneral?.Name ?? "待选武将";
    public string CurrentShanheGeneralSkillsText => _shanheSession?.Snapshot.PlayerGeneralId is { } id
        ? ShanheGenerals.FirstOrDefault(general => general.Id == id)?.SkillSummary ?? string.Empty : SelectedShanheSkillText;

    public bool IsShanheEncounterPreviewOpen => _shanhePreviewNode is not null;
    public string ShanheEncounterTitle => _shanhePreviewEncounter?.Name ?? _shanhePreviewNode?.Name ?? string.Empty;
    public ImageSource? ShanheEncounterBackdrop => AssetImages.Load("Assets/Shanhe/Encounter/roguelike_fight_bg" +
        (_shanhePreviewBattleNode?.Kind == ShanheNodeKind.BossBattle ? "2" : "1") + ".png");
    public string ShanheEncounterDescription => _shanhePreviewNode is { } preview
        ? !string.IsNullOrWhiteSpace(_shanhePreviewEncounter?.StoryText) ? _shanhePreviewEncounter.StoryText!
            : _shanhePreviewBattleNode?.StoryText ?? _shanheSession?.Definition.Nodes.SingleOrDefault(node => node.Id == preview.Id)?.StoryText ?? preview.Description : string.Empty;
    public string ShanheEncounterRouteHint
    {
        get
        {
            if (_shanhePreviewNode is not { } preview || _shanhePreviewBattleNode is not { } node ||
                _shanheSession is not { } session || !IsCurrentShanheNode(preview)) return string.Empty;
            var mainline = _shanheUsesChapterProgress && session.IsChapterNodeMainline(node.Id);
            var flag = ShanheRouteFlagText(node, preview.IsAvailable, mainline);
            if (flag == "追击") return _shanheUsesChapterProgress
                ? "日行千里追击：本章额外战斗，独立于两场基础可选战。可跳过，不消耗六格主线进度；胜利照常获得铜币与战利品。"
                : "日行千里额外战斗，可跳过；胜利照常获得铜币与战利品。";
            if (!_shanheUsesChapterProgress) return string.Empty;
            if (flag == "主线") return node.ChapterRole == ShanheChapterNodeRole.OpeningBattle
                ? "主线第1格：两处战场任选一处，胜利推进1格；另一处保留为可选战斗，可跳过且不再推进主线。"
                : $"主线第{node.ChapterProgressStep}格：胜利后推进1格主线。";
            if (flag == "可选") return (node.ChapterRole == ShanheChapterNodeRole.Arena ? "可选擂台" : "可选战斗") +
                "：可跳过，不消耗六格主线进度；胜利照常获得铜币与战利品。";
            return node.ChapterRole == ShanheChapterNodeRole.Boss
                ? "关底首领：主线六格已满，可直接挑战。" + ShanheChapterBossText + "。" : string.Empty;
        }
    }
    public string ShanheEncounterEnemyName => _shanheEncounterEnemyName;
    public string ShanheEncounterEnemyPortraitName { get; private set; } = string.Empty;
    public Brush ShanheEncounterEnemyPortrait => _shanheEncounterEnemyPortrait;
    public string ShanheEncounterEnemyDetail => _shanheEncounterEnemyDetail + (_selectedShanheEncounterEnemy is { } selected
        ? "\n\n—— 所选敌将详情 ——\n" + selected.FullDetails : string.Empty);
    public IReadOnlyList<ShanheEncounterEnemyCard> ShanheEncounterEnemyCards => _shanheEncounterEnemyCards;
    public IReadOnlyList<ShanheEncounterEnemyCard> ShanheEncounterMainEnemyCards => _shanheEncounterEnemyCards.Any(card => card.IsCommander == true)
        ? Array.AsReadOnly(_shanheEncounterEnemyCards.Where(card => card.IsCommander == true).ToArray()) : _shanheEncounterEnemyCards;
    public IReadOnlyList<ShanheEncounterEnemyCard> ShanheEncounterMinorEnemyCards => _shanheEncounterEnemyCards.Any(card => card.IsCommander == true)
        ? Array.AsReadOnly(_shanheEncounterEnemyCards.Where(card => card.IsCommander == false).ToArray()) : [];
    public bool HasShanheEncounterMinorEnemies => ShanheEncounterMinorEnemyCards.Count > 0;
    public bool HasShanheEncounterEnemySelection => _selectedShanheEncounterEnemy is not null;
    public string ShanheEncounterSelectedEnemyLabel => _selectedShanheEncounterEnemy is { } selected
        ? $"当前查看：{selected.SeatOrdinal}号敌席 · {selected.Name}" : string.Empty;
    public IReadOnlyList<ShanheEncounterSkill> ShanheEncounterEnemySkills => ShanheEncounterMainEnemyCards.FirstOrDefault()?.Skills
        ?? Array.Empty<ShanheEncounterSkill>();
    public string ShanheEncounterEnemyStats => ShanheEncounterMainEnemyCards.FirstOrDefault()?.Stats ?? string.Empty;
    public string ShanheEncounterInitiativeText => _shanheEncounterInitiativeText;
    public string ShanheEncounterRewardText => _shanheEncounterRewardText;
    public IReadOnlyList<ShanheEncounterRewardSummaryItem> ShanheEncounterRewardSummary => _shanheEncounterRewardSummary;
    public ICommand PreviewShanheNodeCommand { get; private set; } = null!;
    public ICommand ChallengeShanheEncounterCommand { get; private set; } = null!;
    public ICommand CloseShanheEncounterPreviewCommand { get; private set; } = null!;
    public ICommand SelectShanheEncounterEnemyCommand { get; private set; } = null!;

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
        SelectShanheEncounterEnemyCommand = new RelayCommand<ShanheEncounterEnemyCard>(card =>
        {
            if (ReferenceEquals(_selectedShanheEncounterEnemy, card)) return;
            _selectedShanheEncounterEnemy = card;
            RaisePropertyChanged(nameof(ShanheEncounterEnemyDetail));
            RaisePropertyChanged(nameof(HasShanheEncounterEnemySelection));
            RaisePropertyChanged(nameof(ShanheEncounterSelectedEnemyLabel));
        }, card => IsShanheEncounterPreviewOpen && card is not null &&
            _shanheEncounterEnemyCards.Any(item => ReferenceEquals(item, card)));
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
        definition = session.PreviewBattleNode(node.Id);
        IReadOnlyList<ShanheEncounterDefinition> encounters = [session.PreviewEncounter(node.Id)];
        // Repeated troops still occupy separate enemy seats in this concrete encounter.
        var enemies = encounters.SelectMany(encounter => encounter.Enemies).ToArray();
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
        _shanhePreviewBattleNode = definition;
        _shanhePreviewEncounter = first;
        _shanheEncounterEnemyName = string.Join(" · ", enemies.Select(enemy => registry.Generals[enemy.GeneralId].Name));
        _selectedShanheEncounterEnemy = null;
        var roles = ShanheEncounterArt.Match(session.Definition, definition, first);
        _shanheEncounterEnemyCards = Array.AsReadOnly(enemies.Select((enemy, index) =>
        {
            var general = registry.Generals[enemy.GeneralId];
            var maximum = general.BaseHp + enemy.MaxHpBonus;
            var hand = (enemy.StartingHand?.Count ?? 4) + enemy.InitialHandBonus;
            var skills = general.SkillIds.Concat(enemy.ExtraSkillIds ?? []).Distinct(StringComparer.Ordinal)
                .Select(registry.GetSkill).Select(skill => new ShanheEncounterSkill(skill.Name, GetVisibleSkillDescription(skill))).ToArray();
            return new ShanheEncounterEnemyCard(general.Name, GeneralArt.GetPortrait(enemy.GeneralId, decodeWidth: 360),
                $"体力 {enemy.Hp ?? general.InitialHp ?? maximum}/{maximum} · 起手 {hand}\n摸牌阶段 {2 + enemy.DrawBonus} · 出杀次数 {1 + enemy.SlashBonus}" + ShanheSourceCombatantStats(enemy) + ShanheNpcEquipmentText(enemy),
                Array.AsReadOnly(skills))
            {
                Hp = enemy.Hp ?? general.InitialHp ?? maximum, MaxHp = maximum,
                SeatOrdinal = index + 1, IsCommander = roles.Count == enemies.Length ? roles[index] : null
            };
        }).ToArray());
        var portraitCard = ShanheEncounterMainEnemyCards[0];
        ShanheEncounterEnemyPortraitName = portraitCard.Name;
        _shanheEncounterEnemyPortrait = portraitCard.Portrait;
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
                        string.Join(" · ", skillIds.Select(id => registry.GetSkill(id).Name)) + ShanheSourceCombatantStats(enemy) + fixedCards + ShanheNpcEquipmentText(enemy);
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
        var rewardPool = !finalEncounter && definition.BossRewards is null && definition.RewardIds?.Count > 0
            ? ShanheEncounterRewardPool(definition, session.Definition, session.PreviewRewardCategory(definition.Id)) : null;
        _shanheEncounterRewardText = finalEncounter
            ? $"基础铜币 × {goldText}\n第三章通关直接进入最终结算，发放图册并保存征程记录，不再选择战法或技能奖励。"
            : (definition.BossRewards is { } boss
                ? $"基础铜币 × {boss.FixedGold}    技能槽 +{boss.SkillSlots}\n" +
                    string.Join("    ", boss.Groups.Select(group => $"{group.Name}（{group.ChoiceCount}选一）"))
                : $"基础铜币 × {goldText}" +
                    (rewardPool is not null ? "\n" + ShanheEncounterRewardPoolText(definition, rewardPool) : string.Empty)) +
                (session.Definition.SourceDifficultyId is not >= 6 && first.HealAfterVictory > 0 &&
                    encounters.All(encounter => encounter.HealAfterVictory == first.HealAfterVictory)
                    ? $"\n胜利后恢复 {first.HealAfterVictory} 点体力，剩余体力带入后续战斗" : string.Empty);
        _shanheEncounterRewardSummary = BuildShanheEncounterRewardSummary(definition, session.Definition,
            goldText, finalEncounter, rewardPool);
        NotifyShanheEncounterPreview();
    }

    private static ShanheRewardCategory ShanheEncounterRewardCategory(ShanheRewardDefinition reward) => IsShanheTactic(reward)
        ? ShanheRewardCategory.Tactic : reward.SkillId is not null ? ShanheRewardCategory.Skill
        : reward.StartingCards?.Count > 0 || reward.StartingEquipment?.Count > 0 ? ShanheRewardCategory.Card : ShanheRewardCategory.Supply;

    private static ShanheEncounterRewardPoolPreview ShanheEncounterRewardPool(ShanheNodeDefinition node, ShanheCampaignDefinition definition,
        ShanheRewardCategory? automaticCategory)
    {
        bool IsBaseline(ShanheRewardDefinition reward)
        {
            var rarity = ShanheEncounterRewardCategory(reward) switch
            {
                ShanheRewardCategory.Tactic => node.PrimaryTacticRarity,
                ShanheRewardCategory.Skill => node.PrimarySkillRarity,
                ShanheRewardCategory.Card => node.PrimaryCardRarity,
                _ => 0
            };
            return (rarity == 0 || reward.Rarity == rarity) && reward.OpeningCompanionPool is null &&
                reward.CampaignEconomy?.StartingGold is not > 0;
        }
        var pool = (node.RewardIds ?? []).Select(id => definition.Rewards.Single(reward => reward.Id == id))
            .Where(reward => IsBaseline(reward) && (automaticCategory is null || ShanheEncounterRewardCategory(reward) == automaticCategory))
            .DistinctBy(reward => reward.Id).ToArray();
        var groups = pool.GroupBy(ShanheEncounterRewardCategory).OrderBy(group => group.Key).ToArray();
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
            return new ShanheEncounterRewardPoolGroup(group.Key, quality + kind,
                Math.Min(node.RewardChoiceCount, group.Count()));
        }).ToArray();
        return new(Array.AsReadOnly(labels), Math.Min(node.RewardChoiceCount, pool.Length), perCategory,
            pool.All(IsShanheTactic), automaticCategory);
    }

    private static string ShanheEncounterRewardPoolText(ShanheNodeDefinition node, ShanheEncounterRewardPoolPreview pool)
    {
        if (node.AutomaticRewardCategory && pool.AutomaticCategory is null)
            return "战利品候选以战后实际结算为准。\n候选数量与品质提升按所持战法结算。" +
                (node.RewardSkipGold > 0
                    ? "\n若有装备、手牌或技能候选，可按其标明金额放弃换取铜币；战法奖励必须领取。"
                    : pool.Groups.Any(group => group.Category == ShanheRewardCategory.Tactic)
                        ? "\n若获得战法奖励，必须领取，不可放弃换取铜币。" : string.Empty);
        if (pool.Groups.Count == 0) return string.Empty;
        return "基础奖励：" + string.Join(" · ", pool.Groups.Select(group => group.Label +
                (pool.PerCategory ? $"（{group.ChoiceCount}选一）" : string.Empty))) +
            (!pool.PerCategory ? $"（合计{pool.ChoiceCount}选一）" : "") +
            (node.AutomaticRewardCategory ? "\n奖励类型已在本地点确定，战后直接选择同类候选。"
                : node.CategorizedRewards ? "\n先选择类别，再查看候选。" : "") +
            "\n候选数量与品质提升按所持战法结算。" +
            (pool.OnlyTactics ? "\n战法必须领取，不可放弃换取铜币。" :
                node.RewardSkipGold > 0 ? "\n装备、手牌与技能可按本次候选标明的金额放弃换取铜币；战法必须领取。" : "");
    }

    private static IReadOnlyList<ShanheEncounterRewardSummaryItem> BuildShanheEncounterRewardSummary(ShanheNodeDefinition node,
        ShanheCampaignDefinition definition, string goldText, bool finalEncounter, ShanheEncounterRewardPoolPreview? pool)
    {
        const string baselineNotice = "候选数量与品质提升按所持战法结算，战后以实际候选为准。";
        var boss = finalEncounter ? null : node.BossRewards;
        var summary = new List<ShanheEncounterRewardSummaryItem>
        {
            new(AssetImages.Load("Assets/Shanhe/Encounter/14021801.png"), "基础铜币",
                "× " + (boss is null ? goldText : boss.FixedGold.ToString()), "基础胜利铜币，所持战法加成另按实际结算。")
        };
        if (finalEncounter)
        {
            summary.Add(new(null, "最终结算", "通关后结算", "发放图册并保存征程记录，不再选择战法或技能奖励。"));
        }
        else if (boss is not null)
        {
            foreach (var group in boss.Groups)
            {
                var categories = group.RewardIds.Select(id => definition.Rewards.Single(reward => reward.Id == id))
                    .Select(ShanheEncounterRewardCategory).Distinct().ToArray();
                summary.Add(new(ShanheEncounterRewardIcon(categories.Length == 1 ? categories[0] : ShanheRewardCategory.Supply),
                    group.Name, $"基础 {group.ChoiceCount} 选一", baselineNotice));
            }
            if (boss.SkillSlots > 0)
                summary.Add(new(AssetImages.Load("Assets/Shanhe/2293.png"), "技能槽", "+ " + boss.SkillSlots,
                    "本章首领固定奖励：增加额外技能槽。"));
        }
        else if (pool is not null && (!node.AutomaticRewardCategory || pool.AutomaticCategory is not null))
        {
            foreach (var group in pool.Groups)
            {
                var chooseCategory = node.CategorizedRewards && !node.AutomaticRewardCategory;
                var value = chooseCategory ? "先选类别" : pool.PerCategory ? $"基础 {group.ChoiceCount} 选一" : "混合奖池";
                var detail = chooseCategory ? $"选择此类别后，基础 {group.ChoiceCount} 选一。" + baselineNotice
                    : !pool.PerCategory ? $"各类别共用一个混合奖池，合计基础 {pool.ChoiceCount} 选一，不分别抽取。" + baselineNotice
                    : baselineNotice;
                summary.Add(new(ShanheEncounterRewardIcon(group.Category), group.Label, value, detail));
            }
        }
        return Array.AsReadOnly(summary.ToArray());
    }

    private static ImageSource? ShanheEncounterRewardIcon(ShanheRewardCategory category) => WushuangArt.Get(
        "rogue_award_type_icon_" + (category switch
        {
            ShanheRewardCategory.Tactic => 2, ShanheRewardCategory.Skill => 3, ShanheRewardCategory.Card => 4, _ => 0
        }) + ".png");

    private sealed record ShanheEncounterRewardPoolGroup(ShanheRewardCategory Category, string Label, int ChoiceCount);
    private sealed record ShanheEncounterRewardPoolPreview(IReadOnlyList<ShanheEncounterRewardPoolGroup> Groups,
        int ChoiceCount, bool PerCategory, bool OnlyTactics, ShanheRewardCategory? AutomaticCategory);

    private void CloseShanheEncounterPreview()
    {
        _shanhePreviewNode = null;
        _shanhePreviewBattleNode = null;
        _shanhePreviewEncounter = null;
        _shanheEncounterRewardSummary = [];
        _selectedShanheEncounterEnemy = null;
        NotifyShanheEncounterPreview();
    }

    private void NotifyShanheEncounterPreview()
    {
        foreach (var property in new[] { nameof(IsShanheEncounterPreviewOpen), nameof(ShanheEncounterTitle),
            nameof(ShanheEncounterDescription), nameof(ShanheEncounterRouteHint), nameof(ShanheEncounterEnemyName), nameof(ShanheEncounterEnemyPortraitName), nameof(ShanheEncounterEnemyPortrait),
            nameof(ShanheEncounterEnemyDetail), nameof(ShanheEncounterRewardText), nameof(ShanheEncounterRewardSummary), nameof(ShanheEncounterEnemyCards),
            nameof(ShanheEncounterEnemySkills), nameof(ShanheEncounterEnemyStats), nameof(ShanheEncounterInitiativeText),
            nameof(ShanheEncounterBackdrop), nameof(ShanheEncounterMainEnemyCards), nameof(ShanheEncounterMinorEnemyCards),
            nameof(HasShanheEncounterMinorEnemies), nameof(HasShanheEncounterEnemySelection),
            nameof(ShanheEncounterSelectedEnemyLabel) }) RaisePropertyChanged(property);
        (ChallengeShanheEncounterCommand as RelayCommand)?.NotifyCanExecuteChanged();
        (SelectShanheEncounterEnemyCommand as RelayCommand<ShanheEncounterEnemyCard>)?.NotifyCanExecuteChanged();
    }

    private void RefreshShanheJourney()
    {
        RefreshShanheRestChoices();
        NotifyShanheJourneyLoadout();
        if (_shanhePreviewNode is { } node && !IsCurrentShanheNode(node)) CloseShanheEncounterPreview();
        foreach (var property in new[] { nameof(IsShanheRunInProgress), nameof(IsShanhePreparation), nameof(CanPrepareShanheExpedition),
            nameof(ShanhePreparationHint), nameof(ShanheCodexPhaseText), nameof(ShanheChapterTitle), nameof(ShanheChapterProgressText),
            nameof(ShanheChapterObjectiveText), nameof(CurrentShanheGeneralPortrait), nameof(CurrentShanheGeneralName),
            nameof(CurrentShanheGeneralSkillsText) }) RaisePropertyChanged(property);
        foreach (var command in new[] { StartShanheCommand, UnlockCodexNodeCommand, EquipCodexTacticCommand, RemoveCodexTacticCommand })
            (command as RelayCommand)?.NotifyCanExecuteChanged();
    }

    private static string ShanheNpcEquipmentText(ShanheCombatantDefinition combatant) =>
        combatant.StartingEquipment is { Count: > 0 } equipment
            ? "\n初始装备：" + string.Join(" · ", equipment.Select(card => $"【{CardCatalog.Get(card.Kind).DisplayName}】"))
            : string.Empty;

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
        _shanheChapterCompleted = nodes.Count(node => node.Kind != ShanheNodeKind.Shop && completed.Contains(node.Id));
        return nodes;
    }
}

public sealed record ShanheEncounterRewardSummaryItem(ImageSource? Icon, string Title, string ValueText, string Detail);
public sealed record ShanheEncounterSkill(string Name, string Description);
public sealed record ShanheEncounterEnemyCard(string Name, Brush Portrait, string Stats, IReadOnlyList<ShanheEncounterSkill> Skills)
{
    private static readonly Brush HealthyHealthBrush = FrozenHealthBrush("#6FBD48");
    private static readonly Brush WarningHealthBrush = FrozenHealthBrush("#EFC64E");
    private static readonly Brush CriticalHealthBrush = FrozenHealthBrush("#EF7264");

    public string VerticalName => string.Join("\n", Name.ToCharArray());
    public int Hp { get; init; }
    public int MaxHp { get; init; }
    public bool HasHealth => MaxHp > 0;
    public string HealthValue => HasHealth ? $"{Hp}/{MaxHp}" : "体力未定义";
    public bool ShowNumericHealth => HasHealth && (MaxHp > 5 || Hp < 0);
    public string NumericHealthText => HasHealth ? $"{Hp}\n/\n{MaxHp}" : string.Empty;
    public Brush HealthBrush => Hp <= 1 ? CriticalHealthBrush : Hp == 2 ? WarningHealthBrush : HealthyHealthBrush;
    public IReadOnlyList<string> HealthImages
    {
        get
        {
            var count = !HasHealth ? 0 : ShowNumericHealth ? 1 : Math.Clamp(MaxHp, 0, 5);
            var filled = ShowNumericHealth ? Hp > 0 ? 1 : 0 : Math.Clamp(Hp, 0, count);
            return Array.AsReadOnly(Enumerable.Range(0, count).Select(index => index >= count - filled
                ? "pack://siteoforigin:,,,/Assets/gallery-hp-wu.png"
                : "pack://siteoforigin:,,,/Assets/gallery-hp-empty.png").ToArray());
        }
    }

    private static Brush FrozenHealthBrush(string color)
    {
        var brush = (SolidColorBrush)new BrushConverter().ConvertFromString(color)!;
        brush.Freeze();
        return brush;
    }

    public int SeatOrdinal { get; init; }
    public bool? IsCommander { get; init; }
    public string RoleLabel => IsCommander is true ? "主帅" : IsCommander is false ? "前锋" : "敌将";
    public string AccessibleName => $"{SeatOrdinal}号敌席 · {RoleLabel} · {Name}";
    public string FullDetails => Name + "\n" + Stats + "\n" + string.Join("\n", Skills.Select(skill => skill.Name + "：" + skill.Description));
    public ImageSource? IdentityArtwork => IsCommander is { } commander
        ? AssetImages.Load("Assets/Shanhe/Encounter/figure_cool_" + (commander ? "master" : "winger") + ".png") : null;
    public bool HasPortrait => Portrait is ImageBrush { ImageSource: not null };
    public string Glyph => Name[..Math.Min(2, Name.Length)];
}
