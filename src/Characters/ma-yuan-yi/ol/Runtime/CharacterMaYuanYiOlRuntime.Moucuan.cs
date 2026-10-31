#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaYuanYiOlRuntime
{
    internal int MoucuanFactionCount() => _coreMatchParticipantsPlayersAccess._players.Select(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId).OfType<string>().Distinct(StringComparer.Ordinal).Count();
    internal bool HasPaidMoucuanSourceLoss(ProgramSkillFrame f) => f.MaYuanYi is { Op: SkillProgramEffectOp.ResolveMoucuanAwakening, Stage: MaYuanYiStage.MaximumChildren } r && f.HpConditionalSourceReceipt is not null && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _coreProgramEventHistoryOperations.ProgramEventHistory<MoucuanMaximumPaidEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.MaximumBefore == r.MaximumBefore && e.MaximumAfter == r.MaximumBefore - 1 && e.HpAfter == Math.Min(r.HpBefore, r.MaximumBefore - 1)) == 1;
}
