namespace CardGame.Core;
public sealed record IssuedPlayPhaseUseProhibition(CardConversionSource Source, int ActorSeat, int TurnNumber, int PhaseInstanceId, long CardActionId);
public sealed record IssuedCardNoResponse(long CardActionId, long ParentFrameId, CardConversionSource Source, int TurnNumber, int PhaseInstanceId);
public sealed record IssuedCounterspellNode(long ActionId, int ChainDepth, IssuedCardNoResponse Policy);
public sealed record IssuedCardNoResponseEvent(IssuedCardNoResponse Effect) : IGameEvent;
