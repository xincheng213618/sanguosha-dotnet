using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed partial record CardUseFrame
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public RoundOnceCardlessBasicUseReceipt? RoundOnceCardlessBasicUse{get;init;}
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public RoundOnceCardlessBasicUseReceipt? RoundOnceCardlessBasicResponse{get;init;}
}
public sealed partial record ProgramSkillFrame
{
 [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public RoundOnceCardlessBasicUseReceipt? RoundOnceCardlessBasicResponse{get;init;}
}
