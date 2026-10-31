#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class DingYuanProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDingYuanOlEntries> getCharacterDingYuanOlRuntime) : IDingYuanProgramHost
{
    public void PrepareAdoptedChild(ProgramSkillFrame frame, bool transfer) => getCharacterDingYuanOlRuntime().PrepareAdoptedChild(frame, transfer);
    public void AssignAdoptedChild(ProgramSkillFrame frame) => getCharacterDingYuanOlRuntime().AssignAdoptedChild(frame);
    public void QualifyFirstRoundDamage(ProgramSkillFrame frame) => getCharacterDingYuanOlRuntime().QualifyFirstRoundDamage(frame);
}
