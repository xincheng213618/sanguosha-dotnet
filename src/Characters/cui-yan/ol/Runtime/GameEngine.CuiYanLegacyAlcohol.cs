using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum CuiYanLegacyAlcoholStage { Targets,Committed,Recovery }
public sealed record CuiYanLegacyAlcoholReturn(long UseId,long ProgramParentId,long DyingId,int InstructionIndex,CardConversionSource ProducerSource,string ProducerHash,long ActionId,CuiYanLegacyAlcoholStage Stage);
public sealed partial record CardUseFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)] public CuiYanLegacyAlcoholReturn? CuiYanLegacyAlcohol {get;init;}}
public sealed record CuiYanLegacyAlcoholWindowIssuedEvent(long UseId,long ProgramParentId,long DyingId,int InstructionIndex,CardConversionSource ProducerSource,string ProducerHash,long ActionId):IGameEvent;
public sealed record CuiYanLegacyAlcoholStageEvent(long UseId,long ActionId,CuiYanLegacyAlcoholStage Stage):IGameEvent;
