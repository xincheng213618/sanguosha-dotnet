#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineMouLuMengRuntime
{
    internal const string RoundDamageCardHistorySkillId = "system:round-damage-card-history";
    internal const string RoundDamageCardUsagePrefix = global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_MouLuMeng.RoundDamageCardUsagePrefix;
    internal bool UsesFormalMouLuMeng => _coreCharacterHostOperations.HasClassicGeneralPackage;

    internal void BeginRoundForTurn(CharacterState current)
    {
        if ((!_modeRulesOperations._modeRules.RequiresRoundTracking && !UsesFormalMouLuMeng && !_contentRegistryOperations._contentRegistry.ProgramDependencies.UsesRoundTracking && !_coreShanheSourceDifficultyOperations.RequiresShanheSourceArmorTracking) || _coreActualTurnProgressTurnProgressionAccess._turnProgression.Kind == ActualTurnKind.Extra)
            return;
        if (_coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber == 0 || _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Contains(current.Seat))
        {
            if (!_coreRoundEndingOperations.TracksRoundEnding || !_coreProgramEventHistoryOperations.ProgramEventHistory<RoundEndedEvent>().Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber))
                _characterGuanYuRuntimeOperations.ReturnDemonRoundHolds();
            _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber++;
            _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Clear();
            _skillRuntimeStateOperations._skillRuntimeState.ResetRound();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundStartedEvent(_coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber, current.Seat));
            _coreShanheSourceDifficultyOperations.RestoreShanheSourceRoundArmor();
            _coreMatchLogOperations.AddLog("RoundStarted", $"第 {(_coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber)} 轮开始。", current.Seat);
        }

        _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Add(current.Seat);
    }

    internal static bool IsDamageCardKind(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_MouLuMeng.IsDamageCardKind(kind);
    internal static string GetRoundDamageCardUsageId(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_MouLuMeng.GetRoundDamageCardUsageId(kind);
}
