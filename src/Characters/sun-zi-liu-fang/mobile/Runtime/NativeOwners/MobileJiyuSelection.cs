#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileJiyuSelection(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunZiLiuFangMobileEntries> getCharacterSunZiLiuFangMobileRuntime, global::CardGame.Core.EngineStates.CoreGameEngineSkillProgramsState gameEngineSkillProgramsState) : IMobileJiyuSelection
{
    public int PhaseLimit(int owner) => getCharacterSunZiLiuFangMobileRuntime().MobileJiyuLimit(owner);
    public bool MaySelect(int owner, int target) => getCharacterSunZiLiuFangMobileRuntime().MobileJiyuMaySelect(owner, target, gameEngineSkillProgramsState.ReferenceToProgramPhaseUses.GetValueOrDefault((owner, global::CardGame.Core.EngineOwners.CharacterSunZiLiuFangMobileRuntime.MobileJiyu, global::CardGame.Core.EngineOwners.CharacterSunZiLiuFangMobileRuntime.MobileJiyuActivation)));
}
