#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreMatchSetupRuntime
{
    internal void StartCore()
    {
        if (_coreActualTurnProgressStartedAccess._started)
        {
            throw new InvalidOperationException("The game has already started.");
        }

        _coreActualTurnProgressStartedAccess._started = true;
        _modeRuntimeOperations._modeRuntime.OnMatchStart(ModeMatchStartPhase.BeforeRunning);
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _modeRuntimeOperations._modeRuntime.OnMatchStart(ModeMatchStartPhase.AfterRunning);
        var presentation = _modeRuntimeOperations._modeRuntime.Find<IMatchModePresentation>() ?? throw new InvalidOperationException("The mode requires its installed presentation feature.");
        _coreMatchLogOperations.AddLog("GameStarted", presentation.StartLog(_coreModePresentationOperations.Value.CaptureModePresentationContext()));
        if (_coreMatchSetupSetupCompleteAccess._setupComplete)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameStartedEvent(_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._playerCount, _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id));
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => player.ChosenFactionId is not null).OrderBy(player => player.Seat))
            {
                _coreMatchLogOperations.AddLog("GodFactionSelected", $"{(player.Name)} 为神势力武将 {(player.General.Name)} 选择了{(CoreMatchPresentationRuntime.GetFactionName(player.ChosenFactionId!))}势力。", player.Seat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GodFactionSelectedEvent(player.Seat, player.ChosenFactionId!));
            }
        }
        else
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SetupStartedEvent(_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._playerCount, _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id));
        }

        if (_modeRulesOperations._modeRules.RevealsTeamsOnStart)
        {
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.OrderBy(player => player.Seat))
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TeamAssignedEvent(player.Seat, player.TeamId!));
            }
        }

        _modeRuntimeOperations._modeRuntime.OnMatchStart(ModeMatchStartPhase.AfterOpeningFacts);
        var dyingAlcoholRules = ("酒可使本回合下一张直接杀伤害 +1，也可在濒死时仅自救 1 点体力");
        var targetCardRules = ("过河拆桥和顺手牵羊的手牌效果由使用者选择不透明牌位，目标牌面不公开");
        var supplyShortageTargetRules = ("对距离为 1 的其他角色使用，奇才可忽略此距离限制");
        var delayedCardRules = ($"乐不思蜀置入目标判定区并在其下回合摸牌前判定，结果不为红桃时跳过出牌阶段；兵粮寸断{(supplyShortageTargetRules)}，置入目标判定区并在其下回合摸牌前判定，结果不为梅花时跳过摸牌阶段；闪电置于自己的判定区，判定为黑桃 2 至 9 时受到 3 点雷电伤害，否则移至下一名存活角色。");
        var modeRules = presentation.RulesText(_coreModePresentationOperations.Value.CaptureModePresentationContext());
        var weaponRules = ("诸葛连弩攻击范围为 1 且杀不受次数限制，青釭剑攻击范围为 2 且使直接杀无视目标防具");
        var armorRules = ("八卦阵在需要使用或打出闪时可选择发动判定，红色判定牌视为闪，仁王盾使黑色杀在指定目标后无效");
        var fireAttackRules = ("火攻可对自己使用，展示牌仍留在目标手牌中");
        var alcoholPlayRules = ("出牌阶段每回合限使用一次酒");
        _coreMatchLogOperations.AddLog("Rules", $"{(modeRules)}；模式 {(_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id)}；牌堆含杀、火杀、雷杀、闪、桃、酒、决斗、无中生有、南蛮入侵、万箭齐发、桃园结义、五谷丰登、过河拆桥、顺手牵羊、火攻、乐不思蜀、兵粮寸断、无懈可击{((_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.ShanheBattle is null ? "和七种装备牌" : "；山河图牌堆不含装备，随身装备开战直接装备，可正常夺取敌方装备"))}。默认战斗距离按存活座位环计算、攻击范围为 1；装备按五类槽位公开替换，{(weaponRules)}，赤兔和绝影修正战斗距离，玉玺额外摸一张，{(armorRules)}。{(dyingAlcoholRules)}，{(alcoholPlayRules)}，桃可在出牌阶段自救或在基础濒死窗口救援，桃园结义按座次使所有存活角色各回复 1 点体力，五谷丰登公开翻牌并按座次私有选牌，{(fireAttackRules)}，并通过攻击者同花色弃牌决定是否造成火焰伤害，无懈可击在可抵消锦囊结算前按座次进入有限多层响应窗口，{((_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.UseInteractiveDiscard ? "人类回合末弃牌由玩家选择，AI 自动处理" : "弃牌自动处理"))}。");
        _coreMatchLogOperations.AddLog("Rules", $"{(targetCardRules)}。");
        _coreMatchLogOperations.AddLog("Rules", delayedCardRules);
        _coreMatchLogOperations.AddLog("Rules", "闪电仅可对自己使用：黑桃 2 至 9 命中并造成 3 点雷电伤害，其他判定牌则移至下一名存活角色。 ");
        if (!_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.UseInteractiveSetup && _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.ConfirmOpeningHand && _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.HumanSeat >= 0)
            _coreOpeningCustomizationOperations.Value.RequestOpeningHandConfirmation();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }
}
