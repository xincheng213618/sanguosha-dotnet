using System.Text.Json.Serialization;
namespace CardGame.Core;
internal sealed record IdentityConcealmentState(string GeneralId,int InitialHp);
public enum IdentityAppearanceEntry { ActualTurnStart, DamageDeduction, ProgramHpLoss }
public sealed record IdentityAppearanceReceipt(IdentityAppearanceEntry Entry,long? ParentFrameId,int OwnerSeat,int ActualTurnNumber,int ActualTurnOwnerSeat,
    string GeneralId,int RestoredHp,int MaximumHp,int RequestedDeduction,int? DamageSourceSeat,CardKind? CardKind,
    int? ObservedAttackAmount=null,int? NativeSourceSeat=null,bool SourceLess=false,DamageNature? Nature=null,long? NativeActionId=null,int? OriginalInstructionIndex=null);
public sealed record IdentityConcealmentStartedEvent(int OwnerSeat):IGameEvent;
public sealed record IdentityHpDeductionPreventedEvent(long AppearanceFrameId,int OwnerSeat,int RequestedDeduction,IdentityAppearanceEntry Entry,int? DamageSourceSeat):IGameEvent;
public sealed record IdentityGeneralAppearedEvent(long AppearanceFrameId,IdentityAppearanceReceipt Receipt):IGameEvent;
public sealed record IdentityAppearanceCompletedEvent(long AppearanceFrameId,int OwnerSeat):IGameEvent;
public sealed record GaolingRecoverySelectedEvent(long ProgramFrameId,long AppearanceFrameId,CardConversionSource Source,string GameplayHash,int TargetSeat):IGameEvent;
public sealed record GaolingRecoveryReceipt(long AppearanceFrameId,CardConversionSource Source,string GameplayHash,int TargetSeat);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public GaolingRecoveryReceipt? GaolingRecovery{get;init;}
}
public sealed record IdentityAppearanceFrame(long Id,IdentityAppearanceReceipt Receipt,long? ActiveWindowId=null,ResolutionFrameStep Step=ResolutionFrameStep.ResolvingEffect)
    :ResolutionFrame(Id,ResolutionFrameKind.IdentityAppearance,Step);
public sealed partial record ProgramSkillWindowContext
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public IdentityAppearanceReceipt? IdentityAppearance{get;init;}
}
