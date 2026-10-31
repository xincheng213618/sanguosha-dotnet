using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum LiuYanLegacyAlcoholStage { Targets,Committed,Recovery }
public sealed record LiuYanLegacyAlcoholReturn(long UseId,long ProgramParentId,long DyingId,int InstructionIndex,CardConversionSource ProducerSource,string ProducerHash,long ActionId,LiuYanLegacyAlcoholStage Stage);
public sealed partial record CardUseFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)] public LiuYanLegacyAlcoholReturn? LiuYanLegacyAlcohol {get;init;}}
public sealed record LiuYanLegacyAlcoholWindowIssuedEvent(long UseId,long ProgramParentId,long DyingId,int InstructionIndex,CardConversionSource ProducerSource,string ProducerHash,long ActionId):IGameEvent;
public sealed record LiuYanLegacyAlcoholStageEvent(long UseId,long ActionId,LiuYanLegacyAlcoholStage Stage):IGameEvent;
