using System.Globalization;
using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed record QingzhongPlayPhaseKey(int ActualTurnNumber,int OwnerSeat,int PhaseInstanceId);
public sealed record QingzhongPlayDebt(long ProgramFrameId,CardConversionSource Source,string GameplayHash,long StartingWindowId,QingzhongPlayPhaseKey Phase,string EndingBindingId,int InstructionIndex);
public sealed record QingzhongDebtArmedEvent(QingzhongPlayDebt Debt):IGameEvent;
public sealed record QingzhongDebtConsumedEvent(long FrameId,QingzhongPlayDebt Debt,bool Settled):IGameEvent;
public enum QingzhongStage { DrawChildren,ChooseMinimum,SwapChildren }
public sealed record QingzhongPending(QingzhongPlayDebt Debt,QingzhongStage Stage,int InstructionIndex,long Before,long After,int ActualCount,int? TargetSeat=null,int OwnerHandCount=0,int TargetHandCount=0);
public sealed record QingzhongMovementIssuedEvent(long FrameId,QingzhongStage Stage,long Before,long After,int ActualCount,int? TargetSeat,int OwnerHandCount,int TargetHandCount):IGameEvent;
public sealed record QingzhongCompletedEvent(long FrameId,QingzhongPlayDebt Debt,QingzhongStage Stage,int? TargetSeat,bool OwnerAlive):IGameEvent;
public sealed partial class CharacterState
{
 private readonly List<QingzhongPlayDebt> _qingzhongDebts=[];
 internal IReadOnlyList<QingzhongPlayDebt> QingzhongDebts=>_qingzhongDebts.AsReadOnly();
 internal void ArmQingzhongDebt(QingzhongPlayDebt debt)=>_qingzhongDebts.Add(debt);
 internal bool ConsumeQingzhongDebt(QingzhongPlayDebt debt)=>_qingzhongDebts.Remove(debt);
}
public sealed partial record ProgramSkillFrame
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public QingzhongPending? Qingzhong{get;init;}
}
public sealed partial record ProgramSkillWindowContext
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public QingzhongPlayPhaseKey? QingzhongPhase{get;init;}
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public QingzhongPlayDebt? QingzhongDebt{get;init;}
}
public sealed partial class GameEngine
{
}
