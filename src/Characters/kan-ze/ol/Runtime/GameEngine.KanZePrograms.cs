using System.Text.Json.Serialization;
namespace CardGame.Core;
public enum XiashuStage { GiftChildren,RecipientReveal,OwnerClaim,ClaimChildren }
public sealed record XiashuInvoice(CardConversionSource Source,string GameplayHash,long WindowId,int InstructionIndex,int TurnNumber,int RecipientSeat,long GiftBefore,long GiftAfter,int GiftCount);
public sealed record XiashuPaidPending(XiashuInvoice Invoice,XiashuStage Stage,long Before,long After,int Count,IReadOnlyList<int>? SelectedReveal=null)
{
 private readonly IReadOnlyList<int> _selectedReveal=Array.AsReadOnly((SelectedReveal??[]).ToArray());
 public IReadOnlyList<int> SelectedReveal {get=>_selectedReveal;init=>_selectedReveal=Array.AsReadOnly((value??[]).ToArray());}
}
public sealed record XiashuGiftPaidEvent(long FrameId,XiashuInvoice Invoice):IGameEvent;
public sealed record XiashuClaimPaidEvent(long FrameId,long Before,long After,int Count,bool Shown):IGameEvent;
public sealed record XiashuCompletedEvent(long FrameId,int GiftCount,int ClaimCount):IGameEvent;
public sealed partial record ProgramSkillFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public XiashuPaidPending? XiashuPaid{get;init;}}
public sealed partial class GameEngine
{
}
