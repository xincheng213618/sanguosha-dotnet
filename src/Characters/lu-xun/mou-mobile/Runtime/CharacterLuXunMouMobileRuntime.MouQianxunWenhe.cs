#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunMouMobileRuntime
{
    internal bool TryBeginMouLuXunWenheTarget(ProgramSkillFrame frame, int actorSeat, int hpAmount)
    {
        if (!HasMouLuXun || frame.WenheChaos is not { CardActionId: not null } chaos || !_coreWenheChaosOperations.Value.IsExactWenheChaos(frame) || _coreCardUseLifecycleOperations.LifecycleCardUse(chaos.ParentFrameId)?.Action is not { } action || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(chaos.ParentFrameId, actorSeat) || !CanMouLuXunRecordNativeTarget(actorSeat, action))
            return false;
        var cursor = frame.NumberBindings.Single(b => b.Name == "participant-cursor-" + (frame.InstructionIndex - 1)).Value;
        return _coreCardEffectProgramsOperations.BeginCardEffectPrograms(action, [actorSeat], null, null, new(chaos.ParentFrameId, actorSeat, cursor, WenheProgramFrameId: frame.Id, HpAmount: hpAmount), perTargetOnly: true);
    }

    internal bool ValidMouLuXunWenheTarget(NativeTrickTargetEffectReturn receipt, CardActionContext action) => receipt.GroupEffect is null && receipt.IronChainReturn is null && receipt.HpAmount == 1 && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == receipt.WenheProgramFrameId)is { } frame && _coreWenheChaosOperations.Value.IsExactWenheChaos(frame) && frame.WenheChaos is { CardActionId: not null } chaos && chaos.ParentFrameId == receipt.UseId && chaos.CardActionId == action.ActionId && receipt.TargetIndex >= 1 && receipt.TargetIndex <= chaos.ActorSeats.Count && chaos.ActorSeats[receipt.TargetIndex - 1] == receipt.TargetSeat && frame.SelectedTargetSeats.SequenceEqual([receipt.TargetSeat]) && frame.NearestLegalSlashRequest is null && frame.ReexecuteParticipantInstruction && frame.NumberBindings.SingleOrDefault(b => b.Name == "participant-cursor-" + (frame.InstructionIndex - 1))?.Value == receipt.TargetIndex;
    internal void ReturnMouLuXunWenheTarget(NativeTrickTargetEffectReturn receipt)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(receipt.WenheProgramFrameId!.Value);
        if (!_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(receipt.UseId, receipt.TargetSeat))
        {
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var outcome = _coreNearestLegalSlashRequestsOperations.RequestLegalSlashByNearestActor(frame, receipt.TargetSeat, receipt.HpAmount, trickEffectReturned: true);
        if (outcome == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
