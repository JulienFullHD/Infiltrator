/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID ENEMY_HIT = 1010055213U;
        static const AkUniqueID GRUNT_BURST = 501485854U;
        static const AkUniqueID KATANA_SWING = 2725920228U;
        static const AkUniqueID KUNAI_THROW = 1445849350U;
        static const AkUniqueID PLAY_AMBIENCEWIND = 4079089048U;
        static const AkUniqueID PLAY_FS_ENEMY = 857770058U;
        static const AkUniqueID PLAY_FS_METAL = 405241043U;
        static const AkUniqueID PLAY_FS_METALWET = 1928192825U;
        static const AkUniqueID PLAY_MAINTHEME = 3009755888U;
        static const AkUniqueID PLAYER_DASH = 2394582229U;
        static const AkUniqueID PLAYER_JUMP = 1305133589U;
        static const AkUniqueID PLAYER_LAND = 3629196698U;
        static const AkUniqueID PLAYER_STOPWALLRUNNING = 328487306U;
        static const AkUniqueID PLAYER_WALLRUNNING = 2134301282U;
        static const AkUniqueID THROWSMOKE = 2085220022U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace PLAYERALIVE
        {
            static const AkUniqueID GROUP = 2557321869U;

            namespace STATE
            {
                static const AkUniqueID FALSE = 2452206122U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID TRUE = 3053630529U;
            } // namespace STATE
        } // namespace PLAYERALIVE

        namespace PLAYERISWALLRUNING
        {
            static const AkUniqueID GROUP = 3701697919U;

            namespace STATE
            {
                static const AkUniqueID FALSE = 2452206122U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID TRUE = 3053630529U;
            } // namespace STATE
        } // namespace PLAYERISWALLRUNING

    } // namespace STATES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID MASTERVOLUME = 2918011349U;
        static const AkUniqueID MUSICVOLUME = 2346531308U;
        static const AkUniqueID PLAYERHEALTH = 151362964U;
        static const AkUniqueID SFXVOLUME = 988953028U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MAIN = 3161908922U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIENCE = 85412153U;
        static const AkUniqueID ENVIRONMENT = 1229948536U;
        static const AkUniqueID MASTER = 4056684167U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID REVERBS = 3545700988U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID OPEN_FIELD = 1933665656U;
        static const AkUniqueID ROOM_MEDIUM_HIGHABSORBTION = 2249836984U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
