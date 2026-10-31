using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed record XionghuoGrant(CardConversionSource Source,string GameplayHash,long GrantFrameId,int TargetSeat,int Count);
public sealed record XuRongTurnRestriction(CardConversionSource Source,long PaidFrameId,int SubjectSeat,int ActualTurnNumber,int ActualTurnOwner,int PhaseInstanceId,int Branch);
public sealed record XuRongCausalEntity(int CardId,CardLocation OriginalLocation);
public sealed record XuRongDyingProof(long DyingFrameId,long ParentFrameId,int VictimSeat,int HpAtEntry,int RescueCardsRequired,long? DamageAttemptResolutionId,long? NativeActionId,int? CauseSourceSeat,bool SourceLess,IReadOnlyList<XuRongCausalEntity> Entities);
public sealed record XuRongPending(CardConversionSource Source,string GameplayHash,string Stage,int InstructionIndex,long ParentFrameId,int SubjectSeat,int ActualTurnNumber,int PhaseInstanceId,IReadOnlyList<XionghuoGrant> Grants,uint RandomBefore,uint RandomAfter,int Branch,long MovementBefore,long MovementAfter,IReadOnlyList<int> PaidCardIds,long? DyingFrameId=null,long? NativeActionId=null);
public sealed record XionghuoPoolChangedEvent(long FrameId,CardConversionSource Source,int Before,int After,string Kind):IGameEvent;
public sealed record XionghuoMarkTransferredEvent(long FrameId,XionghuoGrant Grant,int PoolBefore,int PoolAfter,int TargetBefore,int TargetAfter):IGameEvent;
public sealed record XionghuoPlayDebtPaidEvent(long FrameId,CardConversionSource Source,string GameplayHash,long PlayWindowId,int SubjectSeat,int ActualTurnNumber,int PhaseInstanceId,int RemovedCount,uint RandomBefore,uint RandomAfter,int Branch):IGameEvent;
public sealed record XuRongCardsPaidEvent(long FrameId,CardConversionSource Source,string Kind,long Before,long After,IReadOnlyList<int> CardIds):IGameEvent;
public sealed record XuRongCompletedEvent(long FrameId,CardConversionSource Source,string Stage,int SubjectSeat,long? DyingFrameId):IGameEvent;
public sealed record XuRongCausalClaimReceipt(long ProducerFrameId,CardConversionSource Source,string GameplayHash,long DyingFrameId,long NativeOwnerId,long? NativeActionId,int HpAtEntry,int RescueCardsRequired,IReadOnlyList<int> CardIds);
public sealed record XuRongCausalClaimIssuedEvent(XuRongCausalClaimReceipt Receipt):IGameEvent;
public sealed partial record CardUseFrame
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public IReadOnlyList<XuRongCausalClaimReceipt>? XuRongCausalClaims{get;init;}
}
public sealed partial record JudgmentFrame
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public IReadOnlyList<XuRongCausalClaimReceipt>? XuRongCausalClaims{get;init;}
}
public sealed partial class CharacterState
{
 private readonly List<XionghuoGrant> _xionghuoGrants=[];
 private readonly List<XuRongTurnRestriction> _xuRongRestrictions=[];
 internal IReadOnlyList<XionghuoGrant> XionghuoGrants=>_xionghuoGrants.AsReadOnly();
 internal IReadOnlyList<XuRongTurnRestriction> XuRongRestrictions=>_xuRongRestrictions.AsReadOnly();
 internal void AddXionghuoGrant(XionghuoGrant grant)=>_xionghuoGrants.Add(grant);
 internal bool RemoveXionghuoGrant(XionghuoGrant grant)=>_xionghuoGrants.Remove(grant);
 internal void AddXuRongRestriction(XuRongTurnRestriction r)=>_xuRongRestrictions.Add(r);
 internal void ExpireXuRongRestrictions(int turn)=>_xuRongRestrictions.RemoveAll(r=>r.ActualTurnNumber<=turn);
}
public sealed partial record ProgramSkillFrame
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public XuRongPending? XuRong{get;init;}
}
public sealed partial record DyingFrame
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public XuRongDyingProof? XuRongProof{get;init;}
}
public sealed partial record ProgramSkillWindowContext
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public XuRongDyingProof? XuRongDying{get;init;}
}
