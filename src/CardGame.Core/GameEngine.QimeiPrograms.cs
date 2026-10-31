using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record QimeiPairGrant(long ProgramFrameId,CardConversionSource Source,string GameplayHash,int ActualTurnNumber,int TargetSeat);
public sealed record QimeiPairArmedEvent(QimeiPairGrant Grant):IGameEvent;
public sealed record QimeiPairExpiredEvent(long ProgramFrameId,int ActualTurnNumber):IGameEvent;
public enum QimeiAttribute { Hand, Hp }
public sealed record QimeiEqualityReceipt(QimeiPairGrant Grant,long NativeChangeId,QimeiAttribute Attribute,int ActualTurnNumber,
    int ChangedSeat,int BeneficiarySeat,int Before,int After,int OtherValue);
public sealed record QimeiEqualityQualifiedEvent(QimeiEqualityReceipt Receipt):IGameEvent;
public sealed record QimeiEqualityConsumedEvent(long ProgramFrameId,QimeiEqualityReceipt Receipt):IGameEvent;
public sealed record QimeiEqualityDrawIssuedEvent(long ProgramFrameId,QimeiEqualityReceipt Receipt,long Before,long After,int ActualCount):IGameEvent;
public sealed record QimeiDrawPending(QimeiEqualityReceipt Receipt,int InstructionIndex,long Before,long After,int ActualCount);
public sealed partial record ProgramSkillWindowContext
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)] public QimeiEqualityReceipt? QimeiEquality {get;init;}
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)] public QimeiDrawPending? QimeiDraw {get;init;}
}
