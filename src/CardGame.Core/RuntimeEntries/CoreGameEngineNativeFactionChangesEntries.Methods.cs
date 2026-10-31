#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeFactionChangesEntries
{
    internal void ApplyCaoTengFactionChange(ProgramSkillFrame frame, string faction) => _resolve().ApplyCaoTengFactionChange(frame, faction);
    internal IReadOnlyList<string> AvailableEffectiveFactionIds() => _resolve().AvailableEffectiveFactionIds();
    internal string? CaptureNativeAvatarFactionBeforeDeclaration(CharacterState player) => _resolve().CaptureNativeAvatarFactionBeforeDeclaration(player);
    internal GameEngine.NativeFactionAvatar? CaptureNativeFactionAvatar(CharacterState player) => _resolve().CaptureNativeFactionAvatar(player);
    internal long CaptureNativeFactionQualificationStamp(long underlying) => _resolve().CaptureNativeFactionQualificationStamp(underlying);
    internal void ChangeNativeEffectiveFaction(ProgramSkillFrame frame, string faction) => _resolve().ChangeNativeEffectiveFaction(frame, faction);
    internal void CompleteNativeAvatarFactionDeclaration(ProgramSkillFrame frame, CharacterState player, string? previous) => _resolve().CompleteNativeAvatarFactionDeclaration(frame, player, previous);
    internal bool HasNativeEffectiveFactionDeclaration(CharacterState player) => _resolve().HasNativeEffectiveFactionDeclaration(player);
    internal void SynchronizeNativeEffectiveFactions() => _resolve().SynchronizeNativeEffectiveFactions();
    internal bool TracksNativeEffectiveFactionChanges { get => _resolve().TracksNativeEffectiveFactionChanges; }
}
