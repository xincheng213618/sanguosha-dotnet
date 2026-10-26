namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool IsShanheTrickResponseProhibited(int sourceSeat, int responderSeat,
        CardKind incomingKind, IReadOnlyList<int> targetSeats)
    {
        if (!IsValidPlayerSeat(sourceSeat) || !IsValidPlayerSeat(responderSeat) ||
            !_players[sourceSeat].IsAlive || !_players[responderSeat].IsAlive ||
            incomingKind == CardKind.Nullification || !IsOrdinaryTrick(incomingKind)) return false;
        var source = _players[sourceSeat];
        return IsShanheSchoolDamageTrick(incomingKind) && PublicTeamEnemy(sourceSeat, _players[responderSeat]) &&
                HasCardPolicy(source, SkillProgramCardPolicyKind.ProhibitEnemyDamageTrickResponse, incomingKind) ||
            targetSeats.Contains(responderSeat) &&
                HasCardPolicy(source, SkillProgramCardPolicyKind.ProhibitTargetOrdinaryTrickResponse, incomingKind);
    }

    // An actual counterspell responds to the last counterspell. The original
    // trick's enemy/target scope must never leak into a later chain node.
    private bool ShanheTrickNullificationBanned(int responderSeat) =>
        ActiveNullificationWindow is { ChainDepth: 0 } window &&
        IsShanheTrickResponseProhibited(window.SourceSeat, responderSeat,
            window.EffectCardKind, window.TargetSeats);

    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 3905)]
    private bool ShanheTrickNativeResponseForbidden(ProgramNativeResponseQuery query)
    {
        if (query.DyingUse) return false;
        if (query.Kind == CardKind.Nullification)
            return ShanheTrickNullificationBanned(query.Provider.Seat);
        if (_resolutionStack.OfType<ResponseWindowFrame>().LastOrDefault() is { } response &&
            response.RequiredCardKind is { } required && MatchesRequiredCard(query.Kind, required) &&
            LifecycleCardUse(response.ParentFrameId) is { } use &&
            IsShanheTrickResponseProhibited(use.SourceSeat, response.ResponderSeat,
                response.IncomingCard, use.TargetSeats)) return true;
        // QiZheng owns its native response directly on the original use rather
        // than allocating a second generic response window.
        return (IsSlashCard(query.Kind) || query.Kind == CardKind.Dodge) &&
            _resolutionStack.OfType<CardUseFrame>().LastOrDefault() is
                { Qizheng: { Stage: QizhengStage.Response } qizheng } native &&
            IsShanheTrickResponseProhibited(native.SourceSeat, qizheng.Target,
                CardKind.QizhengXiangsheng, native.TargetSeats);
    }
}
