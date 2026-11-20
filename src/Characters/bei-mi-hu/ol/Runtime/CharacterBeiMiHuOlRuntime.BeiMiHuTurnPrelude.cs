#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBeiMiHuOlRuntime
{
    internal bool HasBeiMiHuPreludeBindings(CharacterState player, SkillProgramTriggerWindow window, SkillProgramEffectOp operation) => player.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(player).GetInstanceTriggers(window).Any(binding => ProgramInstructionResolver.Default.Features(binding.Trigger).HasOperation(operation));
    internal bool TryBeginBeiMiHuActualTurnPrelude(CharacterState owner)
    {
        if (!NeedsBeforeTurnPrelude(owner))
            return false;
        var id = _coreActualTurnsOperations.OpenBeforeTurnPrelude(owner);
        _characterLuSuMobileShiOperations.ExpireShiLsHandGrants(owner.Seat, id);
        _coreRuntimeOperations.AdvanceRuntimeFrame(id);
        return true;
    }

    private bool NeedsBeforeTurnPrelude(CharacterState owner)
    {
        const SkillProgramTriggerWindow ownWindow = SkillProgramTriggerWindow.TurnBeforeStart;
        const SkillProgramTriggerWindow roundWindow = SkillProgramTriggerWindow.RoundStarting;

        return HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.MarkUnmarkedOtherBeforeActualTurn) ||
            _coreMatchParticipantsPlayersAccess._players.Any(player =>
                HasBeiMiHuPreludeBindings(player, roundWindow, SkillProgramEffectOp.MarkLowestHpUnmarkedOtherAtRoundStart)) ||
            _coreMatchParticipantsPlayersAccess._players.Any(player =>
                HasBeiMiHuPreludeBindings(player, roundWindow, SkillProgramEffectOp.ResolveAnniversaryLiuLiDehua)) ||
            (_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn) &&
                HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.AssignOrderedDragonPhoenixOnFirstActualTurn) &&
                _eventHistory.CountActualTurnStarts(owner.Seat) == 0) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ResolveShenTaishiCiPowei) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ResolveHappyTaishiCiChongwei) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ObtainHappyTaishiCiStartSlash) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.EditDinghanNames) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.AwakenJianghuo) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ObtainWushuangDeckCards) ||
            HasBeiMiHuPreludeBindings(owner, ownWindow, SkillProgramEffectOp.ResolveMobileShiGuoYuanXiugeng) ||
            _characterLuSuMobileShiOperations.HasShiLsHandGrantsToExpire(owner.Seat) ||
            _characterChenZhiMobileShiOperations.ShiCzPendingExtra(owner.Seat) is not null ||
            _characterDongZhaoMobileShiOperations.ShiDzPendingExtra(owner.Seat) is not null;
    }
}
