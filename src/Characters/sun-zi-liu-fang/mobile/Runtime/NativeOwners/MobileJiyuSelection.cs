#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileJiyuSelection(Func<RuntimeEntries.CharacterSunZiLiuFangMobileEntries> getCharacterSunZiLiuFangMobileRuntime, EngineStates.CoreGameEngineSkillProgramsState gameEngineSkillProgramsState) : IMobileJiyuSelection
{
    public int PhaseLimit(int owner) => getCharacterSunZiLiuFangMobileRuntime().MobileJiyuLimit(owner);
    public bool MaySelect(int owner, int target) => getCharacterSunZiLiuFangMobileRuntime().MobileJiyuMaySelect(owner, target, gameEngineSkillProgramsState.ReferenceToProgramPhaseUses.GetValueOrDefault((owner, EngineOwners.CharacterSunZiLiuFangMobileRuntime.MobileJiyu, EngineOwners.CharacterSunZiLiuFangMobileRuntime.MobileJiyuActivation)));
}
