using System.Text.Json.Serialization;
namespace CardGame.Core;
public enum JiqiaoPaidStage { CostChildren,RevealChildren,ClaimChildren,CleanupChildren }
public sealed record JiqiaoPaidInvoice(CardConversionSource Source,string GameplayHash,long WindowId,int ActualTurnNumber,int PhaseInstanceId,int InstructionIndex,string CostBind,string RevealBind,int PaidCount,long CostBefore,long CostAfter);
public sealed record JiqiaoCostPaidEvent(long ProgramFrameId,JiqiaoPaidInvoice Invoice):IGameEvent;
public sealed record JiqiaoStageIssuedEvent(long ProgramFrameId,JiqiaoPaidStage Stage,long Before,long After,int ActualCount):IGameEvent;
public sealed record JiqiaoCompletedEvent(long ProgramFrameId,int PaidCount,int RevealedCount,int ClaimedCount,bool OwnerAlive):IGameEvent;
public sealed record JiqiaoPaidPending(JiqiaoPaidInvoice Invoice,JiqiaoPaidStage Stage,long Before,long After,int ActualCount,int RevealedCount=0,int ClaimedCount=0);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public JiqiaoPaidPending? JiqiaoPaid{get;init;}
}
