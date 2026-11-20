#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouShiMouMobileRuntime
{
    internal bool ValidMouXiahouShiQiaoshi(ProgramSkillFrame f)
    {
        if (f.MouXiahouShiQiaoshi is not { } r || f.MouXiahouShiYanyu is not null || r.Source != MouXiahouShiSource(f) || r.GameplayHash != f.GameplayHash || !ExactMouXiahouShiDamageContext(f) || !Enum.IsDefined(r.Stage) || r.WindowId != f.WindowContext!.ParentFrameId || r.DamageFrameId != f.WindowContext.DamageFrameId || r.DamageSourceSeat != f.WindowContext.SourceSeat || r.DamageAmount != f.WindowContext.Amount || r.DamageSourceSeat == f.OwnerSeat || r.Before < 0 || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } p || p.GameplayHash != f.GameplayHash || !MouXiahouShiComposition.IsQiaoshi(p))
            return false;
        var start = new MouXiahouShiQiaoshiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.WindowId, r.DamageFrameId, r.AttackResolutionId, r.DamageSourceSeat, r.DamageAmount);
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouShiQiaoshiStartedEvent>().Count(e => e == start) != 1 || !MouXiahouShiTurnBefore(start, r.Turn))
            return false;
        var branches = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouXiahouShiQiaoshiBranchAcceptedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Branch is null)
            return r.Stage == MouXiahouShiQiaoshiStage.SourceChoice && branches.Length == 0 && r.Draw is null && f.OperationRecovery is null;
        if (r.Branch is not ("recover-and-draw" or "decline") || branches is not [var branch] || branch != new MouXiahouShiQiaoshiBranchAcceptedEvent(f.Id, r.Source, r.Turn, r.DamageSourceSeat, r.Branch) || r.Stage == MouXiahouShiQiaoshiStage.SourceChoice)
            return false;
        if (r.Branch == "decline")
            return r.Stage == MouXiahouShiQiaoshiStage.RecoveryChildren && r.Draw is null && f.OperationRecovery is null;
        if (f.OperationRecovery is not { } recovery || !recovery.BelongsTo(f) || recovery.SourceSeat != r.DamageSourceSeat ||
            recovery.TargetSeat != f.OwnerSeat || recovery.RequestedAmount != r.DamageAmount ||
            recovery.NativeFrameId is { } childId && childId <= f.Id ||
            recovery.Result is { OriginalRecovered: < 0 } or { RedirectedRecovered: < 0 } or
                { RedirectedSeat: not null, OriginalRecovered: > 0 } or { RedirectedSeat: null, RedirectedRecovered: > 0 } ||
            recovery.NativeFrameId is null && recovery.Result is not (null or { OriginalRecovered: 0, RedirectedSeat: null }) ||
            r.Stage is MouXiahouShiQiaoshiStage.Drawing or MouXiahouShiQiaoshiStage.DrawChildren && recovery.Result is null)
            return false;
        return r.Stage == MouXiahouShiQiaoshiStage.DrawChildren ? r.Draw is { } draw && MouXiahouShiDrawSigned(f, draw, r.DamageSourceSeat, 2) : r.Draw is null;
    }
}
