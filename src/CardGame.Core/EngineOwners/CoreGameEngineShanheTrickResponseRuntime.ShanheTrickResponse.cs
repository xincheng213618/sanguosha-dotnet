#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheTrickResponseRuntime
{
    internal bool IsShanheTrickResponseProhibited(int sourceSeat, int responderSeat, CardKind incomingKind, IReadOnlyList<int> targetSeats)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(responderSeat) || !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[responderSeat].IsAlive || incomingKind == CardKind.Nullification || !CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(incomingKind))
            return false;
        var source = _coreMatchParticipantsPlayersAccess._players[sourceSeat];
        return CoreGameEngineShanheSchoolTrickRuntime.IsShanheSchoolDamageTrick(incomingKind) && _coreTeamActivityProgramsOperations.PublicTeamEnemy(sourceSeat, _coreMatchParticipantsPlayersAccess._players[responderSeat]) && _coreProgramCardPoliciesOperations.HasCardPolicy(source, SkillProgramCardPolicyKind.ProhibitEnemyDamageTrickResponse, incomingKind) || targetSeats.Contains(responderSeat) && _coreProgramCardPoliciesOperations.HasCardPolicy(source, SkillProgramCardPolicyKind.ProhibitTargetOrdinaryTrickResponse, incomingKind);
    }

    internal bool ShanheTrickNullificationBanned(int responderSeat) => _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { ChainDepth: 0 } window && IsShanheTrickResponseProhibited(window.SourceSeat, responderSeat, window.EffectCardKind, window.TargetSeats);
    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 3905)]
    internal bool ShanheTrickNativeResponseForbidden(ProgramNativeResponseQuery query)
    {
        if (query.DyingUse)
            return false;
        if (query.Kind == CardKind.Nullification)
            return ShanheTrickNullificationBanned(query.Provider.Seat);
        if (_resolutionStackOperations._resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault()is { } response && response.RequiredCardKind is { } required && CoreNativeCardClassificationsRuntime.MatchesRequiredCard(query.Kind, required) && _coreCardUseLifecycleOperations.LifecycleCardUse(response.ParentFrameId)is { } use && IsShanheTrickResponseProhibited(use.SourceSeat, response.ResponderSeat, response.IncomingCard, use.TargetSeats))
            return true;
        // QiZheng owns its native response directly on the original use rather
        // than allocating a second generic response window.
        return (CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) || query.Kind == CardKind.Dodge) && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()is { Qizheng: { Stage: QizhengStage.Response } qizheng } native && IsShanheTrickResponseProhibited(native.SourceSeat, qizheng.Target, CardKind.QizhengXiangsheng, native.TargetSeats);
    }
}
