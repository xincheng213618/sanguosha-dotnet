namespace CardGame.Core;
public sealed record IssuedPlayPhaseUseProhibition(CardConversionSource Source, int ActorSeat, int TurnNumber, int PhaseInstanceId, long CardActionId);
public sealed record IssuedCardNoResponse(long CardActionId, long ParentFrameId, CardConversionSource Source, int TurnNumber, int PhaseInstanceId);
public sealed record IssuedCounterspellNode(long ActionId, int ChainDepth, IssuedCardNoResponse Policy);
public sealed record IssuedCardNoResponseEvent(IssuedCardNoResponse Effect) : IGameEvent;
public sealed partial class GameEngine
{
    // Issued phase facts survive source loss/suppression until this actual phase ends.
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.IssuedPlayPhaseUseProhibition> _issuedPlayPhaseUseProhibitions => ref _issuedPlayUsePolicies.ReferenceToIssuedPlayPhaseUseProhibitions;
}
