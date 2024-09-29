# P3R Weapon Framework
Welcome to the **P3R Weapon Framework** (P3WF) repository. This mod, when completed, will make it possible to add new weapons to Persona 3 Reload and lay a groundwork for similar mods in Metaphor Refantazio and Persona 6. In spirit, it is similar to Costume Framework - however the weapon system is significantly more cumbersome to hack into. As such, **P3WF is currently non-functional** and the project is currently bloated as I have reached a state of constant trial and error.

You're more than welcome to fork the project if you feel like contributing or tinkering away. If you make significant progress, I only ask that you file a pull request.

## Overview
Unlike the costumes, all data pertaining to the weapon models is housed within a collection of blueprints. I tried implementing a system to edit them, but that was putting the cart before the horse.

### Shell System
Currently **P3R Weapon Framework** is centred around a basic FName redirection system. The system initializes but I have yet to load a save with it enabled. This is necessitated by the inaccessibility of the weapon blueprints. Since we cannot edit the pointers, nor revert assigned FNames to their original values, P3WF uses dummy meshes to emulate such reversion.

All weapons have a defined *ShellType*, and a rich enum system based on SmartEnums extends the *ShellType* into a verbose definition that will tell P3WF which base assets need to be redirected. For the most part a *ShellType* is bound to a single model, meaning any new weapon need only have a single mesh. There are exceptions, as illustrated by P3WF's shell database.

``csharp
    public static ShellDatabase ShellLookup => [
        new (ShellType.None, [],[], false, false, true),
        new (ShellType.Unassigned, [],[], false, false, true),
        new (ShellType.Player, [EArmature.Wp0001_01], [10, 11, 12, 13, 14, 15, 16, 17, 18, 19], astrea: false),
        new (ShellType.Yukari, [EArmature.Wp0002_01], [20, 21, 22, 23, 24, 25, 26, 27, 28]),
        new (ShellType.Stupei, [EArmature.Wp0003_01], [30, 31, 32, 33, 34, 35, 36, 37, 38, 39]),
        new (ShellType.Akihiko, [EArmature.Wp0004_01, EArmature.Wp0004_02], [40, 41, 42, 43, 44, 45, 46, 47, 48]),
        new (ShellType.Mitsuru, [EArmature.Wp0005_01], [50, 51, 52, 53, 54, 55, 56, 57]),
        new (ShellType.Aigis_SmallArms, [EArmature.Wp0007_01, EArmature.Wp0007_02], [326, 327]),
        new (ShellType.Aigis_LongArms, [EArmature.Wp0007_03], [584, 585, 586, 587, 588, 589]),
        new (ShellType.Ken, [EArmature.Wp0008_01], [80, 81, 82, 83, 84, 85, 86, 87, 88, 89]),
        new (ShellType.Koromaru, [EArmature.Wp0009_01], [90, 91, 92, 93, 94, 95, 96, 97]),
        new (ShellType.Shinjiro, [EArmature.Wp0010_01], [100, 101, 102, 103, 104, 105], astrea: false),
        new (ShellType.Metis, [EArmature.Wp0011_01], [100, 101, 102, 103, 104, 105, 106], vanilla: false),
        ];``

## Known Issues