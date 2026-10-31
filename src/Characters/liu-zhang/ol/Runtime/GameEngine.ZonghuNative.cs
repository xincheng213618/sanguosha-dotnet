namespace CardGame.Core;
public sealed record ZonghuKeActionAcceptedEvent(long ProgramId,int InstructionIndex,CardConversionSource RequestSource,string RequestHash,long AttemptId,CardKind OutputKind,int TargetSeat):IGameEvent;
