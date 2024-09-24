#include <stdio.h>
#include <Mod/CppUserModBase.hpp>

class P3R.WeaponFramework.UE4SS.Code : public RC::CppUserModBase
{
public:
    P3R.WeaponFramework.UE4SS.Code() : CppUserModBase()
    {
        ModName = STR("P3R.WeaponFramework.UE4SS.Code");
        ModVersion = STR("1.0");
        ModDescription = STR("Backend for blueprint functionality.");
        ModAuthors = STR("CerebralPolicy");
        // Do not change this unless you want to target a UE4SS version
        // other than the one you're currently building with somehow.
        //ModIntendedSDKVersion = STR("2.6");
        
        printf("P3R.WeaponFramework.UE4SS.Code says hello\n");
    }

    ~P3R.WeaponFramework.UE4SS.Code() override
    {
    }

    auto on_update() -> void override
    {
    }
};

#define MY_AWESOME_MOD_API __declspec(dllexport)
extern "C"
{
    MY_AWESOME_MOD_API RC::CppUserModBase* start_mod()
    {
        return new P3R.WeaponFramework.UE4SS.Code();
    }

    MY_AWESOME_MOD_API void uninstall_mod(RC::CppUserModBase* mod)
    {
        delete mod;
    }
}