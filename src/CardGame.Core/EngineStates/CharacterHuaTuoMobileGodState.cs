#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHuaTuoMobileGodState : global::CardGame.Core.EngineStates.Contracts.ICharacterHuaTuoMobileGodMobileWulingCoursesAccess
{
    // Durable private courses, rather than pending choices. Commands rebuild
    // these immutable accepted orders on replay; public facts reveal no future animal.
    private global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.MobileWulingCourse> _mobileWulingCourses = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.MobileWulingCourse> ReferenceToMobileWulingCourses => ref _mobileWulingCourses;

    ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.MobileWulingCourse> global::CardGame.Core.EngineStates.Contracts.ICharacterHuaTuoMobileGodMobileWulingCoursesAccess._mobileWulingCourses => ref _mobileWulingCourses;
}
