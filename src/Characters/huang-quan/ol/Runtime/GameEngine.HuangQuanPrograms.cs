using System.Globalization;
using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed record DianhuRelation(long FrameId,CardConversionSource Source,string GameplayHash,int TargetSeat,long GameStartingWindowId);
public sealed record DianhuOccurrence(DianhuRelation Relation,bool Recovery,long NativeChangeId,long NativeParentId,int TargetSeat,int ActualAmount,int? SourceSeat);
public sealed record DianhuArmedEvent(DianhuRelation Relation):IGameEvent;
public sealed record DianhuQualifiedEvent(DianhuOccurrence Receipt):IGameEvent;
public sealed record HuangQuanDrawPaidEvent(long FrameId,CardConversionSource Source,string GameplayHash,string Kind,int RecipientSeat,int ActualTurn,int TurnOwner,long ActualPhase,long Before,long After,int CardCount,DianhuOccurrence? Occurrence):IGameEvent;
public sealed record HuangQuanCompletedEvent(long FrameId,CardConversionSource Source,string Kind,long? NativeActionId):IGameEvent;
public sealed record JianjiNativeReceipt(long ProgramFrameId,int InstructionIndex,CardConversionSource Source,string GameplayHash,int ActualTurn,int TurnOwner,long ActualPhase,int ActorSeat,int CardId,LegalAction Action,long UseId,long ActionId);
public sealed record JianjiUseIssuedEvent(JianjiNativeReceipt Receipt):IGameEvent;
public sealed record JianjiUseReturnedEvent(JianjiNativeReceipt Receipt):IGameEvent;
public sealed record HuangQuanPending(CardConversionSource Source,string GameplayHash,int InstructionIndex,string Kind,int RecipientSeat,int ActualTurn,int TurnOwner,long ActualPhase,long Before,long After,IReadOnlyList<int> CardIds,DianhuOccurrence? Occurrence,string Stage,JianjiNativeReceipt? NativeUse=null);
public sealed partial class CharacterState
{
 private IReadOnlyList<DianhuRelation> _dianhuRelations=Array.AsReadOnly(Array.Empty<DianhuRelation>());
 internal IReadOnlyList<DianhuRelation> DianhuRelations{get=>_dianhuRelations;set=>_dianhuRelations=Array.AsReadOnly(value.ToArray());}
}
public sealed partial record ProgramSkillFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public HuangQuanPending? HuangQuan{get;init;}}
public sealed partial record ProgramSkillWindowContext
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public DianhuOccurrence? Dianhu{get;init;}}
public sealed partial record CardUseFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public JianjiNativeReceipt? JianjiUse{get;init;}}

public sealed record JianjiOrdinaryAlcoholPaidEvent(long NativeUseId,long ActionId,int ActualTurn,int ActorSeat,bool CountsQuota,long? JianjiProgramFrameId):IGameEvent;
