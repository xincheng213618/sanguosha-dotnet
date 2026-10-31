#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NextActualUseSelectionQuery(Func<RuntimeEntries.CoreGameEngineHpLossSlashMaterialsEntries> getCoreGameEngineHpLossSlashMaterialsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, Func<RuntimeEntries.CoreGameEngineCardConversionsEntries> getCoreGameEngineCardConversionsRuntime, Func<RuntimeEntries.CoreGameEngineNextActualUseTargetAdjustmentsEntries> getCoreGameEngineNextActualUseTargetAdjustmentsRuntime, Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, Func<IProgramSelectionQueries> getIProgramSelectionQueries) : INextActualUseSelectionQuery
{
    public bool HasHpLossTargets(int ownerSeat) => getCoreGameEngineHpLossSlashMaterialsRuntime().HasHpLossSlashTargets(matchParticipantsState.ReferenceToPlayers[ownerSeat]);
    public SkillProgramEffect? HpLossProducer(ProgramExecutionPlan plan) => EngineOwners.CoreGameEngineHpLossSlashMaterialsRuntime.HpLossSlashProgramProducer(plan);
    public int HpLossMaximum(int ownerSeat, CardKind kind) => getCoreGameEngineHpLossSlashMaterialsRuntime().HpLossSlashMaximum(matchParticipantsState.ReferenceToPlayers[ownerSeat], kind);
    public SkillProgramEffect? SelectedProducer(ProgramExecutionPlan plan) => EngineOwners.CoreGameEngineNextActualUseTargetAdjustmentsRuntime.NextActualUseSelectedProducer(plan);
    public string SkillInstance(int ownerSeat, string skillId) => getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(matchParticipantsState.ReferenceToPlayers[ownerSeat], skillId);
    public ProgramMultiCardViewAsSelection? FindSelection(int ownerSeat, IReadOnlyList<int> cards, CardKind kind, CardConversionSource source) => ProgramSelectionProjection.Freeze(getCoreGameEngineCardConversionsRuntime().FindProgramMultiCardViewAsSelection(matchParticipantsState.ReferenceToPlayers[ownerSeat], cards, kind, false, source));
    public bool HasAdjustment(int ownerSeat) => getCoreGameEngineNextActualUseTargetAdjustmentsRuntime().CurrentNextActualUseAdjustment(ownerSeat)is not null;
    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool CanUseSlash(int ownerSeat, int targetSeat, ProgramMultiCardViewAsSelection selection) => getCoreGameEngineNextActualUseTargetAdjustmentsRuntime().CanUseNextActualUseSelectedSlash(matchParticipantsState.ReferenceToPlayers[ownerSeat], matchParticipantsState.ReferenceToPlayers[targetSeat], selection);
    public IReadOnlyList<int> Targets(int ownerSeat, ProgramExecutionPlan plan, ProgramMultiCardViewAsSelection selection) => Array.AsReadOnly(AdjustedUseSelection.SelectTargets(ownerSeat, plan, [], [selection], getIProgramSelectionQueries().Get<IProgramSelectionSourceQuery>()).ToArray());
}
