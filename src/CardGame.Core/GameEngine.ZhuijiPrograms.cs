using System.Globalization;
using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed record ZhuijiPlayPhaseKey(int ActualTurnNumber,int OwnerSeat,int PhaseInstanceId);
public sealed record ZhuijiPlayGrant(long ProgramFrameId,CardConversionSource Source,string GameplayHash,int ActualTurnNumber,int PhaseInstanceId,bool RecoverThenDiscard,string EndingBindingId);
public sealed record ZhuijiPlayGrantIssuedEvent(ZhuijiPlayGrant Grant):IGameEvent;
public sealed record ZhuijiPlayDebtConsumedEvent(long ProgramFrameId,ZhuijiPlayGrant Grant,bool Paid):IGameEvent;
public sealed record ZhuijiDiscardPaidEvent(long ProgramFrameId,ZhuijiPlayGrant Grant,long Before,long After,int ActualCount):IGameEvent;
public enum ZhuijiDebtStage { ChoosingDiscard, DiscardChildren, HpChildren }
public sealed record ZhuijiDebtPending(ZhuijiPlayGrant Grant,int InstructionIndex,ZhuijiDebtStage Stage,long Before,long After,int ActualCount);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public ZhuijiPlayGrant? ZhuijiInitialPaid {get;init;}
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public ZhuijiDebtPending? ZhuijiDebt {get;init;}
}
public sealed partial record ProgramSkillWindowContext
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public ZhuijiPlayGrant? ZhuijiGrant {get;init;}
}
