#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenDengOlRuntime
{
    internal bool IsXuanhuiDisabledByOwnSwap(int ownerSeat)
    {
        if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramXuanhuiEffectsSwappedEvent>().Any(item => item.OwnerSeat == ownerSeat))
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var swapIndex = Array.FindLastIndex(facts, item => item is ProgramXuanhuiEffectsSwappedEvent swapped && swapped.OwnerSeat == ownerSeat);
        if (swapIndex < 0)
            return false;
        var deathIndex = Array.FindLastIndex(facts, item => item is PlayerDiedEvent);
        return deathIndex < swapIndex;
    }

    internal bool IsFengjiSwapAvailable(int ownerSeat)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || IsXuanhuiDisabledByOwnSwap(ownerSeat))
            return false;
        return CurrentRoundFengjiOptions(ownerSeat).Any(option => option.Accepted && option.RecipientSeat != ownerSeat && _coreMatchParticipantsPlayersAccess._players[option.RecipientSeat].IsAlive);
    }

    internal SkillProgramStepOutcome XuanhuiProgramSwapEffects(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (!owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualRoundProgressRoundNumberAccess._roundNumber == 0 || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active) || IsXuanhuiDisabledByOwnSwap(active.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        var swapped = CurrentRoundFengjiOptions(active.OwnerSeat).Where(option => option.Accepted && option.RecipientSeat != active.OwnerSeat).ToArray();
        if (swapped.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramXuanhuiEffectsSwappedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, swapped.Length));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 旋回：与 {(_coreMatchParticipantsPlayersAccess._players[swapped[0].RecipientSeat].Name)} 等 {(swapped.Length)} 项丰积效果交换，【旋回】失效直到一名角色死亡。", active.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.Continue;
    }
}
